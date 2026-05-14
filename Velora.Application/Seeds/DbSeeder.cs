using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing.Tree;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Collections;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Velora.Application.Services;
using Velora.Application.Shared.Attributes;
using Velora.Application.Shared.Constants;
using Velora.Application.Shared.Dtos;
using Velora.Application.Shared.Enums;
using Velora.Application.Shared.Services;
using Path = System.IO.Path;

namespace Velora.Application.Seeds
{
    public class DbSeeder
    {
        private readonly DatabaseType _dbType;
        private readonly IRoleService _roleService;
        private readonly IUserService _userService;
        private readonly IResourceTypeService _resourceTypeService;
        private readonly IResourceService _resourceService;
        private readonly IResourceLanguageService _resourceLanguageService;
        private readonly IPermissionService _permissionService;
        private readonly IRolePermissionService _rolePermissionService;
        private readonly ITransactionService _transactionService;
        private readonly IUserRoleService _userRoleService;
        private readonly IWebHostEnvironment _env;
        private readonly ILocalizationkeyService _localizationkeyService;
        private readonly ILocalizationtranslationService _localizationtranslationService;
        private readonly IGeneralSettingService _generalSettingService;
        private readonly ISeedHistoryService _seedHistoryService;
        private const string SeederName = "ResourcePermissionSeeder";

        public DbSeeder(
        IConfiguration configuration,
        IRoleService roleService,
        IUserService userService,
        IResourceTypeService resourceTypeService,
        IResourceService resourceService,
        IPermissionService permissionService,
        IRolePermissionService rolePermissionService,
        ITransactionService transactionService,
        IUserRoleService userRoleService, IWebHostEnvironment env, ILocalizationkeyService localizationkeyService, ILocalizationtranslationService localizationtranslationService, IGeneralSettingService generalSettingService, IResourceLanguageService resourceLanguageService, ISeedHistoryService seedHistoryService)
        {
            var dbTypeString = configuration.GetValue<string>("Database:Provider") ?? "PostgreSql";
            _dbType = dbTypeString.Equals("SqlServer", StringComparison.OrdinalIgnoreCase)
                ? DatabaseType.SqlServer
                : DatabaseType.PostgreSql;
            _roleService = roleService;
            _userService = userService;
            _resourceTypeService = resourceTypeService;
            _resourceService = resourceService;
            _permissionService = permissionService;
            _rolePermissionService = rolePermissionService;
            _transactionService = transactionService;
            _userRoleService = userRoleService;
            _env = env;
            _localizationkeyService = localizationkeyService;
            _localizationtranslationService = localizationtranslationService;
            _generalSettingService = generalSettingService;
            _resourceLanguageService = resourceLanguageService;
            _seedHistoryService = seedHistoryService;
        }

        public async Task SeedAllAsync()
        {
            // 2️⃣ Seed Localization از فایل‌های resx
            await SeedLocalizationAsync();
            // 1️⃣ Seed تمام داده‌های اصلی (Role, User, Resource, Permission و …)
            await SeedCoreAsync();
            await SeedResourcesAsync();
            await SeedPermissionsAsync();
            await SeedSettingsAsync();




            // 3️⃣ Commit یک‌باره تراکنش‌ها
            await _transactionService.CommitAsync();
        }

