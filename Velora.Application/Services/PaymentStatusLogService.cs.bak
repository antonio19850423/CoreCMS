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
    public class PaymentStatusLogService : GenericService<SqlPaymentStatusLog, SqlPaymentStatusLog, PaymentStatusLogDto>, IPaymentStatusLogService
    {
        private readonly ISqlRepository<SqlPaymentStatusLog> _sqlrepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ITransactionService _transactionService;
        private readonly IModelValidationService _modelValidationService;
        protected readonly Lazy<ILocalizationMessageService> _messageService;
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _config;
        private readonly Lazy<IExcelTemplateService> _excelTemplateService;
        private readonly IPaymentStatusLogService _rolePaymentStatusLogService;
        protected readonly ICurrentUserService _currentUserService;
        protected readonly IDiscountService _discountService;

        public PaymentStatusLogService(
              ISqlRepository<SqlPaymentStatusLog> sqlRepository,
              IPosgreSqlRepository<SqlPaymentStatusLog> pgRepository,
              IMapper mapper,
              IConfiguration configuration, ITransactionService transactionService, IWebHostEnvironment env,
              Lazy<ILocalizationMessageService> messageService, IModelValidationService modelValidationService, IConfiguration config, Lazy<IExcelTemplateService> excelTemplateService,
              ICurrentUserService currentUserService, IDiscountService discountService)
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
            _discountService = discountService;
        }
        public async Task<IQueryable<PaymentStatusLogCrud>> GetAllViews()
        {
            return await GetAllViewQueryable<SqlPaymentStatusLogView, SqlPaymentStatusLogView, PaymentStatusLogCrud>();
        }

        public async Task<ResultDto<PaymentStatusLogDto>> CreateAsync(PaymentStatusLogCrud input)
        {
            var (successMessage, errorMessage) = await _messageService.Value.GetSaveMessagesAsync();
            try
            {
                var validation = await _modelValidationService.ValidateAsync(input);
                if (!validation.Success)
                    return new ResultDto<PaymentStatusLogDto>
                    {
                        Success = false,
                        Message = await _messageService.Value.GetMessageAsync(LocalizationKeys.ValidationFailed, "Form has errors. Please fix them."),
                        Errors = validation.Data
                    };



                var PaymentStatusLog = new PaymentStatusLogDto
                {
                    Description=input.Description,
                    NewStatus = input.NewStatus,
                    OldStatus = input.OldStatus,
                    PaymentId=input.ParentId,

                };

                var PaymentStatusLogResult = await CreateAsync(PaymentStatusLog);
                if (!PaymentStatusLogResult.Success)
                    return PaymentStatusLogResult;
                await _transactionService.CommitAsync();
                return PaymentStatusLogResult;
            }
            catch (Exception ex)
            {
                await _transactionService.RollbackAsync();
                var result = new ResultDto<PaymentStatusLogDto>
                {
                    Success = false,
                    Message = errorMessage,
                };
                result.Errors.Add(ex.Message);
                return result;
            }
        }

        public async Task<ResultDto<PaymentStatusLogDto>> UpdateAsync(PaymentStatusLogCrud input)
        {
            var (successMessage, errorMessage) = await _messageService.Value.GetSaveMessagesAsync();
            try
            {
                if (input.Id == null)
                {
                    return new ResultDto<PaymentStatusLogDto>
                    {
                        Success = false,
                        Message = await _messageService.Value.GetMessageAsync(LocalizationKeys.IdRequired)
                    };
                }
                var validation = await _modelValidationService.ValidateAsync(input);
                if (!validation.Success)
                    return new ResultDto<PaymentStatusLogDto>
                    {
                        Success = false,
                        Message = await _messageService.Value.GetMessageAsync(LocalizationKeys.ValidationFailed, "Form has errors. Please fix them."),
                        Errors = validation.Data
                    };

                // 1️⃣ به‌روزرسانی کاربر
                var updateDto = new PaymentStatusLogDto
                {
                    Id = input.Id,
                    Description = input.Description,
                    NewStatus = input.NewStatus,
                    OldStatus = input.OldStatus,
                    PaymentId = input.ParentId,

                };

                var PaymentStatusLogResult = await UpdateAsync(updateDto, input.Id);
                if (!PaymentStatusLogResult.Success)
                    return PaymentStatusLogResult;
                await _transactionService.CommitAsync();
                return PaymentStatusLogResult;
            }
            catch (Exception ex)
            {
                await _transactionService.RollbackAsync();
                var result = new ResultDto<PaymentStatusLogDto>
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
            var createdPaymentStatusLogs= new List<PaymentStatusLogDto>();
            var errors = new List<string>();
            var (successMessage, errorMessage) = await _messageService.Value.GetSaveMessagesAsync();
            var errorFileTitle = await _messageService.Value.GetMessageAsync(LocalizationKeys.ErrorFile);
            try
            {
                var (dt, rowContexts) = excelStream.LoadExcelWithErrors();
                var PaymentStatusLogs = dt.ToModelList<PaymentStatusLogCrud>();

                for (int i = 0; i < PaymentStatusLogs.Count; i++)
                {
                    var PaymentStatusLog = PaymentStatusLogs[i];
                    var context = rowContexts[i];

                    var createResult = await CreateAsync(PaymentStatusLog);

                    if (createResult.Success && createResult.Data != null)
                    {
                        createdPaymentStatusLogs.Add(createResult.Data);
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
                        InsertedCount = createdPaymentStatusLogs.Count,
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
bool exportCurrentPaymentStatusLog,
int PaymentStatusLogNumber,
int PaymentStatusLogSize)
        {
            // 1️⃣ گرفتن همه داده‌ها از query
            var query = await GetAllViews(); // IQueryable<Resource>

            // 2️⃣ Paging و Mapping به DTO
            List<PaymentStatusLogCrud> data;

            if (exportCurrentPaymentStatusLog)
            {
                data = query
                    .Skip((PaymentStatusLogNumber - 1) * PaymentStatusLogSize)
                    .Take(PaymentStatusLogSize)
                    .ToList();
            }
            else
            {
                data = query.ToList();
            }
            var resource = _mapper.Map<List<PaymentStatusLogCrud>>(data);

            // 3️⃣ تولید Template اکسل با Lookup (مثلاً 5 ردیف خالی اضافه)
            var templateBytes = await _excelTemplateService.Value.GenerateTemplateWithLookupsAsync(
                LookupEntities.PaymentStatusLog, // نام مدل DTO
                data.Count + 5
            );

            // 4️⃣ پر کردن داده‌ها در Template با Extension Method
            var resultBytes = templateBytes.FillDataIntoTemplate(data, startRow: 3);

            return resultBytes;
        }
    }

}
