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
using Velora.Application.Shared.Repositories;
using Velora.Application.Shared.Services;
using Velora.EntityFrameworkCore.EntityFramework.SqlServer;
using Velora.Infrastructure.ORM.Interfaces.MyApp.Orm.Interfaces;

namespace Velora.Application.Services
{
    public class CityService : GenericService<SqlCity, SqlCity, CityDto>, ICityService
    {
        private readonly ISqlRepository<SqlCity> _sqlrepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ITransactionService _transactionService;
        private readonly IModelValidationService _modelValidationService;
        protected readonly Lazy<ILocalizationMessageService> _messageService;
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _config;
        private readonly Lazy<IExcelTemplateService> _excelTemplateService;
        private readonly ICityService _roleCityService;
        protected readonly ICurrentUserService _currentUserService;
        public CityService(
              ISqlRepository<SqlCity> sqlRepository,
              IPosgreSqlRepository<SqlCity> pgRepository,
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
        public async Task<IQueryable<CityCrud>> GetAllViews()
        {
            return await GetAllViewQueryable<VwCityForm, VwCityForm, CityCrud>();
        }


        public async Task<ResultDto<CityDto>> CreateAsync(CityCrud input)
        {
            var (successMessage, errorMessage) = await _messageService.Value.GetSaveMessagesAsync();
            try
            {
                var validation = await _modelValidationService.ValidateAsync(input);
                if (!validation.Success)
                    return new ResultDto<CityDto>
                    {
                        Success = false,
                        Message = await _messageService.Value.GetMessageAsync(LocalizationKeys.ValidationFailed, "Form has errors. Please fix them."),
                        Errors = validation.Data
                    };
                var City = new CityDto
                {
                    Name=input.StateTitle,
                    StateId=input.StateId,
                };

                var result = await CreateAsync(City);
                if (!result.Success)
                    return result;

                await _transactionService.CommitAsync();
                return result;
            }
            catch (Exception ex)
            {
                await _transactionService.RollbackAsync();
                var result = new ResultDto<CityDto>
                {
                    Success = false,
                    Message = errorMessage,
                };
                result.Errors.Add(ex.Message);
                return result;
            }
        }

        public async Task<ResultDto<CityDto>> UpdateAsync(CityCrud input)
        {
            var (successMessage, errorMessage) = await _messageService.Value.GetSaveMessagesAsync();
            try
            {
                if (input.Id == null)
                {
                    return new ResultDto<CityDto>
                    {
                        Success = false,
                        Message = await _messageService.Value.GetMessageAsync(LocalizationKeys.IdRequired)
                    };
                }
                var validation = await _modelValidationService.ValidateAsync(input);
                if (!validation.Success)
                    return new ResultDto<CityDto>
                    {
                        Success = false,
                        Message = await _messageService.Value.GetMessageAsync(LocalizationKeys.ValidationFailed, "Form has errors. Please fix them."),
                        Errors = validation.Data
                    };


                // 1️⃣ به‌روزرسانی کاربر
                var userUpdateDto = new CityDto
                {
                    Id = input.Id,
                    Name = input.StateTitle,
                    StateId = input.StateId,
                };

                var result = await UpdateAsync(userUpdateDto, input.Id);
                if (!result.Success)
                    return result;
                await _transactionService.CommitAsync();
                return result;
            }
            catch (Exception ex)
            {
                await _transactionService.RollbackAsync();
                var result = new ResultDto<CityDto>
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
            var createdCitys = new List<CityDto>();
            var errors = new List<string>();
            var (successMessage, errorMessage) = await _messageService.Value.GetSaveMessagesAsync();
            var errorFileTitle = await _messageService.Value.GetMessageAsync(LocalizationKeys.ErrorFile);
            try
            {
                var (dt, rowContexts) = excelStream.LoadExcelWithErrors();
                var Citys = dt.ToModelList<CityCrud>();

                for (int i = 0; i < Citys.Count; i++)
                {
                    var City = Citys[i];
                    var context = rowContexts[i];

                    var createResult = await CreateAsync(City);

                    if (createResult.Success && createResult.Data != null)
                    {
                        createdCitys.Add(createResult.Data);
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
                        InsertedCount = createdCitys.Count,
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
bool exportCurrentPage,
int pageNumber,
int pageSize)
        {
            // 1️⃣ گرفتن همه داده‌ها از query
            var query = await GetAllViews(); // IQueryable<Resource>

            // 2️⃣ Paging و Mapping به DTO
            List<CityCrud> data;

            if (exportCurrentPage)
            {
                data = query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();
            }
            else
            {
                data = query.ToList();
            }
            var resource = _mapper.Map<List<CityCrud>>(data);

            // 3️⃣ تولید Template اکسل با Lookup (مثلاً 5 ردیف خالی اضافه)
            var templateBytes = await _excelTemplateService.Value.GenerateTemplateWithLookupsAsync(
                LookupEntities.City, // نام مدل DTO
                data.Count + 5
            );

            // 4️⃣ پر کردن داده‌ها در Template با Extension Method
            var resultBytes = templateBytes.FillDataIntoTemplate(data, startRow: 3);

            return resultBytes;
        }
        /// <summary>
        /// دریافت لیست شهرهای مربوط به یک استان
        /// </summary>
        public async Task<
            ResultDto<List<CityDto>>>
            GetCitiesByStateIdAsync(
                Guid stateId)
        {
            try
            {
                /*
                 * بررسی معتبر بودن شناسه استان
                 */
                if (stateId == Guid.Empty)
                {
                    return new ResultDto<List<CityDto>>
                    {
                        Success = false,

                        Message =
                            "شناسه استان معتبر نیست.",

                        Data =
                            new List<CityDto>()
                    };
                }

                /*
                 * دریافت شهرهای فعال مربوط به استان
                 *
                 * GetAllViews خروجی IQueryable<CityCrud>
                 * برمی‌گرداند.
                 */
                var query =
                    await GetAllViews();

                /*
                 * فیلتر شهرها بر اساس شناسه استان
                 *
                 * در CityCrud طبق کد فعلی شما
                 * نام فیلد StateId است.
                 */
                var cities =
                    query
                        .Where(
                            x =>
                                x.StateId == stateId
                        )
                        .OrderBy(
                            x => x.StateTitle
                        )
                        .Select(
                            x => new CityDto
                            {
                                Id =
                                    x.Id,

                                Name =
                                    x.CityTitle,

                                StateId =
                                    x.StateId
                            }
                        )
                        .ToList();

                return new ResultDto<List<CityDto>>
                {
                    Success = true,

                    Message =
                        "لیست شهرها با موفقیت دریافت شد.",

                    Data =
                        cities
                };
            }
            catch (Exception ex)
            {
                return new ResultDto<List<CityDto>>
                {
                    Success = false,

                    Message =
                        "خطا در دریافت لیست شهرها.",

                    Data =
                        new List<CityDto>(),

                    Errors =
                        new List<string>
                        {
                    ex.Message
                        }
                };
            }
        }

    }

}