        public async Task SeedCoreAsync()
        {
            // --- Roles ---
            var roles = new[]
            {
            new RoleDto { Name = "Admin", Code = "ADMIN" },
            new RoleDto { Name = "User", Code = "USER" },
            new RoleDto { Name = "Developer", Code = "DEV" }
        };

            foreach (var role in roles)
            {
                var existing = _dbType == DatabaseType.SqlServer
                    ? await _roleService.FirstOrDefaultAsync<SqlRole>(x => x.Code == role.Code)
                    : await _roleService.FirstOrDefaultAsync<PgRole>(x => x.Code == role.Code);

                if (existing.Data == null)
                {
                    var created = await _roleService.CreateAsync(role);
                    role.Id = created.Data.Id;
                }
                else
                {
                    role.Id = existing.Data.Id;
                }
            }

            // --- Users ---
            var users = new[]
            {
            new UserDto { UserName = "Admin", Password = "123456", IsActive = true },
            new UserDto { UserName = "Developer", Password = "123456", IsActive = true }
        };

            foreach (var user in users)
            {
                var existing = _dbType == DatabaseType.SqlServer
                                ? await _userService.FirstOrDefaultAsync<SqlUser>(x => x.UserName == user.UserName)
                                : await _userService.FirstOrDefaultAsync<PgUser>(x => x.UserName == user.UserName);
                if (existing.Data == null)
                {
                    if (_env.IsDevelopment())
                        user.Password = BCrypt.Net.BCrypt.HashPassword("123");
                    else // Production یا Staging
                        user.Password = BCrypt.Net.BCrypt.HashPassword("Afe@09035609400@");

                    var created = await _userService.CreateAsync(user);
                    user.Id = created.Data.Id;
                }
                else
                {
                    if (_env.IsDevelopment())
                    {
                        existing.Data.Password = BCrypt.Net.BCrypt.HashPassword("123");
                    }
                    else // Production یا Staging
                        existing.Data.Password = BCrypt.Net.BCrypt.HashPassword("Afe@09035609400@");
                    await _userService.UpdateAsync(existing.Data, existing.Data.Id);
                    // در Prod و Staging کاری انجام نشود
                    user.Id = existing.Data.Id;
                }
            }


            var userRoles = new[]
{
    //new UserRoleDto
    //{
    //    Id = Guid.NewGuid(),
    //    Userid = users.First(u => u.UserName == "Admin").Id,
    //    Roleid = roles.First(r => r.Code == "ADMIN").Id
    //},
    new UserRoleDto
    {
        Id = Guid.NewGuid(),
        Userid = users.First(u => u.UserName == "Developer").Id,
        Roleid = roles.First(r => r.Code == "DEV").Id
    }
};
            var existingRolesData = await _userRoleService.GetAllAsync(); // این خودش Task<IList<UserRoleDto>> میده
            var existingRoles = existingRolesData.Data
                .Select(x => (x.Userid, x.Roleid))
                .ToList();

            foreach (var ur in userRoles)
            {
                if (!existingRoles.Any(x => x.Userid == ur.Userid && x.Roleid == ur.Roleid))
                {
                    await _userRoleService.CreateAsync(ur);
                }
            }
            // --- ResourceTypes ---
            var resourceTypes = new[]
            {
            new ResourceTypeDto { Code = "MENU", Name = "Menu", DisplayName = "Menu" },
            new ResourceTypeDto { Code = "PAGE", Name = "Page", DisplayName = "Page" },
            new ResourceTypeDto { Code = "ACTION", Name = "Action", DisplayName = "Action" }
        };

            foreach (var type in resourceTypes)
            {
                var existing = _dbType == DatabaseType.SqlServer
                 ? await _resourceTypeService.FirstOrDefaultAsync<SqlResourceType>(x => x.Code == type.Code)
                 : await _resourceTypeService.FirstOrDefaultAsync<PgResourcetype>(x => x.Code == type.Code);

                if (existing?.Data == null)  // توجه به ? برای جلوگیری از null reference
                {
                    var created = await _resourceTypeService.CreateAsync(type);
                    type.Id = created.Data.Id;
                }
                else
                {
                    type.Id = existing.Data.Id;
                }
            }

            var menuType = resourceTypes.First(x => x.Code == "MENU");
            var pageType = resourceTypes.First(x => x.Code == "PAGE");
            // --- Resources (Menus and Pages) ---
            // --- Resources (Menus and Pages) ---
            // --- Resources (Menus and Pages) ---
            var resources = new[]
            {
    // Existing resources
    new ResourceDto { ResourceTypeId = menuType.Id, Code = "DASHBOARD", Name = "Dashboard", DisplayName = "Dashboard", Order = 1, IsActive = true },
    new ResourceDto { ResourceTypeId = pageType.Id, Code = "USER_MANAGEMENT", Name = "User Management", DisplayName = "User Management", Order = 2, IsActive = true },

    // New root menu
    new ResourceDto { ResourceTypeId = menuType.Id, Code = "BASIC_INFO", Name = "Basic Info", DisplayName = "Basic Info", Order = 3, IsActive = true },

    // Child menu under Basic Info
    new ResourceDto { ResourceTypeId = menuType.Id, Code = "ROLE_MANAGEMENT", Name = "Role Management", DisplayName = "Role Management", Order = 1, IsActive = true, Route = "basic-info/role-management" },

    // Page under Role Management
    new ResourceDto { ResourceTypeId = pageType.Id, Code = "ROLE_MANAGEMENT_PAGE", Name = "Role Management Page", DisplayName = "Role Management Page", Order = 1, IsActive = true, Route = "basic-info/role-management" },
    // --- New Management Menu ---
    new ResourceDto { ResourceTypeId = menuType.Id, Code = "ADMINISTRATION", Name = "Administration", DisplayName = "مدیریت", Order = 4, IsActive = true },
    new ResourceDto { ResourceTypeId = menuType.Id, Code = "USER_MANAGEMENT_MENU", Name = "User Management", DisplayName = "مدیریت کاربران", Order = 1, IsActive = true, Route = "administration/user-management" },
    new ResourceDto { IsDynamicForm=true,ResourceTypeId = pageType.Id, Code = "USER_MANAGEMENT_PAGE", Name = "User Management Page", DisplayName = "صفحه مدیریت کاربران", Order = 1, IsActive = true, Route = "administration/user-management" },
    new ResourceDto { ResourceTypeId = menuType.Id, Code = "RESOURCETYPE_MANAGEMENT", Name = "ResourceType Management", DisplayName = "ResourceType Management", Order = 2, IsActive = true, Route = "basic-info/resourceType-management" },
    new ResourceDto { ResourceTypeId = pageType.Id, Code = "RESOURCETYPE_MANAGEMENT_PAGE", Name = "ResourceType Management Page", DisplayName = "ResourceType Management Page", Order = 2, IsActive = true, Route = "basic-info/resourceType-management" },
    new ResourceDto { ResourceTypeId = menuType.Id, Code = "RESOURCE_MANAGEMENT_MENU", Name = "Resource Management", DisplayName = "مدیریت منابع", Order = 2, IsActive = true, Route = "administration/resource-management" },
    new ResourceDto { IsDynamicForm=true,ResourceTypeId = pageType.Id, Code = "RESOURCE_MANAGEMENT_PAGE", Name = "Resource Management Page", DisplayName = "صفحه منابع", Order = 2, IsActive = true, Route = "administration/resource-management" },
    new ResourceDto
{
    ResourceTypeId = menuType.Id,
    Code = "PERMISSION_MANAGEMENT_MENU",
    Name = "Permission Management",
    DisplayName = "مدیریت دسترسی‌ها",
    Order = 3,
    IsActive = true,
    Route = "administration/permission-management"
},
new ResourceDto
{
    IsDynamicForm = true,
    ResourceTypeId = pageType.Id,
    Code = "PERMISSION_MANAGEMENT_PAGE",
    Name = "Permission Management Page",
    DisplayName = "صفحه مدیریت دسترسی‌ها",
    Order = 1,
    IsActive = true,
    Route = "administration/permission-management"
},

};

            // --- Insert or update resources ---
            foreach (var resource in resources)
            {
                var existing = _dbType == DatabaseType.SqlServer
                    ? await _resourceService.FirstOrDefaultAsync<SqlResource>(x => x.Code == resource.Code)
                    : await _resourceService.FirstOrDefaultAsync<PgResource>(x => x.Code == resource.Code);

                if (existing.Data == null)
                {
                    var created = await _resourceService.CreateAsync(resource);
                    resource.Id = created.Data.Id;
                }
                else
                {
                    resource.Id = existing.Data.Id;
                    var updated = await _resourceService.UpdateAsync(resource, existing.Data.Id);

                }
            }
            await _transactionService.CommitAsync(); // SaveChanges قبل از ResourceLanguages

            // --- Set ParentId for child menus/pages ---
            var basicInfoMenu = resources.First(r => r.Code == "BASIC_INFO");
            var roleMenu = resources.First(r => r.Code == "ROLE_MANAGEMENT");
            var rolePage = resources.First(r => r.Code == "ROLE_MANAGEMENT_PAGE");
            var resourceTypeMenu = resources.First(r => r.Code == "RESOURCETYPE_MANAGEMENT");
            var resourceTypePage = resources.First(r => r.Code == "RESOURCETYPE_MANAGEMENT_PAGE");
            // New Management hierarchy
            var adminMenu = resources.First(r => r.Code == "ADMINISTRATION");
            var userMenu = resources.First(r => r.Code == "USER_MANAGEMENT_MENU");
            var userPage = resources.First(r => r.Code == "USER_MANAGEMENT_PAGE");
            var resourceMenu = resources.First(r => r.Code == "RESOURCE_MANAGEMENT_MENU");
            var resourcePage = resources.First(r => r.Code == "RESOURCE_MANAGEMENT_PAGE");
            var permissionMenu = resources.First(r => r.Code == "PERMISSION_MANAGEMENT_MENU");
            var permissionPage = resources.First(r => r.Code == "PERMISSION_MANAGEMENT_PAGE");

            permissionMenu.ParentId = adminMenu.Id;   // زیر Resource Management
            permissionPage.ParentId = permissionMenu.Id;

            roleMenu.ParentId = basicInfoMenu.Id;
            rolePage.ParentId = roleMenu.Id;

            resourceTypeMenu.ParentId = basicInfoMenu.Id;
            resourceTypePage.ParentId = resourceTypeMenu.Id;

            userMenu.ParentId = adminMenu.Id;
            userPage.ParentId = userMenu.Id;

            resourceMenu.ParentId = adminMenu.Id;
            resourcePage.ParentId = resourceMenu.Id;

            // --- Update resources with ParentId ---
            foreach (var res in new[] { roleMenu, rolePage, userMenu, userPage, resourceTypeMenu, resourceTypePage, resourceMenu, resourcePage, permissionMenu, permissionPage })
            {
                await _resourceService.UpdateAsync(res, res.Id);
            }

            // --- Resource Languages ---
            var resourceLanguages = new[]
            {
    new ResourceLanguageDto { ResourceId = resources[0].Id, LanguageCode="fa", Name="داشبورد"},
    new ResourceLanguageDto { ResourceId = resources[0].Id, LanguageCode="en", Name="Dashboard"},
    new ResourceLanguageDto { ResourceId = resources[1].Id, LanguageCode="fa", Name="مدیریت کاربر"},
    new ResourceLanguageDto { ResourceId = resources[1].Id, LanguageCode="en", Name="User Management"},

    new ResourceLanguageDto { ResourceId = resources[2].Id, LanguageCode="fa", Name="اطلاعات پایه"},
    new ResourceLanguageDto { ResourceId = resources[2].Id, LanguageCode="en", Name="Basic Info"},

    new ResourceLanguageDto { ResourceId = resources[3].Id, LanguageCode="fa", Name="نقش کاربری"},
    new ResourceLanguageDto { ResourceId = resources[3].Id, LanguageCode="en", Name="Role Management"},

    new ResourceLanguageDto { ResourceId = resources[4].Id, LanguageCode="fa", Name="صفحه مدیریت نقش"},
    new ResourceLanguageDto { ResourceId = resources[4].Id, LanguageCode="en", Name="Role Management Page"},



    // New Management Language
    new ResourceLanguageDto { ResourceId = resources[5].Id, LanguageCode="fa", Name="مدیریت"},
    new ResourceLanguageDto { ResourceId = resources[5].Id, LanguageCode="en", Name="Administration"},
    new ResourceLanguageDto { ResourceId = resources[6].Id, LanguageCode="fa", Name="مدیریت کاربران"},
    new ResourceLanguageDto { ResourceId = resources[6].Id, LanguageCode="en", Name="User Management"},
    new ResourceLanguageDto { ResourceId = resources[7].Id, LanguageCode="fa", Name="صفحه مدیریت کاربران"},
    new ResourceLanguageDto { ResourceId = resources[7].Id, LanguageCode="en", Name="User Management Page"},
    new ResourceLanguageDto { ResourceId = resources[8].Id, LanguageCode="fa", Name="نوع منابع"},
    new ResourceLanguageDto { ResourceId = resources[8].Id, LanguageCode="en", Name="Resource Type Management"},
    new ResourceLanguageDto { ResourceId = resources[9].Id, LanguageCode="fa", Name="صفحه نوع منابع"},
    new ResourceLanguageDto { ResourceId = resources[9].Id, LanguageCode="en", Name="Resource Type Management Page"},
    new ResourceLanguageDto { ResourceId = resources[10].Id, LanguageCode="fa", Name="منابع"},
    new ResourceLanguageDto { ResourceId = resources[10].Id, LanguageCode="en", Name="Resource Management"},
    new ResourceLanguageDto { ResourceId = resources[11].Id, LanguageCode="fa", Name="صفحه منابع"},
    new ResourceLanguageDto { ResourceId = resources[11].Id, LanguageCode="en", Name="Resource Management Page"},
    new ResourceLanguageDto { ResourceId = permissionMenu.Id, LanguageCode="fa", Name="مدیریت دسترسی‌ها"},
new ResourceLanguageDto { ResourceId = permissionMenu.Id, LanguageCode="en", Name="Permission Management"},
new ResourceLanguageDto { ResourceId = permissionPage.Id, LanguageCode="fa", Name="صفحه مدیریت دسترسی‌ها"},
new ResourceLanguageDto { ResourceId = permissionPage.Id, LanguageCode="en", Name="Permission Management Page"},

};

            // --- Insert ResourceLanguages ---
            foreach (var rl in resourceLanguages)
            {
                var existing = _dbType == DatabaseType.SqlServer
                    ? await _resourceLanguageService.FirstOrDefaultAsync<SqlResourceLanguage>(x => x.ResourceId == rl.ResourceId && x.LanguageCode == rl.LanguageCode)
                    : await _resourceLanguageService.FirstOrDefaultAsync<PgResourceLanguage>(x => x.ResourceId == rl.ResourceId && x.LanguageCode == rl.LanguageCode);

                if (existing.Data == null)
                {
                    await _resourceLanguageService.CreateAsync(rl);
                }
            }

        }
        public async Task SeedLocalizationAsync()
        {
            // ---------------- مرحله 0: مسیر Resources ----------------
            string resourcesPath;
            if (_env.IsDevelopment())
            {
                var projectRoot = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "Velora.Application.Shared");
                resourcesPath = Path.Combine(projectRoot, "Resources");
            }
            else
            {
                var assemblyFolder = Path.GetDirectoryName(typeof(LocalizationkeyDto).Assembly.Location)!;
                resourcesPath = Path.Combine(assemblyFolder, "Resources");
            }
            resourcesPath = Path.GetFullPath(resourcesPath);

