using AutoMapper;
using GreenDonut;
using Microsoft.AspNetCore.Hosting;
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
using Velora.Application.Shared.Infrastructure;
using Velora.Application.Shared.Repositories;
using Velora.Application.Shared.Services;
using Velora.EntityFrameworkCore.EntityFramework.SqlServer;
using Velora.Infrastructure.ORM.Interfaces.MyApp.Orm.Interfaces;

namespace Velora.Application.Services
{
    public class SmsSettingService : GenericService<SqlSmsSetting, SqlSmsSetting, SmsSettingDto>, ISmsSettingService
    {
        private readonly ISqlRepository<SqlSmsSetting> _sqlrepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ITransactionService _transactionService;
        private readonly IModelValidationService _modelValidationService;
        protected readonly Lazy<ILocalizationMessageService> _messageService;
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _config;
        private readonly Lazy<IExcelTemplateService> _excelTemplateService;
        private readonly ISmsSettingService _roleSmsSettingService;
        protected readonly ICurrentUserService _currentUserService;
        public SmsSettingService(ISqlRepository<SqlSmsSetting> sqlRepository, IPosgreSqlRepository<SqlSmsSetting> pgRepository, IMapper mapper, IConfiguration configuration, ITransactionService transactionService, IWebHostEnvironment env, Lazy<ILocalizationMessageService> messageService, IModelValidationService modelValidationService, IConfiguration config, Lazy<IExcelTemplateService> excelTemplateService, ICurrentUserService currentUserService) : base(sqlRepository, pgRepository, mapper, configuration, messageService, currentUserService)
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

        public async Task<IQueryable<SmsSettingCrud>> GetAllViews()
        {
            return await GetAllViewQueryable<VwSmsSettingForm, VwSmsSettingForm, SmsSettingCrud>();
        }

        public async Task<ResultDto<SmsSettingDto>> CreateAsync(SmsSettingCrud input)
        {
            var(successMessage, errorMessage) = await _messageService.Value.GetSaveMessagesAsync();
            try
            {
                var validation = await _modelValidationService.ValidateAsync(input);
                if (!validation.Success)
                    return new ResultDto<SmsSettingDto>
                    {
                        Success = false,
                        Message = await _messageService.Value.GetMessageAsync(LocalizationKeys.ValidationFailed, "Form has errors. Please fix them."),
                        Errors = validation.Data
                    };
                var SmsSetting = new SmsSettingDto
                {
                    ApiKey = input.ApiKey,
                    IsActive = input.IsActive,
                    Provider = input.Provider,
                    SenderNumber = input.SenderNumber,
                };
                var result = await CreateAsync(SmsSetting);
                if (!result.Success)
                    return result;
                await _transactionService.CommitAsync();
                return result;
            }
            catch (Exception ex)
            {
                await _transactionService.RollbackAsync();
                var result = new ResultDto<SmsSettingDto>
                {
                    Success = false,
                    Message = errorMessage,
                };
                result.Errors.Add(ex.Message);
                return result;
            }
        }

        public async Task<ResultDto<SmsSettingCrud>> CreateAdvancedAsync(SmsSettingCrud input)
        {
            var(successMessage, errorMessage) = await _messageService.Value.GetSaveMessagesAsync();
            try
            {
                var validation = await _modelValidationService.ValidateAsync(input);
                if (!validation.Success)
                    return new ResultDto<SmsSettingCrud>
                    {
                        Success = false,
                        Message = await _messageService.Value.GetMessageAsync(LocalizationKeys.ValidationFailed, "Form has errors. Please fix them."),
                        Errors = validation.Data
                    };
                var errors = await QueryValidationHelper.ValidateUniquenessAsync(
                        await GetAllQuery(),
                        input.Id,
                        (
                        x => x.ApiKey.ToLower() == input.ApiKey.Trim().ToLower(),
                        "این کلید API قبلاً ثبت شده است"
                        )
                        );
                if (errors.Any())
                {
                    return new ResultDto<SmsSettingCrud>
                    {
                        Success = false,
                        Message = "اطلاعات آیتم ها تکراری است",
                        Errors = errors
                    };
                }
                var smsSetting = new SmsSettingDto
                {
                    ApiKey = input.ApiKey,
                    IsActive = input.IsActive,
                    Provider = input.Provider,
                    SenderNumber = input.SenderNumber
                };
                var result = await CreateWithViewAsync(smsSetting, GetSmsSettingById, x => x.Id);
                if (result.Success)
                    result.Message = successMessage;
                return result;
            }
            catch (Exception ex)
            {
                await _transactionService.RollbackAsync();
                var result = new ResultDto<SmsSettingCrud>
                {
                    Success = false,
                    Message = errorMessage
                };
                result.Errors.Add(ex.Message);
                return result;
            }
        }

