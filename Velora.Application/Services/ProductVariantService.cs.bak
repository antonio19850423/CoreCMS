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
using Velora.Application.Shared.Repositories;
using Velora.Application.Shared.Services;
using Velora.Infrastructure.ORM.Interfaces.MyApp.Orm.Interfaces;

namespace Velora.Application.Services
{
    public class ProductVariantService : GenericService<SqlProductVariant, SqlProductVariant, ProductVariantDto>, IProductVariantService
    {
        private readonly ISqlRepository<SqlProductVariant> _sqlrepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ITransactionService _transactionService;
        private readonly IModelValidationService _modelValidationService;
        protected readonly Lazy<ILocalizationMessageService> _messageService;
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _config;
        private readonly Lazy<IExcelTemplateService> _excelTemplateService;
        private readonly IProductVariantService _roleProductVariantService;
        protected readonly ICurrentUserService _currentUserService;
        public ProductVariantService(
              ISqlRepository<SqlProductVariant> sqlRepository,
              IPosgreSqlRepository<SqlProductVariant> pgRepository,
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
        public async Task<IQueryable<ProductVariantCrud>> GetAllViews()
        {
            return await GetAllViewQueryable<SqlProductVariantView, SqlProductVariantView, ProductVariantCrud>();
        }

        public async Task<ResultDto<ProductVariantDto>> CreateAsync(ProductVariantCrud input)
        {
            var (successMessage, errorMessage) = await _messageService.Value.GetSaveMessagesAsync();
            try
            {
                var validation = await _modelValidationService.ValidateAsync(input);
                if (!validation.Success)
                    return new ResultDto<ProductVariantDto>
                    {
                        Success = false,
                        Message = await _messageService.Value.GetMessageAsync(LocalizationKeys.ValidationFailed, "Form has errors. Please fix them."),
                        Errors = validation.Data
                    };



                var ProductVariant = new ProductVariantDto
                {
                    SortOrder = input.SortOrder,
                    Name = input.Name,
                    Price = input.Price,
                    Barcode = input.Barcode,
                    Image = input.Image,
                    IsActive = input.IsActive,
                    ProductId=input.ParentId,
                    IsDefault = input.IsDefault,
                    Sku= input.Sku,
                    ComparePrice=input.ComparePrice
                };

                var ProductVariantResult = await CreateAsync(ProductVariant);
                if (!ProductVariantResult.Success)
                    return ProductVariantResult;
                await _transactionService.CommitAsync();
                return ProductVariantResult;
            }
            catch (Exception ex)
            {
                await _transactionService.RollbackAsync();
                var result = new ResultDto<ProductVariantDto>
                {
                    Success = false,
                    Message = errorMessage,
                };
                result.Errors.Add(ex.Message);
                return result;
            }
        }

        public async Task<ResultDto<ProductVariantDto>> UpdateAsync(ProductVariantCrud input)
        {
            var (successMessage, errorMessage) = await _messageService.Value.GetSaveMessagesAsync();
            try
            {
                if (input.Id == null)
                {
                    return new ResultDto<ProductVariantDto>
                    {
                        Success = false,
                        Message = await _messageService.Value.GetMessageAsync(LocalizationKeys.IdRequired)
                    };
                }
                var validation = await _modelValidationService.ValidateAsync(input);
                if (!validation.Success)
                    return new ResultDto<ProductVariantDto>
                    {
                        Success = false,
                        Message = await _messageService.Value.GetMessageAsync(LocalizationKeys.ValidationFailed, "Form has errors. Please fix them."),
                        Errors = validation.Data
                    };

                // 1️⃣ به‌روزرسانی کاربر
                var updateDto = new ProductVariantDto
                {
                    Id = input.Id,
                    SortOrder = input.SortOrder,
                    Name = input.Name,
                    Price = input.Price,
                    Barcode = input.Barcode,
                    Image = input.Image,
                    IsActive = input.IsActive,
                    ProductId = input.ParentId,
                    IsDefault = input.IsDefault,
                    Sku = input.Sku,
                    ComparePrice = input.ComparePrice
                };

                var ProductVariantResult = await UpdateAsync(updateDto, input.Id);
                if (!ProductVariantResult.Success)
                    return ProductVariantResult;

                await _transactionService.CommitAsync();
                return ProductVariantResult;
            }
            catch (Exception ex)
            {
                await _transactionService.RollbackAsync();
                var result = new ResultDto<ProductVariantDto>
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
            var createdProductVariants= new List<ProductVariantDto>();
            var errors = new List<string>();
            var (successMessage, errorMessage) = await _messageService.Value.GetSaveMessagesAsync();
            var errorFileTitle = await _messageService.Value.GetMessageAsync(LocalizationKeys.ErrorFile);
            try
            {
                var (dt, rowContexts) = excelStream.LoadExcelWithErrors();
                var ProductVariants = dt.ToModelList<ProductVariantCrud>();

                for (int i = 0; i < ProductVariants.Count; i++)
                {
                    var ProductVariant = ProductVariants[i];
                    var context = rowContexts[i];

                    var createResult = await CreateAsync(ProductVariant);

                    if (createResult.Success && createResult.Data != null)
                    {
                        createdProductVariants.Add(createResult.Data);
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
                        InsertedCount = createdProductVariants.Count,
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
bool exportCurrentProductVariant,
int ProductVariantNumber,
int ProductVariantSize)
        {
            // 1️⃣ گرفتن همه داده‌ها از query
            var query = await GetAllViews(); // IQueryable<Resource>

            // 2️⃣ Paging و Mapping به DTO
            List<ProductVariantCrud> data;

            if (exportCurrentProductVariant)
            {
                data = query
                    .Skip((ProductVariantNumber - 1) * ProductVariantSize)
                    .Take(ProductVariantSize)
                    .ToList();
            }
            else
            {
                data = query.ToList();
            }
            var resource = _mapper.Map<List<ProductVariantCrud>>(data);

            // 3️⃣ تولید Template اکسل با Lookup (مثلاً 5 ردیف خالی اضافه)
            var templateBytes = await _excelTemplateService.Value.GenerateTemplateWithLookupsAsync(
                LookupEntities.ProductVariant, // نام مدل DTO
                data.Count + 5
            );

            // 4️⃣ پر کردن داده‌ها در Template با Extension Method
            var resultBytes = templateBytes.FillDataIntoTemplate(data, startRow: 3);

            return resultBytes;
        }
    }

}
