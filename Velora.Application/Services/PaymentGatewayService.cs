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
using Velora.Application.Shared.Infrastructure;
using Velora.Application.Shared.Repositories;
using Velora.Application.Shared.Services;
using Velora.EntityFrameworkCore.EntityFramework.SqlServer;
using Velora.Infrastructure.ORM.Interfaces.MyApp.Orm.Interfaces;

namespace Velora.Application.Services
{
    public class PaymentGatewayService : GenericService<SqlPaymentGateway, SqlPaymentGateway, PaymentGatewayDto>, IPaymentGatewayService
    {
        private readonly ISqlRepository<SqlPaymentGateway> _sqlrepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ITransactionService _transactionService;
        private readonly IModelValidationService _modelValidationService;
        protected readonly Lazy<ILocalizationMessageService> _messageService;
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _config;
        private readonly Lazy<IExcelTemplateService> _excelTemplateService;
        private readonly IPaymentGatewayService _rolePaymentGatewayService;
        protected readonly ICurrentUserService _currentUserService;
        public PaymentGatewayService(ISqlRepository<SqlPaymentGateway> sqlRepository, IPosgreSqlRepository<SqlPaymentGateway> pgRepository, IMapper mapper, IConfiguration configuration, ITransactionService transactionService, IWebHostEnvironment env, Lazy<ILocalizationMessageService> messageService, IModelValidationService modelValidationService, IConfiguration config, Lazy<IExcelTemplateService> excelTemplateService, ICurrentUserService currentUserService) : base(sqlRepository, pgRepository, mapper, configuration, messageService, currentUserService)
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

        public async Task<IQueryable<PaymentGatewayCrud>> GetAllViews()
        {
            return await GetAllViewQueryable<SqlPaymentGatewayView, SqlPaymentGatewayView, PaymentGatewayCrud>();
        }

        public async Task<ResultDto<PaymentGatewayDto>> CreateAsync(PaymentGatewayCrud input)
        {
            var(successMessage, errorMessage) = await _messageService.Value.GetSaveMessagesAsync();
            try
            {
                var validation = await _modelValidationService.ValidateAsync(input);
                if (!validation.Success)
                    return new ResultDto<PaymentGatewayDto>
                    {
                        Success = false,
                        Message = await _messageService.Value.GetMessageAsync(LocalizationKeys.ValidationFailed, "Form has errors. Please fix them."),
                        Errors = validation.Data
                    };
                var ProviderType = (PaymentProvider)input.ProviderType;
                var PaymentGateway = new PaymentGatewayDto
                {
                    CallbackUrl = input.CallbackUrl,
                    Description = input.Description,
                    DisplayOrder = input.DisplayOrder,
                    GatewayCode = ProviderType.GetCode(),
                    IsActive = input.IsActive,
                    IsDefault = input.IsDefault,
                    LogoUrl = input.LogoUrl,
                    Name = input.Name,
                    ProviderType = input.ProviderType,
                    SettingsJson = input.SettingsJson,
                };
                var PaymentGatewayResult = await CreateAsync(PaymentGateway);
                await _transactionService.CommitAsync();
                return PaymentGatewayResult;
            }
            catch (Exception ex)
            {
                await _transactionService.RollbackAsync();
                var result = new ResultDto<PaymentGatewayDto>
                {
                    Success = false,
                    Message = errorMessage,
                };
                result.Errors.Add(ex.Message);
                return result;
            }
        }

        public async Task<ResultDto<PaymentGatewayCrud>> CreateAdvancedAsync(PaymentGatewayCrud input)
        {
            var(successMessage, errorMessage) = await _messageService.Value.GetSaveMessagesAsync();
            try
            {
                var validation = await _modelValidationService.ValidateAsync(input);
                if (!validation.Success)
                    return new ResultDto<PaymentGatewayCrud>
                    {
                        Success = false,
                        Message = await _messageService.Value.GetMessageAsync(LocalizationKeys.ValidationFailed, "Form has errors. Please fix them."),
                        Errors = validation.Data
                    };
                var errors = await QueryValidationHelper.ValidateUniquenessAsync(
                      await GetAllQuery(),
                      input.Id,
                      (
                          x => (x.ProviderType == input.ProviderType),
                           "ارائه دهنده تکراری است."
                      )
                  );
                if (errors.Any())
                {
                    return new ResultDto<PaymentGatewayCrud>
                    {
                        Success = false,
                        Message = "اطلاعات آیتم‌ها تکراری است",
                        Errors = errors
                    };
                }

                var providerType = (PaymentProvider)input.ProviderType;
                var paymentGateway = new PaymentGatewayDto
                {
                    CallbackUrl = input.CallbackUrl,
                    Description = input.Description,
                    DisplayOrder = input.DisplayOrder,
                    GatewayCode = providerType.GetCode(),
                    IsActive = input.IsActive,
                    IsDefault = input.IsDefault,
                    LogoUrl = input.LogoUrl,
                    Name = input.Name,
                    ProviderType = input.ProviderType,
                    SettingsJson = input.SettingsJson
                };
                if (paymentGateway.DisplayOrder <= 0)
                {
                    paymentGateway.DisplayOrder = (await Query().MaxAsync(x => (int?)x.DisplayOrder) ?? 0) + 1;
                }
                var result = await CreateWithViewAsync(paymentGateway, GetPaymentGatewayById, x => x.Id);
                if (result.Success)
                    result.Message = successMessage;
                return result;
            }
            catch (Exception ex)
            {
                await _transactionService.RollbackAsync();
                var result = new ResultDto<PaymentGatewayCrud>
                {
                    Success = false,
                    Message = errorMessage
                };
                result.Errors.Add(ex.Message);
                return result;
            }
        }

