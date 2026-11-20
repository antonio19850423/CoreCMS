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
using Velora.Infrastructure.ORM.Interfaces.MyApp.Orm.Interfaces;

namespace Velora.Application.Services
{
    public class ProductTagService : GenericService<SqlProductTag, SqlProductTag, ProductTagDto>, IProductTagService
    {
        private readonly ISqlRepository<SqlProductTag> _sqlrepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ITransactionService _transactionService;
        private readonly IModelValidationService _modelValidationService;
        protected readonly Lazy<ILocalizationMessageService> _messageService;
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _config;
        private readonly Lazy<IExcelTemplateService> _excelTemplateService;
        private readonly IProductTagService _roleProductTagService;
        protected readonly ICurrentUserService _currentUserService;
        public ProductTagService(ISqlRepository<SqlProductTag> sqlRepository, IPosgreSqlRepository<SqlProductTag> pgRepository, IMapper mapper, IConfiguration configuration, ITransactionService transactionService, IWebHostEnvironment env, Lazy<ILocalizationMessageService> messageService, IModelValidationService modelValidationService, IConfiguration config, Lazy<IExcelTemplateService> excelTemplateService, ICurrentUserService currentUserService) : base(sqlRepository, pgRepository, mapper, configuration, messageService, currentUserService)
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

        public async Task<IQueryable<ProductTagCrud>> GetAllViews()
        {
            return await GetAllViewQueryable<SqlProductTagView, SqlProductTagView, ProductTagCrud>();
        }

        public async Task<ResultDto<ProductTagDto>> CreateAsync(ProductTagCrud input)
        {
            var(successMessage, errorMessage) = await _messageService.Value.GetSaveMessagesAsync();
            try
            {
                var validation = await _modelValidationService.ValidateAsync(input);
                if (!validation.Success)
                    return new ResultDto<ProductTagDto>
                    {
                        Success = false,
                        Message = await _messageService.Value.GetMessageAsync(LocalizationKeys.ValidationFailed, "Form has errors. Please fix them."),
                        Errors = validation.Data
                    };
                var ProductTag = new ProductTagDto
                {
                    Slug = input.Slug,
                    SortOrder = input.SortOrder.Value,
                    Color = input.Color,
                    Name = input.Name,
                    IsActive = input.IsActive.Value,
                    Description = input.Description,
                    TextColor = input.TextColor,
                };
                var result = await CreateAsync(ProductTag);
                if (!result.Success)
                    return result;
                await _transactionService.CommitAsync();
                return result;
            }
            catch (Exception ex)
            {
                await _transactionService.RollbackAsync();
                var result = new ResultDto<ProductTagDto>
                {
                    Success = false,
                    Message = errorMessage,
                };
                result.Errors.Add(ex.Message);
                return result;
            }
        }

        public async Task<ResultDto<ProductTagCrud>> CreateAdvancedAsync(ProductTagCrud input)
        {
            var(successMessage, errorMessage) = await _messageService.Value.GetSaveMessagesAsync();
            try
            {
                var errors = await QueryValidationHelper.ValidateUniquenessAsync(
       await GetAllQuery(),
       input.Id,
        (
         x => x.Slug.ToLower() == input.Slug.Trim().ToLower(),
         "این اسلاگ قبلاً ثبت شده است"
        ),
          (
         x => x.Name.ToLower() == input.Name.Trim().ToLower(),
         "این نام قبلاً ثبت شده است"
        )
   );
                if (errors.Any())
                {
                    return new ResultDto<ProductTagCrud>
                    {
                        Success = false,
                        Message = "اطلاعات آیتم ها تکراری است",
                        Errors = errors
                    };
                }
                var validation = await _modelValidationService.ValidateAsync(input);
                if (!validation.Success)
                    return new ResultDto<ProductTagCrud>
                    {
                        Success = false,
                        Message = await _messageService.Value.GetMessageAsync(LocalizationKeys.ValidationFailed, "Form has errors. Please fix them."),
                        Errors = validation.Data
                    };
                var productTag = new ProductTagDto
                {
                    Slug = input.Slug,
                    SortOrder = input.SortOrder.Value,
                    Color = input.Color,
                    Name = input.Name,
                    IsActive = input.IsActive.Value,
                    Description = input.Description,
                    TextColor = input.TextColor
                };
                if (productTag.SortOrder <= 0)
                {
                    productTag.SortOrder = (await Query().MaxAsync(x => (int?)x.SortOrder) ?? 0) + 1;
                }
                var result = await CreateWithViewAsync(productTag, GetProductTagById, x => x.Id);
                if (result.Success)
                    result.Message = successMessage;
                return result;
            }
            catch (Exception ex)
            {
                await _transactionService.RollbackAsync();
                var result = new ResultDto<ProductTagCrud>
                {
                    Success = false,
                    Message = errorMessage
                };
                result.Errors.Add(ex.Message);
                return result;
            }
        }

