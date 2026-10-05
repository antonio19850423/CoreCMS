using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
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
        public const string Seed_Products = "Seed_Products";
        public const string Seed_Menus = "Seed_Menus";
        public const string Seed_Brands = "Seed_Brands";
        public const string Seed_Categories = "Seed_Categories";
        public const string Seed_ProductTypes = "Seed_ProductTypes";
        public const string Seed_ProductFiles = "Seed_ProductFiles";
        public const string Seed_ProductTags = "Seed_ProductTags";
        public const string Seed_ProductVariants = "Seed_ProductVariants";
        public const string Seed_ProductAttributes = "Seed_ProductAttributes";
        public const string Seed_Inventory = "Seed_Inventory";
        public const string Seed_Discounts = "Seed_Discounts";
        public const string Seed_DiscountItems = "Seed_DiscountItems";

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
        private readonly IProductService _productService;
        private readonly IProductBrandService _productBrandService;
        private readonly IProductCategoryService _productCategoryService;
        private readonly IProductFileService _productFileService;
        private readonly IProductVariantService _productVariantService;
        private readonly IProductTypeService _productTypeService;
        private readonly IProductAttributeService _productAttributeService;
        private readonly IProductTagService _productTagService;
        private readonly IProductTagMappingService _productTagMappingService;
        private readonly IProductAttributeValueService _productAttributeValueService;
        private readonly IInventoryTransactionReasonService _inventoryTransactionReasonService;
        private readonly IProductInventoryTransactionService _productInventoryTransactionService;
        private readonly ISiteMenuService _siteMenuService;
        private readonly IDiscountService _discountService;
        private readonly IDiscountItemService _discountItemService;
        


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
            IMapper mapper, IPageService pageService, ISectionService sectionService, IComponentTypeService componentTypeService, ISectionItemService sectionItemService, ILinkTypeService linkTypeService, IContentItemService contentItemService, IContentCategoryService contentCategoryService, IContentItemTagService contentItemTagService, ITagService tagService, IProductService productService,
        IProductBrandService productBrandService,
        IProductCategoryService productCategoryService,
        IProductFileService productFileService,
        IProductVariantService productVariantService, IProductTypeService productTypeService, IProductAttributeService productAttributeService, IProductTagService productTagService, IProductAttributeValueService productAttributeValueService, IProductTagMappingService productTagMappingService, IInventoryTransactionReasonService inventoryTransactionReasonService, IProductInventoryTransactionService productInventoryTransactionService, ISiteMenuService siteMenuService, IDiscountService discountService, IDiscountItemService discountItemService)
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
            _contentCategoryService = contentCategoryService;
            _contentItemTagService = contentItemTagService;
            _tagService = tagService;
            _productService = productService;
            _productBrandService = productBrandService;
            _productCategoryService = productCategoryService;
            _productFileService = productFileService;
            _productVariantService = productVariantService;
            _productTypeService = productTypeService;
            _productAttributeService = productAttributeService;
            _productTagService = productTagService;
            _productTagMappingService = productTagMappingService;
            _productAttributeValueService = productAttributeValueService;
            _inventoryTransactionReasonService = inventoryTransactionReasonService;
            _productInventoryTransactionService = productInventoryTransactionService;
            _siteMenuService= siteMenuService;
            _discountService= discountService;
            _discountItemService= discountItemService;

        }

        public async Task SeedAllAsync()
        {
            if (await ShouldRunSeederAsync(SeederNames.Core))
            {
                await SeedCoreAsync();
                await _seedHistoryService.CreateAsync(new() { Name = SeederNames.Core, CreatedAt = DateTime.Now });
                await _transactionService.CommitAsync();
            }

            if (await ShouldRunSeederAsync(SeederNames.Localization))
            {
                await SeedLocalizationAsync();
                await _seedHistoryService.CreateAsync(new() { Name = SeederNames.Localization, CreatedAt = DateTime.Now });
                await _transactionService.CommitAsync();
            }

            if (await ShouldRunSeederAsync(SeederNames.Resources))
            {
                await SeedResourcesAsync();
                await _seedHistoryService.CreateAsync(new() { Name = SeederNames.Resources, CreatedAt = DateTime.Now });
                await _transactionService.CommitAsync();
            }

            if (await ShouldRunSeederAsync(SeederNames.Permissions))
            {
                await SeedPermissionsAsync();
                await _seedHistoryService.CreateAsync(new() { Name = SeederNames.Permissions, CreatedAt = DateTime.Now });
                await _transactionService.CommitAsync();
            }

            if (await ShouldRunSeederAsync(SeederNames.Settings))
            {
                await SeedSettingsAsync();
                await _seedHistoryService.CreateAsync(new() { Name = SeederNames.Settings, CreatedAt = DateTime.Now });
                await _transactionService.CommitAsync();
            }

            if (await ShouldRunSeederAsync(SeederNames.Seed_Core_LinkTypes))
            {
                await SeedCoreLinkTypesAsync();
                await _seedHistoryService.CreateAsync(new()
                {
                    Name = SeederNames.Seed_Core_LinkTypes,
                    CreatedAt = DateTime.Now
                });
                await _transactionService.CommitAsync();
            }

            if (await ShouldRunSeederAsync(SeederNames.Core_SiteSettings))
            {
                await SeedCoreSiteSettingsAsync();
                await _seedHistoryService.CreateAsync(new()
                {
                    Name = SeederNames.Core_SiteSettings,
                    CreatedAt = DateTime.Now
                });
                await _transactionService.CommitAsync();
            }

            if (await ShouldRunSeederAsync(SeederNames.Core_CmsConfiguration))
            {
                await SeedCoreCmsConfigurationAsync();
                await _seedHistoryService.CreateAsync(new()
                {
                    Name = SeederNames.Core_CmsConfiguration,
                    CreatedAt = DateTime.Now
                });
                await _transactionService.CommitAsync();
            }
            if (await ShouldRunSeederAsync(SeederNames.Seed_Core_Template))
            {
                await SeedCmsTemplateAsync();
                await _seedHistoryService.CreateAsync(new()
                {
                    Name = SeederNames.Seed_Core_Template,
                    CreatedAt = DateTime.Now
                });
                await _transactionService.CommitAsync();
            }
            if (await ShouldRunSeederAsync(SeederNames.Seed_Core_SectionGroupItem))
            {
                await SeedCoreSectionGroupItemAsync();
                await _seedHistoryService.CreateAsync(new()
                {
                    Name = SeederNames.Seed_Core_SectionGroupItem,
                    CreatedAt = DateTime.Now
                });
                await _transactionService.CommitAsync();
            }
            if (await ShouldRunSeederAsync(SeederNames.SeedCmsNewsAndArticlePagesAsync))
            {
                await SeedCmsNewsAndArticlePagesAsync();
                await _seedHistoryService.CreateAsync(new()
                {
                    Name = SeederNames.SeedCmsNewsAndArticlePagesAsync,
                    CreatedAt = DateTime.Now
                });
                await _transactionService.CommitAsync();
            }
            if (await ShouldRunSeederAsync(SeederNames.Seed_Categories))
            {
                await SeedCategoriesAsync();

                await _seedHistoryService.CreateAsync(new()
                {
                    Name = SeederNames.Seed_Categories,
                    CreatedAt = DateTime.Now
                });
                await _transactionService.CommitAsync();
            }

            if (await ShouldRunSeederAsync(SeederNames.Seed_Brands))
            {
                await SeedBrandsAsync();

                await _seedHistoryService.CreateAsync(new()
                {
                    Name = SeederNames.Seed_Brands,
                    CreatedAt = DateTime.Now
                });
                await _transactionService.CommitAsync();
            }

            if (await ShouldRunSeederAsync(SeederNames.Seed_ProductTypes))
            {
                await SeedProductTypesAsync();

                await _seedHistoryService.CreateAsync(new()
                {
                    Name = SeederNames.Seed_ProductTypes,
                    CreatedAt = DateTime.Now
                });
                await _transactionService.CommitAsync();
            }
            if (await ShouldRunSeederAsync(SeederNames.Seed_Products))
            {
                await SeedProductsAsync();
                await _seedHistoryService.CreateAsync(new()
                {
                    Name = SeederNames.Seed_Products,
                    CreatedAt = DateTime.Now
                });
                await _transactionService.CommitAsync();
            }
            if (await ShouldRunSeederAsync(SeederNames.Seed_ProductFiles))
            {
                await SeedProductFilesAsync();

                await _seedHistoryService.CreateAsync(new()
                {
                    Name = SeederNames.Seed_ProductFiles,
                    CreatedAt = DateTime.Now
                });
                await _transactionService.CommitAsync();
            }
            if (await ShouldRunSeederAsync(SeederNames.Seed_ProductTags))
            {
                await SeedProductTagsAsync();

                await _seedHistoryService.CreateAsync(new()
                {
                    Name = SeederNames.Seed_ProductTags,
                    CreatedAt = DateTime.Now
                });
                await _transactionService.CommitAsync();
            }
            if (await ShouldRunSeederAsync(SeederNames.Seed_ProductVariants))
            {
                await SeedProductVariantsAsync();

                await _seedHistoryService.CreateAsync(new()
                {
                    Name = SeederNames.Seed_ProductVariants,
                    CreatedAt = DateTime.Now
                });
                await _transactionService.CommitAsync();
            }
            if (await ShouldRunSeederAsync(
    SeederNames.Seed_ProductAttributes))
            {
                await SeedProductAttributesAsync();

                await _seedHistoryService.CreateAsync(new()
                {
                    Name = SeederNames.Seed_ProductAttributes,
                    CreatedAt = DateTime.Now
                });
                await _transactionService.CommitAsync();
            }
            if (await ShouldRunSeederAsync(
    SeederNames.Seed_Inventory))
            {
                await SeedInventoryAsync();

                await _seedHistoryService.CreateAsync(new()
                {
                    Name = SeederNames.Seed_Inventory,
                    CreatedAt = DateTime.Now
                });
                await _transactionService.CommitAsync();
            }
            if (await ShouldRunSeederAsync(
                    SeederNames.Seed_Discounts))
            {
                await SeedDiscountsAsync();

                await _seedHistoryService.CreateAsync(new()
                {
                    Name = SeederNames.Seed_Discounts,
                    CreatedAt = DateTime.Now
                });
                await _transactionService.CommitAsync();
            }
            if (await ShouldRunSeederAsync(
        SeederNames.Seed_DiscountItems))
            {
                await SeedDiscountItemsAsync();

                await _seedHistoryService.CreateAsync(new()
                {
                    Name = SeederNames.Seed_DiscountItems,
                    CreatedAt = DateTime.Now
                });
                await _transactionService.CommitAsync();
            }

        }
        public async Task SeedMenusDataAsync()
        {
            if (!await ShouldRunSeederAsync(SeederNames.Seed_Menus))
                return;

           await SeedMenusAsync();
            await _seedHistoryService.CreateAsync(new()
            {
                Name = SeederNames.Seed_Menus,
                CreatedAt = DateTime.Now
            });
            await _transactionService.CommitAsync();
        }
        private async Task SeedMenusAsync()
        {
            var configurations = await _cmsConfigurationService.GetAllViews();

            var configuration = await configurations
                .FirstOrDefaultAsync(x => x.IsActive);

            if (configuration == null)
                throw new Exception("Active CMS configuration not found.");

            var siteMenus = await _siteMenuService.GetAllViews();
            var existingMenus = await siteMenus.ToListAsync();

            var linkTypes = await _linkTypeService.GetAllViews();

            var productLinkTypeId = await GetLinkTypeIdAsync(linkTypes, "PRODUCT");
            var categoryLinkTypeId = await GetLinkTypeIdAsync(linkTypes, "CATEGORY");
            var brandLinkTypeId = await GetLinkTypeIdAsync(linkTypes, "BRAND");
            var newsLinkTypeId = await GetLinkTypeIdAsync(linkTypes, "NEWS");
            var articleLinkTypeId = await GetLinkTypeIdAsync(linkTypes, "ARTICLE");
            // =========================================================
            // Fixed Pages
            // =========================================================

            var pageLinkTypeId = await GetLinkTypeIdAsync(
                linkTypes,
                "PAGE");

            // Home
            var homePageId = await GetPageIdBySlugAsync("home");

            if (homePageId.HasValue)
            {
                await EnsureMenuAsync(
                    existingMenus,
                    text: "خانه",
                    linkTypeId: pageLinkTypeId,
                    targetId: homePageId,
                    parentId: null,
                    sortOrder: 0,
                    enabled: true);
            }

            // About Us
            var aboutPageId = await GetPageIdBySlugAsync("about");

            if (aboutPageId.HasValue)
            {
                await EnsureMenuAsync(
                    existingMenus,
                    text: "درباره ما",
                    linkTypeId: pageLinkTypeId,
                    targetId: aboutPageId,
                    parentId: null,
                    sortOrder: 4,
                    enabled: true);
            }

            // Contact Us
            var contactPageId = await GetPageIdBySlugAsync("contact");

            if (contactPageId.HasValue)
            {
                await EnsureMenuAsync(
                    existingMenus,
                    text: "تماس با ما",
                    linkTypeId: pageLinkTypeId,
                    targetId: contactPageId,
                    parentId: null,
                    sortOrder: 5,
                    enabled: true);
            }
            // FAQ
            if (configuration.EnableFaq == true)
            {
                var faqPageId = await GetPageIdBySlugAsync("faq");

                if (faqPageId.HasValue)
                {
                    await EnsureMenuAsync(
                        existingMenus,
                        text: "سؤالات متداول",
                        linkTypeId: pageLinkTypeId,
                        targetId: faqPageId,
                        parentId: null,
                        sortOrder: 6,
                        enabled: true);
                }
            }

            // Privacy
            if (configuration.EnablePrivacy == true)
            {
                var privacyPageId = await GetPageIdBySlugAsync("privacy-policy");

                if (privacyPageId.HasValue)
                {
                    await EnsureMenuAsync(
                        existingMenus,
                        text: "حریم خصوصی",
                        linkTypeId: pageLinkTypeId,
                        targetId: privacyPageId,
                        parentId: null,
                        sortOrder: 7,
                        enabled: true);
                }
            }
            // =========================================================
            // Products
            // =========================================================

            Guid? productsMenuId = null;

            if (configuration.EnableShop)
            {
                productsMenuId = await EnsureMenuAsync(
                    existingMenus,
                    text: "محصولات",
                    linkTypeId: productLinkTypeId,
                    targetId: null,
                    parentId: null,
                    sortOrder: 1,
                    enabled: true);
            }
            else
            {
                await DisableMenuAsync(
                    existingMenus,
                    text: "محصولات",
                    linkTypeId: productLinkTypeId,
                    targetId: null,
                    parentId: null);
            }

            // =========================================================
            // Product Categories
            // =========================================================

            if (configuration.EnableShop &&
                configuration.EnableProductCategoriesMenu == true &&
                productsMenuId.HasValue)
            {
                var categoriesMenuId = await EnsureMenuAsync(
                    existingMenus,
                    text: "دسته‌بندی محصولات",
                    linkTypeId: null,
                    targetId: null,
                    parentId: productsMenuId,
                    sortOrder: 1,
                    enabled: true);

                if (categoriesMenuId.HasValue)
                {
                    var categories = await _productCategoryService
                        .GetAllViews();

                    var activeCategories = await categories
                        .Where(x => x.IsActive)
                        .OrderBy(x => x.SortOrder)
                        .ToListAsync();

                    foreach (var category in activeCategories)
                    {
                        await EnsureMenuAsync(
                            existingMenus,
                            text: category.Name,
                            linkTypeId: categoryLinkTypeId,
                            targetId: category.Id,
                            parentId: categoriesMenuId,
                            sortOrder: category.SortOrder,
                            enabled: true);
                    }
                }
            }
            else if (productsMenuId.HasValue)
            {
                var categoriesMenu = existingMenus.FirstOrDefault(x =>
                    x.ParentId == productsMenuId.Value &&
                    x.Link1Text == "دسته‌بندی محصولات" &&
                    x.Link1TypeId == null &&
                    x.Link1TargetId == null);

                if (categoriesMenu != null)
                {
                    await DisableMenuAsync(
                        existingMenus,
                        categoriesMenu.Link1Text,
                        null,
                        null,
                        productsMenuId);

                    await DisableChildMenusAsync(
                        existingMenus,
                        categoriesMenu.Id,
                        categoryLinkTypeId);
                }
            }

            // =========================================================
            // Product Brands
            // =========================================================

            if (configuration.EnableShop &&
                configuration.EnableProductBrandsMenu == true &&
                productsMenuId.HasValue)
            {
                var brandsMenuId = await EnsureMenuAsync(
                    existingMenus,
                    text: "برندها",
                    linkTypeId: null,
                    targetId: null,
                    parentId: productsMenuId,
                    sortOrder: 2,
                    enabled: true);

                if (brandsMenuId.HasValue)
                {
                    var brands = await _productBrandService.GetAllViews();

                    var activeBrands = await brands
                        .Where(x => x.IsActive.Value)
                        .OrderBy(x => x.SortOrder)
                        .ToListAsync();

                    foreach (var brand in activeBrands)
                    {
                        await EnsureMenuAsync(
                            existingMenus,
                            text: brand.Name,
                            linkTypeId: brandLinkTypeId,
                            targetId: brand.Id,
                            parentId: brandsMenuId,
                            sortOrder: brand.SortOrder ?? 0,
                            enabled: true);
                    }
                }
            }
            else if (productsMenuId.HasValue)
            {
                var brandsMenu = existingMenus.FirstOrDefault(x =>
                    x.ParentId == productsMenuId.Value &&
                    x.Link1Text == "برندها" &&
                    x.Link1TypeId == null &&
                    x.Link1TargetId == null);

                if (brandsMenu != null)
                {
                    await DisableMenuAsync(
                        existingMenus,
                        brandsMenu.Link1Text,
                        null,
                        null,
                        productsMenuId);

                    await DisableChildMenusAsync(
                        existingMenus,
                        brandsMenu.Id,
                        brandLinkTypeId);
                }
            }
            // =========================================================
            // News
            // =========================================================

            if (configuration.EnableNews)
            {
                await EnsureMenuAsync(
                    existingMenus,
                    text: "اخبار",
                    linkTypeId: newsLinkTypeId,
                    targetId: null,
                    parentId: null,
                    sortOrder: 2,
                    enabled: true);
            }
            else
            {
                await DisableMenuAsync(
                    existingMenus,
                    text: "اخبار",
                    linkTypeId: newsLinkTypeId,
                    targetId: null,
                    parentId: null);
            }

            // =========================================================
            // Articles
            // =========================================================

            if (configuration.EnableBlog)
            {
                await EnsureMenuAsync(
                    existingMenus,
                    text: "مقالات",
                    linkTypeId: articleLinkTypeId,
                    targetId: null,
                    parentId: null,
                    sortOrder: 3,
                    enabled: true);
            }
            else
            {
                await DisableMenuAsync(
                    existingMenus,
                    text: "مقالات",
                    linkTypeId: articleLinkTypeId,
                    targetId: null,
                    parentId: null);
            }
        }
        private async Task<Guid?> GetPageIdBySlugAsync(string slug)
        {
            var pages = await _pageService.GetAllViews();

            return await pages
                .Where(x => x.Slug == slug)
                .Select(x => (Guid?)x.Id)
                .FirstOrDefaultAsync();
        }
        private async Task<Guid?> EnsureMenuAsync(
            List<SiteMenuCrud> existingMenus,
            string text,
            Guid? linkTypeId,
            Guid? targetId,
            Guid? parentId,
            int sortOrder,
            bool enabled)
        {
            SiteMenuCrud? existing;

            if (linkTypeId.HasValue || targetId.HasValue)
            {
                existing = existingMenus.FirstOrDefault(x =>
                    x.ParentId == parentId &&
                    x.Link1TypeId == linkTypeId &&
                    x.Link1TargetId == targetId);
            }
            else
            {
                existing = existingMenus.FirstOrDefault(x =>
                    x.ParentId == parentId &&
                    x.Link1TypeId == null &&
                    x.Link1TargetId == null &&
                    x.Link1Text == text);
            }

            if (existing != null)
                return existing.Id;

            if (!enabled)
                return null;

            var result = await _siteMenuService.CreateAsync(new SiteMenuCrud
            {
                Link1Text = text,
                Link1TypeId = linkTypeId,
                Link1TargetId = targetId,
                Link1Url = null,
                Link1Color = null,
                Link1OpenInNewTab = false,

                ParentId = parentId,
                SortOrder = sortOrder,

                IsActive = true,

                Icon = null,
                IconColor = null
            });

            if (!result.Success || result.Data == null)
            {
                throw new Exception(
                    $"Failed to create menu: {text}");
            }

            var id = result.Data.Id;

            existingMenus.Add(new SiteMenuCrud
            {
                Id = id,
                Link1Text = text,
                Link1TypeId = linkTypeId,
                Link1TargetId = targetId,
                Link1Url = null,
                Link1Color = null,
                Link1OpenInNewTab = false,

                ParentId = parentId,
                SortOrder = sortOrder,

                IsActive = true
            });

            return id;
        }
        private async Task DisableMenuAsync(
    List<SiteMenuCrud> existingMenus,
    string text,
    Guid? linkTypeId,
    Guid? targetId,
    Guid? parentId)
        {
            var existing = existingMenus.FirstOrDefault(x =>
                x.ParentId == parentId &&
                x.Link1TypeId == linkTypeId &&
                x.Link1TargetId == targetId &&
                (
                    linkTypeId.HasValue ||
                    x.Link1Text == text
                ));

            if (existing == null || !existing.IsActive)
                return;

            existing.IsActive = false;

            await _siteMenuService.UpdateAsync(existing, existing.Id);
        }
        private async Task DisableChildMenusAsync(
    List<SiteMenuCrud> existingMenus,
    Guid parentId,
    Guid? linkTypeId)
        {
            var children = existingMenus
                .Where(x =>
                    x.ParentId == parentId &&
                    (!linkTypeId.HasValue || x.Link1TypeId == linkTypeId) &&
                    x.IsActive)
                .ToList();

            foreach (var child in children)
            {
                child.IsActive = false;

                await _siteMenuService.UpdateAsync(child, child.Id);
            }
        }
        private async Task<Guid?> GetLinkTypeIdAsync(
    IQueryable<LinkTypeCrud> linkTypes,
    string code)
        {
            return await linkTypes
                .Where(x => x.Code == code)
                .Select(x => (Guid?)x.Id)
                .FirstOrDefaultAsync();
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
        new ResourceTypeDto { Code = "TAB", Name = "Tab", DisplayName = "Tab" },
        new ResourceTypeDto { Code = "REPORT", Name = "Report", DisplayName = "Report" }
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
                            GroupKey = prop.Attribute.GroupKey,
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
            // ---------------------------------------------------------
            // LOAD SEED CONFIGURATION
            // ---------------------------------------------------------

            var enabled =
                _configuration.GetValue<bool>(
                    "Seed:SiteSettings:Enabled");

            if (!enabled)
                return;

            var siteSettingsFile =
                _configuration.GetValue<string>(
                    "Seed:SiteSettings:File");

            if (string.IsNullOrWhiteSpace(siteSettingsFile))
            {
                throw new InvalidOperationException(
                    "Seed:SiteSettings:File is not configured.");
            }

            // ---------------------------------------------------------
            // ASSEMBLY
            // ---------------------------------------------------------

            var assembly =
                typeof(SeedJsonModel).Assembly;

            // ---------------------------------------------------------
            // LOAD SITE SETTINGS JSON
            // ---------------------------------------------------------

            var siteSettingsResourceName =
                $"Velora.Application.Shared.Resources.{siteSettingsFile
                    .Replace("/", ".")
                    .Replace("\\", ".")}";

            using var siteSettingsStream =
                assembly.GetManifestResourceStream(
                    siteSettingsResourceName);

            if (siteSettingsStream == null)
            {
                throw new FileNotFoundException(
                    $"SiteSettings seed resource '{siteSettingsResourceName}' not found.");
            }

            // ---------------------------------------------------------
            // DESERIALIZE JSON
            // ---------------------------------------------------------

            var seedModel =
                await JsonSerializer.DeserializeAsync<SiteSettingsSeedModel>(
                    siteSettingsStream,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

            if (seedModel?.SiteSettings == null)
            {
                throw new InvalidOperationException(
                    $"Invalid SiteSettings JSON: {siteSettingsFile}");
            }

            var siteSettings =
                seedModel.SiteSettings;

            // ---------------------------------------------------------
            // VALIDATE REQUIRED DATA
            // ---------------------------------------------------------

            if (string.IsNullOrWhiteSpace(siteSettings.SiteName))
            {
                throw new InvalidOperationException(
                    $"SiteName is empty in SiteSettings JSON: {siteSettingsFile}");
            }

            // ---------------------------------------------------------
            // FIND EXISTING SITE SETTINGS
            // ---------------------------------------------------------

            var existing =
                await _siteSettingService
                    .FirstOrDefaultAsync<SqlSiteSetting>(
                        x => x.IsActive);

            // ---------------------------------------------------------
            // CREATE
            // ---------------------------------------------------------

            if (existing.Data == null)
            {
                siteSettings.IsActive = true;

                await _siteSettingService
                    .CreateAsync(siteSettings);
            }
            else
            {
                // -----------------------------------------------------
                // UPDATE EXISTING SITE SETTINGS
                // -----------------------------------------------------

                var siteSettingEntity =
                    new SiteSettingDto
                    {
                        Id = existing.Data.Id,

                        SiteName = siteSettings.SiteName,
                        DomainName = siteSettings.DomainName,

                        LogoUrl = siteSettings.LogoUrl,
                        LogoAlt = siteSettings.LogoAlt,

                        DarkLogoUrl = siteSettings.DarkLogoUrl,
                        DarkLogoAlt = siteSettings.DarkLogoAlt,

                        FaviconUrl = siteSettings.FaviconUrl,

                        PhoneTitle = siteSettings.PhoneTitle,
                        Phone = siteSettings.Phone,

                        Phone2Title = siteSettings.Phone2Title,
                        Phone2 = siteSettings.Phone2,

                        MobileTitle = siteSettings.MobileTitle,
                        Mobile = siteSettings.Mobile,

                        FaxTitle = siteSettings.FaxTitle,
                        Fax = siteSettings.Fax,

                        Email = siteSettings.Email,

                        AddressTitle = siteSettings.AddressTitle,
                        Address = siteSettings.Address,

                        Address2Title = siteSettings.Address2Title,
                        Address2 = siteSettings.Address2,

                        DefaultMetaTitle = siteSettings.DefaultMetaTitle,
                        DefaultMetaDescription = siteSettings.DefaultMetaDescription,
                        DefaultMetaKeywords = siteSettings.DefaultMetaKeywords,

                        IsActive = siteSettings.IsActive
                    };

                await _siteSettingService
                    .UpdateAsync(
                        siteSettingEntity,
                        siteSettingEntity.Id);
            }

            // ---------------------------------------------------------
            // COMMIT
            // ---------------------------------------------------------

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
    new LinkTypeDto
    {
        Code = "PAGE",
        Name = "صفحه داخلی",
        IsActive = true,
        SortOrder = 1
    },

    new LinkTypeDto
    {
        Code = "EXTERNAL",
        Name = "لینک خارجی",
        IsActive = true,
        SortOrder = 2
    },

    new LinkTypeDto
    {
        Code = "PRODUCT",
        Name = "محصول",
        IsActive = true,
        SortOrder = 3
    },

    new LinkTypeDto
    {
        Code = "BRAND",
        Name = "برند",
        IsActive = true,
        SortOrder = 4
    },

    new LinkTypeDto
    {
        Code = "CATEGORY",
        Name = "دسته‌بندی",
        IsActive = true,
        SortOrder = 5
    },

    new LinkTypeDto
    {
        Code = "ARTICLE",
        Name = "مقاله",
        IsActive = true,
        SortOrder = 6
    },

    new LinkTypeDto
    {
        Code = "NEWS",
        Name = "اخبار",
        IsActive = true,
        SortOrder = 7
    }
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

                await _cmsConfigurationService.UpdateAsync(existing.Data,existing.Data.Id);
            }

            await _transactionService.CommitAsync();
        }

        public async Task SeedCmsTemplateAsync()
        {
            var existing =
                await _cmsConfigurationService
                    .FirstOrDefaultAsync<SqlCmsConfiguration>(
                        x => x.IsActive);

            // ---------------------------------------------------------
            // LOAD TEMPLATE CONFIGURATION
            // ---------------------------------------------------------

            var enabled =
                _configuration.GetValue<bool>(
                    "Seed:Templates:Enabled");

            if (!enabled)
                return;

            var templateFile =
                _configuration.GetValue<string>(
                    "Seed:Templates:File");

            if (string.IsNullOrWhiteSpace(templateFile))
            {
                throw new InvalidOperationException(
                    "Seed:Templates:File is not configured.");
            }

            var assembly =
                typeof(SeedJsonModel).Assembly;

            // ---------------------------------------------------------
            // LOAD TEMPLATE JSON
            // ---------------------------------------------------------

            var templateResourceName =
                $"Velora.Application.Shared.Resources.{templateFile
                    .Replace("/", ".")
                    .Replace("\\", ".")}";

            using var templateStream =
                assembly.GetManifestResourceStream(
                    templateResourceName);

            if (templateStream == null)
            {
                throw new FileNotFoundException(
                    $"Template seed resource '{templateResourceName}' not found.");
            }

            var templateModel =
                await JsonSerializer.DeserializeAsync<TemplateSeedModel>(
                    templateStream,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

            if (templateModel == null)
            {
                throw new InvalidOperationException(
                    $"Invalid Template JSON: {templateFile}");
            }

            var template =
                templateModel.Templates.FirstOrDefault();

            if (template == null)
            {
                throw new InvalidOperationException(
                    $"No template found in '{templateFile}'.");
            }

            // ---------------------------------------------------------
            // LOAD COMPONENT RULES
            // ---------------------------------------------------------

            using var componentStream =
                assembly.GetManifestResourceStream(
                    "Velora.Application.Shared.Resources.ComponentRules.json");

            if (componentStream == null)
            {
                throw new FileNotFoundException(
                    "ComponentRules.json not found.");
            }

            var componentRules =
                await JsonSerializer.DeserializeAsync<
                    Dictionary<string, ComponentRuleModel>>(
                        componentStream,
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

            if (componentRules == null)
            {
                throw new InvalidOperationException(
                    "Invalid ComponentRules JSON.");
            }

            // ---------------------------------------------------------
            // CACHE
            // ---------------------------------------------------------

            var componentTypeCache =
                new Dictionary<string, ComponentTypeDto>(
                    StringComparer.OrdinalIgnoreCase);

            // ---------------------------------------------------------
            // CMS CONFIG
            // ---------------------------------------------------------

            var cmsConfig = existing?.Data;

            // ---------------------------------------------------------
            // BUILD FINAL PAGE LIST FROM JSON
            //
            // Only pages which are actually enabled by CMS configuration
            // participate in the active template.
            // ---------------------------------------------------------

            var templatePages =
                template.Pages
                    .Where(page =>
                    {
                        var slug =
                            page.Slug?
                                .Trim()
                                .ToLowerInvariant();

                        if (string.IsNullOrWhiteSpace(slug))
                            return false;

                        // These pages are controlled by configuration.
                        if (slug == "faq" &&
                            !(cmsConfig?.EnableFaq ?? false))
                        {
                            return false;
                        }

                        if ((slug == "privacy" ||
                             slug == "privacy-policy") &&
                            !(cmsConfig?.EnablePrivacy ?? false))
                        {
                            return false;
                        }

                        return true;
                    })
                    .ToList();

            var activePageSlugs =
                new HashSet<string>(
                    templatePages
                        .Select(x =>
                            x.Slug!
                                .Trim()
                                .ToLowerInvariant()),
                    StringComparer.OrdinalIgnoreCase);

            // ---------------------------------------------------------
            // DEACTIVATE OLD PAGES
            //
            // Any active page which is not in the new JSON becomes
            // inactive. Nothing is deleted.
            // ---------------------------------------------------------

            var activePagesResult =
                await _pageService.GetAllViews();

            var activePages =
                await activePagesResult.ToListAsync();

            foreach (var oldPage in activePages)
            {
                var oldSlug =
                    oldPage.Slug?
                        .Trim()
                        .ToLowerInvariant();

                if (string.IsNullOrWhiteSpace(oldSlug))
                    continue;

                if (activePageSlugs.Contains(oldSlug))
                    continue;

                var deactivatePage =
                    new PageDto
                    {
                        Id = oldPage.Id,
                        Name = oldPage.Name,
                        Slug = oldPage.Slug,
                        IsDynamic = oldPage.IsDynamic,
                        IsPublished = oldPage.IsPublished,
                        MetaTitle = oldPage.MetaTitle,
                        MetaDescription = oldPage.MetaDescription,
                        MetaKeywords = oldPage.MetaKeywords,
                        IsActive = false
                    };

                await _pageService.UpdateAsync(deactivatePage, deactivatePage.Id);
            }

            // ---------------------------------------------------------
            // PAGES LOOP
            // ---------------------------------------------------------

            foreach (var page in templatePages)
            {
                var pageSlug =
                    page.Slug?
                        .Trim()
                        .ToLowerInvariant();

                if (string.IsNullOrWhiteSpace(pageSlug))
                    continue;

                // -----------------------------------------------------
                // PAGE UPSERT
                // -----------------------------------------------------

                var existingPage =
                    _dbType == DatabaseType.SqlServer
                        ? await _pageService
                            .FirstOrDefaultAsync<SqlPage>(
                                x => x.Slug == pageSlug)
                        : await _pageService
                            .FirstOrDefaultAsync<SqlPage>(
                                x => x.Slug == pageSlug);

                Guid pageId;

                if (existingPage.Data == null)
                {
                    // -------------------------------------------------
                    // CREATE NEW PAGE
                    // -------------------------------------------------

                    var pageEntity =
                        new PageDto
                        {
                            Name = page.PageName,
                            Slug = page.Slug,
                            IsDynamic = page.IsDynamic,
                            IsPublished = page.IsPublished,
                            MetaTitle =
                                page.MetaTitle ??
                                $"صفحه {page.PageName}",
                            MetaDescription =
                                page.MetaDescription ??
                                $"توضیحات صفحه {page.PageName}",
                            MetaKeywords =
                                page.MetaKeywords ??
                                "آموزشی، نمونه، سایت",
                            IsActive = true
                        };

                    var created =
                        await _pageService
                            .CreateAsync(pageEntity);

                    pageId =
                        created.Data.Id;
                }
                else
                {
                    // -------------------------------------------------
                    // UPDATE EXISTING PAGE
                    // -------------------------------------------------

                    pageId =
                        existingPage.Data.Id;

                    var pageEntity =
                        new PageDto
                        {
                            Id = existingPage.Data.Id,

                            Name = page.PageName,
                            Slug = page.Slug,

                            IsDynamic = page.IsDynamic,

                            IsPublished = page.IsPublished,

                            MetaTitle =
                                page.MetaTitle ??
                                $"صفحه {page.PageName}",

                            MetaDescription =
                                page.MetaDescription ??
                                $"توضیحات صفحه {page.PageName}",

                            MetaKeywords =
                                page.MetaKeywords ??
                                "آموزشی، نمونه، سایت",

                            IsActive = true
                        };

                    await _pageService
                        .UpdateAsync(pageEntity, pageEntity.Id);
                }

                // -----------------------------------------------------
                // COMPONENTS FROM JSON
                // -----------------------------------------------------

                int sectionIndex = 1;

                var activeComponentTypeIds =
                    new HashSet<Guid>();

                foreach (var component in page.Components)
                {
                    if (string.IsNullOrWhiteSpace(component.Code))
                        continue;

                    var componentCode =
                        component.Code.Trim();

                    // -------------------------------------------------
                    // COMPONENT RULE
                    //
                    // Rule is optional for creating the ComponentType.
                    // If it exists, we use its DefaultData.
                    // -------------------------------------------------

                    componentRules.TryGetValue(
                        componentCode,
                        out var rule);

                    // -------------------------------------------------
                    // COMPONENT TYPE UPSERT
                    // -------------------------------------------------

                    ComponentTypeDto componentTypeEntity;

                    if (!componentTypeCache.TryGetValue(
                            componentCode,
                            out componentTypeEntity!))
                    {
                        var existingComponentType =
                            _dbType == DatabaseType.SqlServer
                                ? await _componentTypeService
                                    .FirstOrDefaultAsync<SqlComponentType>(
                                        x => x.Code == componentCode)
                                : await _componentTypeService
                                    .FirstOrDefaultAsync<PgComponentType>(
                                        x => x.Code == componentCode);

                        if (existingComponentType.Data == null)
                        {
                            // -----------------------------------------
                            // CREATE COMPONENT TYPE
                            // -----------------------------------------

                            componentTypeEntity =
                                new ComponentTypeDto
                                {
                                    Name = componentCode,
                                    Code = componentCode,
                                    Type = component.Type,
                                    IsActive = true
                                };

                            if (string.IsNullOrWhiteSpace(
                                    componentTypeEntity.Code))
                            {
                                throw new InvalidOperationException(
                                    $"ComponentType Code is empty. " +
                                    $"Page='{pageSlug}', " +
                                    $"Component='{componentCode}'.");
                            }

                            var created =
                                await _componentTypeService
                                    .CreateAsync(componentTypeEntity);

                            componentTypeEntity.Id =
                                created.Data.Id;
                        }
                        else
                        {
                            // -----------------------------------------
                            // EXISTING COMPONENT TYPE
                            // -----------------------------------------

                            componentTypeEntity =
                                new ComponentTypeDto
                                {
                                    Id =
                                        existingComponentType.Data.Id,

                                    Name =
                                        componentCode,

                                    Code =
                                        componentCode,

                                    Type =
                                        component.Type,

                                    IsActive = true
                                };

                            // If the ComponentType already exists but
                            // was inactive, make it active again.
                            await _componentTypeService
                                .UpdateAsync(componentTypeEntity, componentTypeEntity.Id);
                        }

                        componentTypeCache[componentCode] =
                            componentTypeEntity;
                    }

                    activeComponentTypeIds.Add(
                        componentTypeEntity.Id);

                    // -------------------------------------------------
                    // FIND EXISTING SECTION
                    //
                    // Important:
                    // FirstOrDefaultAsync must be able to find an
                    // inactive Section as well.
                    // -------------------------------------------------

                    var existingSection =
                        _dbType == DatabaseType.SqlServer
                            ? await _sectionService
                                .FirstOrDefaultAsync<SqlSection>(
                                    x =>
                                        x.PageId == pageId &&
                                        x.ComponentTypeId ==
                                        componentTypeEntity.Id)
                            : await _sectionService
                                .FirstOrDefaultAsync<SqlSection>(
                                    x =>
                                        x.PageId == pageId &&
                                        x.ComponentTypeId ==
                                        componentTypeEntity.Id);

                    var rtl =
                        rule?.DefaultData?.Rtl;

                    var currentSectionSortOrder =
                        sectionIndex++;

                    if (existingSection.Data == null)
                    {
                        // -------------------------------------------------
                        // CREATE NEW SECTION
                        // -------------------------------------------------

                        var sectionEntity =
                            BuildSectionDto(
                                pageId,
                                componentTypeEntity.Id,
                                rtl,
                                currentSectionSortOrder);

                        sectionEntity.IsActive = true;

                        var createdSection =
                            await _sectionService
                                .CreateAsync(sectionEntity);

                        sectionEntity.Id =
                            createdSection.Data.Id;

                        // -------------------------------------------------
                        // CREATE SECTION ITEMS ONLY FOR A NEW SECTION
                        //
                        // Existing SectionItems are never touched.
                        // -------------------------------------------------

                        int itemSortOrder = 1;

                        var sectionGroupCache =
                            new Dictionary<string, Guid>(
                                StringComparer.OrdinalIgnoreCase);

                        if (rtl?.Items != null)
                        {
                            foreach (var item in rtl.Items)
                            {
                                Guid? sectionGroupItemId = null;

                                if (!string.IsNullOrWhiteSpace(
                                        item.SectionGroupItemCode))
                                {
                                    if (!sectionGroupCache.TryGetValue(
                                            item.SectionGroupItemCode,
                                            out var cachedId))
                                    {
                                        var groupResult =
                                            await _sectionGroupItemService
                                                .FirstOrDefaultAsync<
                                                    SqlSectionGroupItem>(
                                                    x =>
                                                        x.Code ==
                                                        item.SectionGroupItemCode);

                                        if (groupResult.Data != null)
                                        {
                                            cachedId =
                                                groupResult.Data.Id;

                                            sectionGroupCache[
                                                item.SectionGroupItemCode] =
                                                cachedId;
                                        }
                                    }

                                    if (cachedId != Guid.Empty)
                                    {
                                        sectionGroupItemId =
                                            cachedId;
                                    }
                                }

                                var sectionItem =
                                    BuildSectionItemDto(
                                        sectionEntity.Id,
                                        item,
                                        sectionGroupItemId,
                                        itemSortOrder++);

                                await _sectionItemService
                                    .CreateAsync(sectionItem);
                            }
                        }
                    }
                    else
                    {
                        // -------------------------------------------------
                        // EXISTING SECTION
                        //
                        // Keep all existing section data.
                        // Only update activation and sort order.
                        // SectionItems are never touched.
                        // -------------------------------------------------

                        var sectionEntity = new SectionDto
                        {
                            Id = existingSection.Data.Id,

                            PageId = existingSection.Data.PageId,

                            ComponentTypeId = existingSection.Data.ComponentTypeId,

                            Title = existingSection.Data.Title,
                            Subtitle = existingSection.Data.Subtitle,
                            Description = existingSection.Data.Description,

                            ImageUrl = existingSection.Data.ImageUrl,
                            ImageAlt = existingSection.Data.ImageAlt,

                            ImageUrl2 = existingSection.Data.ImageUrl2,
                            ImageAlt2 = existingSection.Data.ImageAlt2,

                            ImageUrl3 = existingSection.Data.ImageUrl3,
                            ImageAlt3 = existingSection.Data.ImageAlt3,

                            ImageUrl4 = existingSection.Data.ImageUrl4,
                            ImageAlt4 = existingSection.Data.ImageAlt4,

                            Icon = existingSection.Data.Icon,
                            IconAlt = existingSection.Data.IconAlt,
                            IconColor = existingSection.Data.IconColor,

                            Features = existingSection.Data.Features,
                            CopyrightText = existingSection.Data.CopyrightText,

                            ContactFirstNameLabel = existingSection.Data.ContactFirstNameLabel,
                            ContactLastNameLabel = existingSection.Data.ContactLastNameLabel,
                            ContactEmailLabel = existingSection.Data.ContactEmailLabel,
                            ContactMessageLabel = existingSection.Data.ContactMessageLabel,
                            ContactSubmitButtonText = existingSection.Data.ContactSubmitButtonText,

                            BackgroundColor = existingSection.Data.BackgroundColor,
                            HeaderColor = existingSection.Data.HeaderColor,
                            SubtitleColor = existingSection.Data.SubtitleColor,
                            DescriptionColor = existingSection.Data.DescriptionColor,

                            Link1Text = existingSection.Data.Link1Text,
                            Link1Url = existingSection.Data.Link1Url,
                            Link1Color = existingSection.Data.Link1Color,
                            Link1TargetId = existingSection.Data.Link1TargetId,
                            Link1TypeId = existingSection.Data.Link1TypeId,
                            Link1OpenInNewTab = existingSection.Data.Link1OpenInNewTab,

                            Link2Text = existingSection.Data.Link2Text,
                            Link2Url = existingSection.Data.Link2Url,
                            Link2Color = existingSection.Data.Link2Color,
                            Link2TargetId = existingSection.Data.Link2TargetId,
                            Link2TypeId = existingSection.Data.Link2TypeId,
                            Link2OpenInNewTab = existingSection.Data.Link2OpenInNewTab,

                            Link3Text = existingSection.Data.Link3Text,
                            Link3Url = existingSection.Data.Link3Url,
                            Link3Color = existingSection.Data.Link3Color,
                            Link3TargetId = existingSection.Data.Link3TargetId,
                            Link3TypeId = existingSection.Data.Link3TypeId,
                            Link3OpenInNewTab = existingSection.Data.Link3OpenInNewTab,

                            Link4Text = existingSection.Data.Link4Text,
                            Link4Url = existingSection.Data.Link4Url,
                            Link4Color = existingSection.Data.Link4Color,
                            Link4TargetId = existingSection.Data.Link4TargetId,
                            Link4TypeId = existingSection.Data.Link4TypeId,
                            Link4OpenInNewTab = existingSection.Data.Link4OpenInNewTab,

                            MapEmbedUrl = existingSection.Data.MapEmbedUrl,
                            VideoUrl = existingSection.Data.VideoUrl,
                            ThumbnailUrl = existingSection.Data.ThumbnailUrl,

                            ColumnsCount = existingSection.Data.ColumnsCount,

                            EnglishTitle = existingSection.Data.EnglishTitle,

                            IsLatestNews = existingSection.Data.IsLatestNews,
                            IsLatestProducts = existingSection.Data.IsLatestProducts,
                            IsSpecialOffers = existingSection.Data.IsSpecialOffers,
                            IsBestSellingProducts = existingSection.Data.IsBestSellingProducts,
                            IsActiveBrands = existingSection.Data.IsActiveBrands,
                            IsActiveCategries = existingSection.Data.IsActiveCategries,

                            // Only these are changed by the seeder
                            SortOrder = currentSectionSortOrder,
                            IsActive = true
                        };

                        await _sectionService
                            .UpdateAsync(sectionEntity, sectionEntity.Id);
                    }
                }

                // -----------------------------------------------------
                // DEACTIVATE OLD SECTIONS OF THIS PAGE
                //
                // Example:
                //
                // DB:
                // hero
                // categories
                // brands
                // oldBanner
                //
                // JSON:
                // categories
                // newArrivals
                // ...
                //
                // Result:
                // hero      -> inactive
                // categories -> active
                // brands     -> active
                // oldBanner  -> inactive
                // -----------------------------------------------------

                var activeSectionsQuery =
                    await _sectionService.GetAllViews();

                var activeSections =
                    await activeSectionsQuery
                        .Where(x => x.ParentId == pageId)
                        .ToListAsync();

                foreach (var oldSection in activeSections)
                {
                    if (activeComponentTypeIds.Contains(
                            oldSection.ComponentTypeId))
                    {
                        continue;
                    }

                    var deactivateSection = new SectionDto
                    {
                        Id = oldSection.Id.Value,

                        PageId = oldSection.ParentId,

                        ComponentTypeId = oldSection.ComponentTypeId,

                        Title = oldSection.Title,
                        Subtitle = oldSection.Subtitle,
                        Description = oldSection.Description,

                        ImageUrl = oldSection.ImageUrl,
                        ImageAlt = oldSection.ImageAlt,

                        ImageUrl2 = oldSection.ImageUrl2,
                        ImageAlt2 = oldSection.ImageAlt2,

                        ImageUrl3 = oldSection.ImageUrl3,
                        ImageAlt3 = oldSection.ImageAlt3,

                        ImageUrl4 = oldSection.ImageUrl4,
                        ImageAlt4 = oldSection.ImageAlt4,

                        Icon = oldSection.Icon,
                        IconAlt = oldSection.IconAlt,
                        IconColor = oldSection.IconColor,

                        Features = oldSection.Features,
                        CopyrightText = oldSection.CopyrightText,

                        ContactFirstNameLabel = oldSection.ContactFirstNameLabel,
                        ContactLastNameLabel = oldSection.ContactLastNameLabel,
                        ContactEmailLabel = oldSection.ContactEmailLabel,
                        ContactMessageLabel = oldSection.ContactMessageLabel,
                        ContactSubmitButtonText = oldSection.ContactSubmitButtonText,

                        BackgroundColor = oldSection.BackgroundColor,
                        HeaderColor = oldSection.HeaderColor,
                        SubtitleColor = oldSection.SubtitleColor,
                        DescriptionColor = oldSection.DescriptionColor,

                        Link1Text = oldSection.Link1Text,
                        Link1Url = oldSection.Link1Url,
                        Link1Color = oldSection.Link1Color,
                        Link1TargetId = oldSection.Link1TargetId,
                        Link1TypeId = oldSection.Link1TypeId,
                        Link1OpenInNewTab = oldSection.Link1OpenInNewTab,

                        Link2Text = oldSection.Link2Text,
                        Link2Url = oldSection.Link2Url,
                        Link2Color = oldSection.Link2Color,
                        Link2TargetId = oldSection.Link2TargetId,
                        Link2TypeId = oldSection.Link2TypeId,
                        Link2OpenInNewTab = oldSection.Link2OpenInNewTab,

                        Link3Text = oldSection.Link3Text,
                        Link3Url = oldSection.Link3Url,
                        Link3Color = oldSection.Link3Color,
                        Link3TargetId = oldSection.Link3TargetId,
                        Link3TypeId = oldSection.Link3TypeId,
                        Link3OpenInNewTab = oldSection.Link3OpenInNewTab,

                        Link4Text = oldSection.Link4Text,
                        Link4Url = oldSection.Link4Url,
                        Link4Color = oldSection.Link4Color,
                        Link4TargetId = oldSection.Link4TargetId,
                        Link4TypeId = oldSection.Link4TypeId,
                        Link4OpenInNewTab = oldSection.Link4OpenInNewTab,

                        MapEmbedUrl = oldSection.MapEmbedUrl,
                        VideoUrl = oldSection.VideoUrl,
                        ThumbnailUrl = oldSection.ThumbnailUrl,

                        ColumnsCount = oldSection.ColumnsCount,

                        EnglishTitle = oldSection.EnglishTitle,

                        IsLatestNews = oldSection.IsLatestNews,
                        IsLatestProducts = oldSection.IsLatestProducts,
                        IsSpecialOffers = oldSection.IsSpecialOffers,
                        IsBestSellingProducts = oldSection.IsBestSellingProducts,
                        IsActiveBrands = oldSection.IsActiveBrands,
                        IsActiveCategries = oldSection.IsActiveCategries,

                        // فقط وضعیت Section تغییر می‌کند
                        SortOrder = oldSection.SortOrder,
                        IsActive = false
                    };

                    await _sectionService.UpdateAsync(
                        deactivateSection,
                        deactivateSection.Id);

                    await _sectionService
                        .UpdateAsync(deactivateSection, deactivateSection.Id);
                }
            }

            // ---------------------------------------------------------
            // COMMIT
            // ---------------------------------------------------------

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
                if (!isContentPage)
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
                    if (string.IsNullOrWhiteSpace(componentTypeEntity.Code))
                    {
                        throw new Exception(
                            $"ComponentType Code is NULL. Component Code = '{component.Code}'");
                    }
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

                BackgroundColor = rtl?.BackgroundColor,
                HeaderColor = rtl?.HeaderColor,
                SubtitleColor = rtl?.SubtitleColor,
                DescriptionColor = rtl?.DescriptionColor,

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
                ColumnsCount = 1,
                IsActiveBrands = rtl?.IsActiveBrands,   
                IsActiveCategries = rtl?.IsActiveCategries,
                IsBestSellingProducts= rtl?.IsBestSellingProducts,
                IsLatestNews= rtl?.IsLatestNews,
                IsSpecialOffers= rtl?.IsSpecialOffers,
                EnglishTitle= rtl?.EnglishTitle,
                IsLatestProducts= rtl?.IsLatestProducts,
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
                SourceTitle = rtl.SourceTitle,
                SourceUrl = rtl.SourceUrl,
                Content = rtl.Content,
                ContentType = rtl.ContentType,
                IsPublished = true,
                PublishedAt = rtl?.PublishedAt,
                Slug = rtl.Slug,
                Summary = rtl?.Summary,
                IsActive = true,
                IsTest = false,
                Title = rtl?.Title,
                SortOrder = sortOrder,
                ImageAlt = rtl?.ImageAlt,
                ImageUrl = rtl?.ImageUrl,
                ExternalUrl = rtl?.ExternalUrl,
                CategoryId = rtl?.CategoryId,
                ImageDetailAlt = rtl?.ImageDetailAlt,
                ImageDetailUrl = rtl?.ImageDetailUrl,


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

        private async Task<Guid> SeedCategoryAsync(
            ProductCategorySeedModel category)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category));

            if (string.IsNullOrWhiteSpace(category.Slug))
                throw new InvalidOperationException(
                    "Category Slug cannot be empty.");

            var query =
                await _productCategoryService.GetAllViews();

            var existing =
                await query.FirstOrDefaultAsync(x =>
                    x.Slug == category.Slug);

            if (existing != null)
            {
                var model = new ProductCategoryCrud
                {
                    Id = existing.Id,

                    Name = category.Name,
                    Slug = category.Slug,

                    Description = category.Description,

                    SeoTitle = category.SeoTitle,
                    SeoDescription = category.SeoDescription,

                    Icon = category.Icon,
                    IconColor = category.IconColor,

                    SortOrder = category.SortOrder,
                    IsActive = category.IsActive
                };

                var result =
                    await _productCategoryService.UpdateAsync(
                        model,
                        existing.Id);

                if (!result.Success)
                {
                    throw new InvalidOperationException(
                        $"Category update failed. " +
                        $"Name: {category.Name}, " +
                        $"Slug: {category.Slug}");
                }

                return existing.Id;
            }

            var createModel = new ProductCategoryCrud
            {
                Name = category.Name,
                Slug = category.Slug,

                Description = category.Description,

                SeoTitle = category.SeoTitle,
                SeoDescription = category.SeoDescription,

                Icon = category.Icon,
                IconColor = category.IconColor,

                SortOrder = category.SortOrder,
                IsActive = category.IsActive
            };

            var createResult =
                await _productCategoryService.CreateAsync(createModel);

            if (!createResult.Success)
            {
                throw new InvalidOperationException(
                    $"Category create failed. " +
                    $"Name: {category.Name}, " +
                    $"Slug: {category.Slug}");
            }

            return createResult.Data.Id;
        }

        public async Task SeedCategoriesAsync()
        {
            var enabled = _configuration.GetValue<bool>(
                "Seed:Categories:Enabled");

            if (!enabled)
                return;

            var file = _configuration.GetValue<string>(
                "Seed:Categories:File");

            if (string.IsNullOrWhiteSpace(file))
                throw new InvalidOperationException(
                    "Seed:Categories:File is not configured.");

            var resourceName =
                $"Velora.Application.Shared.Resources.{file
                    .Replace("/", ".")
                    .Replace("\\", ".")}";

            var assembly = typeof(SeedJsonModel).Assembly;

            using var stream =
                assembly.GetManifestResourceStream(resourceName);

            if (stream == null)
                throw new FileNotFoundException(
                    $"Seed resource '{resourceName}' not found.");

            using var reader = new StreamReader(stream);

            var json = await reader.ReadToEndAsync();

            var model =
                JsonSerializer.Deserialize<CategorySeedRoot>(
                    json,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

            if (model == null)
                throw new InvalidOperationException(
                    $"Seed file '{file}' deserialize failed.");

            if (model.Categories == null)
                return;

            foreach (var category in model.Categories)
            {
                await SeedCategoryAsync(category);
            }
        }
        public async Task SeedBrandsAsync()
        {
            var enabled = _configuration.GetValue<bool>(
                "Seed:Brands:Enabled");

            if (!enabled)
                return;

            var file = _configuration.GetValue<string>(
                "Seed:Brands:File");

            if (string.IsNullOrWhiteSpace(file))
                throw new InvalidOperationException(
                    "Seed:Brands:File is not configured.");

            var resourceName =
                $"Velora.Application.Shared.Resources.{file
                    .Replace("/", ".")
                    .Replace("\\", ".")}";

            var assembly = typeof(SeedJsonModel).Assembly;

            using var stream =
                assembly.GetManifestResourceStream(resourceName);

            if (stream == null)
                throw new FileNotFoundException(
                    $"Seed resource '{resourceName}' not found.");

            using var reader = new StreamReader(stream);

            var json = await reader.ReadToEndAsync();

            var model =
                JsonSerializer.Deserialize<BrandSeedRoot>(
                    json,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

            if (model == null)
                throw new InvalidOperationException(
                    $"Seed file '{file}' deserialize failed.");

            if (model.Brands == null)
                return;

            foreach (var brand in model.Brands)
            {
                await SeedBrandAsync(brand);
            }
        }
        private async Task<Guid> SeedBrandAsync(
        ProductBrandSeedModel brand)
        {
            if (brand == null)
                throw new ArgumentNullException(nameof(brand));

            if (string.IsNullOrWhiteSpace(brand.Slug))
                throw new InvalidOperationException(
                    "Brand Slug cannot be empty.");

            var query =
                await _productBrandService.GetAllViews();

            var existing =
                await query.FirstOrDefaultAsync(x =>
                    x.Slug == brand.Slug);

            if (existing != null)
            {
                var model = new ProductBrandCrud
                {
                    Id = existing.Id,

                    Name = brand.Name,
                    Slug = brand.Slug,

                    Logo = brand.Logo,
                    Website = brand.Website,

                    Description = brand.Description,

                    SortOrder = brand.SortOrder,
                    IsActive = brand.IsActive
                };

                var result =
                    await _productBrandService.UpdateAsync(
                        model,
                        existing.Id.Value);

                if (!result.Success)
                {
                    throw new InvalidOperationException(
                        $"Brand update failed. " +
                        $"Name: {brand.Name}, " +
                        $"Slug: {brand.Slug}");
                }

                return existing.Id.Value;
            }

            var createModel = new ProductBrandCrud
            {
                Name = brand.Name,
                Slug = brand.Slug,

                Logo = brand.Logo,
                Website = brand.Website,

                Description = brand.Description,

                SortOrder = brand.SortOrder,
                IsActive = brand.IsActive
            };

            var createResult =
                await _productBrandService.CreateAsync(createModel);

            if (!createResult.Success)
            {
                throw new InvalidOperationException(
                    $"Brand create failed. " +
                    $"Name: {brand.Name}, " +
                    $"Slug: {brand.Slug}");
            }

            return createResult.Data.Id;
        }
        public async Task SeedProductTypesAsync()
        {
            var enabled = _configuration.GetValue<bool>(
                "Seed:ProductTypes:Enabled");

            if (!enabled)
                return;

            var file = _configuration.GetValue<string>(
                "Seed:ProductTypes:File");

            if (string.IsNullOrWhiteSpace(file))
                throw new InvalidOperationException(
                    "Seed:ProductTypes:File is not configured.");

            var resourceName =
                $"Velora.Application.Shared.Resources.{file
                    .Replace("/", ".")
                    .Replace("\\", ".")}";

            var assembly = typeof(SeedJsonModel).Assembly;

            using var stream =
                assembly.GetManifestResourceStream(resourceName);

            if (stream == null)
                throw new FileNotFoundException(
                    $"Seed resource '{resourceName}' not found.");

            using var reader = new StreamReader(stream);

            var json = await reader.ReadToEndAsync();

            var model =
                JsonSerializer.Deserialize<ProductTypeSeedRoot>(
                    json,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

            if (model == null)
                throw new InvalidOperationException(
                    $"Seed file '{file}' deserialize failed.");

            if (model.ProductTypes == null)
                return;

            foreach (var type in model.ProductTypes)
            {
                await SeedProductTypeAsync(type);
            }
        }
        private async Task<Guid> SeedProductTypeAsync(
            ProductTypeSeedModel type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            if (string.IsNullOrWhiteSpace(type.Code))
                throw new InvalidOperationException(
                    "ProductType Code cannot be empty.");

            var query =
                await _productTypeService.GetAllViews();

            var existing =
                await query.FirstOrDefaultAsync(x =>
                    x.Code == type.Code);

            if (existing != null)
            {
                var model = new ProductTypeCrud
                {
                    Id = existing.Id,

                    Name = type.Name,
                    Code = type.Code,

                    Description = type.Description,

                    SortOrder = type.SortOrder,
                    IsActive = type.IsActive
                };

                var result =
                    await _productTypeService.UpdateAsync(
                        model,
                        existing.Id);

                if (!result.Success)
                {
                    throw new InvalidOperationException(
                        $"ProductType update failed. " +
                        $"Name: {type.Name}, " +
                        $"Code: {type.Code}");
                }

                return existing.Id;
            }

            var createModel = new ProductTypeCrud
            {
                Name = type.Name,
                Code = type.Code,

                Description = type.Description,

                SortOrder = type.SortOrder,
                IsActive = type.IsActive
            };

            var createResult =
                await _productTypeService.CreateAsync(createModel);

            if (!createResult.Success)
            {
                throw new InvalidOperationException(
                    $"ProductType create failed. " +
                    $"Name: {type.Name}, " +
                    $"Code: {type.Code}");
            }

            return createResult.Data.Id;
        }
        public async Task SeedProductsAsync()
        {
            var enabled = _configuration.GetValue<bool>(
                "Seed:Products:Enabled");

            if (!enabled)
                return;

            var file = _configuration.GetValue<string>(
                "Seed:Products:File");

            if (string.IsNullOrWhiteSpace(file))
                throw new InvalidOperationException(
                    "Seed:Products:File is not configured.");

            var resourceName =
                $"Velora.Application.Shared.Resources.{file
                    .Replace("/", ".")
                    .Replace("\\", ".")}";

            var assembly = typeof(SeedJsonModel).Assembly;

            using var stream =
                assembly.GetManifestResourceStream(resourceName);

            if (stream == null)
                throw new FileNotFoundException(
                    $"Seed resource '{resourceName}' not found.");

            using var reader = new StreamReader(stream);

            var json = await reader.ReadToEndAsync();

            var model =
                JsonSerializer.Deserialize<ProductSeedRoot>(
                    json,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

            if (model == null)
                throw new InvalidOperationException(
                    $"Seed file '{file}' deserialize failed.");

            foreach (var product in model.Products)
            {
                if (product == null)
                    throw new InvalidOperationException(
                        "Product seed item cannot be null.");

                if (string.IsNullOrWhiteSpace(product.Name))
                    throw new InvalidOperationException(
                        "Product Name cannot be empty.");

                if (string.IsNullOrWhiteSpace(product.Slug))
                    throw new InvalidOperationException(
                        $"Product Slug cannot be empty. " +
                        $"Name: {product.Name}");

                await SeedProductAsync(product);
            }
        }
        //public async Task SeedProductsAsync()
        //{
        //    var enabled = _configuration.GetValue<bool>(
        //        "Seed:Products:Enabled");

        //    if (!enabled)
        //        return;

        //    var file = _configuration.GetValue<string>(
        //        "Seed:Products:File");

        //    if (string.IsNullOrWhiteSpace(file))
        //        throw new InvalidOperationException(
        //            "Seed:Products:File is not configured.");

        //    var resourceName =
        //        $"Velora.Application.Shared.Resources.{file
        //            .Replace("/", ".")
        //            .Replace("\\", ".")}";

        //    var assembly = typeof(SeedJsonModel).Assembly;

        //    using var stream =
        //        assembly.GetManifestResourceStream(resourceName);

        //    if (stream == null)
        //        throw new FileNotFoundException(
        //            $"Seed resource '{resourceName}' not found.");

        //    using var reader = new StreamReader(stream);

        //    var json = await reader.ReadToEndAsync();

        //    var model =
        //        JsonSerializer.Deserialize<ProductSeedRoot>(
        //            json,
        //            new JsonSerializerOptions
        //            {
        //                PropertyNameCaseInsensitive = true
        //            });

        //    if (model == null)
        //        throw new InvalidOperationException(
        //            $"Seed file '{file}' deserialize failed.");

        //    foreach (var item in model.Products)
        //    {
        //        // دسته بندی
        //        var categoryId =
        //            await SeedCategoryAsync(item.Category);

        //        // برند
        //        var brandId =
        //            await SeedBrandAsync(item.Brand);

        //        // نوع محصول
        //        var productTypeId =
        //            await SeedProductTypeAsync(item.ProductType);

        //        var productId = await SeedProductAsync(
        //     item.Product,
        //     categoryId,
        //     brandId,
        //     productTypeId);

        //        var productQuery = await _productService.GetAllViews();

        //        var productExists = await productQuery
        //            .AnyAsync(x => x.Id == productId);

        //        if (!productExists)
        //        {
        //            throw new Exception(
        //                $"Product was not found after seed. ProductId: {productId}, Slug: {item.Product.Slug}");
        //        }

        //        // تصاویر
        //        await SeedProductFilesAsync(
        //            productId,
        //            item.Files);

        //        // موجودی اولیه
        //        if (item.Variants == null || !item.Variants.Any())
        //        {
        //            await SeedProductInventoryAsync(
        //                productId,
        //                item.Product.InitialStock);
        //        }

        //        // واریانت ها
        //        await SeedProductVariantsAsync(
        //            productId,
        //            item.Variants);

        //        // ویژگی ها
        //        await SeedProductAttributeValuesAsync(
        //            productId,
        //            item.Attributes);

        //        // تگ ها
        //        await SeedProductTagsAsync(
        //            productId,
        //            item.Tags);
        //    }
        //}

        public async Task SeedProductTagsAsync()
        {
            var enabled =
                _configuration.GetValue<bool>(
                    "Seed:ProductTags:Enabled");

            if (!enabled)
                return;

            var file =
                _configuration.GetValue<string>(
                    "Seed:ProductTags:File");

            if (string.IsNullOrWhiteSpace(file))
                throw new InvalidOperationException(
                    "Seed:ProductTags:File is not configured.");

            var resourceName =
                $"Velora.Application.Shared.Resources.{file
                    .Replace("/", ".")
                    .Replace("\\", ".")}";

            var assembly =
                typeof(SeedJsonModel).Assembly;

            using var stream =
                assembly.GetManifestResourceStream(resourceName);

            if (stream == null)
                throw new FileNotFoundException(
                    $"Seed resource '{resourceName}' not found.");

            using var reader =
                new StreamReader(stream);

            var json =
                await reader.ReadToEndAsync();

            var model =
                JsonSerializer.Deserialize<ProductTagSeedRoot>(
                    json,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

            if (model == null)
                throw new InvalidOperationException(
                    $"Seed file '{file}' deserialize failed.");

            foreach (var item in model.ProductTags)
            {
                if (item == null)
                    throw new InvalidOperationException(
                        "ProductTag seed item cannot be null.");

                if (string.IsNullOrWhiteSpace(item.ProductSlug))
                    throw new InvalidOperationException(
                        "ProductTag ProductSlug cannot be empty.");

                var productId =
                    await GetProductIdBySlugAsync(
                        item.ProductSlug);

                await SeedProductTagsAsync(
                    productId,
                    item.Tags);
            }
        }
        private async Task SeedProductTagsAsync(
            Guid productId,
            List<ProductTagSeedModel>? tags)
        {
            if (tags == null)
                return;

            var existingMappings =
                await _productTagMappingService
                    .GetByProductTagMappingsAsync(productId);

            var existingTagsQuery =
                await _productTagService
                    .GetAllViews();

            var existingTags =
                await existingTagsQuery
                    .ToListAsync();

            // ---------------------------------------------------------
            // REMOVE OLD MAPPINGS
            // ---------------------------------------------------------

            var incomingSlugs =
                tags
                    .Select(x => x.Slug)
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .ToHashSet(StringComparer.OrdinalIgnoreCase);

            foreach (var mapping in existingMappings)
            {
                var tag =
                    existingTags.FirstOrDefault(
                        x => x.Id == mapping.ProductTagId);

                if (tag == null)
                    continue;

                if (!incomingSlugs.Contains(tag.Slug))
                {
                    await _productTagMappingService
                        .DeleteAsync(mapping.Id);
                }
            }

            // ---------------------------------------------------------
            // CREATE / UPDATE TAGS
            // ---------------------------------------------------------

            foreach (var item in tags)
            {
                if (string.IsNullOrWhiteSpace(item.Name))
                    throw new InvalidOperationException(
                        $"Product tag Name cannot be empty. " +
                        $"ProductId: {productId}");

                if (string.IsNullOrWhiteSpace(item.Slug))
                    throw new InvalidOperationException(
                        $"Product tag Slug cannot be empty. " +
                        $"ProductId: {productId}");

                var existingTag =
                    existingTags.FirstOrDefault(
                        x => x.Slug == item.Slug);

                Guid tagId;

                // -----------------------------------------------------
                // CREATE TAG
                // -----------------------------------------------------

                if (existingTag == null)
                {
                    var tagCrud =
                        new ProductTagCrud
                        {
                            Name = item.Name,
                            Slug = item.Slug,
                          SortOrder = item.SortOrder,
                          IsActive=true
                        };

                    var result =
                        await _productTagService
                            .CreateAsync(tagCrud);

                    if (!result.Success)
                    {
                        throw new InvalidOperationException(
                            $"Product tag create failed. " +
                            $"Slug: {item.Slug}");
                    }

                    tagId = result.Data.Id;
                }
                else
                {
                    var tagCrud =
                        new ProductTagCrud
                        {
                            Id = existingTag.Id,
                            Name = item.Name,
                            Slug = item.Slug,
                            SortOrder= item.SortOrder,
                            IsActive=true
                        };

                    var result =
                        await _productTagService
                            .UpdateAsync(
                                tagCrud,
                                existingTag.Id);

                    if (!result.Success)
                    {
                        throw new InvalidOperationException(
                            $"Product tag update failed. " +
                            $"Slug: {item.Slug}");
                    }

                    tagId = existingTag.Id.Value;
                }

                // -----------------------------------------------------
                // PRODUCT ↔ TAG MAPPING
                // -----------------------------------------------------

                var existingMapping =
                    existingMappings.FirstOrDefault(
                        x => x.ProductTagId == tagId);

                if (existingMapping == null)
                {
                    var mapping =
                        new ProductTagMappingDto
                        {
                            ProductId = productId,
                            ProductTagId = tagId
                        };

                    var result =
                        await _productTagMappingService
                            .CreateAsync(mapping);

                    if (!result.Success)
                    {
                        throw new InvalidOperationException(
                            $"Product tag mapping create failed. " +
                            $"ProductId: {productId}, " +
                            $"TagId: {tagId}");
                    }
                }
                else
                {
                    var mapping =
                        new ProductTagMappingDto
                        {
                            Id = existingMapping.Id,
                            ProductId = productId,
                            ProductTagId = tagId
                        };

                    var result =
                        await _productTagMappingService
                            .UpdateAsync(
                                mapping,
                                existingMapping.Id);

                    if (!result.Success)
                    {
                        throw new InvalidOperationException(
                            $"Product tag mapping update failed. " +
                            $"ProductId: {productId}, " +
                            $"TagId: {tagId}");
                    }
                }
            }
        }
        private async Task<Guid> SeedProductAsync(ProductSeedModel item)
        {
            if (item == null)
                throw new Exception("Product seed item is NULL.");

            if (string.IsNullOrWhiteSpace(item.Name))
            {
                throw new Exception(
                    $"PRODUCT NAME IS EMPTY BEFORE SEED. Slug: '{item.Slug}'");
            }

            if (string.IsNullOrWhiteSpace(item.Slug))
            {
                throw new Exception(
                    $"PRODUCT SLUG IS EMPTY. Name: '{item.Name}'");
            }

            // ---------------------------------------------------------
            // CATEGORY
            // ---------------------------------------------------------

            if (string.IsNullOrWhiteSpace(item.CategorySlug))
            {
                throw new Exception(
                    $"Product CategorySlug is empty. " +
                    $"Product: '{item.Name}', Slug: '{item.Slug}'");
            }

            var categoryQuery =
                await _productCategoryService.GetAllViews();

            var category =
                await categoryQuery.FirstOrDefaultAsync(x =>
                    x.Slug == item.CategorySlug);

            if (category == null)
            {
                throw new Exception(
                    $"Product category not found. " +
                    $"CategorySlug: '{item.CategorySlug}', " +
                    $"Product: '{item.Name}', " +
                    $"Slug: '{item.Slug}'");
            }

            var categoryId = category.Id;

            // ---------------------------------------------------------
            // BRAND
            // ---------------------------------------------------------

            if (string.IsNullOrWhiteSpace(item.BrandSlug))
            {
                throw new Exception(
                    $"Product BrandSlug is empty. " +
                    $"Product: '{item.Name}', Slug: '{item.Slug}'");
            }

            var brandQuery =
                await _productBrandService.GetAllViews();

            var brand =
                await brandQuery.FirstOrDefaultAsync(x =>
                    x.Slug == item.BrandSlug);

            if (brand == null)
            {
                throw new Exception(
                    $"Product brand not found. " +
                    $"BrandSlug: '{item.BrandSlug}', " +
                    $"Product: '{item.Name}', " +
                    $"Slug: '{item.Slug}'");
            }

            var brandId = brand.Id;

            // ---------------------------------------------------------
            // PRODUCT TYPE
            // ---------------------------------------------------------

            if (string.IsNullOrWhiteSpace(item.ProductTypeCode))
            {
                throw new Exception(
                    $"Product ProductTypeCode is empty. " +
                    $"Product: '{item.Name}', Slug: '{item.Slug}'");
            }

            var productTypeQuery =
                await _productTypeService.GetAllViews();

            var productType =
                await productTypeQuery.FirstOrDefaultAsync(x =>
                    x.Code == item.ProductTypeCode);

            if (productType == null)
            {
                throw new Exception(
                    $"Product type not found. " +
                    $"ProductTypeCode: '{item.ProductTypeCode}', " +
                    $"Product: '{item.Name}', " +
                    $"Slug: '{item.Slug}'");
            }

            var productTypeId = productType.Id;

            // ---------------------------------------------------------
            // PRODUCT
            // ---------------------------------------------------------

            var products =
                await _productService.GetAllViews();

            var existProduct =
                await products.FirstOrDefaultAsync(x =>
                    x.Slug == item.Slug);

            ProductCrud model;

            // ---------------------------------------------------------
            // UPDATE
            // ---------------------------------------------------------

            if (existProduct != null)
            {
                model = new ProductCrud
                {
                    Id = existProduct.Id,

                    Name = item.Name,
                    Slug = item.Slug,

                    CategoryId = categoryId,
                    BrandId = brandId,
                    ProductTypeId = productTypeId,

                    Summary = item.Summary,
                    Description = item.Description,

                    Price = item.Price,

                    Barcode = item.Barcode,
                    Sku = item.Sku,

                    Weight = item.Weight,

                    MainImage = item.MainImage,
                    Thumbnail = item.Thumbnail,

                    SeoTitle = item.SeoTitle,
                    SeoDescription = item.SeoDescription,

                    SortOrder = item.SortOrder,

                    IsFeatured = item.IsFeatured,
                    IsPublished = item.IsPublished,
                    IsActive = item.IsActive,

                    // اطلاعات موجود محصول حفظ شود
                    ProductTagIds = existProduct.ProductTagIds,
                    BrandName = existProduct.BrandName,
                    CategoryName = existProduct.CategoryName,
                    ProductTagNames = existProduct.ProductTagNames,
                    ProductTypeName = existProduct.ProductTypeName,
                };

                Console.WriteLine(
                    $"SEED PRODUCT UPDATE => " +
                    $"Id={model.Id}, " +
                    $"Name='{model.Name}', " +
                    $"Slug='{model.Slug}', " +
                    $"Category='{item.CategorySlug}', " +
                    $"Brand='{item.BrandSlug}', " +
                    $"Type='{item.ProductTypeCode}'");

                if (string.IsNullOrWhiteSpace(model.Name))
                {
                    throw new Exception(
                        $"PRODUCT UPDATE NAME IS EMPTY. " +
                        $"Id: {model.Id}, " +
                        $"Slug: {model.Slug}");
                }

                var result =
                    await _productService.UpdateAsync(
                        model,
                        model.Id);

                if (!result.Success)
                {
                    throw new Exception(
                        $"Product update failed: {item.Name}");
                }

                return existProduct.Id;
            }

            // ---------------------------------------------------------
            // CREATE
            // ---------------------------------------------------------

            model = new ProductCrud
            {
                Name = item.Name,
                Slug = item.Slug,

                CategoryId = categoryId,
                BrandId = brandId,
                ProductTypeId = productTypeId,

                Summary = item.Summary,
                Description = item.Description,

                Price = item.Price,

                Barcode = item.Barcode,
                Sku = item.Sku,

                Weight = item.Weight,

                MainImage = item.MainImage,
                Thumbnail = item.Thumbnail,

                SeoTitle = item.SeoTitle,
                SeoDescription = item.SeoDescription,

                SortOrder = item.SortOrder,

                IsFeatured = item.IsFeatured,
                IsPublished = item.IsPublished,
                IsActive = item.IsActive,
            };

            Console.WriteLine(
                $"SEED PRODUCT CREATE => " +
                $"Name='{model.Name}', " +
                $"Slug='{model.Slug}', " +
                $"Category='{item.CategorySlug}', " +
                $"Brand='{item.BrandSlug}', " +
                $"Type='{item.ProductTypeCode}'");

            var createResult =
                await _productService.CreateAsync(model);

            if (!createResult.Success)
            {
                throw new Exception(
                    $"Product create failed: {item.Name}");
            }

            return createResult.Data.Id;
        }
        public async Task SeedProductFilesAsync()
        {
            var enabled =
                _configuration.GetValue<bool>(
                    "Seed:ProductFiles:Enabled");

            if (!enabled)
                return;

            var file =
                _configuration.GetValue<string>(
                    "Seed:ProductFiles:File");

            if (string.IsNullOrWhiteSpace(file))
                throw new InvalidOperationException(
                    "Seed:ProductFiles:File is not configured.");

            var resourceName =
                $"Velora.Application.Shared.Resources.{file
                    .Replace("/", ".")
                    .Replace("\\", ".")}";

            var assembly =
                typeof(SeedJsonModel).Assembly;

            using var stream =
                assembly.GetManifestResourceStream(resourceName);

            if (stream == null)
                throw new FileNotFoundException(
                    $"Seed resource '{resourceName}' not found.");

            using var reader =
                new StreamReader(stream);

            var json =
                await reader.ReadToEndAsync();

            var model =
                JsonSerializer.Deserialize<ProductFileSeedRoot>(
                    json,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

            if (model == null)
                throw new InvalidOperationException(
                    $"Seed file '{file}' deserialize failed.");

            foreach (var item in model.ProductFiles)
            {
                if (item == null)
                    throw new InvalidOperationException(
                        "ProductFile seed item cannot be null.");

                if (string.IsNullOrWhiteSpace(item.ProductSlug))
                    throw new InvalidOperationException(
                        "ProductFile ProductSlug cannot be empty.");

                var productId =
                    await GetProductIdBySlugAsync(
                        item.ProductSlug);

                await SeedProductFilesAsync(
                    productId,
                    item.Files);
            }
        }
        private async Task<Guid> GetProductIdBySlugAsync(
    string productSlug)
        {
            if (string.IsNullOrWhiteSpace(productSlug))
                throw new InvalidOperationException(
                    "ProductSlug cannot be empty.");

            var products =
                await _productService.GetAllViews();

            var product =
                await products.FirstOrDefaultAsync(
                    x => x.Slug == productSlug);

            if (product == null)
                throw new InvalidOperationException(
                    $"Product with slug '{productSlug}' was not found.");

            return product.Id;
        }
        private async Task SeedProductFilesAsync(
    Guid productId,
    List<ProductFileSeedModel> files)
        {
            if (files == null || !files.Any())
                return;

            var existingFilesQuery =
                await _productFileService.GetAllViews();

            var existingFiles =
                await existingFilesQuery
                    .Where(x => x.ParentId == productId)
                    .ToListAsync();

            foreach (var item in files)
            {
                if (string.IsNullOrWhiteSpace(item.FileUrl))
                    throw new InvalidOperationException(
                        $"Product file FileUrl cannot be empty. " +
                        $"ProductId: {productId}");

                var existing =
                    existingFiles.FirstOrDefault(
                        x => x.FileUrl == item.FileUrl);

                if (existing != null)
                {
                    var model =
                        new ProductFileCrud
                        {
                            Id = existing.Id,
                            ParentId = productId,
                            FileUrl = item.FileUrl,
                            ThumbnailUrl = item.ThumbnailUrl,
                            Title = item.Title,
                            Alt = item.Alt,
                            MediaType = item.MediaType,
                            IsMain = item.IsMain,
                            SortOrder = item.SortOrder,
                            IsActive = true
                        };

                    var result =
                        await _productFileService
                            .UpdateAsync(
                                model,
                                existing.Id);

                    if (!result.Success)
                    {
                        throw new InvalidOperationException(
                            $"Product file update failed. " +
                            $"FileUrl: {item.FileUrl}");
                    }
                }
                else
                {
                    var model =
                        new ProductFileCrud
                        {
                            ParentId = productId,
                            FileUrl = item.FileUrl,
                            ThumbnailUrl = item.ThumbnailUrl,
                            Title = item.Title,
                            Alt = item.Alt,
                            MediaType = item.MediaType,
                            IsMain = item.IsMain,
                            SortOrder = item.SortOrder,
                            IsActive = true
                        };

                    var result =
                        await _productFileService
                            .CreateAsync(model);

                    if (!result.Success)
                    {
                        throw new InvalidOperationException(
                            $"Product file create failed. " +
                            $"FileUrl: {item.FileUrl}");
                    }
                }
            }
        }
        public async Task SeedProductVariantsAsync()
        {
            var enabled =
                _configuration.GetValue<bool>(
                    "Seed:ProductVariants:Enabled");

            if (!enabled)
                return;

            var file =
                _configuration.GetValue<string>(
                    "Seed:ProductVariants:File");

            if (string.IsNullOrWhiteSpace(file))
                throw new InvalidOperationException(
                    "Seed:ProductVariants:File is not configured.");

            var resourceName =
                $"Velora.Application.Shared.Resources.{file
                    .Replace("/", ".")
                    .Replace("\\", ".")}";

            var assembly =
                typeof(SeedJsonModel).Assembly;

            using var stream =
                assembly.GetManifestResourceStream(resourceName);

            if (stream == null)
                throw new FileNotFoundException(
                    $"Seed resource '{resourceName}' not found.");

            using var reader =
                new StreamReader(stream);

            var json =
                await reader.ReadToEndAsync();

            var model =
                JsonSerializer.Deserialize<ProductVariantSeedRoot>(
                    json,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

            if (model == null)
                throw new InvalidOperationException(
                    $"Seed file '{file}' deserialize failed.");

            foreach (var item in model.ProductVariants)
            {
                if (item == null)
                    throw new InvalidOperationException(
                        "ProductVariant seed item cannot be null.");

                if (string.IsNullOrWhiteSpace(item.ProductSlug))
                    throw new InvalidOperationException(
                        "ProductVariant ProductSlug cannot be empty.");

                var productId =
                    await GetProductIdBySlugAsync(
                        item.ProductSlug);

                await SeedProductVariantsAsync(
                    productId,
                    item.Variants);
            }
        }
        private async Task SeedProductVariantsAsync(
          Guid productId,
          List<ProductVariantSeedModel>? variants)
        {
            if (variants == null)
                return;

            var existingVariantsQuery =
                await _productVariantService
                    .GetAllViews();

            var existingVariants =
                await existingVariantsQuery
                    .Where(x => x.ParentId == productId)
                    .ToListAsync();

            // ---------------------------------------------------------
            // DEACTIVATE REMOVED VARIANTS
            // ---------------------------------------------------------

            var incomingSkus =
                variants
                    .Where(x => !string.IsNullOrWhiteSpace(x.Sku))
                    .Select(x => x.Sku!)
                    .ToHashSet(
                        StringComparer.OrdinalIgnoreCase);

            foreach (var existing in existingVariants)
            {
                if (string.IsNullOrWhiteSpace(existing.Sku))
                    continue;

                if (!incomingSkus.Contains(existing.Sku))
                {
                    var model =
                        new ProductVariantCrud
                        {
                            Id = existing.Id,
                            ParentId = productId,
                            Name = existing.Name,
                            Price = existing.Price,
                            ComparePrice = existing.ComparePrice,
                            Image = existing.Image,
                            Sku = existing.Sku,
                            Barcode = existing.Barcode,
                            IsDefault = existing.IsDefault,
                            SortOrder = existing.SortOrder,
                            IsActive = false
                        };

                    var result =
                        await _productVariantService
                            .UpdateAsync(
                                model,
                                existing.Id);

                    if (!result.Success)
                    {
                        throw new InvalidOperationException(
                            $"Product variant deactivation failed. " +
                            $"SKU: {existing.Sku}");
                    }
                }
            }

            // ---------------------------------------------------------
            // CREATE / UPDATE VARIANTS
            // ---------------------------------------------------------

            foreach (var item in variants)
            {
                if (string.IsNullOrWhiteSpace(item.Name))
                    throw new InvalidOperationException(
                        $"Product variant Name cannot be empty. " +
                        $"ProductId: {productId}");

                if (string.IsNullOrWhiteSpace(item.Sku))
                    throw new InvalidOperationException(
                        $"Product variant SKU cannot be empty. " +
                        $"ProductId: {productId}");

                var existing =
                    existingVariants.FirstOrDefault(
                        x => x.Sku == item.Sku);

                if (existing != null)
                {
                    var model =
                        new ProductVariantCrud
                        {
                            Id = existing.Id,
                            ParentId = productId,
                            Name = item.Name,
                            Price = item.Price,
                            ComparePrice = item.ComparePrice,
                            Image = item.Image,
                            Sku = item.Sku,
                            Barcode = item.Barcode,
                            IsDefault = item.IsDefault,
                            SortOrder = item.SortOrder,
                            IsActive = item.IsActive
                        };

                    var result =
                        await _productVariantService
                            .UpdateAsync(
                                model,
                                existing.Id);

                    if (!result.Success)
                    {
                        throw new InvalidOperationException(
                            $"Product variant update failed. " +
                            $"SKU: {item.Sku}");
                    }
                }
                else
                {
                    var model =
                        new ProductVariantCrud
                        {
                            ParentId = productId,
                            Name = item.Name,
                            Price = item.Price,
                            ComparePrice = item.ComparePrice,
                            Image = item.Image,
                            Sku = item.Sku,
                            Barcode = item.Barcode,
                            IsDefault = item.IsDefault,
                            SortOrder = item.SortOrder,
                            IsActive = item.IsActive
                        };

                    var result =
                        await _productVariantService
                            .CreateAsync(model);

                    if (!result.Success)
                    {
                        throw new InvalidOperationException(
                            $"Product variant create failed. " +
                            $"SKU: {item.Sku}");
                    }
                }
            }
        }
        private const string InitialStockReasonCode = "INITIAL_STOCK";

        private async Task SeedProductAttributesAsync()
        {
            var enabled =
                _configuration.GetValue<bool>(
                    "Seed:ProductAttributes:Enabled");

            if (!enabled)
                return;

            var file =
                _configuration.GetValue<string>(
                    "Seed:ProductAttributes:File");

            if (string.IsNullOrWhiteSpace(file))
                throw new InvalidOperationException(
                    "Seed:ProductAttributes:File is not configured.");

            var resourceName =
                $"Velora.Application.Shared.Resources.{file
                    .Replace("/", ".")
                    .Replace("\\", ".")}";

            var assembly =
                typeof(SeedJsonModel).Assembly;

            await using var stream =
                assembly.GetManifestResourceStream(resourceName);

            if (stream == null)
            {
                throw new FileNotFoundException(
                    $"Embedded seed resource not found: {resourceName}");
            }

            using var reader =
                new StreamReader(stream);

            var json =
                await reader.ReadToEndAsync();

            var root =
                JsonSerializer.Deserialize<ProductAttributeSeedRoot>(
                    json,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

            if (root == null)
                throw new InvalidOperationException(
                    "Product attribute seed JSON is invalid.");

            foreach (var item in root.ProductAttributes)
            {
                if (string.IsNullOrWhiteSpace(item.ProductSlug))
                {
                    throw new InvalidOperationException(
                        "ProductSlug cannot be empty in product attribute seed.");
                }

                var productId =
                    await GetProductIdBySlugAsync(
                        item.ProductSlug);

                await SeedProductAttributeValuesAsync(
                    productId,
                    item.Attributes);
            }
        }
        private async Task SeedProductAttributeValuesAsync(
            Guid productId,
            List<ProductAttributeSeedModel> attributes)
        {
            if (attributes == null || !attributes.Any())
                return;

            // =========================================================
            // ALL ATTRIBUTES
            // =========================================================

            var attributesQuery =
                await _productAttributeService.GetAllViews();

            // =========================================================
            // EXISTING VALUES FOR THIS PRODUCT
            // =========================================================

            var valuesQuery =
                await _productAttributeValueService.GetAllViews();

            var existingValues =
                await valuesQuery
                    .Where(x => x.ParentId == productId)
                    .ToListAsync();

            // =========================================================
            // CREATE / UPDATE
            // =========================================================

            foreach (var item in attributes)
            {
                if (string.IsNullOrWhiteSpace(item.Code))
                    throw new InvalidOperationException(
                        $"Product attribute Code cannot be empty. ProductId: {productId}");

                if (string.IsNullOrWhiteSpace(item.Name))
                    throw new InvalidOperationException(
                        $"Product attribute Name cannot be empty. Code: {item.Code}");

                if (string.IsNullOrWhiteSpace(item.Value))
                    throw new InvalidOperationException(
                        $"Product attribute Value cannot be empty. Code: {item.Code}");

                // =====================================================
                // FIND ATTRIBUTE BY CODE
                // =====================================================

                var attribute =
                    await attributesQuery
                        .FirstOrDefaultAsync(
                            x => x.Code == item.Code);

                // =====================================================
                // CREATE ATTRIBUTE IF NOT EXISTS
                // =====================================================

                if (attribute == null)
                {
                    var createAttribute =
                        new ProductAttributeCrud
                        {
                            Name = item.Name,
                            Code = item.Code,
                            IsActive = true
                        };

                    var result =
                        await _productAttributeService
                            .CreateAsync(createAttribute);

                    if (!result.Success)
                    {
                        throw new InvalidOperationException(
                            $"Product attribute create failed. " +
                            $"Code: {item.Code}");
                    }

                    // بعد از Create دوباره Attribute را پیدا می‌کنیم
                    attribute =
                        await attributesQuery
                            .FirstOrDefaultAsync(
                                x => x.Id == result.Data.Id);

                    if (attribute == null)
                    {
                        throw new InvalidOperationException(
                            $"Product attribute was created but could not be loaded. " +
                            $"Code: {item.Code}");
                    }
                }

                // =====================================================
                // FIND PRODUCT ATTRIBUTE VALUE
                // =====================================================

                var existingValue =
                    existingValues.FirstOrDefault(
                        x =>
                            x.ProductAttributeId == attribute.Id);

                // =====================================================
                // UPDATE
                // =====================================================

                if (existingValue != null)
                {
                    var model =
                        new ProductAttributeValueCrud
                        {
                            Id = existingValue.Id,

                            ParentId = productId,

                            ProductAttributeId =
                                attribute.Id,

                            Value = item.Value,

                            SortOrder = item.SortOrder
                        };

                    var result =
                        await _productAttributeValueService
                            .UpdateAsync(
                                model,
                                existingValue.Id);

                    if (!result.Success)
                    {
                        throw new InvalidOperationException(
                            $"Product attribute value update failed. " +
                            $"ProductId: {productId}, " +
                            $"AttributeCode: {item.Code}");
                    }
                }
                // =====================================================
                // CREATE
                // =====================================================

                else
                {
                    var model =
                        new ProductAttributeValueCrud
                        {
                            ParentId = productId,

                            ProductAttributeId =
                                attribute.Id,

                            Value = item.Value,

                            SortOrder = item.SortOrder
                        };

                    var result =
                        await _productAttributeValueService
                            .CreateAsync(model);

                    if (!result.Success)
                    {
                        throw new InvalidOperationException(
                            $"Product attribute value create failed. " +
                            $"ProductId: {productId}, " +
                            $"AttributeCode: {item.Code}");
                    }
                }
            }
        }
        private async Task<Guid> GetInitialStockReasonIdAsync()
        {
            if (_initialStockReasonId.HasValue)
                return _initialStockReasonId.Value;

            var reasonsQuery =
                await _inventoryTransactionReasonService
                    .GetAllViews();

            var reason =
                await reasonsQuery
                    .AsNoTracking()
                    .FirstOrDefaultAsync(
                        x => x.Code == InitialStockReasonCode);

            if (reason != null)
            {
                _initialStockReasonId = reason.Id;
                return reason.Id;
            }

            var createReason =
                new InventoryTransactionReasonCrud
                {
                    Name = "موجودی اولیه",
                    Code = InitialStockReasonCode
                };

            var result =
                await _inventoryTransactionReasonService
                    .CreateAsync(createReason);

            if (!result.Success)
            {
                throw new InvalidOperationException(
                    "Initial stock reason creation failed. " +
                    $"Message: {result.Message}");
            }

            _initialStockReasonId = result.Data.Id;

            return result.Data.Id;
        }
        private async Task SeedProductInventoryAsync(
            Guid productId,
            int quantity,
            Guid? variantId = null)
        {
            if (quantity <= 0)
                return;

            // =========================================================
            // INITIAL STOCK REASON
            // =========================================================

            var reasonId =
                await GetInitialStockReasonIdAsync();

            // =========================================================
            // EXISTING TRANSACTION
            // =========================================================

            var transactionsQuery =
                await _productInventoryTransactionService
                    .GetInventoryTransactionQuery();

            var existing =
                await transactionsQuery
                    .FirstOrDefaultAsync(
                        x =>
                            x.ParentId == productId &&
                            x.ProductVariantId == variantId &&
                            x.ReasonId == reasonId);

            // =========================================================
            // CREATE / UPDATE
            // =========================================================

            if (existing != null)
            {
                var model =
                    new ProductInventoryTransactionCrud
                    {
                        Id = existing.Id,

                        ProductId = productId,
                        ParentId = productId,

                        ProductVariantId = variantId,

                        OperationType = 1,

                        ChangeQuantity = quantity,

                        ReasonId = reasonId,

                        Note = "موجودی اولیه"
                    };

                var result =
                    await _productInventoryTransactionService
                        .UpdateAsync(
                            model,
                            existing.Id);

                if (!result.Success)
                {
                    throw new InvalidOperationException(
                        $"Initial stock update failed. " +
                        $"ProductId: {productId}, " +
                        $"VariantId: {variantId?.ToString() ?? "None"}. " +
                        $"Message: {result.Message}");
                }
            }
            else
            {
                var model =
                    new ProductInventoryTransactionCrud
                    {
                        ProductId = productId,
                        ParentId = productId,

                        ProductVariantId = variantId,

                        OperationType = 1,

                        ChangeQuantity = quantity,

                        ReasonId = reasonId,

                        Note = "موجودی اولیه"
                    };

                var result =
                    await _productInventoryTransactionService
                        .CreateAsync(model);

                if (!result.Success)
                {
                    throw new InvalidOperationException(
                        $"Initial stock creation failed. " +
                        $"ProductId: {productId}, " +
                        $"VariantId: {variantId?.ToString() ?? "None"}. " +
                        $"Message: {result.Message}");
                }
            }
        }
        private async Task SeedInventoryAsync()
        {
            var enabled =
                _configuration.GetValue<bool>(
                    "Seed:Inventory:Enabled");

            if (!enabled)
                return;

            var file =
                _configuration.GetValue<string>(
                    "Seed:Inventory:File");

            if (string.IsNullOrWhiteSpace(file))
                throw new InvalidOperationException(
                    "Seed:Inventory:File is not configured.");

            var resourceName =
                $"Velora.Application.Shared.Resources.{file
                    .Replace("/", ".")
                    .Replace("\\", ".")}";

            var assembly =
                typeof(SeedJsonModel).Assembly;

            await using var stream =
                assembly.GetManifestResourceStream(resourceName);

            if (stream == null)
            {
                throw new FileNotFoundException(
                    $"Embedded seed resource not found: {resourceName}");
            }

            using var reader =
                new StreamReader(stream);

            var json =
                await reader.ReadToEndAsync();

            var root =
                JsonSerializer.Deserialize<InventorySeedRoot>(
                    json,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

            if (root == null)
                throw new InvalidOperationException(
                    "Inventory seed JSON is invalid.");

            if (root.Inventory == null ||
                !root.Inventory.Any())
                return;

            foreach (var item in root.Inventory)
            {
                // =========================================================
                // PRODUCT
                // =========================================================

                if (string.IsNullOrWhiteSpace(item.ProductSlug))
                {
                    throw new InvalidOperationException(
                        "ProductSlug cannot be empty in inventory seed.");
                }

                var productId =
                    await GetProductIdBySlugAsync(
                        item.ProductSlug);

                // =========================================================
                // PRODUCT INVENTORY
                // =========================================================

                if (item.InitialStock.HasValue)
                {
                    await SeedProductInventoryAsync(
                        productId,
                        item.InitialStock.Value);
                }

                // =========================================================
                // VARIANT INVENTORY
                // =========================================================

                if (item.Variants == null ||
                    !item.Variants.Any())
                {
                    continue;
                }

                var variantsQuery =
                    await _productVariantService
                        .GetAllViews();

                foreach (var variantItem in item.Variants)
                {
                    if (string.IsNullOrWhiteSpace(
                            variantItem.Sku))
                    {
                        throw new InvalidOperationException(
                            $"Variant SKU cannot be empty. " +
                            $"ProductSlug: {item.ProductSlug}");
                    }

                    var variant =
                        await variantsQuery
                            .FirstOrDefaultAsync(
                                x =>
                                    x.ParentId == productId &&
                                    x.Sku == variantItem.Sku);

                    if (variant == null)
                    {
                        throw new InvalidOperationException(
                            $"Product variant with SKU '{variantItem.Sku}' " +
                            $"was not found. " +
                            $"ProductSlug: {item.ProductSlug}");
                    }

                    await SeedProductInventoryAsync(
                        productId,
                        variantItem.InitialStock,
                        variant.Id
                        );
                }
            }
        }
        private async Task SeedDiscountsAsync()
        {
            var enabled =
                _configuration.GetValue<bool>(
                    "Seed:Discounts:Enabled");

            if (!enabled)
                return;

            if (!await ShouldRunSeederAsync(
                    SeederNames.Seed_Discounts))
                return;

            var file =
                _configuration.GetValue<string>(
                    "Seed:Discounts:File");

            if (string.IsNullOrWhiteSpace(file))
            {
                throw new InvalidOperationException(
                    "Seed:Discounts:File is not configured.");
            }

            var resourceName =
                $"Velora.Application.Shared.Resources.{file
                    .Replace("/", ".")
                    .Replace("\\", ".")}";

            var assembly =
                typeof(SeedJsonModel).Assembly;

            await using var stream =
                assembly.GetManifestResourceStream(resourceName);

            if (stream == null)
            {
                throw new FileNotFoundException(
                    $"Embedded seed resource not found: {resourceName}");
            }

            using var reader =
                new StreamReader(stream);

            var json =
                await reader.ReadToEndAsync();

            var root =
                JsonSerializer.Deserialize<DiscountSeedRoot>(
                    json,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

            if (root == null)
            {
                throw new InvalidOperationException(
                    "Discount seed JSON is invalid.");
            }

            if (root.Discounts == null ||
                !root.Discounts.Any())
            {
                return;
            }

            var discountsQuery =
                await _discountService.GetAllViews();

            var existingDiscounts =
                await discountsQuery.ToListAsync();

            foreach (var item in root.Discounts)
            {
                if (string.IsNullOrWhiteSpace(item.Name))
                {
                    throw new InvalidOperationException(
                        "Discount Name cannot be empty.");
                }

                if (item.DiscountValue < 0)
                {
                    throw new InvalidOperationException(
                        $"DiscountValue cannot be negative. " +
                        $"Discount: {item.Name}");
                }

                if (item.EndDate <= item.StartDate)
                {
                    throw new InvalidOperationException(
                        $"Discount EndDate must be greater than StartDate. " +
                        $"Discount: {item.Name}");
                }

                var existing =
                    existingDiscounts.FirstOrDefault(
                        x => x.Name == item.Name);

                if (existing == null)
                {
                    var model =
                        new DiscountCrud
                        {
                            Name = item.Name,
                            DiscountType = item.DiscountType,
                            DiscountValue = item.DiscountValue,
                            StartDate = item.StartDate,
                            EndDate = item.EndDate,
                            IsActive = item.IsActive
                        };

                    var result =
                        await _discountService
                            .CreateAsync(model);

                    if (!result.Success)
                    {
                        throw new InvalidOperationException(
                            $"Discount creation failed. " +
                            $"Name: {item.Name}. " +
                            $"Message: {result.Message}");
                    }

                    continue;
                }

                var updateModel =
                    new DiscountCrud
                    {
                        Id = existing.Id,
                        Name = item.Name,
                        DiscountType = item.DiscountType,
                        DiscountValue = item.DiscountValue,
                        StartDate = item.StartDate,
                        EndDate = item.EndDate,
                        IsActive = item.IsActive
                    };

                var updateResult =
                    await _discountService
                        .UpdateAsync(
                            updateModel,
                            existing.Id);

                if (!updateResult.Success)
                {
                    throw new InvalidOperationException(
                        $"Discount update failed. " +
                        $"Name: {item.Name}. " +
                        $"Message: {updateResult.Message}");
                }
            }

 
        }
        private async Task<Guid> GetDiscountIdByNameAsync(
    string discountName)
        {
            if (string.IsNullOrWhiteSpace(discountName))
            {
                throw new InvalidOperationException(
                    "DiscountName cannot be empty.");
            }

            var discountsQuery =
                await _discountService.GetAllViews();

            var discount =
                await discountsQuery
                    .FirstOrDefaultAsync(
                        x => x.Name == discountName);

            if (discount == null)
            {
                throw new InvalidOperationException(
                    $"Discount with name '{discountName}' was not found.");
            }

            return discount.Id;
        }
        private async Task SeedDiscountItemsAsync()
        {
            var enabled =
                _configuration.GetValue<bool>(
                    "Seed:DiscountItems:Enabled");

            if (!enabled)
                return;

            if (!await ShouldRunSeederAsync(
                    SeederNames.Seed_DiscountItems))
                return;

            var file =
                _configuration.GetValue<string>(
                    "Seed:DiscountItems:File");

            if (string.IsNullOrWhiteSpace(file))
            {
                throw new InvalidOperationException(
                    "Seed:DiscountItems:File is not configured.");
            }

            var resourceName =
                $"Velora.Application.Shared.Resources.{file
                    .Replace("/", ".")
                    .Replace("\\", ".")}";

            var assembly =
                typeof(SeedJsonModel).Assembly;

            await using var stream =
                assembly.GetManifestResourceStream(resourceName);

            if (stream == null)
            {
                throw new FileNotFoundException(
                    $"Embedded seed resource not found: {resourceName}");
            }

            using var reader =
                new StreamReader(stream);

            var json =
                await reader.ReadToEndAsync();

            var root =
                JsonSerializer.Deserialize<DiscountItemSeedRoot>(
                    json,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

            if (root == null)
            {
                throw new InvalidOperationException(
                    "Discount item seed JSON is invalid.");
            }

            if (root.DiscountItems == null ||
                !root.DiscountItems.Any())
            {
                return;
            }

            foreach (var item in root.DiscountItems)
            {
                if (string.IsNullOrWhiteSpace(item.DiscountName))
                {
                    throw new InvalidOperationException(
                        "DiscountName cannot be empty.");
                }

                var targetCount = 0;

                if (!string.IsNullOrWhiteSpace(item.ProductSlug))
                    targetCount++;

                if (!string.IsNullOrWhiteSpace(item.VariantSku))
                    targetCount++;

                if (!string.IsNullOrWhiteSpace(item.CategorySlug))
                    targetCount++;

                if (!string.IsNullOrWhiteSpace(item.BrandSlug))
                    targetCount++;

                if (targetCount != 1)
                {
                    throw new InvalidOperationException(
                        $"Discount item must have exactly one target. " +
                        $"Discount: {item.DiscountName}");
                }

                var discountId =
                    await GetDiscountIdByNameAsync(
                        item.DiscountName);

                Guid? productId = null;
                Guid? productVariantId = null;
                Guid? productCategoryId = null;
                Guid? productBrandId = null;

                // =========================================================
                // PRODUCT
                // =========================================================

                if (!string.IsNullOrWhiteSpace(
                        item.ProductSlug))
                {
                    productId =
                        await GetProductIdBySlugAsync(
                            item.ProductSlug);
                }

                // =========================================================
                // VARIANT
                // =========================================================

                if (!string.IsNullOrWhiteSpace(
                        item.VariantSku))
                {
                    if (string.IsNullOrWhiteSpace(
                            item.ProductSlug))
                    {
                        throw new InvalidOperationException(
                            $"VariantSku requires ProductSlug. " +
                            $"Discount: {item.DiscountName}, " +
                            $"SKU: {item.VariantSku}");
                    }

                    var variantProductId =
                        await GetProductIdBySlugAsync(
                            item.ProductSlug);

                    var variantsQuery =
                        await _productVariantService
                            .GetAllViews();

                    var variant =
                        await variantsQuery
                            .FirstOrDefaultAsync(
                                x =>
                                    x.ParentId == variantProductId &&
                                    x.Sku == item.VariantSku);

                    if (variant == null)
                    {
                        throw new InvalidOperationException(
                            $"Product variant with SKU " +
                            $"'{item.VariantSku}' was not found. " +
                            $"ProductSlug: {item.ProductSlug}");
                    }

                    productId = variantProductId;
                    productVariantId = variant.Id;
                }

                // =========================================================
                // CATEGORY
                // =========================================================

                if (!string.IsNullOrWhiteSpace(
                        item.CategorySlug))
                {
                    var categoriesQuery =
                        await _productCategoryService
                            .GetAllViews();

                    var category =
                        await categoriesQuery
                            .FirstOrDefaultAsync(
                                x =>
                                    x.Slug == item.CategorySlug);

                    if (category == null)
                    {
                        throw new InvalidOperationException(
                            $"Product category with slug " +
                            $"'{item.CategorySlug}' was not found.");
                    }

                    productCategoryId =
                        category.Id;
                }

                // =========================================================
                // BRAND
                // =========================================================

                if (!string.IsNullOrWhiteSpace(
                        item.BrandSlug))
                {
                    var brandsQuery =
                        await _productBrandService
                            .GetAllViews();

                    var brand =
                        await brandsQuery
                            .FirstOrDefaultAsync(
                                x =>
                                    x.Slug == item.BrandSlug);

                    if (brand == null)
                    {
                        throw new InvalidOperationException(
                            $"Product brand with slug " +
                            $"'{item.BrandSlug}' was not found.");
                    }

                    productBrandId =
                        brand.Id;
                }

                // =========================================================
                // CHECK DUPLICATE
                // =========================================================

                var itemsQuery =
                    await _discountItemService
                        .GetAllViews();

                var existing =
                    await itemsQuery
                        .FirstOrDefaultAsync(
                            x =>
                                x.ParentId == discountId &&
                                x.ProductId == productId &&
                                x.ProductVariantId == productVariantId &&
                                x.ProductCategoryId == productCategoryId &&
                                x.ProductBrandId == productBrandId);

                // =========================================================
                // UPDATE
                // =========================================================

                if (existing != null)
                {
                    var updateModel =
                        new DiscountItemCrud
                        {
                            Id = existing.Id,
                            ParentId = discountId,
                            ProductId = productId.Value,
                            ProductVariantId = productVariantId,
                            ProductCategoryId = productCategoryId,
                            ProductBrandId = productBrandId,
                            SortOrder = item.SortOrder
                        };

                    var result =
                        await _discountItemService
                            .UpdateAsync(
                                updateModel,
                                existing.Id);

                    if (!result.Success)
                    {
                        throw new InvalidOperationException(
                            $"Discount item update failed. " +
                            $"Discount: {item.DiscountName}. " +
                            $"Message: {result.Message}");
                    }
                }
                // =========================================================
                // CREATE
                // =========================================================
                else
                {
                    var createModel =
                        new DiscountItemCrud
                        {
                            ParentId = discountId,
                            ProductId = productId.Value,
                            ProductVariantId = productVariantId,
                            ProductCategoryId = productCategoryId,
                            ProductBrandId = productBrandId,
                            SortOrder = item.SortOrder
                        };

                    var result =
                        await _discountItemService
                            .CreateAsync(createModel);

                    if (!result.Success)
                    {
                        throw new InvalidOperationException(
                            $"Discount item creation failed. " +
                            $"Discount: {item.DiscountName}. " +
                            $"Message: {result.Message}");
                    }
                }
            }
        }
        private Guid? _initialStockReasonId;

    }
}

