using AutoMapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Velora.Application.Shared.Constants;
using Velora.Application.Shared.Dtos;
using Velora.Application.Shared.Enums;
using Velora.Application.Shared.Extensions;
using Velora.Application.Shared.Repositories;
using Velora.Application.Shared.Services;
using Velora.Infrastructure.ORM.Interfaces.MyApp.Orm.Interfaces;
using Velora.EntityFrameworkCore.EntityFramework.SqlServer;

namespace Velora.Application.Services
{
    public class ProductCategoryService : GenericService<SqlProductCategory, SqlProductCategory, ProductCategoryDto>, IProductCategoryService
    {
        private readonly ISqlRepository<SqlProductCategory> _sqlrepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ITransactionService _transactionService;
        private readonly IModelValidationService _modelValidationService;
        protected readonly Lazy<ILocalizationMessageService> _messageService;
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _config;
        private readonly Lazy<IExcelTemplateService> _excelTemplateService;
        private readonly IProductCategoryService _roleProductCategoryService;
        protected readonly ICurrentUserService _currentUserService;
        public ProductCategoryService(
              ISqlRepository<SqlProductCategory> sqlRepository,
              IPosgreSqlRepository<SqlProductCategory> pgRepository,
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
            _currentUserService= currentUserService;
        }
        public async Task<IQueryable<ProductCategoryCrud>> GetAllViews()
        {
            return await GetAllViewQueryable<VwProductCategoryForm, VwProductCategoryForm, ProductCategoryCrud>();
        }
        public async Task<ResultDto<ProductCategoryDto>> CreateAsync(ProductCategoryCrud input)
        {
            var (successMessage, errorMessage) = await _messageService.Value.GetSaveMessagesAsync();
            try
            {
                var validation = await _modelValidationService.ValidateAsync(input);
                if (!validation.Success)
                    return new ResultDto<ProductCategoryDto>
                    {
                        Success = false,
                        Message = await _messageService.Value.GetMessageAsync(LocalizationKeys.ValidationFailed, "Form has errors. Please fix them."),
                        Errors = validation.Data
                    };
                var ProductCategory = new ProductCategoryDto
                {
                 Icon = input.Icon,
                 IconColor = input.IconColor,
                 IsActive = input.IsActive,
                 Description = input.Description,
                 Name = input.Name,
                 Slug = input.Slug,
                 ParentId = input.ParentId,
                 SortOrder = input.SortOrder,
                 SeoDescription = input.SeoDescription,
                 SeoTitle = input.SeoTitle,
                };

                var result = await CreateAsync(ProductCategory);
                if (!result.Success)
                    return result;

                await _transactionService.CommitAsync();
                return result;
            }
            catch (Exception ex)
            {
                await _transactionService.RollbackAsync();
                var result = new ResultDto<ProductCategoryDto>
                {
                    Success = false,
                    Message = errorMessage,
                };
                result.Errors.Add(ex.Message);
                return result;
            }
        }

        public async Task<ResultDto<ProductCategoryDto>> UpdateAsync(ProductCategoryCrud input)
        {
            var (successMessage, errorMessage) = await _messageService.Value.GetSaveMessagesAsync();
            try
            {
                if (input.Id == null)
                {
                    return new ResultDto<ProductCategoryDto>
                    {
                        Success = false,
                        Message = await _messageService.Value.GetMessageAsync(LocalizationKeys.IdRequired)
                    };
                }
                var validation = await _modelValidationService.ValidateAsync(input);
                if (!validation.Success)
                    return new ResultDto<ProductCategoryDto>
                    {
                        Success = false,
                        Message = await _messageService.Value.GetMessageAsync(LocalizationKeys.ValidationFailed, "Form has errors. Please fix them."),
                        Errors = validation.Data
                    };

                // 1️⃣ به‌روزرسانی کاربر
                var userUpdateDto = new ProductCategoryDto
                {
                    Id = input.Id,
                    Icon = input.Icon,
                    IconColor = input.IconColor,
                    IsActive = input.IsActive,
                    Description = input.Description,
                    Name = input.Name,
                    Slug = input.Slug,
                    ParentId = input.ParentId,
                    SortOrder = input.SortOrder,
                    SeoTitle = input.SeoTitle,
                    SeoDescription = input.SeoDescription,
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
                var result = new ResultDto<ProductCategoryDto>
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
            var createdProductCategorys= new List<ProductCategoryDto>();
            var errors = new List<string>();
            var (successMessage, errorMessage) = await _messageService.Value.GetSaveMessagesAsync();
            var errorFileTitle = await _messageService.Value.GetMessageAsync(LocalizationKeys.ErrorFile);
            try
            {
                var (dt, rowContexts) = excelStream.LoadExcelWithErrors();
                var ProductCategorys = dt.ToModelList<ProductCategoryCrud>();

                for (int i = 0; i < ProductCategorys.Count; i++)
                {
                    var ProductCategory = ProductCategorys[i];
                    var context = rowContexts[i];

                    var createResult = await CreateAsync(ProductCategory);

                    if (createResult.Success && createResult.Data != null)
                    {
                        createdProductCategorys.Add(createResult.Data);
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
                        InsertedCount = createdProductCategorys.Count,
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
bool exportCurrentProductCategory,
int ProductCategoryNumber,
int ProductCategorySize)
        {
            // 1️⃣ گرفتن همه داده‌ها از query
            var query = await GetAllViews(); // IQueryable<Resource>

            // 2️⃣ Paging و Mapping به DTO
            List<ProductCategoryCrud> data;

            if (exportCurrentProductCategory)
            {
                data = query
                    .Skip((ProductCategoryNumber - 1) * ProductCategorySize)
                    .Take(ProductCategorySize)
                    .ToList();
            }
            else
            {
                data = query.ToList();
            }
            var resource = _mapper.Map<List<ProductCategoryCrud>>(data);

            // 3️⃣ تولید Template اکسل با Lookup (مثلاً 5 ردیف خالی اضافه)
            var templateBytes = await _excelTemplateService.Value.GenerateTemplateWithLookupsAsync(
                LookupEntities.ProductCategory, // نام مدل DTO
                data.Count + 5
            );

            // 4️⃣ پر کردن داده‌ها در Template با Extension Method
            var resultBytes = templateBytes.FillDataIntoTemplate(data, startRow: 3);

            return resultBytes;
        }

        public async Task<List<ProductCategoryTreeDto>> GetProductCategoryTreeAsync()
        {

            var query = await GetAllViews();


            var categories =
                 query
                .Where(x => x.IsActive)
                .OrderBy(x => x.SortOrder)
                .ToList();



            var tree =
                BuildTree(categories, null);


            return tree;

        }

        private List<ProductCategoryTreeDto> BuildTree(
    List<ProductCategoryCrud> categories,
    Guid? parentId)
        {

            return categories
                .Where(x => x.ParentId == parentId)
                .Select(x => new ProductCategoryTreeDto
                {

                    Id = x.Id,

                    Name = x.Name,

                    Slug = x.Slug,

                    ParentId = x.ParentId,


                    Children =
                        BuildTree(
                            categories,
                            x.Id
                        )

                })
                .ToList();

        }


    }

}