        public async Task<ResultDto<ProductTagCrud>> UpdateAsync(ProductTagCrud input)
        {
            var(successMessage, errorMessage) = await _messageService.Value.GetSaveMessagesAsync();
            try
            {
                var errors = await QueryValidationHelper.ValidateUniquenessAsync(
await GetAllQuery(),
input.Id,
(
x => x.Slug.ToLower() == input.Slug.Trim().ToLower(),
"این اسلاگ قبلاً ثبت شده است"
),
(
x => x.Name.ToLower() == input.Name.Trim().ToLower(),
"این نام قبلاً ثبت شده است"
)
);
                if (errors.Any())
                {
                    return new ResultDto<ProductTagCrud>
                    {
                        Success = false,
                        Message = "اطلاعات آیتم ها تکراری است",
                        Errors = errors
                    };
                }
                var validation = await _modelValidationService.ValidateAsync(input);
                if (!validation.Success)
                    return new ResultDto<ProductTagCrud>
                    {
                        Success = false,
                        Message = await _messageService.Value.GetMessageAsync(LocalizationKeys.ValidationFailed, "Form has errors. Please fix them."),
                        Errors = validation.Data
                    };
                if (input.Id == null)
                {
                    return new ResultDto<ProductTagCrud>
                    {
                        Success = false,
                        Message = await _messageService.Value.GetMessageAsync(LocalizationKeys.IdRequired)
                    };
                }
                // 1️⃣ به‌روزرسانی کاربر
                var userUpdateDto = new ProductTagDto
                {
                    Id = input.Id.Value,
                    Slug = input.Slug,
                    SortOrder = input.SortOrder.Value,
                    Color = input.Color,
                    Name = input.Name,
                    IsActive = input.IsActive.Value,
                    TextColor = input.TextColor,
                    Description = input.Description,
                };
                var result = await UpdateAsync(userUpdateDto, input.Id);
                if (!result.Success)
                {
                    return new ResultDto<ProductTagCrud>
                    {
                        Success = false,
                        Message = result.Message,
                        Errors = result.Errors
                    };
                }

                await _transactionService.CommitAsync();
                var viewResult = await GetProductTagById(input.Id.Value);
                if (viewResult == null)
                {
                    return new ResultDto<ProductTagCrud>
                    {
                        Success = false,
                        Message = "اطلاعات پس از ویرایش قابل دریافت نیست."
                    };
                }

                return new ResultDto<ProductTagCrud>
                {
                    Success = true,
                    Message = successMessage,
                    Data = viewResult
                };
            }
            catch (Exception ex)
            {
                await _transactionService.RollbackAsync();
                var result = new ResultDto<ProductTagCrud>
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
            var createdProductTags = new List<ProductTagDto>();
            var errors = new List<string>();
            var(successMessage, errorMessage) = await _messageService.Value.GetSaveMessagesAsync();
            var errorFileTitle = await _messageService.Value.GetMessageAsync(LocalizationKeys.ErrorFile);
            try
            {
                var(dt, rowContexts) = excelStream.LoadExcelWithErrors();
                var ProductTags = dt.ToModelList<ProductTagCrud>();
                for (int i = 0; i < ProductTags.Count; i++)
                {
                    var ProductTag = ProductTags[i];
                    var context = rowContexts[i];
                    var createResult = await CreateAsync(ProductTag);
                    if (createResult.Success && createResult.Data != null)
                    {
                        createdProductTags.Add(createResult.Data);
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
                        InsertedCount = createdProductTags.Count,
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
            List<ProductTagCrud> data;
            if (exportCurrentPage)
            {
                data = query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            }
            else
            {
                data = query.ToList();
            }

            var resource = _mapper.Map<List<ProductTagCrud>>(data);
            // 3️⃣ تولید Template اکسل با Lookup (مثلاً 5 ردیف خالی اضافه)
            var templateBytes = await _excelTemplateService.Value.GenerateTemplateWithLookupsAsync(LookupEntities.ProductTag, // نام مدل DTO
 data.Count + 5);
            // 4️⃣ پر کردن داده‌ها در Template با Extension Method
            var resultBytes = templateBytes.FillDataIntoTemplate(data, startRow: 3);
            return resultBytes;
        }

        public async Task<ProductTagCrud?> GetProductTagById(Guid id)
        {
            return await GetSqlViewByIdQueryable<SqlProductTagView, ProductTagCrud>(id);
        }
    }
}