            if (!Directory.Exists(resourcesPath))
                throw new DirectoryNotFoundException($"Resources folder not found: {resourcesPath}");

            var resxFiles = Directory.GetFiles(resourcesPath, "*.resx", SearchOption.TopDirectoryOnly);

            // ---------------- مرحله 1: خواندن همه فایل‌ها و merge بر اساس زبان ----------------
            var langKeyValues = new Dictionary<string, Dictionary<string, string>>(); // langCode -> (key -> value)
            foreach (var file in resxFiles)
            {
                var fileName = Path.GetFileNameWithoutExtension(file); // Localization.en یا Column.Role.1.Name.en
                var parts = fileName.Split('.');
                if (parts.Length < 2) continue;

                string langCode = parts[^1]; // آخرین بخش: en, fa, ar
                if (!langKeyValues.ContainsKey(langCode))
                    langKeyValues[langCode] = new Dictionary<string, string>();

                foreach (var kvp in ReadResxFile(file))
                {
                    langKeyValues[langCode][kvp.Key] = kvp.Value; // آخرین مقدار overwrite می‌کند
                }
            }

            // ---------------- مرحله 2: دریافت کلیدهای و ترجمه‌های موجود ----------------
            var existingKeys = (await _localizationkeyService.GetAllAsync()).Data
                               .ToDictionary(x => x.Code, x => x); // Code -> LocalizationKeyDto

