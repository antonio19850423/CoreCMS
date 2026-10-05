using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Constants;
using Velora.Application.Shared.Dtos;
using Velora.Application.Shared.Enums;
using Velora.Application.Shared.Extensions;
using Velora.Application.Shared.Repositories;
using Velora.Application.Shared.Services;
using Velora.Infrastructure.ORM.Interfaces.MyApp.Orm.Interfaces;

namespace Velora.Application.Services
{
    public class NewsletterSubscriberService : GenericService<SqlNewsletterSubscriber, SqlNewsletterSubscriber, NewsletterSubscriberDto>, INewsletterSubscriberService
    {
        private readonly ISqlRepository<SqlNewsletterSubscriber> _sqlrepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ITransactionService _transactionService;
        private readonly IModelValidationService _modelValidationService;
        protected readonly Lazy<ILocalizationMessageService> _messageService;
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _config;
        private readonly Lazy<IExcelTemplateService> _excelTemplateService;
        private readonly INewsletterSubscriberService _roleNewsletterSubscriberService;
        protected readonly ICurrentUserService _currentUserService;
        public NewsletterSubscriberService(
              ISqlRepository<SqlNewsletterSubscriber> sqlRepository,
              IPosgreSqlRepository<SqlNewsletterSubscriber> pgRepository,
              IMapper mapper,
              IConfiguration configuration, ITransactionService transactionService, IWebHostEnvironment env,
              Lazy<ILocalizationMessageService> messageService, IModelValidationService modelValidationService, IConfiguration config, Lazy<IExcelTemplateService> excelTemplateService,
              ICurrentUserService currentUserService)
              : base(sqlRepository, pgRepository, mapper, configuration, messageService, currentUserService)
        {
            _mapper = mapper;
            _transactionService = transactionService;
            _messageService = messageService;
            _modelValidationService = modelValidationService;
            _env = env;
            _config = config;
            _excelTemplateService = excelTemplateService;
            _currentUserService = currentUserService;
        }
        public async Task<IQueryable<NewsletterSubscriberCrud>> GetAllViews()
        {
            return await GetAllViewQueryable<SqlNewsletterSubscriberView, SqlNewsletterSubscriberView, NewsletterSubscriberCrud>();
        }

        public async Task<ResultDto<NewsletterSubscriberDto>> CreateAsync(NewsletterSubscriberCrud input)
        {
            var (successMessage, errorMessage) = await _messageService.Value.GetSaveMessagesAsync();
            try
            {
                var validation = await _modelValidationService.ValidateAsync(input);
                if (!validation.Success)
                    return new ResultDto<NewsletterSubscriberDto>
                    {
                        Success = false,
                        Message = await _messageService.Value.GetMessageAsync(LocalizationKeys.ValidationFailed, "Form has errors. Please fix them."),
                        Errors = validation.Data
                    };


                var NewsletterSubscriber = new NewsletterSubscriberDto
                {
                  Email = input.Email,
                  IsActive = input.IsActive,
                  IsConfirmed = input.IsConfirmed,
                };

                var NewsletterSubscriberResult = await CreateAsync(NewsletterSubscriber);
                if (!NewsletterSubscriberResult.Success)
                    return NewsletterSubscriberResult;
                return NewsletterSubscriberResult;
            }
            catch (Exception ex)
            {
                var result = new ResultDto<NewsletterSubscriberDto>
                {
                    Success = false,
                    Message = errorMessage,
                };
                result.Errors.Add(ex.Message);
                return result;
            }
        }

