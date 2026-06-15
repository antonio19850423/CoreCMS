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

namespace Velora.Application.Services
{
    public class ContentItemService : GenericService<SqlContentItem, SqlContentItem, ContentItemDto>, IContentItemService
    {
        private readonly ISqlRepository<SqlContentItem> _sqlrepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ITransactionService _transactionService;
        private readonly IModelValidationService _modelValidationService;
        protected readonly Lazy<ILocalizationMessageService> _messageService;
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _config;
        private readonly Lazy<IExcelTemplateService> _excelTemplateService;
        private readonly IContentItemService _roleContentItemService;
        protected readonly ICurrentUserService _currentUserService;
        public ContentItemService(
              ISqlRepository<SqlContentItem> sqlRepository,
              IPosgreSqlRepository<SqlContentItem> pgRepository,
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
        public async Task<IQueryable<ContentItemCrud>> GetAllViews()
        {
            return await GetAllViewQueryable<SqlContentItemView, SqlContentItemView, ContentItemCrud>();
        }
        public async Task<ResultDto<ContentItemDto>> CreateAsync(ContentItemCrud input)
        {
            var (successMessage, errorMessage) = await _messageService.Value.GetSaveMessagesAsync();
            try
            {

                var ContentItem = new ContentItemDto
                {
                   Title = input.Title,
                   Content = input.Content,
                   Tags = input.Tags,
                   Summary = input.Summary,
                   PublishedAt = input.PublishedAt,
                   PageId = input.PageId,
                   ImageUrl = input.ImageUrl,
                   ImageAlt = input.ImageAlt,
                   ExternalUrl = input.ExternalUrl,
                   AuthorAvatarUrl = input.AuthorAvatarUrl,
                   AuthorName = input.AuthorName,
                   AuthorTitle = input.AuthorTitle,
                   CategoryId = input.CategoryId,
                   ContentType = input.ContentType,
                   IsActive = input.IsActive,
                   SortOrder = input.SortOrder,
                    
                };

                var result = await CreateAsync(ContentItem);
                if (!result.Success)
                    return result;

                await _transactionService.CommitAsync();
                return result;
            }
            catch (Exception ex)
            {
                await _transactionService.RollbackAsync();
                var result = new ResultDto<ContentItemDto>
                {
                    Success = false,
                    Message = errorMessage,
                };
                result.Errors.Add(ex.Message);
                return result;
            }
        }

        public async Task<ResultDto<ContentItemDto>> UpdateAsync(ContentItemCrud input)
        {
            var (successMessage, errorMessage) = await _messageService.Value.GetSaveMessagesAsync();
            try
            {
                if (input.Id == null)
                {
                    return new ResultDto<ContentItemDto>
                    {
                        Success = false,
                        Message = "Id is required"
                    };
                }

                // 1️⃣ به‌روزرسانی کاربر
                var userUpdateDto = new ContentItemDto
                {
                    Id = input.Id,
                    Title = input.Title,
                    Content = input.Content,
                    Tags = input.Tags,
                    Summary = input.Summary,
                    PublishedAt = input.PublishedAt,
                    PageId = input.PageId,
                    ImageUrl = input.ImageUrl,
                    ImageAlt = input.ImageAlt,
                    ExternalUrl = input.ExternalUrl,
                    AuthorAvatarUrl = input.AuthorAvatarUrl,
                    AuthorName = input.AuthorName,
                    AuthorTitle = input.AuthorTitle,
                    CategoryId = input.CategoryId,
                    ContentType = input.ContentType,
                    IsActive = input.IsActive,
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
                var result = new ResultDto<ContentItemDto>
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
            var createdContentItems= new List<ContentItemDto>();
            var errors = new List<string>();
            var (successMessage, errorMessage) = await _messageService.Value.GetSaveMessagesAsync();
            var errorFileTitle = await _messageService.Value.GetMessageAsync(LocalizationKeys.ErrorFile);
            try
            {
                var (dt, rowContexts) = excelStream.LoadExcelWithErrors();
                var ContentItems = dt.ToModelList<ContentItemCrud>();

                for (int i = 0; i < ContentItems.Count; i++)
                {
                    var ContentItem = ContentItems[i];
                    var context = rowContexts[i];

                    var createResult = await CreateAsync(ContentItem);

                    if (createResult.Success && createResult.Data != null)
                    {
                        createdContentItems.Add(createResult.Data);
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
                        InsertedCount = createdContentItems.Count,
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
bool exportCurrentContentItem,
int ContentItemNumber,
int ContentItemSize)
        {
            // 1️⃣ گرفتن همه داده‌ها از query
            var query = await GetAllViews(); // IQueryable<Resource>

            // 2️⃣ Paging و Mapping به DTO
            List<ContentItemCrud> data;

            if (exportCurrentContentItem)
            {
                data = query
                    .Skip((ContentItemNumber - 1) * ContentItemSize)
                    .Take(ContentItemSize)
                    .ToList();
            }
            else
            {
                data = query.ToList();
            }
            var resource = _mapper.Map<List<ContentItemCrud>>(data);

            // 3️⃣ تولید Template اکسل با Lookup (مثلاً 5 ردیف خالی اضافه)
            var templateBytes = await _excelTemplateService.Value.GenerateTemplateWithLookupsAsync(
                LookupEntities.ContentItem, // نام مدل DTO
                data.Count + 5
            );

            // 4️⃣ پر کردن داده‌ها در Template با Extension Method
            var resultBytes = templateBytes.FillDataIntoTemplate(data, startRow: 3);

            return resultBytes;
        }


    }

}
