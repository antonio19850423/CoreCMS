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
    public class UserAddressService : GenericService<SqlUserAddress, SqlUserAddress, UserAddressDto>, IUserAddressService
    {
        private readonly ISqlRepository<SqlUserAddress> _sqlrepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ITransactionService _transactionService;
        private readonly IModelValidationService _modelValidationService;
        protected readonly Lazy<ILocalizationMessageService> _messageService;
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _config;
        private readonly Lazy<IExcelTemplateService> _excelTemplateService;
        private readonly IUserAddressService _roleUserAddressService;
        protected readonly ICurrentUserService _currentUserService;
        public UserAddressService(
              ISqlRepository<SqlUserAddress> sqlRepository,
              IPosgreSqlRepository<SqlUserAddress> pgRepository,
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
        public async Task<IQueryable<UserAddressCrud>> GetAllViews()
        {
            return await GetAllViewQueryable<VwUserAddressForm, VwUserAddressForm, UserAddressCrud>();
        }


        public async Task<ResultDto<UserAddressDto>> CreateAsync(UserAddressCrud input)
        {
            var (successMessage, errorMessage) = await _messageService.Value.GetSaveMessagesAsync();
            try
            {
                var validation = await _modelValidationService.ValidateAsync(input);
                if (!validation.Success)
                    return new ResultDto<UserAddressDto>
                    {
                        Success = false,
                        Message = await _messageService.Value.GetMessageAsync(LocalizationKeys.ValidationFailed, "Form has errors. Please fix them."),
                        Errors = validation.Data
                    };
                var UserAddress = new UserAddressDto
                {
                    Address = input.Address,
                    CityId = input.CityId,
                    IsActive = input.IsActive,
                    IsDefault = input.IsDefault,
                    PhoneNumber = input.PhoneNumber,
                    PostalCode = input.PostalCode,
                    ProvinceId = input.ProvinceId,
                    Title = input.Title,
                    UserId = input.UserId,

                };

                var result = await CreateAsync(UserAddress);
                if (!result.Success)
                    return result;

                await _transactionService.CommitAsync();
                return result;
            }
            catch (Exception ex)
            {
                await _transactionService.RollbackAsync();
                var result = new ResultDto<UserAddressDto>
                {
                    Success = false,
                    Message = errorMessage,
                };
                result.Errors.Add(ex.Message);
                return result;
            }
        }

        public async Task<ResultDto<UserAddressDto>> UpdateAsync(UserAddressCrud input)
        {
            var (successMessage, errorMessage) = await _messageService.Value.GetSaveMessagesAsync();
            try
            {
                if (input.Id == null)
                {
                    return new ResultDto<UserAddressDto>
                    {
                        Success = false,
                        Message = await _messageService.Value.GetMessageAsync(LocalizationKeys.IdRequired)
                    };
                }
                var validation = await _modelValidationService.ValidateAsync(input);
                if (!validation.Success)
                    return new ResultDto<UserAddressDto>
                    {
                        Success = false,
                        Message = await _messageService.Value.GetMessageAsync(LocalizationKeys.ValidationFailed, "Form has errors. Please fix them."),
                        Errors = validation.Data
                    };


                // 1️⃣ به‌روزرسانی کاربر
                var userUpdateDto = new UserAddressDto
                {
                    Id = input.Id,
                    Address = input.Address,
                    CityId = input.CityId,
                    IsActive = input.IsActive,
                    IsDefault = input.IsDefault,
                    PhoneNumber = input.PhoneNumber,
                    PostalCode = input.PostalCode,  
                    ProvinceId = input.ProvinceId,
                    Title = input.Title,
                    UserId = input.UserId,
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
                var result = new ResultDto<UserAddressDto>
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
            var createdUserAddresss = new List<UserAddressDto>();
            var errors = new List<string>();
            var (successMessage, errorMessage) = await _messageService.Value.GetSaveMessagesAsync();
            var errorFileTitle = await _messageService.Value.GetMessageAsync(LocalizationKeys.ErrorFile);
            try
            {
                var (dt, rowContexts) = excelStream.LoadExcelWithErrors();
                var UserAddresss = dt.ToModelList<UserAddressCrud>();

                for (int i = 0; i < UserAddresss.Count; i++)
                {
                    var UserAddress = UserAddresss[i];
                    var context = rowContexts[i];

                    var createResult = await CreateAsync(UserAddress);

                    if (createResult.Success && createResult.Data != null)
                    {
                        createdUserAddresss.Add(createResult.Data);
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
                        InsertedCount = createdUserAddresss.Count,
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
            List<UserAddressCrud> data;

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
            var resource = _mapper.Map<List<UserAddressCrud>>(data);

            // 3️⃣ تولید Template اکسل با Lookup (مثلاً 5 ردیف خالی اضافه)
            var templateBytes = await _excelTemplateService.Value.GenerateTemplateWithLookupsAsync(
                LookupEntities.UserAddress, // نام مدل DTO
                data.Count + 5
            );

            // 4️⃣ پر کردن داده‌ها در Template با Extension Method
            var resultBytes = templateBytes.FillDataIntoTemplate(data, startRow: 3);

            return resultBytes;
        }

        public async Task<ResultDto<IEnumerable<UserAddressDto>>> GetUserAddressesAsync()
        {
            var userId = _currentUserService.GetUserId();
            return await GetByPredicateAsync<SqlUserAddress>(
                x =>
                    x.UserId == userId &&
                    !x.IsDeleted);
        }
        public async Task<ResultDto<IEnumerable<UserAddressDto>>> GetUserAddressesByUserIdAsync(
    Guid userId)
        {
            return await GetByPredicateAsync<SqlUserAddress>(
                x =>
                    x.UserId == userId &&
                    !x.IsDeleted);
        }
        public async Task<ResultDto<UserAddressDto?>> GetUserAddressByIdAsync(
    Guid addressId)
        {
            try
            {
                var userId = _currentUserService.GetUserId();
                if (userId == Guid.Empty)
                {
                    return new ResultDto<UserAddressDto?>
                    {
                        Success = false,
                        Message = "کاربر معتبر نیست."
                    };
                }

                if (addressId == Guid.Empty)
                {
                    return new ResultDto<UserAddressDto?>
                    {
                        Success = false,
                        Message = "شناسه آدرس معتبر نیست."
                    };
                }


                var result =
                    await FirstOrDefaultAsync<SqlUserAddress>(
                        x =>
                            x.Id == addressId &&
                            x.UserId == userId &&
                            !x.IsDeleted);


                if (!result.Success || result.Data == null)
                {
                    return new ResultDto<UserAddressDto?>
                    {
                        Success = false,
                        Message = "آدرس موردنظر یافت نشد."
                    };
                }


                return new ResultDto<UserAddressDto?>
                {
                    Success = true,

                    Message = "آدرس با موفقیت دریافت شد.",

                    Data = result.Data
                };
            }
            catch (Exception ex)
            {
                return new ResultDto<UserAddressDto?>
                {
                    Success = false,

                    Message = "خطا در دریافت آدرس.",

                    Errors = new List<string>
            {
                ex.Message
            }
                };
            }
        }
    }

}
