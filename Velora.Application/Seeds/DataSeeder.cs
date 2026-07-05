using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.OData.Edm;
using System.Reflection;
using System.Text.Json;
using System.Xml.Linq;
using Velora.Application.Services;
using Velora.Application.Shared.Attributes;
using Velora.Application.Shared.Constants;
using Velora.Application.Shared.Dtos;
using Velora.Application.Shared.Enums;
using Velora.Application.Shared.Services;
using Velora.EntityFrameworkCore.EntityFramework.SqlServer;
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


		public const string Core_SiteSettings = "Seed_Core_SiteSettings";
		public const string Core_CmsConfiguration = "Seed_Core_CmsConfiguration";
		public const string Seed_Core_Template = "Seed_Core_Template";
		public const string Seed_Core_SectionGroupItem = "Seed_Core_SectionGroupItem";
		public const string Seed_Core_LinkTypes = "Seed_Core_LinkTypes";
        public const string SeedCmsNewsAndArticlePagesAsync = "SeedCmsNewsAndArticlePagesAsync";
        

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
		private readonly ISiteSettingService _siteSettingService;
		private readonly IPageService _pageService;
		private readonly ISectionService _sectionService;
		private readonly ISectionItemService _sectionItemService;
		private readonly IConfiguration _configuration;
		private readonly IComponentTypeService _componentTypeService;
		private readonly ISectionGroupItemService _sectionGroupItemService;
		private readonly ILinkTypeService _linkTypeService;
		ICmsConfigurationService _cmsConfigurationService;
        private readonly IContentItemService _contentItemService;
        private readonly IContentCategoryService _contentCategoryService;
        private readonly IContentItemTagService _contentItemTagService;
        private readonly ITagService _tagService;
        



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
			ISiteSettingService siteSettingService,
			ICmsConfigurationService cmsConfigurationService,
			ISectionGroupItemService sectionGroupItemService,
			IMapper mapper, IPageService pageService, ISectionService sectionService, IComponentTypeService componentTypeService, ISectionItemService sectionItemService, ILinkTypeService linkTypeService, IContentItemService contentItemService, IContentCategoryService contentCategoryService,IContentItemTagService contentItemTagService,ITagService tagService)
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
			_siteSettingService = siteSettingService;
			_cmsConfigurationService = cmsConfigurationService;
			_pageService = pageService;
			_sectionService = sectionService;
			_configuration = configuration;
			_componentTypeService = componentTypeService;
			_sectionItemService = sectionItemService;
			_sectionGroupItemService = sectionGroupItemService;
			_linkTypeService = linkTypeService;
			_contentItemService = contentItemService;
			_contentCategoryService= contentCategoryService;
			_contentItemTagService = contentItemTagService;
			_tagService= tagService;
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

			if (await ShouldRunSeederAsync(SeederNames.Seed_Core_LinkTypes))
			{
				await SeedCoreLinkTypesAsync();
				await _seedHistoryService.CreateAsync(new()
				{
					Name = SeederNames.Seed_Core_LinkTypes,
					CreatedAt = DateTime.Now
				});
			}

			if (await ShouldRunSeederAsync(SeederNames.Core_SiteSettings))
			{
				await SeedCoreSiteSettingsAsync();
				await _seedHistoryService.CreateAsync(new()
				{
					Name = SeederNames.Core_SiteSettings,
					CreatedAt = DateTime.Now
				});
			}

			if (await ShouldRunSeederAsync(SeederNames.Core_CmsConfiguration))
			{
				await SeedCoreCmsConfigurationAsync();
				await _seedHistoryService.CreateAsync(new()
				{
					Name = SeederNames.Core_CmsConfiguration,
					CreatedAt = DateTime.Now
				});
			}
			if (await ShouldRunSeederAsync(SeederNames.Seed_Core_Template))
			{
				await SeedCmsTemplateAsync();
				await _seedHistoryService.CreateAsync(new()
				{
					Name = SeederNames.Seed_Core_Template,
					CreatedAt = DateTime.Now
				});
			}
			if (await ShouldRunSeederAsync(SeederNames.Seed_Core_SectionGroupItem))
			{
				await SeedCoreSectionGroupItemAsync();
				await _seedHistoryService.CreateAsync(new()
				{
					Name = SeederNames.Seed_Core_SectionGroupItem,
					CreatedAt = DateTime.Now
				});
			}
            if (await ShouldRunSeederAsync(SeederNames.SeedCmsNewsAndArticlePagesAsync))
            {
                await SeedCmsNewsAndArticlePagesAsync();
                await _seedHistoryService.CreateAsync(new()
                {
                    Name = SeederNames.SeedCmsNewsAndArticlePagesAsync,
                    CreatedAt = DateTime.Now
                });
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
		new ResourceTypeDto { Code = "FIELD", Name = "Field", DisplayName = "Field" },
		new ResourceTypeDto { Code = "TAB", Name = "Tab", DisplayName = "Tab" }
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
						ParentId = parentId,
						Route = res.Route,
					});
					resourceDto = created.Data;
				}
				else
				{
					existing.Data.DisplayName = res.Name;
					existing.Data.Order = res.Order;
					existing.Data.ParentId = parentId;
					existing.Data.Route = res.Route;
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
							Actions = (int)Shared.Enums.Permission.All,
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
					string entityName = !string.IsNullOrWhiteSpace(prop.Attribute.EntityName)
	? prop.Attribute.EntityName
	: dto.Name.Replace("Dto", "").Replace("Crud", "");
					string resourceCode = $"{dto.Name.Replace("Dto", "").Replace("Crud", "")}.{prop.Property.Name}";
					string serviceName =
	char.ToLowerInvariant(entityName[0]) + entityName.Substring(1) + "View";
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
						existing.Data.EntityName = entityName;
						existing.Data.ServiceName = serviceName;
						existing.Data.SelectDisplayFields = prop.Attribute.SelectDisplayFields;
						existing.Data.GroupKey = prop.Attribute.GroupKey;
                        existing.Data.ShowInTreeView = prop.Attribute.ShowInTreeView;
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
							EntityName = entityName,
							ServiceName = serviceName,
							GroupKey= prop.Attribute.GroupKey,
                            ShowInTreeView = prop.Attribute.ShowInTreeView
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
			string entityName = controller.Replace("Controller", "");
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
					IsActive = true,
					EntityName = entityName,
				});

				resource = created.Data;
			}
			else
			{
				resource.Name = action;
				resource.ResourceTypeId = resourceTypeExisting.Data.Id;
				resource.DisplayName = $"{controller} {action}";
				resource.EntityName = entityName;
				resource.IsActive = true;

				await _resourceService.UpdateAsync(resource, resource.Id);
			}

			// ---------- Permission ----------
			var permission = await _permissionService.GetByResourceIdAsync(resource.Id);
			if (permission == null)
			{
				var created = await _permissionService.CreateAsync(new PermissionDto
				{
					Actions = (int)Shared.Enums.Permission.All,
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


		public async Task SeedCoreSiteSettingsAsync()
		{
			const string seederName = SeederNames.Core_SiteSettings;

			if (await _seedHistoryService.GetByNameAsync(seederName) != null)
				return;

			var existing = await _siteSettingService
				.FirstOrDefaultAsync<SqlSiteSetting>(x => x.IsActive);

			if (existing.Data == null)
			{
				await _siteSettingService.CreateAsync(new SiteSettingDto
				{
					SiteName = "CMS پیش‌فرض",
					DomainName = "localhost",

					LogoUrl = "http://localhost:5274/logo/xing.png",
					LogoAlt = "لوگوی سایت",
					DarkLogoUrl = "http://localhost:5274/logo/xing.png",
					DarkLogoAlt = "لوگوی حالت تاریک",
					FaviconUrl = "/assets/favicon.ico",

					PhoneTitle = "شماره تماس",
					Phone = "021-00000000",

					Phone2Title = "شماره تماس دوم",
					Phone2 = "021-11111111",

					MobileTitle = "موبایل",
					Mobile = "09120000000",

					FaxTitle = "فکس",
					Fax = "021-22222222",

					Email = "info@example.com",

					AddressTitle = "آدرس",
					Address = "تهران، ایران",

					Address2Title = "آدرس دوم",
					Address2 = "دفتر دوم",

					DefaultMetaTitle = "CMS پیش‌فرض",
					DefaultMetaDescription = "سیستم مدیریت محتوای پیش‌فرض",
					DefaultMetaKeywords = "cms, website, management",

					IsActive = true
				});
			}
			else
			{
				// 🔥 UPDATE
				existing.Data.SiteName = "CMS پیش‌فرض";
				existing.Data.DomainName = "localhost";

				existing.Data.LogoUrl = "http://localhost:5274/uploads/logo/xing.png";
				existing.Data.LogoAlt = "لوگوی سایت";
				existing.Data.DarkLogoUrl = "http://localhost:5274/uploads/logo/xing.png";
				existing.Data.DarkLogoAlt = "لوگوی حالت تاریک";
				existing.Data.FaviconUrl = "http://localhost:5274/uploads/logo/favicon.ico";

				existing.Data.PhoneTitle = "شماره تماس";
				existing.Data.Phone = "021-00000000";

				existing.Data.Phone2Title = "شماره تماس دوم";
				existing.Data.Phone2 = "021-11111111";

				existing.Data.MobileTitle = "موبایل";
				existing.Data.Mobile = "09120000000";

				existing.Data.FaxTitle = "فکس";
				existing.Data.Fax = "021-22222222";

				existing.Data.Email = "info@example.com";

				existing.Data.AddressTitle = "آدرس";
				existing.Data.Address = "تهران، ایران";

				existing.Data.Address2Title = "آدرس دوم";
				existing.Data.Address2 = "دفتر دوم";

				existing.Data.DefaultMetaTitle = "CMS پیش‌فرض";
				existing.Data.DefaultMetaDescription = "سیستم مدیریت محتوای پیش‌فرض";
				existing.Data.DefaultMetaKeywords = "cms, website, management";

				await _siteSettingService.UpdateAsync(existing.Data, existing.Data.Id);
			}

			await _transactionService.CommitAsync();
		}
		public async Task SeedCoreLinkTypesAsync()
		{
			const string seederName = SeederNames.Seed_Core_LinkTypes;

			if (await _seedHistoryService.GetByNameAsync(seederName) != null)
				return;

			var now = DateTime.Now;

			var seedItems = new List<LinkTypeDto>
							{
						new LinkTypeDto { Code = "PAGE", Name = "صفحه داخلی" ,IsActive=true,SortOrder=1},

						new LinkTypeDto { Code = "EXTERNAL", Name = "لینک خارجی",IsActive=true,SortOrder=2 },

						new LinkTypeDto { Code = "PRODUCT", Name = "محصول" ,IsActive=true,SortOrder=3},

						new LinkTypeDto { Code = "ARTICLE", Name = "مقاله",IsActive=true ,SortOrder=4},

						new LinkTypeDto { Code = "NEWS", Name = "اخبار",IsActive=true,SortOrder=5 }
							};

			foreach (var item in seedItems)
			{
				var existing = await _linkTypeService
					.FirstOrDefaultAsync<SqlLinkType>(x => x.Code == item.Code && !x.IsDeleted);

				if (existing.Data == null)
				{
					// =========================
					// INSERT
					// =========================
					item.CreatedAt = now;

					await _linkTypeService.CreateAsync(item);
				}
				else
				{
					// =========================
					// UPDATE
					// =========================
					existing.Data.Name = item.Name;
					existing.Data.IsActive = true;
					existing.Data.UpdatedAt = now;

					await _linkTypeService.UpdateAsync(
						_mapper.Map<LinkTypeCrud>(existing.Data), existing.Data.Id
                    );
				}
			}

			await _transactionService.CommitAsync();
		}

		public async Task SeedCoreCmsConfigurationAsync()
		{
			const string seederName = SeederNames.Core_CmsConfiguration;

			// جلوگیری از اجرای دوباره seeder
			if (await _seedHistoryService.GetByNameAsync(seederName) != null)
				return;

			var existing = await _cmsConfigurationService
				.FirstOrDefaultAsync<SqlCmsConfiguration>(x => x.IsActive);

			if (existing.Data == null)
			{
				// ✅ CREATE
				await _cmsConfigurationService.CreateAsync(new CmsConfigurationDto
				{
					DefaultTheme = "default",

					EnableBlog = true,
					EnableShop = true,
					EnableNews = true,

					EnableSeo = true,
					EnableCache = true,
					EnableComments = false,
					EnableMultiLanguage = false,

					SiteType = SiteTypes.COMPANY,
					IsActive = true
				});
			}
			else
			{
				// 🔥 UPDATE
				existing.Data.DefaultTheme = "default";

				existing.Data.EnableBlog = true;
				existing.Data.EnableShop = true;
				existing.Data.EnableNews = true;

				existing.Data.EnableSeo = true;
				existing.Data.EnableCache = true;
				existing.Data.EnableComments = false;
				existing.Data.EnableMultiLanguage = false;

				existing.Data.SiteType = SiteTypes.COMPANY;
				existing.Data.IsActive = true;

				await _cmsConfigurationService.UpdateAsync(existing.Data);
			}

			await _transactionService.CommitAsync();
		}

		public async Task SeedCmsTemplateAsync()
		{
            var excludedSlugs = new[] { "news", "articles" };
            var seederName = SeederNames.Seed_Core_Template;
            var existing = await _cmsConfigurationService
    .FirstOrDefaultAsync<SqlCmsConfiguration>(x => x.IsActive);
            if (await _seedHistoryService.GetByNameAsync(seederName) != null)
				return;

			var templateName = _configuration["Cms:DefaultTemplate"];

			if (string.IsNullOrWhiteSpace(templateName))
				throw new Exception("DefaultTemplate is not configured in appsettings");

			var assembly = typeof(SeedJsonModel).Assembly;

			// =====================================================
			// LOAD TEMPLATE
			// =====================================================
			using var templateStream = assembly.GetManifestResourceStream(
				"Velora.Application.Shared.Resources.Templates.json");

			if (templateStream == null)
				throw new FileNotFoundException("Templates.json not found");

			var templateModel = await JsonSerializer.DeserializeAsync<TemplateSeedModel>(
				templateStream,
				new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

			if (templateModel == null)
				throw new Exception("Invalid Template JSON");

			var template = templateModel.Templates
				.FirstOrDefault(x => x.TemplateName == templateName);

			if (template == null)
				throw new Exception($"Template {templateName} not found");

			// =====================================================
			// LOAD COMPONENT RULES
			// =====================================================
			using var componentStream = assembly.GetManifestResourceStream(
				"Velora.Application.Shared.Resources.ComponentRules.json");

			if (componentStream == null)
				throw new FileNotFoundException("ComponentRules.json not found");

			var componentRules = await JsonSerializer.DeserializeAsync<
				Dictionary<string, ComponentRuleModel>>(
				componentStream,
				new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

			if (componentRules == null)
				throw new Exception("Invalid ComponentRules JSON");

			int sortOrder = 1;
			var componentTypeCache = new Dictionary<string, ComponentTypeDto>();

			// =====================================================
			// PAGES LOOP (UPSERT)
			// =====================================================
			foreach (var page in template.Pages)
			{
                // ❌ SKIP special dynamic pages
                if (excludedSlugs.Contains(page.Slug?.ToLower()))
                    continue;
                var cmsConfig = existing?.Data;
                if (page.Slug == "faq" && !(cmsConfig?.EnableFaq ?? false))
                {
                    continue;
                }
                if (page.Slug == "privacy" && !(cmsConfig?.EnablePrivacy ?? false))
                {
                    continue;
                }
                // ================= PAGE UPSERT =================
                var existingPage = _dbType == DatabaseType.SqlServer
					 ? await _pageService.FirstOrDefaultAsync<SqlPage>(x => x.Slug == page.Slug)
					 : await _pageService.FirstOrDefaultAsync<SqlPage>(x => x.Slug == page.Slug);

				var pageEntity = new PageDto
				{
					Name = page.PageName,
					Slug = page.Slug,
					IsDynamic = page.IsDynamic,
					IsPublished = false,
					MetaTitle = page.MetaTitle ?? $"صفحه {page.PageName}",
					MetaDescription = page.MetaDescription ?? $"توضیحات صفحه {page.PageName}",
					MetaKeywords = page.MetaKeywords ?? "آموزشی، نمونه، سایت",
					IsActive=true
				};

				if (existingPage.Data == null)
				{
					var created = await _pageService.CreateAsync(pageEntity);
					pageEntity.Id = created.Data.Id;
				}
				else
				{
					pageEntity.Id = existingPage.Data.Id;
                    pageEntity.IsActive=true;

                }

				// =================================================
				// COMPONENT LOOP
				// =================================================
				int sectionIndex = 1;

				foreach (var component in page.Components)
				{

					if (!componentRules.TryGetValue(component.Code, out var rule))
						continue;

					// ================= COMPONENT TYPE UPSERT =================
					var existingComponentType = _dbType == DatabaseType.SqlServer
						? await _componentTypeService.FirstOrDefaultAsync<SqlComponentType>(x => x.Code == component.Code)
						: await _componentTypeService.FirstOrDefaultAsync<PgComponentType>(x => x.Code == component.Code);

					var componentTypeEntity = new ComponentTypeDto
					{
						Name = component.Code,
						Code = component.Code,
						Type = component.Type,
						IsActive = true
					};

					if (existingComponentType.Data == null)
					{
						var created = await _componentTypeService.CreateAsync(componentTypeEntity);
						componentTypeEntity.Id = created.Data.Id;
					}
					else
					{
						componentTypeEntity.Id = existingComponentType.Data.Id;
					}

					componentTypeCache[component.Code] = componentTypeEntity;

					// ================= SECTION UPSERT =================
					var existingSection = _dbType == DatabaseType.SqlServer
						? await _sectionService.FirstOrDefaultAsync<SqlSection>(
							x => x.PageId == pageEntity.Id &&
								 x.ComponentTypeId == componentTypeEntity.Id)
						: await _sectionService.FirstOrDefaultAsync<SqlSection>(
							x => x.PageId == pageEntity.Id &&
								 x.ComponentTypeId == componentTypeEntity.Id);

					SectionDto sectionEntity;
					var rtl = rule.DefaultData?.Rtl;

					// 🔥 FIXED: deterministic SortOrder instead of sortOrder++
					var currentSectionSortOrder = sectionIndex++;

					if (existingSection.Data == null)
					{
                        sectionEntity = BuildSectionDto(
    pageEntity.Id,
    componentTypeEntity.Id,
    rtl,
    currentSectionSortOrder);

						var created = await _sectionService.CreateAsync(sectionEntity);
						sectionEntity.Id = created.Data.Id;

                        // =========================================
                        // SECTION ITEMS SEED
                        // =========================================
                        int itemSortOrder = 1;
                        var sectionGroupCache = new Dictionary<string, Guid>();
                        foreach (var item in rtl.Items)
                        {

                            Guid? sectionGroupItemId = null;

                            if (!string.IsNullOrWhiteSpace(item.SectionGroupItemCode))
                            {
                                if (!sectionGroupCache.TryGetValue(item.SectionGroupItemCode, out var cachedId))
                                {
                                    var groupResult =
                                        await _sectionGroupItemService.FirstOrDefaultAsync<SqlSectionGroupItem>(
                                            x => x.Code == item.SectionGroupItemCode);

                                    if (groupResult.Data != null)
                                    {
                                        cachedId = groupResult.Data.Id;

                                        sectionGroupCache[item.SectionGroupItemCode] = cachedId;
                                    }
                                }

                                if (cachedId != Guid.Empty)
                                {
                                    sectionGroupItemId = cachedId;
                                }
                            }
                            var existingItem = await _sectionItemService
                                .FirstOrDefaultAsync<SqlSectionItem>(
                                    x => x.SectionId == sectionEntity.Id &&
                                         x.Title == item.Title);

                            var sectionItem = BuildSectionItemDto(
                    sectionEntity.Id,
                    item,
                    sectionGroupItemId,
                    itemSortOrder++);

                            if (existingItem.Data == null)
                            {
                                await _sectionItemService.CreateAsync(sectionItem);
                            }
                            else
                            {
                                sectionItem.Id = existingItem.Data.Id;
                                await _sectionItemService.UpdateAsync(sectionItem, sectionItem.Id);
                            }
                        }
                    }
					else
					{
						sectionEntity = existingSection.Data;
                        // ===============================
                        // 🔥 FULL SECTION UPDATE FROM SEED
                        // ===============================
                        var updatedSection = BuildSectionDto(
                            pageEntity.Id,
                            componentTypeEntity.Id,
                            rtl,
                            currentSectionSortOrder);

                        updatedSection.Id = sectionEntity.Id;

                        await _sectionService.UpdateAsync(
                            updatedSection,
                            updatedSection.Id);
                        // =========================================
                        // SECTION ITEMS SEED IF NOT EXISTS
                        // =========================================
                        int itemSortOrder = 1;
                        var sectionGroupCache = new Dictionary<string, Guid>();
                        if (rtl?.Items != null)
						{



                            foreach (var item in rtl.Items)
							{
                                Guid? sectionGroupItemId = null;

                                if (!string.IsNullOrWhiteSpace(item.SectionGroupItemCode))
                                {
                                    if (!sectionGroupCache.TryGetValue(item.SectionGroupItemCode, out var cachedId))
                                    {
                                        var groupResult =
                                            await _sectionGroupItemService.FirstOrDefaultAsync<SqlSectionGroupItem>(
                                                x => x.Code == item.SectionGroupItemCode);

                                        if (groupResult.Data != null)
                                        {
                                            cachedId = groupResult.Data.Id;

                                            sectionGroupCache[item.SectionGroupItemCode] = cachedId;
                                        }
                                    }

                                    if (cachedId != Guid.Empty)
                                    {
                                        sectionGroupItemId = cachedId;
                                    }
                                }
                                var existingItem = await _sectionItemService
									.FirstOrDefaultAsync<SqlSectionItem>(
										x => x.SectionId == sectionEntity.Id &&
											 x.Title == item.Title);
                                var sectionItem = BuildSectionItemDto(
                                    sectionEntity.Id,
                                    item,
                                    sectionGroupItemId,
                                    itemSortOrder++);

                                if (existingItem.Data == null)
								{
									await _sectionItemService.CreateAsync(sectionItem);
								}
								else
								{
									sectionItem.Id = existingItem.Data.Id;
									await _sectionItemService.UpdateAsync(sectionItem, sectionItem.Id);
								}
							}
						}

					}
				}
			}

			await _transactionService.CommitAsync();
		}
        private static string GenerateSlug(string text)
        {
            return text
                .Trim()
                .ToLowerInvariant()
                .Replace(" ", "-");
        }
        public async Task SeedCmsNewsAndArticlePagesAsync()
        {
            var seederName = SeederNames.SeedCmsNewsAndArticlePagesAsync;

            var existing = await _cmsConfigurationService
                .FirstOrDefaultAsync<SqlCmsConfiguration>(x => x.IsActive);
            if (await _seedHistoryService.GetByNameAsync(seederName) != null)
                return;

            var templateName = _configuration["Cms:DefaultTemplate"];

            if (string.IsNullOrWhiteSpace(templateName))
                throw new Exception("DefaultTemplate is not configured in appsettings");

            var assembly = typeof(SeedJsonModel).Assembly;

            // =========================
            // LOAD TEMPLATE
            // =========================
            using var templateStream = assembly.GetManifestResourceStream(
                "Velora.Application.Shared.Resources.Templates.json");

            if (templateStream == null)
                throw new FileNotFoundException("Templates.json not found");

            var templateModel = await JsonSerializer.DeserializeAsync<TemplateSeedModel>(
                templateStream,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (templateModel == null)
                throw new Exception("Invalid Template JSON");

            var template = templateModel.Templates
                .FirstOrDefault(x => x.TemplateName == templateName);

            if (template == null)
                throw new Exception($"Template {templateName} not found");

            // =========================
            // LOAD COMPONENT RULES
            // =========================

            using var componentStream = assembly.GetManifestResourceStream(
"Velora.Application.Shared.Resources.ContentItemRules.json");

            if (componentStream == null)
                throw new FileNotFoundException("ContentItemRules.json not found");

            var componentRules = await JsonSerializer.DeserializeAsync<
                Dictionary<string, ContentItemRuleModel>>(
                componentStream,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            // 🚨 فقط این صفحات Content دارند
            var contentPages = new[] { "news", "articles" };

            // =====================================================
            // PAGES LOOP (UPSERT)
            // =====================================================
            foreach (var page in template.Pages)
            {
                var cmsConfig = existing?.Data;

                if (page.Slug == "news" && !(cmsConfig?.EnableNews ?? false))
                {
                    continue;
                }

                if (page.Slug == "articles" && !(cmsConfig?.EnableBlog ?? false))
                {
                    continue;
                }
                var isContentPage = !string.IsNullOrEmpty(page.Slug) &&
                                    contentPages.Contains(page.Slug.ToLower());
				if(!isContentPage)
				{
					continue;
				}
                var existingPage = _dbType == DatabaseType.SqlServer
                    ? await _pageService.FirstOrDefaultAsync<SqlPage>(x => x.Slug == page.Slug)
                    : await _pageService.FirstOrDefaultAsync<SqlPage>(x => x.Slug == page.Slug);

                var pageEntity = new PageDto
                {
                    Name = page.PageName,
                    Slug = page.Slug,
                    IsPublished = false,
                    MetaTitle = page.MetaTitle ?? $"صفحه {page.PageName}",
                    MetaDescription = page.MetaDescription ?? $"توضیحات صفحه {page.PageName}",
                    MetaKeywords = page.MetaKeywords ?? "آموزشی، نمونه، سایت",
                    IsActive = true
                };

                if (existingPage.Data == null)
                {
                    var created = await _pageService.CreateAsync(pageEntity);
                    pageEntity.Id = created.Data.Id;
                }
                else
                {
                    pageEntity.Id = existingPage.Data.Id;
                    pageEntity.IsActive = true;
                }

                // =====================================================
                // NORMAL PAGES → SECTION SYSTEM (مثل قبل)
                // =====================================================
                int sectionIndex = 1;


                if (componentRules == null)
                    throw new Exception("Invalid ComponentRules JSON");
                foreach (var component in page.Components)
                {
                    if (!componentRules.TryGetValue(component.Code, out var rule))
                        continue;
                    var existingComponentType = _dbType == DatabaseType.SqlServer
                        ? await _componentTypeService.FirstOrDefaultAsync<SqlComponentType>(x => x.Code == component.Code)
                        : await _componentTypeService.FirstOrDefaultAsync<PgComponentType>(x => x.Code == component.Code);

                    var componentTypeEntity = new ComponentTypeDto
                    {
                        Name = component.Code,
                        Code = component.Code,
                        Type = component.Type,
                        IsActive = true
                    };

                    if (existingComponentType.Data == null)
                    {
                        var created = await _componentTypeService.CreateAsync(componentTypeEntity);
                        componentTypeEntity.Id = created.Data.Id;
                    }
                    else
                    {
                        componentTypeEntity.Id = existingComponentType.Data.Id;
                    }

                    var existingContentItem = _dbType == DatabaseType.SqlServer
                        ? await _contentItemService.FirstOrDefaultAsync<SqlContentItem>(
                            x => x.PageId == pageEntity.Id)
                        : await _contentItemService.FirstOrDefaultAsync<SqlContentItem>(
                            x => x.PageId == pageEntity.Id);

                    var rtl = rule.DefaultData?.Rtl;
                    var currentSectionSortOrder = sectionIndex++;
					// =====================================================
					// CATEGORY
					// =====================================================
					Guid? categoryId = null;
                    if (!string.IsNullOrWhiteSpace(rtl?.CategoryName))
                    {
                        var existingCategory = await _contentCategoryService
                            .FirstOrDefaultAsync<SqlContentCategory>(
                                x => x.Name == rtl.CategoryName);



                        if (existingCategory.Data == null)
                        {
                            var createdCategory = await _contentCategoryService.CreateAsync(
                                new ContentCategoryDto
                                {
                                    Name = rtl.CategoryName,
                                    Slug = GenerateSlug(rtl.CategoryName),
                                    IsActive = true
                                });

                            categoryId = createdCategory.Data.Id;
                        }
                        else
                        {
                            categoryId = existingCategory.Data.Id;
                        }
                    }
                    Guid currentContentItemId;

                    if (existingContentItem.Data == null)
                    {
                        rtl.CategoryId = categoryId;
                        var contentItemEntity = BuildContentItemDto(
                            pageEntity.Id,
                            componentTypeEntity.Id,
                            rtl,
                            currentSectionSortOrder);

                        var created =
                            await _contentItemService.CreateAsync(contentItemEntity);

                        currentContentItemId = created.Data.Id;
                    }
                    else
                    {
                        rtl.CategoryId = categoryId;
                        var contentItemUpdated = BuildContentItemDto(
                            pageEntity.Id,
                            componentTypeEntity.Id,
                            rtl,
                            currentSectionSortOrder);
                        contentItemUpdated.Id = existingContentItem.Data.Id;

                        await _contentItemService.UpdateAsync(
                            contentItemUpdated,
                            contentItemUpdated.Id);

                        currentContentItemId =
                            existingContentItem.Data.Id;
                    }
					var oldTags =
						await _contentItemTagService.GetByContentItemTagsAsync(currentContentItemId);

                    foreach (var item in oldTags)
                    {
                        await _contentItemTagService.DeleteAsync(item.Id);
                    }
                    // =====================================================
                    // TAGS
                    // =====================================================
                    if (rtl?.Tags != null && rtl.Tags.Any())
                    {
                        foreach (var tag in rtl.Tags)
                        {
                            var existingTag =
                                await _tagService
                                    .FirstOrDefaultAsync<SqlTag>(
                                        x => x.Slug == tag.Slug);

                            Guid tagId;

                            if (existingTag.Data == null)
                            {
                                var createdTag =
                                    await _tagService.CreateAsync(
                                        new TagDto
                                        {
                                            Name = tag.Name,
                                            Slug = tag.Slug,
                                            IsActive = true
                                        });

                                tagId = createdTag.Data.Id;
                            }
                            else
                            {
                                tagId = existingTag.Data.Id;
                            }

                            var existingContentTag =
                                await _contentItemTagService
                                    .FirstOrDefaultAsync<SqlContentItemTag>(
                                        x =>
                                            x.ContentItemId == currentContentItemId &&
                                            x.TagId == tagId);

                            if (existingContentTag.Data == null)
                            {
                                await _contentItemTagService.CreateAsync(
                                    new ContentItemTagDto
                                    {
                                        ContentItemId = currentContentItemId,
                                        TagId = tagId
                                    });
                            }
                        }
                    }
                }
            }

            await _transactionService.CommitAsync();
        }
        private SectionDto BuildSectionDto(
    Guid pageId,
    Guid componentTypeId,
    ComponentLanguageData rtl,
    int sortOrder)
        {
            return new SectionDto
            {
                PageId = pageId,
                ComponentTypeId = componentTypeId,

                IsActive = true,
                IsTest = false,

                SortOrder = sortOrder,

                BackgroundColor = "#ffffff",
                HeaderColor = "#000000",
                SubtitleColor = "#333333",
                DescriptionColor = "#555555",

                Title = rtl?.Title,
                Subtitle = rtl?.Subtitle,
                Description = rtl?.Description,

                ImageUrl = rtl?.ImageUrl,
                ImageUrl2 = rtl?.ImageUrl2,
                ImageUrl3 = rtl?.ImageUrl3,
                ImageUrl4 = rtl?.ImageUrl4,

                ImageAlt = rtl?.ImageAlt,
                ImageAlt2 = rtl?.ImageAlt2,
                ImageAlt3 = rtl?.ImageAlt3,
                ImageAlt4 = rtl?.ImageAlt4,

                Icon = rtl?.Icon,
                IconAlt = rtl?.IconAlt,
                IconColor = rtl?.IconColor,

                Features = rtl?.Features,
                CopyrightText = rtl?.CopyrightText,

                ContactEmailLabel = rtl?.ContactEmailLabel,
                ContactFirstNameLabel = rtl?.ContactFirstNameLabel,
                ContactLastNameLabel = rtl?.ContactLastNameLabel,
                ContactMessageLabel = rtl?.ContactMessageLabel,
                ContactSubmitButtonText = rtl?.ContactSubmitButtonText,

                Link1Text = rtl?.Link1Text,
                Link1Url = rtl?.Link1Url,
                Link1Color = rtl?.Link1Color,
                Link1TypeId = rtl?.Link1TypeId,
                Link1TargetId = rtl?.Link1TargetId,
                Link1OpenInNewTab = rtl?.Link1OpenInNewTab,

                Link2Text = rtl?.Link2Text,
                Link2Url = rtl?.Link2Url,
                Link2Color = rtl?.Link2Color,
                Link2TypeId = rtl?.Link2TypeId,
                Link2TargetId = rtl?.Link2TargetId,
                Link2OpenInNewTab = rtl?.Link2OpenInNewTab,

                Link3Text = rtl?.Link3Text,
                Link3Url = rtl?.Link3Url,
                Link3Color = rtl?.Link3Color,
                Link3TypeId = rtl?.Link3TypeId,
                Link3TargetId = rtl?.Link3TargetId,
                Link3OpenInNewTab = rtl?.Link3OpenInNewTab,

                Link4Text = rtl?.Link4Text,
                Link4Url = rtl?.Link4Url,
                Link4Color = rtl?.Link4Color,
                Link4TypeId = rtl?.Link4TypeId,
                Link4TargetId = rtl?.Link4TargetId,
                Link4OpenInNewTab = rtl?.Link4OpenInNewTab,

                MapEmbedUrl = rtl?.MapEmbedUrl,
				ThumbnailUrl = rtl?.ThumbnailUrl,
				VideoUrl = rtl?.VideoUrl,
                ColumnsCount = 1
            };
        }
        private ContentItemDto BuildContentItemDto(
Guid pageId,
Guid componentTypeId,
ContentItemLanguageData rtl,
int sortOrder)
        {
            return new ContentItemDto
            {
                PageId = pageId,
				AuthorAvatarUrl = rtl?.AuthorAvatarUrl,
				AuthorName = rtl?.AuthorName,
				AuthorTitle = rtl?.AuthorTitle,
				SourceTitle=rtl.SourceTitle,
				SourceUrl=rtl.SourceUrl,
				Content=rtl.Content,
				ContentType=rtl.ContentType,
				IsPublished=true,
				PublishedAt = rtl?.PublishedAt,
				Slug=rtl.Slug,
				Summary=rtl?.Summary,
                IsActive = true,
                IsTest = false,
                Title = rtl?.Title,
                SortOrder = sortOrder,
			ImageAlt=rtl?.ImageAlt,
			ImageUrl=rtl?.ImageUrl,
			ExternalUrl=rtl?.ExternalUrl,
			CategoryId=rtl?.CategoryId,
			ImageDetailAlt=rtl?.ImageDetailAlt,
			ImageDetailUrl=rtl?.ImageDetailUrl,
			

            };
        }
        private SectionItemDto BuildSectionItemDto(
    Guid sectionId,
    ComponentItemData item,
    Guid? sectionGroupItemId,
    int sortOrder)
        {
            return new SectionItemDto
            {
                SectionId = sectionId,

                Title = item.Title,
                Subtitle = item.Subtitle,
                Description = item.Description,

                Icon = item.Icon,
                ImageUrl = item.ImageUrl,
                ImageAlt = item.ImageAlt,

                AvatarUrl = item.AvatarUrl,
                AvatarAlt = item.AvatarAlt,

                BackgroundColor = item.BackgroundColor,
                DescriptionColor = item.DescriptionColor,
                SubtitleColor = item.SubtitleColor,
                TitleColor = item.TitleColor,

                IconAlt = item.IconAlt,
                IconColor = item.IconColor,

                Features = item.Features,

                Link1Text = item.Link1Text,
                Link1Url = item.Link1Url,
                Link1Color = item.Link1Color,
                Link1TypeId = item.Link1TypeId,
                Link1TargetId = item.Link1TargetId,
                Link1OpenInNewTab = item.Link1OpenInNewTab,

                Link2Text = item.Link2Text,
                Link2Url = item.Link2Url,
                Link2Color = item.Link2Color,
                Link2TypeId = item.Link2TypeId,
                Link2TargetId = item.Link2TargetId,
                Link2OpenInNewTab = item.Link2OpenInNewTab,

                Link3Text = item.Link3Text,
                Link3Url = item.Link3Url,
                Link3Color = item.Link3Color,
                Link3TypeId = item.Link3TypeId,
                Link3TargetId = item.Link3TargetId,
                Link3OpenInNewTab = item.Link3OpenInNewTab,

                Link4Text = item.Link4Text,
                Link4Url = item.Link4Url,
                Link4Color = item.Link4Color,
                Link4TypeId = item.Link4TypeId,
                Link4TargetId = item.Link4TargetId,
                Link4OpenInNewTab = item.Link4OpenInNewTab,

                Name = item.Name,
                Price = item.Price,
                Question = item.Question,
                Answer = item.Answer,
                Role = item.Role,

                SectionGroupItemId = sectionGroupItemId,

                IsActive = true,
                SortOrder = sortOrder
            };
        }
        public async Task SeedCoreSectionGroupItemAsync()
        {
            const string seederName = SeederNames.Seed_Core_SectionGroupItem;

            if (await _seedHistoryService.GetByNameAsync(seederName) != null)
                return;

            async Task UpsertAsync(
                string code,
                string name,
                string description,
                int sortOrder,
                Guid? groupId = null)
            {
                var existing = await _sectionGroupItemService
                    .FirstOrDefaultAsync<SqlSectionGroupItem>(x => x.Code == code);

                if (existing.Data == null)
                {
                    await _sectionGroupItemService.CreateAsync(new SectionGroupItemCrud
                    {
                        Code = code,
                        Name = name,
                        Description = description,
                        SortOrder = sortOrder,
                        IsActive = true,
                        GroupId = groupId
                    });
                }
                else
                {
                    existing.Data.Code = code;
                    existing.Data.Name = name;
                    existing.Data.Description = description;
                    existing.Data.SortOrder = sortOrder;
                    existing.Data.IsActive = true;
                    existing.Data.GroupId = groupId;

                    await _sectionGroupItemService.UpdateAsync(existing.Data, existing.Data.Id);
                }
            }

            // =========================
            // FOOTER ROOT GROUP
            // =========================
            var footerGroup = await _sectionGroupItemService
                .FirstOrDefaultAsync<SqlSectionGroupItem>(x => x.Code == "FOOTER");

            Guid footerGroupId;

            if (footerGroup.Data == null)
            {
                var created = await _sectionGroupItemService.CreateAsync(new SectionGroupItemCrud
                {
                    Code = "FOOTER",
                    Name = "فوتر",
                    Description = "فوتر",
                    SortOrder = 1,
                    IsActive = true
                });

                footerGroupId = created.Data.Id;
            }
            else
            {
                footerGroupId = footerGroup.Data.Id;

                footerGroup.Data.Name = "فوتر";
                footerGroup.Data.Description = "فوتر";
                footerGroup.Data.SortOrder = 1;
                footerGroup.Data.IsActive = true;

                await _sectionGroupItemService.UpdateAsync(footerGroup.Data, footerGroup.Data.Id);
            }

            // =========================
            // FOOTER CHILD GROUPS
            // =========================
            await UpsertAsync("FOOTER_COMPANY", "شرکت", "بخش شرکت", 1, footerGroupId);
            await UpsertAsync("FOOTER_SUPPORT", "پشتیبانی", "بخش پشتیبانی", 2, footerGroupId);
            await UpsertAsync("FOOTER_SOCIAL", "شبکه‌های اجتماعی", "شبکه‌های اجتماعی", 3, footerGroupId);
            await UpsertAsync("FOOTER_CONTACT", "تماس با ما", "بخش تماس", 4, footerGroupId);
            await UpsertAsync("FOOTER_LEGAL", "قوانین", "بخش قوانین", 5, footerGroupId);
            await UpsertAsync("FOOTER_TRUST", "نمادها", "نمادهای اعتماد", 6, footerGroupId);

            await _transactionService.CommitAsync();
        }
    }
}