        public async Task<ResultDto<NewsletterSubscriberDto>> UpdateAsync(NewsletterSubscriberCrud input)
        {
            var (successMessage, errorMessage) = await _messageService.Value.GetSaveMessagesAsync();
            try
            {
                if (input.Id == null)
                {
                    return new ResultDto<NewsletterSubscriberDto>
                    {
                        Success = false,
                        Message = await _messageService.Value.GetMessageAsync(LocalizationKeys.IdRequired)
                    };
                }
                var validation = await _modelValidationService.ValidateAsync(input);
                if (!validation.Success)
                    return new ResultDto<NewsletterSubscriberDto>
                    {
                        Success = false,
                        Message = await _messageService.Value.GetMessageAsync(LocalizationKeys.ValidationFailed, "Form has errors. Please fix them."),
                        Errors = validation.Data
                    };

                // 1️⃣ به‌روزرسانی کاربر
                var updateDto = new NewsletterSubscriberDto
                {
                    Id = input.Id,
                    Email = input.Email,
                    IsActive = input.IsActive,
                    IsConfirmed = input.IsConfirmed
                };

                var NewsletterSubscriberResult = await UpdateAsync(updateDto, input.Id);
                if (!NewsletterSubscriberResult.Success)
                    return NewsletterSubscriberResult;
                return NewsletterSubscriberResult;
            }
            catch (Exception ex)
            {
                var result = new ResultDto<NewsletterSubscriberDto>
                {
                    Success = false,
                    Message = errorMessage,
                };
                result.Errors.Add(ex.Message);
                return result;
            }
        }
        public async Task<ResultDto<NewsletterSubscriberDto>> SubscribeAsync(string email)
        {
            try
            {
                // =========================================================
                // 1. بررسی مقدار ایمیل
                // =========================================================

                if (string.IsNullOrWhiteSpace(email))
                {
                    return new ResultDto<NewsletterSubscriberDto>
                    {
                        Success = false,
                        Message = "لطفاً ایمیل خود را وارد کنید."
                    };
                }

                // =========================================================
                // 2. پاکسازی و یکسان‌سازی ایمیل
                // =========================================================

                email = email.Trim().ToLowerInvariant();

                // =========================================================
                // 3. محدودیت طول
                // =========================================================

                if (email.Length > 320)
                {
                    return new ResultDto<NewsletterSubscriberDto>
                    {
                        Success = false,
                        Message = "ایمیل وارد شده معتبر نیست."
                    };
                }

                // =========================================================
                // 4. بررسی ساختار ایمیل
                // =========================================================

                try
                {
                    var mailAddress = new System.Net.Mail.MailAddress(email);

                    if (mailAddress.Address != email)
                    {
                        return new ResultDto<NewsletterSubscriberDto>
                        {
                            Success = false,
                            Message = "ایمیل وارد شده معتبر نیست."
                        };
                    }
                }
                catch
                {
                    return new ResultDto<NewsletterSubscriberDto>
                    {
                        Success = false,
                        Message = "ایمیل وارد شده معتبر نیست."
                    };
                }

                // =========================================================
                // 5. بررسی اینکه ایمیل قبلاً ثبت نشده باشد
                // =========================================================

                var exists = await Query()
                    .AsNoTracking()
                    .AnyAsync(x => x.Email == email);

                if (exists)
                {
                    return new ResultDto<NewsletterSubscriberDto>
                    {
                        Success = false,
                        Message = "این ایمیل قبلاً در خبرنامه عضو شده است."
                    };
                }

                // =========================================================
                // 6. ایجاد مشترک جدید
                // =========================================================

                var subscriber = new NewsletterSubscriberDto
                {
                    Email = email,
                    IsActive = true,

                    // چون فعلاً سیستم تأیید ایمیل نداریم،
                    // عضویت مستقیماً تأیید شده در نظر گرفته می‌شود.
                    IsConfirmed = true
                };

                var result = await CreateAsync(subscriber);

                if (!result.Success)
                    return result;

                // =========================================================
                // 7. ثبت تراکنش
                // =========================================================

                await _transactionService.CommitAsync();

                // =========================================================
                // 8. پیام موفقیت
                // =========================================================

                return new ResultDto<NewsletterSubscriberDto>
                {
                    Success = true,
                    Message = "با موفقیت در خبرنامه عضو شدید.",
                    Data = result.Data
                };
            }
            catch (Exception ex)
            {
                await _transactionService.RollbackAsync();

                return new ResultDto<NewsletterSubscriberDto>
                {
                    Success = false,
                    Message = "ثبت عضویت در خبرنامه با خطا مواجه شد.",
                    Errors = new List<string>
            {
                ex.Message
            }
                };
            }
        }
        public async Task<ResultDto<BulkInsertResult>> BulkInsertAsync(Stream excelStream)
        {
            var createdNewsletterSubscribers= new List<NewsletterSubscriberDto>();
            var errors = new List<string>();
            var (successMessage, errorMessage) = await _messageService.Value.GetSaveMessagesAsync();
            var errorFileTitle = await _messageService.Value.GetMessageAsync(LocalizationKeys.ErrorFile);
            try
            {
                var (dt, rowContexts) = excelStream.LoadExcelWithErrors();
                var NewsletterSubscribers = dt.ToModelList<NewsletterSubscriberCrud>();

                for (int i = 0; i < NewsletterSubscribers.Count; i++)
                {
                    var NewsletterSubscriber = NewsletterSubscribers[i];
                    var context = rowContexts[i];

                    var createResult = await CreateAsync(NewsletterSubscriber);

                    if (createResult.Success && createResult.Data != null)
                    {
                        createdNewsletterSubscribers.Add(createResult.Data);
                    }
                    else
                    {
                        string errorMsg =
                            createResult.Errors != null && createResult.Errors.Any()
                                ? string.Join("; ", createResult.Errors)
                                : !string.IsNullOrWhiteSpace(createResult.Message)
                                    ? createResult.Message
                                    : "Unknown error";

                        dt.SetRowError(context.DataTableRowIndex, errorMsg);
                        errors.Add($"Row {context.ExcelRowNumber}: {errorMsg}");
                    }
                }

                await _transactionService.CommitAsync();

                string? errorFileUrl = null;
                if (errors.Any())
                {
                    errorFileUrl = dt.SaveErrorExcel(_env.WebRootPath!, _config);
                }

                return new ResultDto<BulkInsertResult>
                {
                    Success = errors.Count == 0,
                    Message = errors.Count == 0
                        ? successMessage
                        : errorFileTitle,
                    Data = new BulkInsertResult
                    {
                        InsertedCount = createdNewsletterSubscribers.Count,
                        ErrorCount = errors.Count,
                        ErrorFileUrl = errorFileUrl
                    },
                    Errors = errors
                };
            }
            catch (Exception ex)
            {
                await _transactionService.RollbackAsync();
                return new ResultDto<BulkInsertResult>
                {
                    Success = false,
                    Message = errorFileTitle,
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<byte[]> ExportAsync(
bool exportCurrentNewsletterSubscriber,
int NewsletterSubscriberNumber,
int NewsletterSubscriberSize)
        {
            // 1️⃣ گرفتن همه داده‌ها از query
            var query = await GetAllViews(); // IQueryable<Resource>

            // 2️⃣ Paging و Mapping به DTO
            List<NewsletterSubscriberCrud> data;

            if (exportCurrentNewsletterSubscriber)
            {
                data = query
                    .Skip((NewsletterSubscriberNumber - 1) * NewsletterSubscriberSize)
                    .Take(NewsletterSubscriberSize)
                    .ToList();
            }
            else
            {
                data = query.ToList();
            }
            var resource = _mapper.Map<List<NewsletterSubscriberCrud>>(data);

            // 3️⃣ تولید Template اکسل با Lookup (مثلاً 5 ردیف خالی اضافه)
            var templateBytes = await _excelTemplateService.Value.GenerateTemplateWithLookupsAsync(
                LookupEntities.NewsletterSubscriber, // نام مدل DTO
                data.Count + 5
            );

            // 4️⃣ پر کردن داده‌ها در Template با Extension Method
            var resultBytes = templateBytes.FillDataIntoTemplate(data, startRow: 3);

            return resultBytes;
        }


    }

}
