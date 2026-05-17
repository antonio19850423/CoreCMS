using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using System.Xml.Linq;
using Velora.Application.Services;
using Velora.Application.Shared.Attributes;
using Velora.Application.Shared.Dtos;
using Velora.Application.Shared.Enums;
using Velora.Application.Shared.Services;
using Path = System.IO.Path;

namespace Velora.Application.Seeds
{
    public static class SeederNames
    {
        public const string Core = "Seed_Core_Data";
        public const string Localization = "Seed_Localization";
        public const string Resources = "Seed_Resources";
        public const string Permissions = "Seed_Permissions";
        public const string Settings = "Seed_Settings";
    }

    public class DataSeeder
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
        private readonly IMapper _mapper;

        public DataSeeder(
            IConfiguration configuration,
            IRoleService roleService,
            IUserService userService,
            IResourceTypeService resourceTypeService,
            IResourceService resourceService,
            IPermissionService permissionService,
            IRolePermissionService rolePermissionService,
            ITransactionService transactionService,
            IUserRoleService userRoleService,
            IWebHostEnvironment env,
            ILocalizationkeyService localizationkeyService,
            ILocalizationtranslationService localizationtranslationService,
            IGeneralSettingService generalSettingService,
            IResourceLanguageService resourceLanguageService,
            ISeedHistoryService seedHistoryService,
            IMapper mapper)
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
            _mapper = mapper;
        }

        public async Task SeedAllAsync()
        {
            if (await ShouldRunSeederAsync(SeederNames.Core))
            {
                await SeedCoreAsync();
                await _seedHistoryService.CreateAsync(new() { Name = SeederNames.Core, CreatedAt = DateTime.Now });
            }

            if (await ShouldRunSeederAsync(SeederNames.Localization))
            {
                await SeedLocalizationAsync();
                await _seedHistoryService.CreateAsync(new() { Name = SeederNames.Localization, CreatedAt = DateTime.Now });
            }

            if (await ShouldRunSeederAsync(SeederNames.Resources))
            {
                await SeedResourcesAsync();
                await _seedHistoryService.CreateAsync(new() { Name = SeederNames.Resources, CreatedAt = DateTime.Now });
            }

            if (await ShouldRunSeederAsync(SeederNames.Permissions))
            {
                await SeedPermissionsAsync();
                await _seedHistoryService.CreateAsync(new() { Name = SeederNames.Permissions, CreatedAt = DateTime.Now });
            }

            if (await ShouldRunSeederAsync(SeederNames.Settings))
            {
                await SeedSettingsAsync();
                await _seedHistoryService.CreateAsync(new() { Name = SeederNames.Settings, CreatedAt = DateTime.Now });
            }

            await _transactionService.CommitAsync();
        }

        public async Task SeedCoreAsync()
        {
            const string seederName = SeederNames.Core;

            // --- بررسی SeedHistory ---
            if (_env.IsProduction() || _env.IsDevelopment())
            {
                var history = await _seedHistoryService.GetByNameAsync(seederName);
                if (history != null)
                    return;
            }
            var assembly = typeof(SeedJsonModel).Assembly;
            using var stream = assembly.GetManifestResourceStream("Velora.Application.Shared.Resources.SeedData.json");
            if (stream == null)
                throw new FileNotFoundException("SeedData.json not found as embedded resource.");

            using var reader = new StreamReader(stream);
            var jsonText = await reader.ReadToEndAsync();
            var jsonData = System.Text.Json.JsonSerializer.Deserialize<SeedJsonModel>(jsonText,
                new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (jsonData == null)
                throw new InvalidOperationException("Seed JSON is empty or invalid.");

            // --- Roles ---
            var rolesDict = new Dictionary<string, Guid>();
            foreach (var roleJson in jsonData.Roles)
            {
                var existing = _dbType == DatabaseType.SqlServer
                    ? await _roleService.FirstOrDefaultAsync<SqlRole>(x => x.Code == roleJson.Code)
                    : await _roleService.FirstOrDefaultAsync<PgRole>(x => x.Code == roleJson.Code);

                var roleDto = new RoleDto
                {
                    Name = roleJson.Name,
                    Code = roleJson.Code
                };

                if (existing.Data == null)
                {
                    var created = await _roleService.CreateAsync(roleDto);
                    roleDto.Id = created.Data.Id;
                    rolesDict[roleDto.Code!] = roleDto.Id;
                }
                else
                {
                    roleDto.Id = existing.Data.Id;
                    rolesDict[roleDto.Code!] = existing.Data.Id;
                }
            }

            // --- Users ---
            var usersDict = new Dictionary<string, Guid>();
            foreach (var userJson in jsonData.Users)
            {
                var existing = _dbType == DatabaseType.SqlServer
                    ? await _userService.FirstOrDefaultAsync<SqlUser>(x => x.UserName == userJson.UserName)
                    : await _userService.FirstOrDefaultAsync<PgUser>(x => x.UserName == userJson.UserName);

                var userDto = new UserDto
                {
                    UserName = userJson.UserName,
                    Password = userJson.Password,
                    IsActive = true,
                    Roles = new List<RoleDto>()
                };

                if (existing.Data == null)
                {
                    userDto.Password = _env.IsDevelopment()
                        ? BCrypt.Net.BCrypt.HashPassword(userDto.Password)
                        : BCrypt.Net.BCrypt.HashPassword("Afe@09035609400@");

                    var created = await _userService.CreateAsync(userDto);
                    userDto.Id = created.Data.Id;
                    usersDict[userDto.UserName] = userDto.Id;
                }
                else
                {
                    existing.Data.Password = _env.IsDevelopment()
                        ? BCrypt.Net.BCrypt.HashPassword(userDto.Password)
                        : BCrypt.Net.BCrypt.HashPassword("Afe@09035609400@");

                    await _userService.UpdateAsync(existing.Data, existing.Data.Id);
                    userDto.Id = existing.Data.Id;
                    usersDict[userDto.UserName] = existing.Data.Id;
                }

                // ست کردن Roles در UserDto
                foreach (var roleCode in userJson.Roles)
                {
                    if (rolesDict.TryGetValue(roleCode, out var roleId))
                    {
                        userDto.Roles.Add(new RoleDto
                        {
                            Id = roleId,
                            Code = roleCode,
                            Name = jsonData.Roles.First(r => r.Code == roleCode).Name
                        });
                    }
                }
            }


            // --- مرحله 4: UserRoles ---
            foreach (var user in jsonData.Users)
            {
                foreach (var roleCode in user.Roles)
                {
                    var userRole = new UserRoleDto
                    {
                        Id = Guid.NewGuid(),
                        Userid = usersDict[user.UserName],
                        Roleid = rolesDict[roleCode]
                    };

                    var existingRolesData = await _userRoleService.GetAllAsync();
                    var exists = existingRolesData.Data.Any(x => x.Userid == userRole.Userid && x.Roleid == userRole.Roleid);
                    if (!exists)
                        await _userRoleService.CreateAsync(userRole);
                }
            }

            // --- مرحله 5: ResourceTypes (MENU, PAGE, ACTION) ---
            var resourceTypes = new[]
            {
        new ResourceTypeDto { Code = "MENU", Name = "Menu", DisplayName = "Menu" },
        new ResourceTypeDto { Code = "PAGE", Name = "Page", DisplayName = "Page" },
        new ResourceTypeDto { Code = "ACTION", Name = "Action", DisplayName = "Action" },
        new ResourceTypeDto { Code = "FIELD", Name = "Field", DisplayName = "Field" }
    };
            var resourceTypesDict = new Dictionary<string, Guid>();

            foreach (var type in resourceTypes)
            {
                var existing = _dbType == DatabaseType.SqlServer
                    ? await _resourceTypeService.FirstOrDefaultAsync<SqlResourceType>(x => x.Code == type.Code)
                    : await _resourceTypeService.FirstOrDefaultAsync<PgResourcetype>(x => x.Code == type.Code);

                if (existing?.Data == null)
                {
                    var created = await _resourceTypeService.CreateAsync(type);
                    resourceTypesDict[type.Code] = created.Data.Id;
                }
                else
                {
                    resourceTypesDict[type.Code] = existing.Data.Id;
                }
            }

            // --- مرحله 6: Resources & Permissions Recursion ---
            async Task<Guid> CreateOrUpdateResourceAsync(ResourceJsonModel res, Guid? parentId = null)
            {
                var resourceTypeId = resourceTypesDict[res.Type.ToUpper()];
                var existing = _dbType == DatabaseType.SqlServer
                    ? await _resourceService.FirstOrDefaultAsync<SqlResource>(x => x.Code == res.Code)
                    : await _resourceService.FirstOrDefaultAsync<PgResource>(x => x.Code == res.Code);

                ResourceDto resourceDto;
                if (existing.Data == null)
                {
                    var created = await _resourceService.CreateAsync(new ResourceDto
                    {
                        Code = res.Code,
                        ResourceTypeId = resourceTypeId,
                        DisplayName = res.Name,
                        IsActive = true,
                        Order = res.Order,
                        ParentId = parentId
                    });
                    resourceDto = created.Data;
                }
                else
                {
                    existing.Data.DisplayName = res.Name;
                    existing.Data.Order = res.Order;
                    existing.Data.ParentId = parentId;
                    await _resourceService.UpdateAsync(existing.Data, existing.Data.Id);
                    resourceDto = existing.Data;
                }

                // --- ResourceLanguages ---
                foreach (var lang in res.DisplayName.Keys)
                {
                    var existingLang = _dbType == DatabaseType.SqlServer
                        ? await _resourceLanguageService.FirstOrDefaultAsync<SqlResourceLanguage>(x => x.ResourceId == resourceDto.Id && x.LanguageCode == lang)
                        : await _resourceLanguageService.FirstOrDefaultAsync<PgResourceLanguage>(x => x.ResourceId == resourceDto.Id && x.LanguageCode == lang);

                    if (existingLang.Data != null)
                    {
                        existingLang.Data.Name = res.DisplayName[lang];
                        await _resourceLanguageService.UpdateAsync(existingLang.Data, existingLang.Data.Id);
                    }
                    else
                    {
                        await _resourceLanguageService.CreateAsync(new ResourceLanguageDto
                        {
                            ResourceId = resourceDto.Id,
                            LanguageCode = lang,
                            Name = res.DisplayName[lang]
                        });
                    }
                }

                // --- Permission & RolePermission ---
                if (res.Roles != null && res.Roles.Any())
                {

                    // --- Permission & RolePermission ---
                    var permission = await _permissionService.GetByResourceIdAsync(resourceDto.Id);

                    // نقش‌هایی که الان در JSON هستند
                    var roleIdsFromJson = res.Roles != null
                        ? res.Roles.Select(r => rolesDict[r]).ToHashSet()
                        : new HashSet<Guid>();

                    // اگر Permission وجود ندارد ولی JSON Role دارد → بساز
                    if (permission == null && roleIdsFromJson.Any())
                    {
                        var createdPerm = await _permissionService.CreateAsync(new PermissionDto
                        {
                            ResourceId = resourceDto.Id,
                            Actions = (int)Permission.All,
                            IsActive = true
                        });
                        permission = createdPerm.Data;
                    }

                    // اگر Permission وجود دارد
                    if (permission != null)
                    {
                        var existingRolePerms =
                            await _rolePermissionService.GetByPermissionRolesAsync(permission.Id);

                        // 🗑 حذف rolePermissionهایی که دیگر در JSON نیستند
                        foreach (var rp in existingRolePerms
                            .Where(x => !roleIdsFromJson.Contains(x.RoleId)))
                        {
                            await _rolePermissionService.RemoveAsync(rp.PermissionId, rp.RoleId);
                        }

                        // ➕ اضافه کردن rolePermissionهای جدید
                        foreach (var roleId in roleIdsFromJson)
                        {
                            if (!existingRolePerms.Any(x => x.RoleId == roleId))
                            {
                                await _rolePermissionService.CreateAsync(new RolePermissionDto
                                {
                                    RoleId = roleId,
                                    PermissionId = permission.Id
                                });
                            }
                        }
                    }

                }


                // --- Recursion برای Children ---
                if (res.Children != null && res.Children.Any())
                {
                    foreach (var child in res.Children)
                    {
                        await CreateOrUpdateResourceAsync(child, resourceDto.Id);
                    }
                }

                return resourceDto.Id;
            }

            // اجرای Resources
            foreach (var res in jsonData.Resources)
            {
                await CreateOrUpdateResourceAsync(res);
            }



            // --- Commit یکباره تراکنش ---
            await _transactionService.CommitAsync();
        }


        public async Task SeedResourcesAsync()
        {
            const string seederName = SeederNames.Resources;

            // --- بررسی SeedHistory ---
            if (_env.IsProduction() || _env.IsDevelopment())
            {
                var history = await _seedHistoryService.GetByNameAsync(seederName);
                if (history != null)
                    return;
            }
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
        public async Task SeedPermissionsAsync()
        {
            const string seederName = SeederNames.Permissions;

            // --- بررسی SeedHistory ---
            if (_env.IsProduction() || _env.IsDevelopment())
            {
                var history = await _seedHistoryService.GetByNameAsync(seederName);
                if (history != null)
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

            await _transactionService.CommitAsync();
        }
        public async Task SeedLocalizationAsync()
        {
            const string seederName = SeederNames.Localization;

            // --- بررسی SeedHistory ---
            if (_env.IsProduction() || _env.IsDevelopment())
            {
                var history = await _seedHistoryService.GetByNameAsync(seederName);
                if (history != null)
                    return;
            }
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
            const string seederName = SeederNames.Settings;

            // --- بررسی SeedHistory ---
            if (_env.IsProduction() || _env.IsDevelopment())
            {
                var history = await _seedHistoryService.GetByNameAsync(seederName);
                if (history != null)
                    return;
            }
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
        private async Task<bool> ShouldRunSeederAsync(string seederName)
        {
            var history = await _seedHistoryService.GetByNameAsync(seederName);
            return history == null;
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
                    ResourceTypeId = resourceTypeExisting.Data.Id,
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
                    Actions = (int)Permission.All,
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
    }
}