            var existingTranslations = (await _localizationtranslationService.GetAllAsync())
                                       .Data
                                       .Select(x => (x.LocalizationKeyCode, x.LanguageCode))
                                       .ToHashSet();

            // ---------------- مرحله 3: insert کلیدها و ترجمه‌ها ----------------
            if (!langKeyValues.ContainsKey("en"))
                throw new InvalidOperationException("File resx انگلیسی پیدا نشد.");

            foreach (var kvp in langKeyValues["en"])
            {
                string key = kvp.Key;
                string enValue = kvp.Value;

                // مقدار فارسی از dictionary fa یا fallback به انگلیسی
                string faValue = langKeyValues.ContainsKey("fa") && langKeyValues["fa"].ContainsKey(key)
                                 ? langKeyValues["fa"][key]
                                 : enValue;

                // ---------------- کلید جدید ----------------
                if (!existingKeys.ContainsKey(key))
                {
                    var type = key.StartsWith("Column.") ? "Column" :
                               key.StartsWith("Button.") ? "Button" :
                               key.StartsWith("Message.") ? "Message" :
                               key.StartsWith("System.") ? "System" :
                                key.StartsWith("Form.") ? "Form" :
                               "Other";

                    // استخراج order از key
                    int? order = null;
                    var parts = key.Split('.');
                    if (parts.Length >= 4 && int.TryParse(parts[2], out int parsedOrder))
                        order = parsedOrder;

                    var keyDto = new LocalizationkeyDto
                    {
                        Code = key,
                        Type = type,
                        IsTest = false,
                        Order = order
                    };

                    var createdKey = await _localizationkeyService.CreateAsync(keyDto);
                    keyDto.Id = createdKey.Data.Id;
                    existingKeys[key] = keyDto;
                }

                // ---------------- ترجمه انگلیسی ----------------
                if (!existingTranslations.Contains((key, "en")))
                {
                    await _localizationtranslationService.CreateAsync(new LocalizationtranslationDto
                    {
                        LocalizationKeyCode = key,
                        LanguageCode = "en",
                        Value = enValue,
                        IsTest = false
                    });
                    existingTranslations.Add((key, "en"));
                }

                // ---------------- ترجمه فارسی ----------------
                if (!existingTranslations.Contains((key, "fa")))
                {
                    await _localizationtranslationService.CreateAsync(new LocalizationtranslationDto
                    {
                        LocalizationKeyCode = key,
                        LanguageCode = "fa",
                        Value = faValue,
                        IsTest = false
                    });
                    existingTranslations.Add((key, "fa"));
                }
            }

