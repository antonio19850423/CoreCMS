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
    public class SectionService : GenericService<SqlSection, SqlSection, SectionDto>, ISectionService
    {
        private readonly ISqlRepository<SqlSection> _sqlrepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ITransactionService _transactionService;
        private readonly IModelValidationService _modelValidationService;
        protected readonly Lazy<ILocalizationMessageService> _messageService;
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _config;
        private readonly Lazy<IExcelTemplateService> _excelTemplateService;
        private readonly ISectionService _roleSectionService;
        protected readonly ICurrentUserService _currentUserService;
        public SectionService(
              ISqlRepository<SqlSection> sqlRepository,
              IPosgreSqlRepository<SqlSection> pgRepository,
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
        public async Task<IQueryable<SectionCrud>> GetAllViews()
        {
            return await GetAllViewQueryable<SqlSectionView, SqlSectionView, SectionCrud>();
        }
        public async Task<ResultDto<SectionDto>> CreateAsync(SectionCrud input)
        {
            var (successMessage, errorMessage) = await _messageService.Value.GetSaveMessagesAsync();
            try
            {

                var Section = new SectionDto
                {
                    ColumnsCount = input.ColumnsCount,
                    ComponentTypeId = input.ComponentTypeId,
                    Description = input.Description,
                    ImageUrl = input.ImageUrl,
                    IsActive = input.IsActive,
                    PageId=input.ParentId,
                    SortOrder = input.SortOrder,
                    Subtitle = input.Subtitle,
                    Title = input.Title,
                    ImageAlt = input.ImageAlt,
                    IconAlt = input.IconAlt,
                    Icon=input.Icon,
                    Features = input.Features,
                    BackgroundColor = input.BackgroundColor,
                    ContactEmailLabel = input.ContactEmailLabel,
                    ContactFirstNameLabel = input.ContactFirstNameLabel,
                    ContactLastNameLabel = input.ContactLastNameLabel,
                    ContactMessageLabel = input.ContactMessageLabel,
                    ContactSubmitButtonText = input.ContactSubmitButtonText,
                    CopyrightText = input.CopyrightText,
                    DescriptionColor = input.DescriptionColor,
                    HeaderColor = input.HeaderColor,
                    IconColor = input.IconColor,
                    ImageAlt2 = input.ImageAlt2,
                    ImageAlt3 = input.ImageAlt3,
                    ImageAlt4 = input.ImageAlt4,
                    ImageUrl2 = input.ImageUrl2,
                    ImageUrl3 = input.ImageUrl3,
                    ImageUrl4 = input.ImageUrl4,
                    Link1Color = input.Link1Color,
                    Link1TargetId = input.Link1TargetId,
                    Link1TypeId = input.Link1TypeId,
                    Link1OpenInNewTab = input.Link1OpenInNewTab,
                    Link1Text = input.Link1Text,
                    Link1Url = input.Link1Url,
                    Link2Color = input.Link2Color,
                    Link2TargetId = input.Link2TargetId,
                    Link2TypeId = input.Link2TypeId,
                    Link2OpenInNewTab = input.Link2OpenInNewTab,
                    Link2Text = input.Link2Text,
                    Link2Url = input.Link2Url,
                    Link3Color = input.Link3Color,
                    Link3TargetId = input.Link3TargetId,
                    Link3TypeId = input.Link3TypeId,
                    Link3OpenInNewTab = input.Link3OpenInNewTab,
                    Link3Text = input.Link3Text,
                    Link3Url = input.Link3Url,
                    Link4Color = input.Link4Color,
                    Link4TargetId = input.Link4TargetId,
                    Link4TypeId = input.Link4TypeId,
                    Link4OpenInNewTab = input.Link4OpenInNewTab,
                    Link4Text = input.Link4Text,
                    Link4Url = input.Link4Url,
                    MapEmbedUrl = input.MapEmbedUrl,
                    SubtitleColor = input.SubtitleColor,
                    ThumbnailUrl = input.ThumbnailUrl,
                    VideoUrl = input.VideoUrl,


                };

                var result = await CreateAsync(Section);
                if (!result.Success)
                    return result;

                await _transactionService.CommitAsync();
                return result;
            }
            catch (Exception ex)
            {
                await _transactionService.RollbackAsync();
                var result = new ResultDto<SectionDto>
                {
                    Success = false,
                    Message = errorMessage,
                };
                result.Errors.Add(ex.Message);
                return result;
            }
        }

        public async Task<ResultDto<SectionDto>> UpdateAsync(SectionCrud input)
        {
            var (successMessage, errorMessage) = await _messageService.Value.GetSaveMessagesAsync();
            try
            {
                if (input.Id == null)
                {
                    return new ResultDto<SectionDto>
                    {
                        Success = false,
                        Message = "Id is required"
                    };
                }

                // 1️⃣ به‌روزرسانی کاربر
                var userUpdateDto = new SectionDto
                {
                    Id = input.Id.Value,
                    Title = input.Title,
                    Subtitle= input.Subtitle,
                    SortOrder= input.SortOrder,
                    ColumnsCount = input.ColumnsCount,
                    ComponentTypeId = input.ComponentTypeId,
                    Description = input.Description,
                    ImageUrl = input.ImageUrl,
                    IsActive = input.IsActive,
                    PageId=input.ParentId,
                    IconAlt = input.IconAlt ,
                    ImageAlt = input.ImageAlt , 
                    Features = input.Features,
                    BackgroundColor = input.BackgroundColor,
                    ContactEmailLabel = input.ContactEmailLabel,
                    ContactFirstNameLabel = input.ContactFirstNameLabel,
                    ContactLastNameLabel = input.ContactLastNameLabel,
                    ContactMessageLabel = input.ContactMessageLabel,
                    ContactSubmitButtonText = input.ContactSubmitButtonText,
                    CopyrightText = input.CopyrightText,
                    DescriptionColor = input.DescriptionColor,
                    HeaderColor = input.HeaderColor,
                    Icon=input.Icon,
                    IconColor = input.IconColor,
                    ImageAlt2 = input.ImageAlt2,
                    ImageAlt3 = input.ImageAlt3,
                    ImageAlt4 = input.ImageAlt4,
                    ImageUrl2 = input.ImageUrl2 ,
                    ImageUrl3 = input.ImageUrl3 ,
                    ImageUrl4 = input.ImageUrl4 ,
                    Link1Color = input.Link1Color,
                    Link1TargetId = input.Link1TargetId,
                    Link1TypeId = input.Link1TypeId,
                    Link1OpenInNewTab = input.Link1OpenInNewTab,
                    Link1Text = input.Link1Text,
                    Link1Url = input.Link1Url,
                    Link2Color = input.Link2Color,
                    Link2TargetId = input.Link2TargetId,
                    Link2TypeId = input.Link2TypeId,
                    Link2OpenInNewTab = input.Link2OpenInNewTab,
                    Link2Text = input.Link2Text,
                    Link2Url = input.Link2Url,
                    Link3Color = input.Link3Color,
                    Link3TargetId = input.Link3TargetId,
                    Link3TypeId = input.Link3TypeId,
                    Link3OpenInNewTab = input.Link3OpenInNewTab,
                    Link3Text = input.Link3Text,
                    Link3Url = input.Link3Url,
                    Link4Color = input.Link4Color,
                    Link4TargetId = input.Link4TargetId,
                    Link4TypeId = input.Link4TypeId,
                    Link4OpenInNewTab = input.Link4OpenInNewTab,
                    Link4Text = input.Link4Text,
                    Link4Url = input.Link4Url,
                    MapEmbedUrl = input.MapEmbedUrl ,
                    SubtitleColor = input.SubtitleColor,
                    ThumbnailUrl = input.ThumbnailUrl,
                    VideoUrl = input.VideoUrl,


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
                var result = new ResultDto<SectionDto>
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
            var createdSections= new List<SectionDto>();
            var errors = new List<string>();
            var (successMessage, errorMessage) = await _messageService.Value.GetSaveMessagesAsync();
            var errorFileTitle = await _messageService.Value.GetMessageAsync(LocalizationKeys.ErrorFile);
            try
            {
                var (dt, rowContexts) = excelStream.LoadExcelWithErrors();
                var Sections = dt.ToModelList<SectionCrud>();

                for (int i = 0; i < Sections.Count; i++)
                {
                    var Section = Sections[i];
                    var context = rowContexts[i];

                    var createResult = await CreateAsync(Section);

                    if (createResult.Success && createResult.Data != null)
                    {
                        createdSections.Add(createResult.Data);
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
                        InsertedCount = createdSections.Count,
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
bool exportCurrentSection,
int SectionNumber,
int SectionSize)
        {
            // 1️⃣ گرفتن همه داده‌ها از query
            var query = await GetAllViews(); // IQueryable<Resource>

            // 2️⃣ Paging و Mapping به DTO
            List<SectionCrud> data;

            if (exportCurrentSection)
            {
                data = query
                    .Skip((SectionNumber - 1) * SectionSize)
                    .Take(SectionSize)
                    .ToList();
            }
            else
            {
                data = query.ToList();
            }
            var resource = _mapper.Map<List<SectionCrud>>(data);

            // 3️⃣ تولید Template اکسل با Lookup (مثلاً 5 ردیف خالی اضافه)
            var templateBytes = await _excelTemplateService.Value.GenerateTemplateWithLookupsAsync(
                LookupEntities.Section, // نام مدل DTO
                data.Count + 5
            );

            // 4️⃣ پر کردن داده‌ها در Template با Extension Method
            var resultBytes = templateBytes.FillDataIntoTemplate(data, startRow: 3);

            return resultBytes;
        }


    }

}
