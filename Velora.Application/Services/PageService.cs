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
using Microsoft.EntityFrameworkCore;

namespace Velora.Application.Services
{
    public class PageService : GenericService<SqlPage, SqlPage, PageDto>, IPageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ITransactionService _transactionService;
        private readonly IModelValidationService _modelValidationService;
        protected readonly Lazy<ILocalizationMessageService> _messageService;
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _config;
        private readonly Lazy<IExcelTemplateService> _excelTemplateService;
        private readonly IPageService _rolePageService;
        protected readonly ICurrentUserService _currentUserService;
        public PageService(
              ISqlRepository<SqlPage> sqlRepository,
              IPosgreSqlRepository<SqlPage> pgRepository,
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
        public async Task<IQueryable<PageCrud>> GetAllViews()
        {
            return await GetAllViewQueryable<SqlPageView, SqlPageView, PageCrud>();
        }
        public async Task<ResultDto<PageDto>> CreateAsync(PageCrud input)
        {
            var (successMessage, errorMessage) = await _messageService.Value.GetSaveMessagesAsync();
            try
            {

                var Page = new PageDto
                {
                    CanonicalUrl = input.CanonicalUrl,
                    IsHome=input.IsHome,
                    IsPublished=input.IsPublished,
                    MetaDescription = input.MetaDescription,
                    MetaKeywords = input.MetaKeywords,
                    MetaTitle = input.MetaTitle,
                    OgImageUrl= input.OgImageUrl,
                    PageTemplateId= input.PageTemplateId,
                    Slug= input.Slug,
                    IsActive = input.IsActive,
                    Name = input.Name,
                };

                var result = await CreateAsync(Page);
                if (!result.Success)
                    return result;

                await _transactionService.CommitAsync();
                return result;
            }
            catch (Exception ex)
            {
                await _transactionService.RollbackAsync();
                var result = new ResultDto<PageDto>
                {
                    Success = false,
                    Message = errorMessage,
                };
                result.Errors.Add(ex.Message);
                return result;
            }
        }

        public async Task<ResultDto<PageDto>> UpdateAsync(PageCrud input)
        {
            var (successMessage, errorMessage) = await _messageService.Value.GetSaveMessagesAsync();
            try
            {
                if (input.Id == null)
                {
                    return new ResultDto<PageDto>
                    {
                        Success = false,
                        Message = "Id is required"
                    };
                }

                // 1️⃣ به‌روزرسانی کاربر
                var userUpdateDto = new PageDto
                {
                    Id = input.Id,
                    Slug = input.Slug,
                    IsActive = input.IsActive,
                    Name = input.Name,
                    PageTemplateId = input.PageTemplateId,
                    OgImageUrl = input.OgImageUrl,
                    MetaTitle = input.MetaTitle,
                    MetaKeywords = input.MetaKeywords,
                    MetaDescription = input.MetaDescription,
                    IsPublished = input.IsPublished,
                    CanonicalUrl = input.CanonicalUrl,
                    IsHome = input.IsHome,
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
                var result = new ResultDto<PageDto>
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
            var createdPages= new List<PageDto>();
            var errors = new List<string>();
            var (successMessage, errorMessage) = await _messageService.Value.GetSaveMessagesAsync();
            var errorFileTitle = await _messageService.Value.GetMessageAsync(LocalizationKeys.ErrorFile);
            try
            {
                var (dt, rowContexts) = excelStream.LoadExcelWithErrors();
                var Pages = dt.ToModelList<PageCrud>();

                for (int i = 0; i < Pages.Count; i++)
                {
                    var Page = Pages[i];
                    var context = rowContexts[i];

                    var createResult = await CreateAsync(Page);

                    if (createResult.Success && createResult.Data != null)
                    {
                        createdPages.Add(createResult.Data);
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
                        InsertedCount = createdPages.Count,
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
            List<PageCrud> data;

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
            var resource = _mapper.Map<List<PageCrud>>(data);

            // 3️⃣ تولید Template اکسل با Lookup (مثلاً 5 ردیف خالی اضافه)
            var templateBytes = await _excelTemplateService.Value.GenerateTemplateWithLookupsAsync(
                LookupEntities.Page, // نام مدل DTO
                data.Count + 5
            );

            // 4️⃣ پر کردن داده‌ها در Template با Extension Method
            var resultBytes = templateBytes.FillDataIntoTemplate(data, startRow: 3);

            return resultBytes;
        }

        public async Task<ResultDto<PageViewDto>> GetPageAsync(string slug)
        {
            var result = new ResultDto<PageViewDto>();
            try
            {
                // واکشی صفحه + Sections + SectionItems
                var pageEntity = await Query()
     .Include(p => p.Sections)
         .ThenInclude(s => s.ComponentType)
     .Include(p => p.Sections)
         .ThenInclude(s => s.SectionItems)
     .FirstOrDefaultAsync(p => p.Slug == slug);

                if (pageEntity == null)
                {
                    result.Success = false;
                    result.Message = $"Page with slug '{slug}' not found.";
                    return result;
                }

                // map مستقیم به PageViewDto
                var pageViewDto = new PageViewDto
                {
                    Id = pageEntity.Id,
                    Name = pageEntity.Name,
                    Slug = pageEntity.Slug,
                    PageTemplateId = pageEntity.PageTemplateId,
                    IsHome = pageEntity.IsHome,
                    IsPublished = pageEntity.IsPublished,
                    MetaTitle = pageEntity.MetaTitle,
                    MetaDescription = pageEntity.MetaDescription,
                    MetaKeywords = pageEntity.MetaKeywords,
                    CanonicalUrl = pageEntity.CanonicalUrl,
                    OgImageUrl = pageEntity.OgImageUrl,
                    IsActive = pageEntity.IsActive,

                    Sections = pageEntity.Sections.OrderBy(c=>c.SortOrder).Select(s => new SectionViewDto
                    {
                        Id = s.Id,
                        ParentId = pageEntity.Id,
                        ComponentTypeId = s.ComponentTypeId,
                        ComponentTypeName=s.ComponentType.Name,
                        Title = s.Title,
                        Subtitle = s.Subtitle,
                        Description = s.Description,
                        ImageUrl = s.ImageUrl,
                        ColumnsCount = s.ColumnsCount,
                        SortOrder = s.SortOrder,
                        IsActive = s.IsActive,
                        BackgroundColor = s.BackgroundColor,
                        HeaderColor = s.HeaderColor,
                        SubtitleColor = s.SubtitleColor,
                        DescriptionColor = s.DescriptionColor,
                        Icon = s.Icon,
                        IconColor = s.IconColor,
                        IconAlt = s.IconAlt,
                        ImageAlt = s.ImageAlt,
                        Link1Text = s.Link1Text,
                        Link1Url = s.Link1Url,
                        Link1Color = s.Link1Color,
                        Link2Text = s.Link2Text,
                        Link2Url = s.Link2Url,
                        Link2Color = s.Link2Color,
                        Link3Text = s.Link3Text,
                        Link3Url = s.Link3Url,
                        Link3Color = s.Link3Color,
                        Link4Text = s.Link4Text,
                        Link4Url = s.Link4Url,
                        Link4Color = s.Link4Color,

                        Items = s.SectionItems.OrderBy(c => c.SortOrder).Select(si => new SectionItemCrud
                        {
                            Id = si.Id,
                            ParentId = si.SectionId,
                            Title = si.Title,
                            Subtitle = si.Subtitle,
                            Description = si.Description,
                            Price = si.Price,
                            ImageUrl = si.ImageUrl,
                            AvatarUrl = si.AvatarUrl,
                            SortOrder = si.SortOrder,
                            IsActive = si.IsActive,
                            BackgroundColor = si.BackgroundColor,
                            SubtitleColor = si.SubtitleColor,
                            DescriptionColor = si.DescriptionColor,
                            Link1Text = si.Link1Text,
                            Link1Url = si.Link1Url,
                            Link1Color = si.Link1Color,
                            Link2Text = si.Link2Text,
                            Link2Url = si.Link2Url,
                            Link2Color = si.Link2Color,
                            Link3Text = si.Link3Text,
                            Link3Url = si.Link3Url,
                            Link3Color = si.Link3Color,
                            Link4Text = si.Link4Text,
                            Link4Url = si.Link4Url,
                            Link4Color = si.Link4Color,
                            Icon = si.Icon,
                            IconColor = si.IconColor,
                            IconAlt = si.IconAlt,
                            ImageAlt = si.ImageAlt,
                            TitleColor = si.TitleColor,
                            AvatarAlt = si.AvatarAlt
                        }).ToList()
                    }).ToList()
                };

                result.Data = pageViewDto;
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Failed to load page.";
                result.Errors.Add(ex.Message);
            }

            return result;
        }


    }

}