            // ---------------- مرحله 4: سایر زبان‌ها ----------------
            foreach (var langCode in langKeyValues.Keys.Where(l => l != "en" && l != "fa"))
            {
                foreach (var kvp in langKeyValues[langCode])
                {
                    string key = kvp.Key;
                    string value = kvp.Value;

                    if (!existingKeys.ContainsKey(key))
                    {
                        // ایجاد LocalizationKey اگر وجود نداشت
                        var type = key.StartsWith("Column.") ? "Column" :
                                   key.StartsWith("Button.") ? "Button" :
                                   key.StartsWith("Message.") ? "Message" :
                                   key.StartsWith("System.") ? "System" :
                                    key.StartsWith("Form.") ? "Form" :
                                   "Other";

                        int? order = null;
                        var parts = key.Split('.');
                        if (parts.Length >= 4 && int.TryParse(parts[2], out int parsedOrder))
                            order = parsedOrder;

                        var keyDto = new LocalizationkeyDto
                        {
                            Code = key,
                            Type = type,
                            IsTest = false,
                            Order = order
                        };

                        var createdKey = await _localizationkeyService.CreateAsync(keyDto);
                        keyDto.Id = createdKey.Data.Id;
                        existingKeys[key] = keyDto;
                    }

                    if (!existingTranslations.Contains((key, langCode)))
                    {
                        await _localizationtranslationService.CreateAsync(new LocalizationtranslationDto
                        {
                            LocalizationKeyCode = key,
                            LanguageCode = langCode,
                            Value = value,
                            IsTest = false
                        });
                        existingTranslations.Add((key, langCode));
                    }
                }
            }

