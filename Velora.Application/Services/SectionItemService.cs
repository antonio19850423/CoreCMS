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
    public class SectionItemService : GenericService<SqlSectionItem, SqlSectionItem, SectionItemDto>, ISectionItemService
    {
        private readonly ISqlRepository<SqlSectionItem> _sqlrepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ITransactionService _transactionService;
        private readonly IModelValidationService _modelValidationService;
        protected readonly Lazy<ILocalizationMessageService> _messageService;
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _config;
        private readonly Lazy<IExcelTemplateService> _excelTemplateService;
        private readonly ISectionItemService _roleSectionItemService;
        protected readonly ICurrentUserService _currentUserService;
        public SectionItemService(
              ISqlRepository<SqlSectionItem> sqlRepository,
              IPosgreSqlRepository<SqlSectionItem> pgRepository,
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
        public async Task<IQueryable<SectionItemCrud>> GetAllViews()
        {
            return await GetAllViewQueryable<SqlSectionItemView, SqlSectionItemView, SectionItemCrud>();
        }
        public async Task<ResultDto<SectionItemDto>> CreateAsync(SectionItemCrud input)
        {
            var (successMessage, errorMessage) = await _messageService.Value.GetSaveMessagesAsync();
            try
            {

                var SectionItem = new SectionItemDto
                {
                    AvatarAlt = input.AvatarAlt,
                    AvatarUrl = input.AvatarUrl,
                    BackgroundColor = input.BackgroundColor,
                    Description = input.Description,
                    DescriptionColor = input.DescriptionColor,
                    Title = input.Title,
                    Icon = input.Icon,
                    IconAlt = input.IconAlt,
                    IconColor = input.IconColor,
                    ImageAlt = input.ImageAlt,
                    IsActive = input.IsActive,
                    ImageUrl = input.ImageUrl,
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
                    Price = input.Price,
                    SectionId=input.ParentId,
                    SortOrder=input.SortOrder,
                    Subtitle=input.Subtitle,
                    SubtitleColor=input.SubtitleColor,
                    TitleColor=input.TitleColor,
                    Features = input.Features,
                    SectionGroupItemId=input.SectionGroupItemId,
                    Question = input.Question,
                    Role= input.Role,
                    Name=input.Name,
                    
                    
                };

                var result = await CreateAsync(SectionItem);
                if (!result.Success)
                    return result;

                await _transactionService.CommitAsync();
                return result;
            }
            catch (Exception ex)
            {
                await _transactionService.RollbackAsync();
                var result = new ResultDto<SectionItemDto>
                {
                    Success = false,
                    Message = errorMessage,
                };
                result.Errors.Add(ex.Message);
                return result;
            }
        }

        public async Task<ResultDto<SectionItemDto>> UpdateAsync(SectionItemCrud input)
        {
            var (successMessage, errorMessage) = await _messageService.Value.GetSaveMessagesAsync();
            try
            {
                if (input.Id == null)
                {
                    return new ResultDto<SectionItemDto>
                    {
                        Success = false,
                        Message = "Id is required"
                    };
                }

                // 1️⃣ به‌روزرسانی کاربر
                var userUpdateDto = new SectionItemDto
                {
                    Id = input.Id,
                    Title = input.Title,
                    Subtitle= input.Subtitle,
                    SortOrder= input.SortOrder,
                    Description = input.Description,
                    ImageUrl = input.ImageUrl,
                    IsActive = input.IsActive,
                    SectionId=input.ParentId,
                    IconAlt = input.IconAlt ,
                    ImageAlt = input.ImageAlt , 
                    TitleColor = input.TitleColor,
                    SubtitleColor = input.SubtitleColor,
                    AvatarAlt = input.AvatarAlt ,
                    AvatarUrl = input.AvatarUrl ,
                    BackgroundColor = input.BackgroundColor,
                    DescriptionColor = input.DescriptionColor,
                    Icon= input.Icon,
                    IconColor = input.IconColor,
                    Price=input.Price,
                    Features=input.Features,
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
                    Answer=input.Answer,
                    Question=input.Question,
                    Name=input.Name,
                    Role=input.Role,
                    SectionGroupItemId=input.SectionGroupItemId,
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
                var result = new ResultDto<SectionItemDto>
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
            var createdSectionItems= new List<SectionItemDto>();
            var errors = new List<string>();
            var (successMessage, errorMessage) = await _messageService.Value.GetSaveMessagesAsync();
            var errorFileTitle = await _messageService.Value.GetMessageAsync(LocalizationKeys.ErrorFile);
            try
            {
                var (dt, rowContexts) = excelStream.LoadExcelWithErrors();
                var SectionItems = dt.ToModelList<SectionItemCrud>();

                for (int i = 0; i < SectionItems.Count; i++)
                {
                    var SectionItem = SectionItems[i];
                    var context = rowContexts[i];

                    var createResult = await CreateAsync(SectionItem);

                    if (createResult.Success && createResult.Data != null)
                    {
                        createdSectionItems.Add(createResult.Data);
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
                        InsertedCount = createdSectionItems.Count,
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
bool exportCurrentSectionItem,
int SectionItemNumber,
int SectionItemSize)
        {
            // 1️⃣ گرفتن همه داده‌ها از query
            var query = await GetAllViews(); // IQueryable<Resource>

            // 2️⃣ Paging و Mapping به DTO
            List<SectionItemCrud> data;

            if (exportCurrentSectionItem)
            {
                data = query
                    .Skip((SectionItemNumber - 1) * SectionItemSize)
                    .Take(SectionItemSize)
                    .ToList();
            }
            else
            {
                data = query.ToList();
            }
            var resource = _mapper.Map<List<SectionItemCrud>>(data);

            // 3️⃣ تولید Template اکسل با Lookup (مثلاً 5 ردیف خالی اضافه)
            var templateBytes = await _excelTemplateService.Value.GenerateTemplateWithLookupsAsync(
                LookupEntities.SectionItem, // نام مدل DTO
                data.Count + 5
            );

            // 4️⃣ پر کردن داده‌ها در Template با Extension Method
            var resultBytes = templateBytes.FillDataIntoTemplate(data, startRow: 3);

            return resultBytes;
        }


    }

}
