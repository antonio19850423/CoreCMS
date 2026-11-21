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
    public class ShippingMethodCityService : GenericService<SqlShippingMethodCity, SqlShippingMethodCity, ShippingMethodCityDto>, IShippingMethodCityService
    {
        private readonly ISqlRepository<SqlShippingMethodCity> _sqlrepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ITransactionService _transactionService;
        private readonly IModelValidationService _modelValidationService;
        protected readonly Lazy<ILocalizationMessageService> _messageService;
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _config;
        private readonly Lazy<IExcelTemplateService> _excelTemplateService;
        private readonly IShippingMethodCityService _roleShippingMethodCityService;
        protected readonly ICurrentUserService _currentUserService;
        public ShippingMethodCityService(ISqlRepository<SqlShippingMethodCity> sqlRepository, IPosgreSqlRepository<SqlShippingMethodCity> pgRepository, IMapper mapper, IConfiguration configuration, ITransactionService transactionService, IWebHostEnvironment env, Lazy<ILocalizationMessageService> messageService, IModelValidationService modelValidationService, IConfiguration config, Lazy<IExcelTemplateService> excelTemplateService, ICurrentUserService currentUserService) : base(sqlRepository, pgRepository, mapper, configuration, messageService, currentUserService)
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

        public async Task<IQueryable<ShippingMethodCityCrud>> GetAllViews()
        {
            return await GetAllViewQueryable<SqlShippingMethodCityView, SqlShippingMethodCityView, ShippingMethodCityCrud>();
        }

        public async Task<ResultDto<ShippingMethodCityDto>> CreateAsync(ShippingMethodCityCrud input)
        {
            var(successMessage, errorMessage) = await _messageService.Value.GetSaveMessagesAsync();
            try
            {
                var validation = await _modelValidationService.ValidateAsync(input);
                if (!validation.Success)
                    return new ResultDto<ShippingMethodCityDto>
                    {
                        Success = false,
                        Message = await _messageService.Value.GetMessageAsync(LocalizationKeys.ValidationFailed, "Form has errors. Please fix them."),
                        Errors = validation.Data
                    };
                var ShippingMethodCity = new ShippingMethodCityDto
                {
                    IsActive = input.IsActive,
                    SortOrder = input.SortOrder,
                    ShippingMethodId = input.ParentId,
                    CityId = input.CityId.Value,
                };
                var ShippingMethodCityResult = await CreateAsync(ShippingMethodCity);
                await _transactionService.CommitAsync();
                return ShippingMethodCityResult;
            }
            catch (Exception ex)
            {
                await _transactionService.RollbackAsync();
                var result = new ResultDto<ShippingMethodCityDto>
                {
                    Success = false,
                    Message = errorMessage,
                };
                result.Errors.Add(ex.Message);
                return result;
            }
        }

        public async Task<ResultDto<ShippingMethodCityCrud>> CreateAdvancedAsync(ShippingMethodCityCrud input)
        {
            var(successMessage, errorMessage) = await _messageService.Value.GetSaveMessagesAsync();
            try
            {
                var validation = await _modelValidationService.ValidateAsync(input);
                if (!validation.Success)
                    return new ResultDto<ShippingMethodCityCrud>
                    {
                        Success = false,
                        Message = await _messageService.Value.GetMessageAsync(LocalizationKeys.ValidationFailed, "Form has errors. Please fix them."),
                        Errors = validation.Data
                    };
                var errors = await QueryValidationHelper.ValidateUniquenessAsync(
                      await GetAllQuery(),
                      input.Id,
                      (
                          x => ( x.CityId!= Guid.Empty &&x.CityId == input.CityId.Value),
                           "این شهر قبلا ثبت شده است."
                      )
                  );
                if (errors.Any())
                {
                    return new ResultDto<ShippingMethodCityCrud>
                    {
                        Success = false,
                        Message = "اطلاعات آیتم ها تکراری است",
                        Errors = errors
                    };
                }

                var shippingMethodCity = new ShippingMethodCityDto
                {
                    IsActive = input.IsActive,
                    SortOrder = input.SortOrder,
                    ShippingMethodId = input.ParentId,
                    CityId = input.CityId.Value
                };
                if (shippingMethodCity.SortOrder <= 0)
                {
                    shippingMethodCity.SortOrder = (await Query().MaxAsync(x => (int?)x.SortOrder) ?? 0) + 1;
                }
                var result = await CreateWithViewAsync(shippingMethodCity, GetShippingMethodCityById, x => x.Id);
                if (result.Success)
                    result.Message = successMessage;
                return result;
            }
            catch (Exception ex)
            {
                await _transactionService.RollbackAsync();
                var result = new ResultDto<ShippingMethodCityCrud>
                {
                    Success = false,
                    Message = errorMessage
                };
                result.Errors.Add(ex.Message);
                return result;
            }
        }

        public async Task<ResultDto<ShippingMethodCityCrud>> UpdateAsync(ShippingMethodCityCrud input)
        {
            var(successMessage, errorMessage) = await _messageService.Value.GetSaveMessagesAsync();
            try
            {
                if (input.Id == null)
                {
                    return new ResultDto<ShippingMethodCityCrud>
                    {
                        Success = false,
                        Message = await _messageService.Value.GetMessageAsync(LocalizationKeys.IdRequired)
                    };
                }
                var validation = await _modelValidationService.ValidateAsync(input);
                if (!validation.Success)
                    return new ResultDto<ShippingMethodCityCrud>
                    {
                        Success = false,
                        Message = await _messageService.Value.GetMessageAsync(LocalizationKeys.ValidationFailed, "Form has errors. Please fix them."),
                        Errors = validation.Data
                    };
                var errors = await QueryValidationHelper.ValidateUniquenessAsync(
                      await GetAllQuery(),
                      input.Id,
                      (
                          x => (x.CityId != Guid.Empty && x.CityId == input.CityId),
                           "این شهر قبلا ثبت شده است."
                      )
                  );
                if (errors.Any())
                {
                    return new ResultDto<ShippingMethodCityCrud>
                    {
                        Success = false,
                        Message = "اطلاعات آیتم ها تکراری است",
                        Errors = errors
                    };
                }
                // 1️⃣ به‌روزرسانی کاربر
                var updateDto = new ShippingMethodCityDto
                {
                    Id = input.Id,
                    IsActive = input.IsActive,
                    SortOrder = input.SortOrder,
                    ShippingMethodId = input.ParentId,
                    CityId = input.CityId.Value,
                };
                var ShippingMethodCityResult = await UpdateAsync(updateDto, input.Id);
                if (!ShippingMethodCityResult.Success)
                {
                    return new ResultDto<ShippingMethodCityCrud>
                    {
                        Success = false,
                        Message = ShippingMethodCityResult.Message,
                        Errors = ShippingMethodCityResult.Errors
                    };
                }

                await _transactionService.CommitAsync();
                var viewResult = await GetShippingMethodCityById(input.Id);
                if (viewResult == null)
                {
                    return new ResultDto<ShippingMethodCityCrud>
                    {
                        Success = false,
                        Message = "اطلاعات پس از ویرایش قابل دریافت نیست."
                    };
                }

                return new ResultDto<ShippingMethodCityCrud>
                {
                    Success = true,
                    Message = successMessage,
                    Data = viewResult
                };
            }
            catch (Exception ex)
            {
                await _transactionService.RollbackAsync();
                var result = new ResultDto<ShippingMethodCityCrud>
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
            var createdShippingMethodCitys = new List<ShippingMethodCityDto>();
            var errors = new List<string>();
            var(successMessage, errorMessage) = await _messageService.Value.GetSaveMessagesAsync();
            var errorFileTitle = await _messageService.Value.GetMessageAsync(LocalizationKeys.ErrorFile);
            try
            {
                var(dt, rowContexts) = excelStream.LoadExcelWithErrors();
                var ShippingMethodCitys = dt.ToModelList<ShippingMethodCityCrud>();
                for (int i = 0; i < ShippingMethodCitys.Count; i++)
                {
                    var ShippingMethodCity = ShippingMethodCitys[i];
                    var context = rowContexts[i];
                    var createResult = await CreateAsync(ShippingMethodCity);
                    if (createResult.Success && createResult.Data != null)
                    {
                        createdShippingMethodCitys.Add(createResult.Data);
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
                        InsertedCount = createdShippingMethodCitys.Count,
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

        public async Task<byte[]> ExportAsync(bool exportCurrentShippingMethodCity, int ShippingMethodCityNumber, int ShippingMethodCitySize)
        {
            // 1️⃣ گرفتن همه داده‌ها از query
            var query = await GetAllViews(); // IQueryable<Resource>
            // 2️⃣ Paging و Mapping به DTO
            List<ShippingMethodCityCrud> data;
            if (exportCurrentShippingMethodCity)
            {
                data = query.Skip((ShippingMethodCityNumber - 1) * ShippingMethodCitySize).Take(ShippingMethodCitySize).ToList();
            }
            else
            {
                data = query.ToList();
            }

            var resource = _mapper.Map<List<ShippingMethodCityCrud>>(data);
            // 3️⃣ تولید Template اکسل با Lookup (مثلاً 5 ردیف خالی اضافه)
            var templateBytes = await _excelTemplateService.Value.GenerateTemplateWithLookupsAsync(LookupEntities.ShippingMethodCity, // نام مدل DTO
 data.Count + 5);
            // 4️⃣ پر کردن داده‌ها در Template با Extension Method
            var resultBytes = templateBytes.FillDataIntoTemplate(data, startRow: 3);
            return resultBytes;
        }

        public async Task<ShippingMethodCityCrud?> GetShippingMethodCityById(Guid id)
        {
            return await GetSqlViewByIdQueryable<SqlShippingMethodCityView, ShippingMethodCityCrud>(id);
        }
    }
}