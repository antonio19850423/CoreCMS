using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using Velora.Application.Shared.Dtos;
using Velora.Application.Shared.Enums;
using Velora.Application.Shared.Repositories;
using Velora.Application.Shared.Services;
using Velora.Infrastructure.ORM.Interfaces.MyApp.Orm.Interfaces;
using Path = System.IO.Path;

namespace Velora.Application.Services
    {
    public class GeneralSettingService
        :GenericService<SqlGeneralsetting,PgGeneralsetting,GeneralSettingDto>, IGeneralSettingService
        {
        private readonly IMapper _mapper;
        private readonly IResourceLanguageService _resourceLanguageService;
        private readonly ILocalizationtranslationService _localizationtranslationService;
        private readonly IHostEnvironment _env;
        private readonly IHttpContextAccessor _httpContextAccessor;
        protected readonly Lazy<ILocalizationMessageService> _messageService;
        public GeneralSettingService(
            ISqlRepository<SqlGeneralsetting> sqlRepository,
            IPosgreSqlRepository<PgGeneralsetting> pgRepository,
            IMapper mapper,
            IConfiguration configuration,
            IResourceLanguageService resourceLanguageService,
            ILocalizationtranslationService localizationtranslationService,
            IHostEnvironment env,IHttpContextAccessor httpContextAccessor,
            Lazy<ILocalizationMessageService> messageService, ICurrentUserService currentUserService
        ) : base(sqlRepository,pgRepository,mapper,configuration,messageService, currentUserService)
            {
            _mapper = mapper;
            _resourceLanguageService = resourceLanguageService;
            _localizationtranslationService = localizationtranslationService;
            _env = env;
            _httpContextAccessor = httpContextAccessor;
            _messageService = messageService;
            }

        /// <summary>
        /// دریافت یک رکورد GeneralSetting بر اساس Key
        /// </summary>
        public async Task<GeneralSettingDto?> GetByKeyAsync(string key)
            {
            if(_dbType == DatabaseType.SqlServer)
                {
                var repo = (ISqlRepository<SqlGeneralsetting>)GetRepository();
                var entity = await repo.FirstOrDefaultAsync(x => x.Key == key);
                return _mapper.Map<GeneralSettingDto>(entity);
                }
            else
                {
                var repo = (IPosgreSqlRepository<PgGeneralsetting>)GetRepository();
                var entity = await repo.FirstOrDefaultAsync(x => x.Key == key);
                return _mapper.Map<GeneralSettingDto>(entity);
                }
            }

        /// <summary>
        /// گرفتن لیست زبان‌های فعال سیستم
        /// </summary>
        public async Task<List<LocalizationtranslationDto>> GetAvailableLanguagesAsync(HttpContext httpContext)
            {
            var setting = await GetByKeyAsync("AvailableLanguages");
            if(setting == null || string.IsNullOrWhiteSpace(setting.Value))
                setting = new GeneralSettingDto { Value = "en" }; // fallback

            var availableLanguages = setting.Value
                .Split(',',StringSplitOptions.RemoveEmptyEntries)
                .Select(l => l.Trim())
                .ToList();

            // مسیر Resources
            string resourcesPath;
            if(_env.IsDevelopment())
                {
                var projectRoot = Path.Combine(AppContext.BaseDirectory,"..","..","..","..","Velora.Application.Shared");
                resourcesPath = Path.Combine(projectRoot,"Resources");
                }
            else
                {
                var assemblyFolder = Path.GetDirectoryName(typeof(LocalizationkeyDto).Assembly.Location)!;
                resourcesPath = Path.Combine(assemblyFolder,"Resources");
                }
            resourcesPath = Path.GetFullPath(resourcesPath);

            if(!Directory.Exists(resourcesPath))
                throw new DirectoryNotFoundException($"Resources folder not found: {resourcesPath}");

            var resxFiles = Directory.GetFiles(resourcesPath,"Languages.resx",SearchOption.TopDirectoryOnly);
            var result = new List<LocalizationtranslationDto>();

            foreach(var lang in availableLanguages)
                {
                // پیدا کردن فایل زبان مربوط
                var file = resxFiles.FirstOrDefault();
                if(file == null) continue;

                // خواندن فایل resx با XmlDocument
                var translations = new Dictionary<string,string>();
                var doc = new XmlDocument();
                doc.Load(file);

                var dataNodes = doc.SelectNodes("//data");
                if(dataNodes != null)
                    {
                    foreach(XmlNode node in dataNodes)
                        {
                        var keyAttr = node.Attributes?["name"]?.Value;
                        var valueNode = node.SelectSingleNode("value")?.InnerText;
                        if(!string.IsNullOrEmpty(keyAttr) && !string.IsNullOrEmpty(valueNode))
                            translations[keyAttr] = valueNode;
                        }
                    }

                // تعیین direction بر اساس زبان
                var direction = lang switch
                    {
                        "fa" => "rtl",
                        "ar" => "rtl",
                        _ => "ltr"
                        };

                // گرفتن نام ترجمه شده زبان
                var currentLanguage = await GetCurrentLanguageAsync(httpContext);
                var translatedName = await _localizationtranslationService.GetByCodeAsync($"system.{lang}",currentLanguage);

                result.Add(new LocalizationtranslationDto
                    {
                    LanguageCode = lang,
                    Value = translatedName?.Value ?? "",
                    Direction = direction
                    });
                }

            return result;
            }



        /// <summary>
        /// دریافت تمام کلیدهای ترجمه برای زبان‌های فعال سیستم
        /// </summary>
        public async Task<Dictionary<string,string>> GetAllTranslationsAsync(string currentLanguage)
            {
            // دریافت رکورد ترجمه براساس نوع دیتابیس
            var translationsRecord = await _localizationtranslationService.GetTranslationsByLanguageAsync(currentLanguage);

            return translationsRecord;
        }


        /// <summary>
        /// گرفتن زبان فعلی کاربر - اول از کوکی، اگر نبود از GeneralSettings
        /// </summary>
        public async Task<string> GetCurrentLanguageAsync(HttpContext httpContext)
            {
            // 1️⃣ بررسی کوکی
            var userLang = httpContext.Request.Cookies["UserLanguage"];
            if(!string.IsNullOrEmpty(userLang))
                return userLang;

            // 2️⃣ بررسی GeneralSettings
            var defaultLangSetting = await GetByKeyAsync("DefaultLanguage");
            if(defaultLangSetting != null && !string.IsNullOrEmpty(defaultLangSetting.Value))
                return defaultLangSetting.Value;

            // 3️⃣ fallback نهایی
            return "en";
            }
        }
    }