            // ✅ commit فقط در SeedAllAsync انجام شود
        }

        public async Task SeedSettingsAsync()
        {
            // بررسی DefaultLanguage
            var defaultLang = await _generalSettingService.GetByKeyAsync("DefaultLanguage");
            if (defaultLang == null)
            {
                await _generalSettingService.CreateAsync(new GeneralSettingDto
                {
                    Key = "DefaultLanguage",
                    Value = "en",
                    Description = "System default language"
                });
            }

            // بررسی AvailableLanguages
            var availableLangs = await _generalSettingService.GetByKeyAsync("AvailableLanguages");
            if (availableLangs == null)
            {
                await _generalSettingService.CreateAsync(new GeneralSettingDto
                {
                    Key = "AvailableLanguages",
                    Value = "en,fa",
                    Description = "Available system languages"
                });
            }
        }



        private Dictionary<string, string> ReadResxFile(string filePath)
        {
            var dict = new Dictionary<string, string>();
            var doc = XDocument.Load(filePath);

            foreach (var data in doc.Descendants("data"))
            {
                var key = data.Attribute("name")?.Value;
                var value = data.Element("value")?.Value;

                if (!string.IsNullOrEmpty(key) && value != null)
                {
                    dict[key] = value;
                }
            }

            return dict;
        }

        public async Task SeedResourcesAsync()
        {
            // بررسی وجود ResourceType.FIELD و ایجاد در صورت نبود
            var fieldType = _dbType == DatabaseType.SqlServer
                 ? await _resourceTypeService.FirstOrDefaultAsync<SqlResourceType>(x => x.Code == "FIELD")
                 : await _resourceTypeService.FirstOrDefaultAsync<PgResourcetype>(x => x.Code == "FIELD");
            if (fieldType.Data == null)
            {
                var createdFieldType = await _resourceTypeService.CreateAsync(new ResourceTypeDto
                {
                    Code = "FIELD",
                    Name = "Field",
                    DisplayName = "Field"
                });
                fieldType.Data = createdFieldType.Data;
            }
            // ---------------- مرحله 0: تعیین مسیر Resources ----------------
            string resourcesPath;
            if (_env.IsDevelopment())
            {
                var projectRoot = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "Velora.Application.Shared");
                resourcesPath = Path.Combine(projectRoot, "Resources", "FormResources");
            }
            else
            {
                // Production یا Publish شده
                var assemblyFolder = Path.GetDirectoryName(typeof(LocalizationkeyDto).Assembly.Location)!;
                resourcesPath = Path.Combine(assemblyFolder, "Resources", "FormResources");
            }
            resourcesPath = Path.GetFullPath(resourcesPath);

            if (!Directory.Exists(resourcesPath))
                Directory.CreateDirectory(resourcesPath);

            // ---------------- مرحله 1: گرفتن همه DTO ها ----------------
            var dtoAssembly = typeof(RoleDto).Assembly;
            var dtoTypes = dtoAssembly.GetTypes()
                .Where(t => t.Namespace == "Velora.Application.Shared.Dtos" && t.IsClass);

