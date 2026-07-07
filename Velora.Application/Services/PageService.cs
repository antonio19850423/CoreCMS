using AutoMapper;
using GreenDonut;
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
using Velora.EntityFrameworkCore.EntityFramework.SqlServer;
using Velora.Infrastructure.ORM.Interfaces.MyApp.Orm.Interfaces;

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
        protected readonly IContentItemService _contentItemService;
        public PageService(
              ISqlRepository<SqlPage> sqlRepository,
              IPosgreSqlRepository<SqlPage> pgRepository,
              IMapper mapper,
              IConfiguration configuration, ITransactionService transactionService, IWebHostEnvironment env,
              Lazy<ILocalizationMessageService> messageService, IModelValidationService modelValidationService, IConfiguration config, Lazy<IExcelTemplateService> excelTemplateService,
              ICurrentUserService currentUserService, IContentItemService contentItemService)
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
            _contentItemService = contentItemService;
        }
        public async Task<IQueryable<PageCrud>> GetAllViews()
        {
            var Result= await GetAllViewQueryable<SqlPageView, SqlPageView, PageCrud>();
            Result = Result.Where(c => c.IsActive == true);
            return Result;
        }
        public async Task<ResultDto<PageDto>> CreateAsync(PageCrud input)
        {
            var (successMessage, errorMessage) = await _messageService.Value.GetSaveMessagesAsync();
            try
            {
                var validation = await _modelValidationService.ValidateAsync(input);
                if (!validation.Success)
                    return new ResultDto<PageDto>
                    {
                        Success = false,
                        Message = await _messageService.Value.GetMessageAsync(LocalizationKeys.ValidationFailed, "Form has errors. Please fix them."),
                        Errors = validation.Data
                    };
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
                    IsDynamic=input.IsDynamic,
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
                        Message = await _messageService.Value.GetMessageAsync(LocalizationKeys.IdRequired)
                    };
                }
                var validation = await _modelValidationService.ValidateAsync(input);
                if (!validation.Success)
                    return new ResultDto<PageDto>
                    {
                        Success = false,
                        Message = await _messageService.Value.GetMessageAsync(LocalizationKeys.ValidationFailed, "Form has errors. Please fix them."),
                        Errors = validation.Data
                    };

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
     .Include(p => p.SectionPages)
         .ThenInclude(s => s.ComponentType)
     .Include(p => p.SectionPages)
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

                    Sections = pageEntity.SectionPages.OrderBy(c=>c.SortOrder).Select(s => new SectionViewDto
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
                        Features = s.Features,
                        ContactEmailLabel = s.ContactEmailLabel,
                        Link1OpenInNewTab = s.Link1OpenInNewTab,
                        Link2OpenInNewTab =s.Link2OpenInNewTab,
                        Link3OpenInNewTab =s.Link3OpenInNewTab,
                        Link4OpenInNewTab =s.Link4OpenInNewTab,
                        Link1TypeId = s.Link1TypeId,
                        Link2TypeId = s.Link2TypeId,
                        Link3TypeId = s.Link3TypeId,
                        Link4TypeId = s.Link4TypeId,
                        ContactFirstNameLabel = s.ContactFirstNameLabel,
                        ContactLastNameLabel = s.ContactLastNameLabel,
                        ContactMessageLabel = s.ContactMessageLabel,
                        ContactSubmitButtonText = s.ContactSubmitButtonText,
                        CopyrightText = s.CopyrightText,
                        ImageAlt2 = s.ImageAlt2,
                        ImageAlt3 = s.ImageAlt3,
                        ImageAlt4 = s.ImageAlt4,
                        ImageUrl2 = s.ImageUrl2,
                        ImageUrl3 = s.ImageUrl3,
                        ImageUrl4 = s.ImageUrl4,
                        Link1TargetId = s.Link1TargetId,
                        Link2TargetId = s.Link2TargetId,
                        Link3TargetId = s.Link3TargetId,
                        Link4TargetId = s.Link4TargetId,
                        MapEmbedUrl = s.MapEmbedUrl,
                        ThumbnailUrl = s.ThumbnailUrl,
                        VideoUrl = s.VideoUrl,
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
                            AvatarAlt = si.AvatarAlt,
                            Features=si.Features,
                            Link4TargetId=si.Link4TargetId,
                            Link3TargetId=si.Link3TargetId,
                            Link2TargetId=si.Link2TargetId,
                            Link1TargetId=si.Link1TargetId,
                            Answer = si.Answer,
                            //ComponentTypeName=si.ComponentTypeName,
                            Link1OpenInNewTab=si.Link1OpenInNewTab,
                            Link1TypeId=si.Link1TypeId,
                            Link2OpenInNewTab= si.Link2OpenInNewTab,
                            Link2TypeId=si.Link2TypeId,
                            Link3OpenInNewTab = si.Link3OpenInNewTab,
                            Link3TypeId=si.Link3TypeId,
                            Link4OpenInNewTab=si.Link4OpenInNewTab,
                            Link4TypeId=si.Link4TypeId,
                            Name=si.Name,   
                            Question=si.Question,
                            Role=si.Role,
                            SectionGroupItemId=si.SectionGroupItemId,
                            //SectionGroupItemName=si.SectionGroupItemName
                            
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
        public async Task<ResultDto<FooterDto>> GetFooterAsync()
        {
            var result = new ResultDto<FooterDto>();

            try
            {
                var page = await Query()
            .Include(p => p.SectionPages)
                .ThenInclude(s => s.ComponentType)

            .Include(p => p.SectionPages)
                .ThenInclude(s => s.SectionItems)
                    .ThenInclude(i => i.SectionGroupItem)

            .Include(p => p.SectionPages)
                .ThenInclude(s => s.SectionItems)
                    .ThenInclude(i => i.Link1Type)

            .Include(p => p.SectionPages)
                .ThenInclude(s => s.SectionItems)
                    .ThenInclude(i => i.Link1Target)

            .FirstOrDefaultAsync(p => p.Slug == "layout");

                if (page == null)
                {
                    result.Success = false;
                    result.Message = "Layout page not found.";
                    return result;
                }

                var footerSections = page.SectionPages
                    .Where(s =>
                        s.ComponentType != null &&
                        s.ComponentType.Code == "footer")
                    .ToList();

                var footerDto = new FooterDto
                {
                    CopyRight = footerSections
                        .Select(s => s.CopyrightText)
                        .FirstOrDefault(x => !string.IsNullOrWhiteSpace(x)),

                    Groups = footerSections
                        .SelectMany(s => s.SectionItems ?? new List<SectionItem>())
                        .Where(i => i.SectionGroupItem != null)

                        .GroupBy(i => new
                        {
                            i.SectionGroupItem.Id,
                            i.SectionGroupItem.Name,
                            i.SectionGroupItem.Code,
                            i.SectionGroupItem.SortOrder
                        })

                        .OrderBy(g => g.Key.SortOrder)

                        .Select(g => new FooterGroupDto
                        {
                            Title = g.Key.Name,
                            Order = g.Key.SortOrder,
                            Code=g.Key.Code,
                            Icon = g.FirstOrDefault().Icon,
                            IconColor = g.FirstOrDefault().IconColor,
                            IconAlt = g.Key.Name,

                            Items = g
                                .OrderBy(i => i.SortOrder)
                                .Select(i => new FooterItemDto
                                {
                                    Title =
                                        !string.IsNullOrWhiteSpace(i.Link1Text)
                                            ? i.Link1Text
                                            : i.Title,

                                    Url = i.Link1Url,
                                    ImageAlt=i.ImageAlt,
                                    ImageUrl=i.ImageUrl,
                                    LinkColor = i.Link1Color,
                                    IsInternalLink =
                                    i.Link1Type != null &&
                                    i.Link1Type.Code != "EXTERNAL",
                                    OpenInNewTab = i.Link1OpenInNewTab,

                                    Order = i.SortOrder,

                                    Icon = i.Icon,
                                    IconAlt = i.IconAlt,
                                    IconColor = i.IconColor
                                })
                                .ToList()
                        })
                        .ToList()
                };

                result.Data = footerDto;
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Failed to load footer.";
                result.Errors.Add(ex.Message);
            }

            return result;

        }


        public async Task<ResultDto<PageViewDto>> GetContentPageAsync(
            string slug,
            int page = 1,
            int pageSize = 10,
            string? categorySlug = null,
            string? search = null,
            string? contentType = null,
            string sort = "newest")
        {
            var result = new ResultDto<PageViewDto>();

            try
            {
                // =========================
                // 1. PAGE ONLY (LIGHT QUERY)
                // =========================
                var pageEntity = await Query()
                    .FirstOrDefaultAsync(p => p.Slug == slug);

                if (pageEntity == null)
                {
                    result.Success = false;
                    result.Message = $"Page '{slug}' not found.";
                    return result;
                }
                var contentItem = await _contentItemService.GetPagedAsync(pageEntity.Id, page, pageSize, categorySlug, search, contentType, sort);


                // =========================
                // 3. MAP PAGE + SECTIONS
                // =========================
                var pageViewDto = new PageViewDto
                {
                    Id = pageEntity.Id,
                    Name = pageEntity.Name,
                    Slug = pageEntity.Slug,
                    MetaTitle = pageEntity.MetaTitle,
                    MetaDescription = pageEntity.MetaDescription,
                    MetaKeywords = pageEntity.MetaKeywords,
                    CanonicalUrl = pageEntity.CanonicalUrl,
                    OgImageUrl = pageEntity.OgImageUrl,
                    ContentItems = contentItem.Items,
                    TotalCount = contentItem.TotalCount,
                    Page = page,
                    PageSize = pageSize
                };

                result.Data = pageViewDto;
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Failed to load content page.";
                result.Errors.Add(ex.Message);
            }

            return result;
        }


        public async Task<ResultDto<ContentItemListDto>> GetContentDetailAsync(
            string contentType,
            string slug)
        {
            var result = new ResultDto<ContentItemListDto>();

            try
            {
                var item = await _contentItemService
                    .Query()
                    .Include(x => x.ContentItemTags)
                        .ThenInclude(x => x.Tag)
                    .Where(x =>
                        x.ContentType == contentType &&
                        x.Slug == slug &&
                        x.IsActive &&
                        x.IsPublished &&
                        !x.IsDeleted)
                    .FirstOrDefaultAsync();

                if (item == null)
                {
                    return new ResultDto<ContentItemListDto>
                    {
                        Success = false,
                        Message = "Content not found"
                    };
                }

                var dto = _mapper.Map<ContentItemListDto>(item);

                // Tags mapping (correct & safe)
                dto.Tags = item.ContentItemTags?
                    .Where(x => x.Tag != null)
                    .Select(x => x.Tag.Name)
                    .ToList() ?? new List<string>();

                result.Data = dto;
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Failed to load content detail";
                result.Errors.Add(ex.Message);
            }

            return result;
        }



    }

}
