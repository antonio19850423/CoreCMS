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
    public class CategoryAttributeService : GenericService<SqlCategoryAttribute, SqlCategoryAttribute, CategoryAttributeDto>, ICategoryAttributeService
    {
        private readonly ISqlRepository<SqlCategoryAttribute> _sqlrepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ITransactionService _transactionService;
        private readonly IModelValidationService _modelValidationService;
        protected readonly Lazy<ILocalizationMessageService> _messageService;
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _config;
        private readonly Lazy<IExcelTemplateService> _excelTemplateService;
        private readonly ICategoryAttributeService _roleCategoryAttributeService;
        protected readonly ICurrentUserService _currentUserService;
        public CategoryAttributeService(ISqlRepository<SqlCategoryAttribute> sqlRepository, IPosgreSqlRepository<SqlCategoryAttribute> pgRepository, IMapper mapper, IConfiguration configuration, ITransactionService transactionService, IWebHostEnvironment env, Lazy<ILocalizationMessageService> messageService, IModelValidationService modelValidationService, IConfiguration config, Lazy<IExcelTemplateService> excelTemplateService, ICurrentUserService currentUserService) : base(sqlRepository, pgRepository, mapper, configuration, messageService, currentUserService)
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

        public async Task<IQueryable<CategoryAttributeCrud>> GetAllViews()
        {
            return await GetAllViewQueryable<SqlCategoryAttributeView, SqlCategoryAttributeView, CategoryAttributeCrud>();
        }

        public async Task<ResultDto<CategoryAttributeDto>> CreateAsync(CategoryAttributeCrud input)
        {
            var(successMessage, errorMessage) = await _messageService.Value.GetSaveMessagesAsync();
            try
            {
                var validation = await _modelValidationService.ValidateAsync(input);
                if (!validation.Success)
                    return new ResultDto<CategoryAttributeDto>
                    {
                        Success = false,
                        Message = await _messageService.Value.GetMessageAsync(LocalizationKeys.ValidationFailed, "Form has errors. Please fix them."),
                        Errors = validation.Data
                    };
                var CategoryAttribute = new CategoryAttributeDto
                {
                    AttributeId = input.AttributeId.Value,
                    CategoryId = input.CategoryId.Value,
                    SortOrder = input.SortOrder,
                };
                var result = await CreateAsync(CategoryAttribute);
                if (!result.Success)
                    return result;
                await _transactionService.CommitAsync();
                return result;
            }
            catch (Exception ex)
            {
                await _transactionService.RollbackAsync();
                var result = new ResultDto<CategoryAttributeDto>
                {
                    Success = false,
                    Message = errorMessage,
                };
                result.Errors.Add(ex.Message);
                return result;
            }
        }

        public async Task<ResultDto<CategoryAttributeCrud>> CreateAdvancedAsync(CategoryAttributeCrud input)
        {
            var(successMessage, errorMessage) = await _messageService.Value.GetSaveMessagesAsync();
            try
            {
                var validation = await _modelValidationService.ValidateAsync(input);
                if (!validation.Success)
                    return new ResultDto<CategoryAttributeCrud>
                    {
                        Success = false,
                        Message = await _messageService.Value.GetMessageAsync(LocalizationKeys.ValidationFailed, "Form has errors. Please fix them."),
                        Errors = validation.Data
                    };
                var errors = await QueryValidationHelper.ValidateUniquenessAsync(
                      await GetAllQuery(),
                      input.Id,
                      (
                          x => x.CategoryId == input.CategoryId &&
                               x.AttributeId == input.AttributeId,
                          "این ویژگی و دسته‌بندی قبلاً ثبت شده است"
                      )
                  );

                if (errors.Any())
                {
                    return new ResultDto<CategoryAttributeCrud>
                    {
                        Success = false,
                        Message = "اطلاعات آیتم‌ها تکراری است",
                        Errors = errors
                    };
                }
                var categoryAttribute = new CategoryAttributeDto
                {
                    AttributeId = input.AttributeId.Value,
                    CategoryId = input.CategoryId.Value,
                    SortOrder = input.SortOrder
                };
                if (categoryAttribute.SortOrder <= 0)
                {
                    categoryAttribute.SortOrder = (await Query().MaxAsync(x => (int?)x.SortOrder) ?? 0) + 1;
                }
                var result = await CreateWithViewAsync(categoryAttribute, GetCategoryAttributeById, x => x.Id);
                if (result.Success)
                    result.Message = successMessage;
                return result;
            }
            catch (Exception ex)
            {
                await _transactionService.RollbackAsync();
                var result = new ResultDto<CategoryAttributeCrud>
                {
                    Success = false,
                    Message = errorMessage
                };
                result.Errors.Add(ex.Message);
                return result;
            }
        }

        public async Task<ResultDto<CategoryAttributeCrud>> UpdateAsync(CategoryAttributeCrud input)
        {
            var(successMessage, errorMessage) = await _messageService.Value.GetSaveMessagesAsync();
            try
            {
                if (input.Id == null)
                {
                    return new ResultDto<CategoryAttributeCrud>
                    {
                        Success = false,
                        Message = await _messageService.Value.GetMessageAsync(LocalizationKeys.IdRequired)
                    };
                }
                var validation = await _modelValidationService.ValidateAsync(input);
                if (!validation.Success)
                    return new ResultDto<CategoryAttributeCrud>
                    {
                        Success = false,
                        Message = await _messageService.Value.GetMessageAsync(LocalizationKeys.ValidationFailed, "Form has errors. Please fix them."),
                        Errors = validation.Data
                    };
                var errors = await QueryValidationHelper.ValidateUniquenessAsync(
                      await GetAllQuery(),
                      input.Id,
                      (
                          x => x.CategoryId == input.CategoryId &&
                               x.AttributeId == input.AttributeId,
                          "این ویژگی و دسته‌بندی قبلاً ثبت شده است"
                      )
                  );

                if (errors.Any())
                {
                    return new ResultDto<CategoryAttributeCrud>
                    {
                        Success = false,
                        Message = "اطلاعات آیتم‌ها تکراری است",
                        Errors = errors
                    };
                }
                // 1️⃣ به‌روزرسانی کاربر
                var userUpdateDto = new CategoryAttributeDto
                {
                    Id = input.Id,
                    AttributeId = input.AttributeId.Value,
                    CategoryId = input.CategoryId.Value,
                    SortOrder = input.SortOrder,
                };
                var result = await UpdateAsync(userUpdateDto, input.Id);
                if (!result.Success)
                {
                    return new ResultDto<CategoryAttributeCrud>
                    {
                        Success = false,
                        Message = result.Message,
                        Errors = result.Errors
                    };
                }

                await _transactionService.CommitAsync();
                var viewResult = await GetCategoryAttributeById(input.Id);
                if (viewResult == null)
                {
                    return new ResultDto<CategoryAttributeCrud>
                    {
                        Success = false,
                        Message = "اطلاعات پس از ویرایش قابل دریافت نیست."
                    };
                }

                return new ResultDto<CategoryAttributeCrud>
                {
                    Success = true,
                    Message = successMessage,
                    Data = viewResult
                };
            }
            catch (Exception ex)
            {
                await _transactionService.RollbackAsync();
                var result = new ResultDto<CategoryAttributeCrud>
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
            var createdCategoryAttributes = new List<CategoryAttributeDto>();
            var errors = new List<string>();
            var(successMessage, errorMessage) = await _messageService.Value.GetSaveMessagesAsync();
            var errorFileTitle = await _messageService.Value.GetMessageAsync(LocalizationKeys.ErrorFile);
            try
            {
                var(dt, rowContexts) = excelStream.LoadExcelWithErrors();
                var CategoryAttributes = dt.ToModelList<CategoryAttributeCrud>();
                for (int i = 0; i < CategoryAttributes.Count; i++)
                {
                    var CategoryAttribute = CategoryAttributes[i];
                    var context = rowContexts[i];
                    var createResult = await CreateAsync(CategoryAttribute);
                    if (createResult.Success && createResult.Data != null)
                    {
                        createdCategoryAttributes.Add(createResult.Data);
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
                        InsertedCount = createdCategoryAttributes.Count,
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
            List<CategoryAttributeCrud> data;
            if (exportCurrentPage)
            {
                data = query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            }
            else
            {
                data = query.ToList();
            }

            var resource = _mapper.Map<List<CategoryAttributeCrud>>(data);
            // 3️⃣ تولید Template اکسل با Lookup (مثلاً 5 ردیف خالی اضافه)
            var templateBytes = await _excelTemplateService.Value.GenerateTemplateWithLookupsAsync(LookupEntities.CategoryAttribute, // نام مدل DTO
 data.Count + 5);
            // 4️⃣ پر کردن داده‌ها در Template با Extension Method
            var resultBytes = templateBytes.FillDataIntoTemplate(data, startRow: 3);
            return resultBytes;
        }

        public async Task<CategoryAttributeCrud?> GetCategoryAttributeById(Guid id)
        {
            return await GetSqlViewByIdQueryable<SqlCategoryAttributeView, CategoryAttributeCrud>(id);
        }
    }
}