        public async Task<ResultDto<SmsSettingCrud>> UpdateAsync(SmsSettingCrud input)
        {
            var(successMessage, errorMessage) = await _messageService.Value.GetSaveMessagesAsync();
            try
            {
                if (input.Id == null)
                {
                    return new ResultDto<SmsSettingCrud>
                    {
                        Success = false,
                        Message = await _messageService.Value.GetMessageAsync(LocalizationKeys.IdRequired)
                    };
                }

                var validation = await _modelValidationService.ValidateAsync(input);
                if (!validation.Success)
                    return new ResultDto<SmsSettingCrud>
                    {
                        Success = false,
                        Message = await _messageService.Value.GetMessageAsync(LocalizationKeys.ValidationFailed, "Form has errors. Please fix them."),
                        Errors = validation.Data
                    };
                var errors = await QueryValidationHelper.ValidateUniquenessAsync(
                        await GetAllQuery(),
                        input.Id,
                        (
                        x => x.ApiKey.ToLower() == input.ApiKey.Trim().ToLower(),
                        "این کلید API قبلاً ثبت شده است"
                        )
                        );
                if (errors.Any())
                {
                    return new ResultDto<SmsSettingCrud>
                    {
                        Success = false,
                        Message = "اطلاعات آیتم ها تکراری است",
                        Errors = errors
                    };
                }
                // 1️⃣ به‌روزرسانی کاربر
                var userUpdateDto = new SmsSettingDto
                {
                    Id = input.Id,
                    ApiKey = input.ApiKey,
                    IsActive = input.IsActive,
                    Provider = input.Provider,
                    SenderNumber = input.SenderNumber,
                };
                var result = await UpdateAsync(userUpdateDto, input.Id);
                if (!result.Success)
                {
                    return new ResultDto<SmsSettingCrud>
                    {
                        Success = false,
                        Message = result.Message,
                        Errors = result.Errors
                    };
                }

                await _transactionService.CommitAsync();
                var viewResult = await GetSmsSettingById(input.Id);
                if (viewResult == null)
                {
                    return new ResultDto<SmsSettingCrud>
                    {
                        Success = false,
                        Message = "اطلاعات پس از ویرایش قابل دریافت نیست."
                    };
                }

                return new ResultDto<SmsSettingCrud>
                {
                    Success = true,
                    Message = successMessage,
                    Data = viewResult
                };
            }
            catch (Exception ex)
            {
                await _transactionService.RollbackAsync();
                var result = new ResultDto<SmsSettingCrud>
                {
                    Success = false,
                    Message = errorMessage,
                };
                result.Errors.Add(ex.Message);
                return result;
            }
        }

        public async Task<ResultDto<BulkInsertResult>> BulkInsertAsync(Stream excelStream)
        {
            var createdSmsSettings = new List<SmsSettingDto>();
            var errors = new List<string>();
            var(successMessage, errorMessage) = await _messageService.Value.GetSaveMessagesAsync();
            var errorFileTitle = await _messageService.Value.GetMessageAsync(LocalizationKeys.ErrorFile);
            try
            {
                var(dt, rowContexts) = excelStream.LoadExcelWithErrors();
                var SmsSettings = dt.ToModelList<SmsSettingCrud>();
                for (int i = 0; i < SmsSettings.Count; i++)
                {
                    var SmsSetting = SmsSettings[i];
                    var context = rowContexts[i];
                    var createResult = await CreateAsync(SmsSetting);
                    if (createResult.Success && createResult.Data != null)
                    {
                        createdSmsSettings.Add(createResult.Data);
                    }
                    else
                    {
                        string errorMsg = createResult.Errors != null && createResult.Errors.Any() ? string.Join("; ", createResult.Errors) : !string.IsNullOrWhiteSpace(createResult.Message) ? createResult.Message : "Unknown error";
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
                    Message = errors.Count == 0 ? successMessage : errorFileTitle,
                    Data = new BulkInsertResult
                    {
                        InsertedCount = createdSmsSettings.Count,
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
                    Errors = new List<string>
                    {
                        ex.Message
                    }
                };
            }
        }

        public async Task<byte[]> ExportAsync(bool exportCurrentPage, int pageNumber, int pageSize)
        {
            // 1️⃣ گرفتن همه داده‌ها از query
            var query = await GetAllViews(); // IQueryable<Resource>
            // 2️⃣ Paging و Mapping به DTO
            List<SmsSettingCrud> data;
            if (exportCurrentPage)
            {
                data = query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            }
            else
            {
                data = query.ToList();
            }

            var resource = _mapper.Map<List<SmsSettingCrud>>(data);
            // 3️⃣ تولید Template اکسل با Lookup (مثلاً 5 ردیف خالی اضافه)
            var templateBytes = await _excelTemplateService.Value.GenerateTemplateWithLookupsAsync(LookupEntities.SmsSetting, // نام مدل DTO
 data.Count + 5);
            // 4️⃣ پر کردن داده‌ها در Template با Extension Method
            var resultBytes = templateBytes.FillDataIntoTemplate(data, startRow: 3);
            return resultBytes;
        }

        public async Task<SmsSettingCrud?> GetSmsSettingById(Guid id)
        {
            return await GetSqlViewByIdQueryable<SqlSmsSettingView, SmsSettingCrud>(id);
        }
    }
}