        public async Task<ResultDto<PaymentGatewayCrud>> UpdateAsync(PaymentGatewayCrud input)
        {
            var(successMessage, errorMessage) = await _messageService.Value.GetSaveMessagesAsync();
            try
            {
                if (input.Id == null)
                {
                    return new ResultDto<PaymentGatewayCrud>
                    {
                        Success = false,
                        Message = await _messageService.Value.GetMessageAsync(LocalizationKeys.IdRequired)
                    };
                }
                var validation = await _modelValidationService.ValidateAsync(input);
                if (!validation.Success)
                    return new ResultDto<PaymentGatewayCrud>
                    {
                        Success = false,
                        Message = await _messageService.Value.GetMessageAsync(LocalizationKeys.ValidationFailed, "Form has errors. Please fix them."),
                        Errors = validation.Data
                    };
                var errors = await QueryValidationHelper.ValidateUniquenessAsync(
                      await GetAllQuery(),
                      input.Id,
                      (
                          x => (x.ProviderType == input.ProviderType),
                           "ارائه دهنده تکراری است."
                      )
                  );
                var ProviderType = (PaymentProvider)input.ProviderType;
                // 1️⃣ به‌روزرسانی کاربر
                var updateDto = new PaymentGatewayDto
                {
                    Id = input.Id,
                    CallbackUrl = input.CallbackUrl,
                    Description = input.Description,
                    DisplayOrder = input.DisplayOrder,
                    GatewayCode = ProviderType.GetCode(),
                    IsActive = input.IsActive,
                    IsDefault = input.IsDefault,
                    LogoUrl = input.LogoUrl,
                    Name = input.Name,
                    ProviderType = input.ProviderType,
                    SettingsJson = input.SettingsJson,
                };
                var PaymentGatewayResult = await UpdateAsync(updateDto, input.Id);
                if (!PaymentGatewayResult.Success)
                {
                    return new ResultDto<PaymentGatewayCrud>
                    {
                        Success = false,
                        Message = PaymentGatewayResult.Message,
                        Errors = PaymentGatewayResult.Errors
                    };
                }

                await _transactionService.CommitAsync();
                var viewResult = await GetPaymentGatewayById(input.Id);
                if (viewResult == null)
                {
                    return new ResultDto<PaymentGatewayCrud>
                    {
                        Success = false,
                        Message = "اطلاعات پس از ویرایش قابل دریافت نیست."
                    };
                }

                return new ResultDto<PaymentGatewayCrud>
                {
                    Success = true,
                    Message = successMessage,
                    Data = viewResult
                };
            }
            catch (Exception ex)
            {
                await _transactionService.RollbackAsync();
                var result = new ResultDto<PaymentGatewayCrud>
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
            var createdPaymentGateways = new List<PaymentGatewayDto>();
            var errors = new List<string>();
            var(successMessage, errorMessage) = await _messageService.Value.GetSaveMessagesAsync();
            var errorFileTitle = await _messageService.Value.GetMessageAsync(LocalizationKeys.ErrorFile);
            try
            {
                var(dt, rowContexts) = excelStream.LoadExcelWithErrors();
                var PaymentGateways = dt.ToModelList<PaymentGatewayCrud>();
                for (int i = 0; i < PaymentGateways.Count; i++)
                {
                    var PaymentGateway = PaymentGateways[i];
                    var context = rowContexts[i];
                    var createResult = await CreateAsync(PaymentGateway);
                    if (createResult.Success && createResult.Data != null)
                    {
                        createdPaymentGateways.Add(createResult.Data);
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
                        InsertedCount = createdPaymentGateways.Count,
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

        public async Task<byte[]> ExportAsync(bool exportCurrentPaymentGateway, int PaymentGatewayNumber, int PaymentGatewaySize)
        {
            // 1️⃣ گرفتن همه داده‌ها از query
            var query = await GetAllViews(); // IQueryable<Resource>
            // 2️⃣ Paging و Mapping به DTO
            List<PaymentGatewayCrud> data;
            if (exportCurrentPaymentGateway)
            {
                data = query.Skip((PaymentGatewayNumber - 1) * PaymentGatewaySize).Take(PaymentGatewaySize).ToList();
            }
            else
            {
                data = query.ToList();
            }

            var resource = _mapper.Map<List<PaymentGatewayCrud>>(data);
            // 3️⃣ تولید Template اکسل با Lookup (مثلاً 5 ردیف خالی اضافه)
            var templateBytes = await _excelTemplateService.Value.GenerateTemplateWithLookupsAsync(LookupEntities.PaymentGateway, // نام مدل DTO
 data.Count + 5);
            // 4️⃣ پر کردن داده‌ها در Template با Extension Method
            var resultBytes = templateBytes.FillDataIntoTemplate(data, startRow: 3);
            return resultBytes;
        }

        public async Task<PaymentGatewayCrud?> GetPaymentGatewayById(Guid id)
        {
            return await GetSqlViewByIdQueryable<SqlPaymentGatewayView, PaymentGatewayCrud>(id);
        }
    }
}