            foreach (var dto in dtoTypes)
            {
                // بررسی وجود ResourceColumnAttribute روی پراپرتی‌ها
                var properties = dto.GetProperties()
                    .Select(p => new
                    {
                        Property = p,
                        Attribute = p.GetCustomAttribute<ResourceColumnAttribute>()
                    })
                    .Where(x => x.Attribute != null)
                    .ToList();

                if (!properties.Any()) continue;

                // ---------------- مرحله 2: ایجاد Resource ----------------
                foreach (var prop in properties)
                {
                    string resourceCode = $"{dto.Name.Replace("Dto", "").Replace("Crud", "")}.{prop.Property.Name}";

                    ResourceDto resourceDto;

                    var existing = _dbType == DatabaseType.SqlServer
                        ? await _resourceService.FirstOrDefaultAsync<SqlResource>(x => x.Code == resourceCode)
                        : await _resourceService.FirstOrDefaultAsync<PgResource>(x => x.Code == resourceCode);

                    if (existing.Data != null)
                    {
                        // UPDATE
                        existing.Data.ResourceTypeId = fieldType.Data.Id;
                        existing.Data.DisplayName = prop.Property.Name;
                        existing.Data.Description = prop.Attribute.Description;
                        existing.Data.Order = prop.Attribute.GridOrder;
                        existing.Data.FieldType = prop.Attribute.FieldType;
                        existing.Data.FormOrder = prop.Attribute.FormOrder;
                        existing.Data.GridOrder = prop.Attribute.GridOrder;
                        existing.Data.IsRequired = prop.Attribute.IsRequired;
                        existing.Data.MaxLength = prop.Attribute.MaxLength;
                        existing.Data.ShowInForm = prop.Attribute.ShowInForm;
                        existing.Data.ShowInGrid = prop.Attribute.ShowInGrid;
                        existing.Data.IsActive = true;
                        existing.Data.InputMask = prop.Attribute.InputMask;
                        existing.Data.LinkedFieldCode = prop.Attribute.LinkedFieldCode;
                        existing.Data.Route = prop.Attribute.Route;
                        existing.Data.ShowInSelectBox = prop.Attribute.ShowInSelectBox;
                        existing.Data.SelectBoxOrder = prop.Attribute.SelectBoxOrder;
                        existing.Data.EntityName = prop.Attribute.EntityName;
                        existing.Data.ServiceName = prop.Attribute.ServiceName;
                        existing.Data.SelectDisplayFields = prop.Attribute.SelectDisplayFields;

                        await _resourceService.UpdateAsync(existing.Data, existing.Data.Id);

                        resourceDto = existing.Data;
                    }
                    else
                    {
                        // CREATE
                        var created = await _resourceService.CreateAsync(new ResourceDto
                        {
                            ResourceTypeId = fieldType.Data.Id,
                            Code = resourceCode,
                            DisplayName = prop.Property.Name,
                            Description = prop.Attribute.Description,
                            Order = prop.Attribute.GridOrder,
                            FieldType = prop.Attribute.FieldType,
                            FormOrder = prop.Attribute.FormOrder,
                            GridOrder = prop.Attribute.GridOrder,
                            IsRequired = prop.Attribute.IsRequired,
                            MaxLength = prop.Attribute.MaxLength,
                            ShowInForm = prop.Attribute.ShowInForm,
                            ShowInGrid = prop.Attribute.ShowInGrid,
                            IsActive = true,
                            InputMask = prop.Attribute.InputMask,
                            LinkedFieldCode = prop.Attribute.LinkedFieldCode,
                            Route = prop.Attribute.Route,
                            ShowInSelectBox = prop.Attribute.ShowInSelectBox,
                            SelectBoxOrder = prop.Attribute.SelectBoxOrder,
                            SelectDisplayFields = prop.Attribute.SelectDisplayFields,
                        });

                        resourceDto = created.Data;
                    }

                    // --- IMPORTANT: اجرای ترجمه‌ها برای CREATE و UPDATE ---
                    await SeedResourceTranslationsAsync(dto.Name, prop.Property.Name, resourceDto.Id, resourcesPath);
                }

            }
        }

        private async Task SeedResourceTranslationsAsync(string dtoName, string propName, Guid resourceId, string resourcesPath)
        {
            var translationKey = $"{dtoName.Replace("Dto", "").Replace("Crud", "")}.{propName}";

            var allFiles = Directory.GetFiles(resourcesPath, "*.resx", SearchOption.AllDirectories);
            var resxFiles = allFiles
                .Where(f => Path.GetFileName(f).StartsWith(dtoName.Replace("Dto", "").Replace("Crud", "")))
                .ToArray();

            foreach (var file in resxFiles)
            {
                var fileName = Path.GetFileNameWithoutExtension(file);
                var parts = fileName.Split('.');
                if (parts.Length < 2) continue;

                var langCode = parts[^1];
                var translations = ReadResxFile(file);

                if (!translations.TryGetValue(translationKey, out var value))
                    continue;

                var existingLang = _dbType == DatabaseType.SqlServer
                    ? await _resourceLanguageService.FirstOrDefaultAsync<SqlResourceLanguage>(x => x.ResourceId == resourceId && x.LanguageCode == langCode)
                    : await _resourceLanguageService.FirstOrDefaultAsync<PgResourceLanguage>(x => x.ResourceId == resourceId && x.LanguageCode == langCode);

                if (existingLang.Data != null)
                {
                    // آپدیت ترجمه موجود
                    existingLang.Data.Name = value;
                    await _resourceLanguageService.UpdateAsync(existingLang.Data, existingLang.Data.Id);
                    continue;
                }

                // ایجاد ترجمه جدید
                await _resourceLanguageService.CreateAsync(new ResourceLanguageDto
                {
                    ResourceId = resourceId,
                    LanguageCode = langCode,
                    Name = value
                });
            }
        }

        public async Task SeedPermissionsAsync()
        {
            // -------- Production: فقط یکبار --------
            if (_env.IsProduction())
            {
                var executed = await _seedHistoryService.GetByNameAsync(SeederName);
                if (executed!=null)
                    return;
            }

            var controllers = Assembly.GetEntryAssembly()!
                .GetTypes()
                .Where(t =>
                    t.IsClass &&
                    !t.IsAbstract &&
                    typeof(ControllerBase).IsAssignableFrom(t) &&
                    t.Namespace == "Velora.Host.Controllers");

            foreach (var controller in controllers)
            {
                var controllerAttr = controller.GetCustomAttribute<AuthorizeResourceAttribute>();
                if (controllerAttr == null)
                    continue;

                var actions = controller.GetMethods(BindingFlags.Instance | BindingFlags.Public)
                    .Where(m =>
                        !m.IsDefined(typeof(NonActionAttribute)) &&
                        m.GetCustomAttribute<HttpMethodAttribute>() != null);

                foreach (var action in actions)
                {
                    var actionAttr = action.GetCustomAttribute<AuthorizeResourceAttribute>() ?? controllerAttr;

                    await SyncActionAsync(
                        controller.Name.Replace("Controller", ""),
                        action.Name,
                        actionAttr.Roles.ToList()
                    );
                }
            }

            if (_env.IsProduction())
            {
                await _seedHistoryService.CreateAsync(new() { Name=SeederName,CreatedAt=DateTime.Now});
            }

            await _transactionService.CommitAsync();
        }

        private async Task SyncActionAsync(string controller, string action, List<string> roles)
        {
            var resourceTypeExisting = _dbType == DatabaseType.SqlServer
 ? await _resourceTypeService.FirstOrDefaultAsync<SqlResourceType>(x => x.Code.ToUpper() == "Action".ToUpper())
 : await _resourceTypeService.FirstOrDefaultAsync<PgResourcetype>(x => x.Code.ToUpper() == "Action".ToUpper());
            var resourceCode = $"API.{controller}.{action}";

            // ---------- Resource ----------
            var resource = await _resourceService.GetByCodeAsync(resourceCode);
            if (resource == null)
            {
                var created = await _resourceService.CreateAsync(new ResourceDto
                {
                    Code = resourceCode,
                    Name = action,
                    ResourceTypeId= resourceTypeExisting.Data.Id,
                    DisplayName = $"{controller} {action}",
                    IsActive = true
                });

                resource = created.Data;
            }

            // ---------- Permission ----------
            var permission = await _permissionService.GetByResourceIdAsync(resource.Id);
            if (permission == null)
            {
                var created = await _permissionService.CreateAsync(new PermissionDto
                {
                    Actions=(int)Permission.All,
                    ResourceId = resource.Id,
                    IsActive = true
                });

                permission = created.Data;
            }

            // ---------- RolePermission Sync ----------
            var roleIds = (await _roleService.GetByNamesAsync(roles))
                .Select(r => r.Id)
                .ToHashSet();

            var existing = await _rolePermissionService.GetByPermissionRolesAsync(permission.Id);

            // حذف roleهای اضافه
            foreach (var rp in existing.Where(x => !roleIds.Contains(x.RoleId)))
            {
                await _rolePermissionService.DeleteAsync(rp.Id);
            }

            // اضافه کردن roleهای جدید
            foreach (var roleId in roleIds)
            {
                if (!existing.Any(x => x.RoleId == roleId))
                {
                    await _rolePermissionService.CreateAsync(new RolePermissionDto
                    {
                        RoleId = roleId,
                        PermissionId = permission.Id
                    });
                }
            }
        }

        private async Task<bool> ShouldRunSeederAsync(string seederName)
        {
            if (_env.IsDevelopment())
                return true;

            var history = await _seedHistoryService.GetByNameAsync(seederName);
            return history == null;
        }



    }
}

