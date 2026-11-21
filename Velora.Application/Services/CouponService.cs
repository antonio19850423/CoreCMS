using AutoMapper;
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
    public class CouponService : GenericService<SqlCoupon, SqlCoupon, CouponDto>, ICouponService
    {
        private readonly ISqlRepository<SqlCoupon> _sqlrepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ITransactionService _transactionService;
        private readonly IModelValidationService _modelValidationService;
        protected readonly Lazy<ILocalizationMessageService> _messageService;
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _config;
        private readonly Lazy<IExcelTemplateService> _excelTemplateService;
        private readonly ICouponService _roleCouponService;
        protected readonly ICurrentUserService _currentUserService;
        public CouponService(ISqlRepository<SqlCoupon> sqlRepository, IPosgreSqlRepository<SqlCoupon> pgRepository, IMapper mapper, IConfiguration configuration, ITransactionService transactionService, IWebHostEnvironment env, Lazy<ILocalizationMessageService> messageService, IModelValidationService modelValidationService, IConfiguration config, Lazy<IExcelTemplateService> excelTemplateService, ICurrentUserService currentUserService) : base(sqlRepository, pgRepository, mapper, configuration, messageService, currentUserService)
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

        public async Task<IQueryable<CouponCrud>> GetAllViews()
        {
            return await GetAllViewQueryable<VwCouponForm, VwCouponForm, CouponCrud>();
        }

        public async Task<ResultDto<CouponDto>> CreateAsync(CouponCrud input)
        {
            var(successMessage, errorMessage) = await _messageService.Value.GetSaveMessagesAsync();
            try
            {
                var validation = await _modelValidationService.ValidateAsync(input);
                if (!validation.Success)
                    return new ResultDto<CouponDto>
                    {
                        Success = false,
                        Message = await _messageService.Value.GetMessageAsync(LocalizationKeys.ValidationFailed, "Form has errors. Please fix them."),
                        Errors = validation.Data
                    };
                if (await IsCouponCodeDuplicateAsync(input.Code))
                {
                    return new ResultDto<CouponDto>
                    {
                        Success = false,
                        Message = "کد کوپن تکراری است."
                    };
                }

                var Coupon = new CouponDto
                {
                    CouponType = input.CouponType,
                    CanCombineWithDiscount = input.CanCombineWithDiscount,
                    CouponValue = input.CouponValue,
                    MaximumDiscountAmount = input.MaximumDiscountAmount,
                    MinimumOrderAmount = input.MinimumOrderAmount,
                    Code = input.Code,
                    EndDate = input.EndDate,
                    IsActive = input.IsActive,
                    IsSingleUsePerUser = input.IsSingleUsePerUser,
                    StartDate = input.StartDate,
                    UsageLimit = input.UsageLimit,
                    UsedCount = input.UsedCount,
                };
                var result = await CreateAsync(Coupon);
                if (!result.Success)
                    return result;
                await _transactionService.CommitAsync();
                return result;
            }
            catch (Exception ex)
            {
                await _transactionService.RollbackAsync();
                var result = new ResultDto<CouponDto>
                {
                    Success = false,
                    Message = errorMessage,
                };
                result.Errors.Add(ex.Message);
                return result;
            }
        }

        public async Task<ResultDto<CouponCrud>> CreateAdvancedAsync(CouponCrud input)
        {
            var(successMessage, errorMessage) = await _messageService.Value.GetSaveMessagesAsync();
            try
            {
                var validation = await _modelValidationService.ValidateAsync(input);
                if (!validation.Success)
                    return new ResultDto<CouponCrud>
                    {
                        Success = false,
                        Message = await _messageService.Value.GetMessageAsync(LocalizationKeys.ValidationFailed, "Form has errors. Please fix them."),
                        Errors = validation.Data
                    };
                var errors = await QueryValidationHelper.ValidateUniquenessAsync(
                      await GetAllQuery(),
                      input.Id,
                      (
                          x => (x.Code != null &&
                             input.Code != null &&
                             x.Code.ToLower() == input.Code.ToLower()),
                           "کد کوپن تکراری است."
                      )
                  );
                if (errors.Any())
                {
                    return new ResultDto<CouponCrud>
                    {
                        Success = false,
                        Message = "اطلاعات آیتم‌ها تکراری است",
                        Errors = errors
                    };
                }
                var coupon = new CouponDto
                {
                    CouponType = input.CouponType,
                    CanCombineWithDiscount = input.CanCombineWithDiscount,
                    CouponValue = input.CouponValue,
                    MaximumDiscountAmount = input.MaximumDiscountAmount,
                    MinimumOrderAmount = input.MinimumOrderAmount,
                    Code = input.Code,
                    EndDate = input.EndDate,
                    IsActive = input.IsActive,
                    IsSingleUsePerUser = input.IsSingleUsePerUser,
                    StartDate = input.StartDate,
                    UsageLimit = input.UsageLimit,
                    UsedCount = input.UsedCount
                };
                var result = await CreateWithViewAsync(coupon, GetCouponById, x => x.Id);
                if (result.Success)
                    result.Message = successMessage;
                return result;
            }
            catch (Exception ex)
            {
                await _transactionService.RollbackAsync();
                var result = new ResultDto<CouponCrud>
                {
                    Success = false,
                    Message = errorMessage
                };
                result.Errors.Add(ex.Message);
                return result;
            }
        }

        public async Task<ResultDto<CouponCrud>> UpdateAsync(CouponCrud input)
        {
            var(successMessage, errorMessage) = await _messageService.Value.GetSaveMessagesAsync();
            try
            {
                if (input.Id == null)
                {
                    return new ResultDto<CouponCrud>
                    {
                        Success = false,
                        Message = await _messageService.Value.GetMessageAsync(LocalizationKeys.IdRequired)
                    };
                }
                var validation = await _modelValidationService.ValidateAsync(input);
                if (!validation.Success)
                    return new ResultDto<CouponCrud>
                    {
                        Success = false,
                        Message = await _messageService.Value.GetMessageAsync(LocalizationKeys.ValidationFailed, "Form has errors. Please fix them."),
                        Errors = validation.Data
                    };
                var errors = await QueryValidationHelper.ValidateUniquenessAsync(
                      await GetAllQuery(),
                      input.Id,
                      (
                          x => (x.Code != null &&
                             input.Code != null &&
                             x.Code.ToLower() == input.Code.ToLower()),
                           "کد کوپن تکراری است."
                      )
                  );
                if (errors.Any())
                {
                    return new ResultDto<CouponCrud>
                    {
                        Success = false,
                        Message = "اطلاعات آیتم‌ها تکراری است",
                        Errors = errors
                    };
                }

                // 1️⃣ به‌روزرسانی کاربر
                var userUpdateDto = new CouponDto
                {
                    Id = input.Id,
                    CouponType = input.CouponType,
                    CanCombineWithDiscount = input.CanCombineWithDiscount,
                    CouponValue = input.CouponValue,
                    MaximumDiscountAmount = input.MaximumDiscountAmount,
                    MinimumOrderAmount = input.MinimumOrderAmount,
                    Code = input.Code,
                    EndDate = input.EndDate,
                    IsActive = input.IsActive,
                    IsSingleUsePerUser = input.IsSingleUsePerUser,
                    StartDate = input.StartDate,
                    UsageLimit = input.UsageLimit,
                    UsedCount = input.UsedCount,
                };
                var result = await UpdateAsync(userUpdateDto, input.Id);
                if (!result.Success)
                {
                    return new ResultDto<CouponCrud>
                    {
                        Success = false,
                        Message = result.Message,
                        Errors = result.Errors
                    };
                }

                await _transactionService.CommitAsync();
                var viewResult = await GetCouponById(input.Id);
                if (viewResult == null)
                {
                    return new ResultDto<CouponCrud>
                    {
                        Success = false,
                        Message = "اطلاعات پس از ویرایش قابل دریافت نیست."
                    };
                }

                return new ResultDto<CouponCrud>
                {
                    Success = true,
                    Message = successMessage,
                    Data = viewResult
                };
            }
            catch (Exception ex)
            {
                await _transactionService.RollbackAsync();
                var result = new ResultDto<CouponCrud>
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
            var createdCoupons = new List<CouponDto>();
            var errors = new List<string>();
            var(successMessage, errorMessage) = await _messageService.Value.GetSaveMessagesAsync();
            var errorFileTitle = await _messageService.Value.GetMessageAsync(LocalizationKeys.ErrorFile);
            try
            {
                var(dt, rowContexts) = excelStream.LoadExcelWithErrors();
                var Coupons = dt.ToModelList<CouponCrud>();
                for (int i = 0; i < Coupons.Count; i++)
                {
                    var Coupon = Coupons[i];
                    var context = rowContexts[i];
                    var createResult = await CreateAsync(Coupon);
                    if (createResult.Success && createResult.Data != null)
                    {
                        createdCoupons.Add(createResult.Data);
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
                        InsertedCount = createdCoupons.Count,
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

        private async Task<bool> IsCouponCodeDuplicateAsync(string code, Guid? id = null)
        {
            return await Query().AnyAsync(x => x.Code == code && (!id.HasValue || x.Id != id.Value));
        }

        public async Task<byte[]> ExportAsync(bool exportCurrentCoupon, int CouponNumber, int CouponSize)
        {
            // 1️⃣ گرفتن همه داده‌ها از query
            var query = await GetAllViews(); // IQueryable<Resource>
            // 2️⃣ Paging و Mapping به DTO
            List<CouponCrud> data;
            if (exportCurrentCoupon)
            {
                data = query.Skip((CouponNumber - 1) * CouponSize).Take(CouponSize).ToList();
            }
            else
            {
                data = query.ToList();
            }

            var resource = _mapper.Map<List<CouponCrud>>(data);
            // 3️⃣ تولید Template اکسل با Lookup (مثلاً 5 ردیف خالی اضافه)
            var templateBytes = await _excelTemplateService.Value.GenerateTemplateWithLookupsAsync(LookupEntities.Coupon, // نام مدل DTO
 data.Count + 5);
            // 4️⃣ پر کردن داده‌ها در Template با Extension Method
            var resultBytes = templateBytes.FillDataIntoTemplate(data, startRow: 3);
            return resultBytes;
        }

        public async Task<CouponCrud?> GetCouponById(Guid id)
        {
            return await GetSqlViewByIdQueryable<SqlCouponView, CouponCrud>(id);
        }

        public async Task<SqlCoupon?> GetByCodeAsync(string couponCode)
        {
            if (string.IsNullOrWhiteSpace(couponCode))
                return null;
            couponCode = couponCode.Trim();
            return await Query().FirstOrDefaultAsync(x => x.Code == couponCode);
        }
    }
}