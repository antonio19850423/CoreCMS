using AutoMapper;
using Microsoft.AspNetCore.Hosting;
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
    public class SiteSettingService : GenericService<SqlSiteSetting, SqlSiteSetting, SiteSettingDto>, ISiteSettingService
    {
        private readonly ISqlRepository<SqlSiteSetting> _sqlrepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ITransactionService _transactionService;
        private readonly IModelValidationService _modelValidationService;
        protected readonly Lazy<ILocalizationMessageService> _messageService;
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _config;
        private readonly Lazy<IExcelTemplateService> _excelTemplateService;
        private readonly ISiteSettingService _roleSiteSettingService;
        protected readonly ICurrentUserService _currentUserService;
        public SiteSettingService(
              ISqlRepository<SqlSiteSetting> sqlRepository,
              IPosgreSqlRepository<SqlSiteSetting> pgRepository,
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
        public async Task<IQueryable<SiteSettingCrud>> GetAllViews()
        {
            return await GetAllViewQueryable<SqlSiteSettingView, SqlSiteSettingView, SiteSettingCrud>();
        }

        public async Task<ResultDto<SiteSettingDto>> CreateAsync(SiteSettingCrud input)
        {
            var (successMessage, errorMessage) = await _messageService.Value.GetSaveMessagesAsync();
            try
            {
                var validation = await _modelValidationService.ValidateAsync(input);
                if (!validation.Success)
                    return new ResultDto<SiteSettingDto>
                    {
                        Success = false,
                        Message = await _messageService.Value.GetMessageAsync(LocalizationKeys.ValidationFailed, "Form has errors. Please fix them."),
                        Errors = validation.Data
                    };
                var SiteSetting = new SiteSettingDto
                {
                    Address = input.Address,
                    Address2 = input.Address2,
                    Address2Title = input.Address2Title,
                    AddressTitle = input.AddressTitle,
                    DarkLogoAlt = input.DarkLogoAlt,
                    DarkLogoUrl = input.DarkLogoUrl,
                    DefaultMetaDescription = input.DefaultMetaDescription,
                    DefaultMetaKeywords = input.DefaultMetaKeywords,
                    DefaultMetaTitle = input.DefaultMetaTitle,
                    DomainName = input.DomainName,
                    Email = input.Email,
                    FaviconUrl = input.FaviconUrl,
                    Fax= input.Fax,
                    FaxTitle = input.FaxTitle,
                    IsActive = input.IsActive,
                    LogoAlt = input.LogoAlt,
                    LogoUrl = input.LogoUrl,
                    Mobile = input.Mobile,
                    MobileTitle = input.MobileTitle,
                    Phone = input.Phone,
                    Phone2 = input.Phone2,
                    PhoneTitle = input.PhoneTitle,
                    Phone2Title = input.Phone2Title,
                    SiteName = input.SiteName,
                    SmtpHost = input.SmtpHost,
                    SmtpPort = input.SmtpPort,
                    SmtpUserName = input.SmtpUserName,
                    SmtpPassword = input.SmtpPassword,
                    SmtpEnableSsl = input.SmtpEnableSsl,
                    
                };

                var result = await CreateAsync(SiteSetting);
                if (!result.Success)
                    return result;

                await _transactionService.CommitAsync();
                return result;
            }
            catch (Exception ex)
            {
                await _transactionService.RollbackAsync();
                var result = new ResultDto<SiteSettingDto>
                {
                    Success = false,
                    Message = errorMessage,
                };
                result.Errors.Add(ex.Message);
                return result;
            }
        }

        public async Task<ResultDto<SiteSettingDto>> UpdateAsync(SiteSettingCrud input)
        {
            var (successMessage, errorMessage) = await _messageService.Value.GetSaveMessagesAsync();
            try
            {
                if (input.Id == null)
                {
                    return new ResultDto<SiteSettingDto>
                    {
                        Success = false,
                        Message = await _messageService.Value.GetMessageAsync(LocalizationKeys.IdRequired)
                    };
                }
                var validation = await _modelValidationService.ValidateAsync(input);
                if (!validation.Success)
                    return new ResultDto<SiteSettingDto>
                    {
                        Success = false,
                        Message = await _messageService.Value.GetMessageAsync(LocalizationKeys.ValidationFailed, "Form has errors. Please fix them."),
                        Errors = validation.Data
                    };


                // 1️⃣ به‌روزرسانی کاربر
                var userUpdateDto = new SiteSettingDto
                {
                    Id = input.Id,
                    Address = input.Address,
                    Address2 = input.Address2,
                    SiteName = input.SiteName,
                    Phone2Title= input.Phone2Title,
                    PhoneTitle = input.PhoneTitle,
                    Address2Title = input.Address2Title,
                    AddressTitle = input.AddressTitle,
                    DarkLogoAlt = input.DarkLogoAlt,
                    DarkLogoUrl = input.DarkLogoUrl,
                    DefaultMetaDescription = input.DefaultMetaDescription,
                    DefaultMetaKeywords = input.DefaultMetaKeywords,
                    DefaultMetaTitle = input.DefaultMetaTitle,
                    //این قسمت نباید توسط مدیر سایت تغییر کند در صورت نیاز به صورت دستی تغییر خواهد کرد 
                    //DomainName = input.DomainName,
                    Email = input.Email,
                    FaviconUrl = input.FaviconUrl,
                    Fax= input.Fax,
                    FaxTitle = input.FaxTitle,
                    IsActive = input.IsActive,
                    LogoAlt = input.LogoAlt,
                    LogoUrl = input.LogoUrl,
                    Mobile= input.Mobile,
                    MobileTitle = input.MobileTitle,
                    Phone = input.Phone,
                    Phone2 = input.Phone2,
                    SmtpEnableSsl=input.SmtpEnableSsl,
                    SmtpPassword=input.SmtpPassword,
                    SmtpUserName=input.SmtpUserName,
                    SmtpPort=input.SmtpPort,
                    SmtpHost=input.SmtpHost,
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
                var result = new ResultDto<SiteSettingDto>
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
            var createdSiteSettings= new List<SiteSettingDto>();
            var errors = new List<string>();
            var (successMessage, errorMessage) = await _messageService.Value.GetSaveMessagesAsync();
            var errorFileTitle = await _messageService.Value.GetMessageAsync(LocalizationKeys.ErrorFile);
            try
            {
                var (dt, rowContexts) = excelStream.LoadExcelWithErrors();
                var SiteSettings = dt.ToModelList<SiteSettingCrud>();

                for (int i = 0; i < SiteSettings.Count; i++)
                {
                    var SiteSetting = SiteSettings[i];
                    var context = rowContexts[i];

                    var createResult = await CreateAsync(SiteSetting);

                    if (createResult.Success && createResult.Data != null)
                    {
                        createdSiteSettings.Add(createResult.Data);
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
                        InsertedCount = createdSiteSettings.Count,
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
            List<SiteSettingCrud> data;

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
            var resource = _mapper.Map<List<SiteSettingCrud>>(data);

            // 3️⃣ تولید Template اکسل با Lookup (مثلاً 5 ردیف خالی اضافه)
            var templateBytes = await _excelTemplateService.Value.GenerateTemplateWithLookupsAsync(
                LookupEntities.SiteSetting, // نام مدل DTO
                data.Count + 5
            );

            // 4️⃣ پر کردن داده‌ها در Template با Extension Method
            var resultBytes = templateBytes.FillDataIntoTemplate(data, startRow: 3);

            return resultBytes;
        }


    }

}
