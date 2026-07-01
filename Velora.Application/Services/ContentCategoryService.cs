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
    public class ContentCategoryService : GenericService<SqlContentCategory, SqlContentCategory, ContentCategoryDto>, IContentCategoryService
    {
        private readonly ISqlRepository<SqlContentCategory> _sqlrepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ITransactionService _transactionService;
        private readonly IModelValidationService _modelValidationService;
        protected readonly Lazy<ILocalizationMessageService> _messageService;
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _config;
        private readonly Lazy<IExcelTemplateService> _excelTemplateService;
        private readonly IContentCategoryService _roleContentCategoryService;
        protected readonly ICurrentUserService _currentUserService;
        public ContentCategoryService(
              ISqlRepository<SqlContentCategory> sqlRepository,
              IPosgreSqlRepository<SqlContentCategory> pgRepository,
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
        public async Task<IQueryable<ContentCategoryCrud>> GetAllViews()
        {
            return await GetAllViewQueryable<VwContentCategoryForm, VwContentCategoryForm, ContentCategoryCrud>();
        }
        public async Task<ResultDto<ContentCategoryDto>> CreateAsync(ContentCategoryCrud input)
        {
            var (successMessage, errorMessage) = await _messageService.Value.GetSaveMessagesAsync();
            try
            {

                var ContentCategory = new ContentCategoryDto
                {
                 Icon = input.Icon,
                 IconColor = input.IconColor,
                 IsActive = input.IsActive,
                 Description = input.Description,
                 Name = input.Name,
                 Slug = input.Slug,
                 ParentId = input.ParentId,
                 SortOrder = input.SortOrder,
                };

                var result = await CreateAsync(ContentCategory);
                if (!result.Success)
                    return result;

                await _transactionService.CommitAsync();
                return result;
            }
            catch (Exception ex)
            {
                await _transactionService.RollbackAsync();
                var result = new ResultDto<ContentCategoryDto>
                {
                    Success = false,
                    Message = errorMessage,
                };
                result.Errors.Add(ex.Message);
                return result;
            }
        }

        public async Task<ResultDto<ContentCategoryDto>> UpdateAsync(ContentCategoryCrud input)
        {
            var (successMessage, errorMessage) = await _messageService.Value.GetSaveMessagesAsync();
            try
            {
                if (input.Id == null)
                {
                    return new ResultDto<ContentCategoryDto>
                    {
                        Success = false,
                        Message = "Id is required"
                    };
                }

                // 1️⃣ به‌روزرسانی کاربر
                var userUpdateDto = new ContentCategoryDto
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
                var result = new ResultDto<ContentCategoryDto>
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
            var createdContentCategorys= new List<ContentCategoryDto>();
            var errors = new List<string>();
            var (successMessage, errorMessage) = await _messageService.Value.GetSaveMessagesAsync();
            var errorFileTitle = await _messageService.Value.GetMessageAsync(LocalizationKeys.ErrorFile);
            try
            {
                var (dt, rowContexts) = excelStream.LoadExcelWithErrors();
                var ContentCategorys = dt.ToModelList<ContentCategoryCrud>();

                for (int i = 0; i < ContentCategorys.Count; i++)
                {
                    var ContentCategory = ContentCategorys[i];
                    var context = rowContexts[i];

                    var createResult = await CreateAsync(ContentCategory);

                    if (createResult.Success && createResult.Data != null)
                    {
                        createdContentCategorys.Add(createResult.Data);
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
                        InsertedCount = createdContentCategorys.Count,
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
bool exportCurrentContentCategory,
int ContentCategoryNumber,
int ContentCategorySize)
        {
            // 1️⃣ گرفتن همه داده‌ها از query
            var query = await GetAllViews(); // IQueryable<Resource>

            // 2️⃣ Paging و Mapping به DTO
            List<ContentCategoryCrud> data;

            if (exportCurrentContentCategory)
            {
                data = query
                    .Skip((ContentCategoryNumber - 1) * ContentCategorySize)
                    .Take(ContentCategorySize)
                    .ToList();
            }
            else
            {
                data = query.ToList();
            }
            var resource = _mapper.Map<List<ContentCategoryCrud>>(data);

            // 3️⃣ تولید Template اکسل با Lookup (مثلاً 5 ردیف خالی اضافه)
            var templateBytes = await _excelTemplateService.Value.GenerateTemplateWithLookupsAsync(
                LookupEntities.ContentCategory, // نام مدل DTO
                data.Count + 5
            );

            // 4️⃣ پر کردن داده‌ها در Template با Extension Method
            var resultBytes = templateBytes.FillDataIntoTemplate(data, startRow: 3);

            return resultBytes;
        }


    }

}
