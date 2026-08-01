using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Dtos;
using Velora.EntityFrameworkCore.EntityFramework.SqlServer;

namespace Velora.Application.MappingProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<PgRole, RoleDto>().ReverseMap();
            CreateMap<SqlRole, RoleDto>().ReverseMap();
            CreateMap<PgUser, UserDto>().ReverseMap();
            CreateMap<SqlUser, UserDto>().ReverseMap();
            CreateMap<PgUserRole, UserRoleDto>().ReverseMap();
            CreateMap<SqlUserRole, UserRoleDto>().ReverseMap();
            CreateMap<PgUserProfile, UserProfileDto>().ReverseMap();
            CreateMap<SqlUserProfile, UserProfileDto>().ReverseMap();
            CreateMap<SqlResourceType, ResourceTypeDto>().ReverseMap();
            CreateMap<PgResourcetype, ResourceTypeDto>().ReverseMap();
            CreateMap<SqlResource, ResourceDto>().ReverseMap();
            CreateMap<PgResource, ResourceDto>().ReverseMap();
            CreateMap<SqlResourceLanguage, ResourceLanguageDto>().ReverseMap();
            CreateMap<PgResourceLanguage, ResourceLanguageDto>().ReverseMap();
            CreateMap<SqlPermission, PermissionDto>().ReverseMap();
            CreateMap<PgPermission, PermissionDto>().ReverseMap();
            CreateMap<SqlRolePermission, RolePermissionDto>().ReverseMap();
            CreateMap<PgRolePermission, RolePermissionDto>().ReverseMap();
            CreateMap<PgUserRolesView, UserRoleViewDto>().ReverseMap();
            CreateMap<SqlUserRolesView, UserRoleViewDto>().ReverseMap();
            CreateMap<PgLocalizationView, LocalizationViewDto>().ReverseMap();
            CreateMap<SqlLocalizationView, LocalizationViewDto>().ReverseMap();
            CreateMap<PgLocalizationtranslation, LocalizationtranslationDto>().ReverseMap();
            CreateMap<SqlLocalizationtranslation, LocalizationtranslationDto>().ReverseMap();
            CreateMap<PgLocalizationkey, LocalizationkeyDto>().ReverseMap();
            CreateMap<SqlLocalizationKey, LocalizationkeyDto>().ReverseMap();
            CreateMap<PgGeneralsetting, GeneralSettingDto>().ReverseMap();
            CreateMap<SqlGeneralsetting, GeneralSettingDto>().ReverseMap();
            CreateMap<PgResourcesView, ResourcesViewDto>().ReverseMap();
            CreateMap<SqlResourcesView, ResourcesViewDto>().ReverseMap();
            CreateMap<PgUserDetailView, UserCrud>().ReverseMap();
            CreateMap<SqlUserDetailView, UserCrud>().ReverseMap();
            CreateMap<PgRole,RoleCrud>().ReverseMap();
            CreateMap<SqlRole,RoleCrud>().ReverseMap();
            CreateMap<PgRole, RoleDto>().ReverseMap();
            CreateMap<SqlRole, RoleDto>().ReverseMap();
            CreateMap<RoleDto, RoleJsonModel>().ReverseMap();
            CreateMap<UserDto, UserJsonModel>().ReverseMap();
            CreateMap<RoleJsonModel, RoleDto>().ReverseMap();
            CreateMap<UserJsonModel, UserDto>().ReverseMap();
            CreateMap<PgResourcetype, ResourceTypeCrud>().ReverseMap();
            CreateMap<SqlResourceType, ResourceTypeCrud>().ReverseMap();
            CreateMap<PgResource, ResourceCrud>().ReverseMap();
            CreateMap<SqlResource, ResourceCrud>().ReverseMap();
            CreateMap<PgResourceFormView, ResourceCrud>().ReverseMap();
            CreateMap<SqlResourceFormView, ResourceCrud>().ReverseMap();
            CreateMap<ResourceDto, ResourceCrud>().ReverseMap();
            CreateMap<SqlPermissionView, PermissionCrud>().ReverseMap();
            CreateMap<PgPermissionView, PermissionCrud>().ReverseMap();
            CreateMap<PermissionDto, PermissionCrud>().ReverseMap();
            CreateMap<PgSeedHistory, SeedHistoryDto>().ReverseMap();
            CreateMap<SqlSeedHistory, SeedHistoryDto>().ReverseMap();
            CreateMap<SqlComponentType, ComponentTypeCrud>().ReverseMap();
            CreateMap<SqlComponentTypeView, ComponentTypeCrud>().ReverseMap();
            CreateMap<SqlComponentType, ComponentTypeDto>().ReverseMap();

            CreateMap<SqlPageTemplate, PageTemplateCrud>().ReverseMap();
            CreateMap<SqlPageTemplateView, PageTemplateCrud>().ReverseMap();
            CreateMap<SqlPageTemplate, PageTemplateDto>().ReverseMap();

            CreateMap<SqlPageTemplateComponent, PageTemplateComponentCrud>().ReverseMap();
            CreateMap<SqlPageTemplateComponentView, PageTemplateComponentCrud>().ReverseMap();
            CreateMap<SqlPageTemplateComponent, PageTemplateComponentDto>().ReverseMap();

            CreateMap<SqlPage, PageCrud>().ReverseMap();
            CreateMap<SqlPageView, PageCrud>().ReverseMap();
            CreateMap<SqlPage, PageDto>().ReverseMap();

            CreateMap<SqlSection, SectionCrud>().ReverseMap();
            CreateMap<SqlSectionView, SectionCrud>().ReverseMap();
            CreateMap<SqlSection, SectionDto>().ReverseMap();


            CreateMap<SqlCmsConfiguration, CmsConfigurationCrud>().ReverseMap();
            CreateMap<SqlCmsConfigurationView, CmsConfigurationCrud>().ReverseMap();
            CreateMap<SqlCmsConfiguration, CmsConfigurationDto>().ReverseMap();

            CreateMap<SqlSiteSetting, SiteSettingCrud>().ReverseMap();
            CreateMap<SqlSiteSettingView, SiteSettingCrud>().ReverseMap();
            CreateMap<SqlSiteSetting, SiteSettingDto>().ReverseMap();

            CreateMap<SqlSiteGlobalSetting,VwSiteGlobalSetting>().ReverseMap();



            CreateMap<SqlProductTagMapping, ProductTagMappingDto>().ReverseMap();

            

            CreateMap<SqlSectionItem, SectionItemCrud>().ReverseMap();
            CreateMap<SqlSectionItemView, SectionItemCrud>().ReverseMap();
            CreateMap<SqlSectionItem, SectionItemDto>().ReverseMap();


            CreateMap<SqlContentItem, ContentItemCrud>().ReverseMap();
            CreateMap<SqlContentItemView, ContentItemCrud>().ReverseMap();
            CreateMap<SqlContentItem, ContentItemDto>().ReverseMap();
            CreateMap<ContentItem, ContentItemListDto>().ReverseMap();

            CreateMap<SqlSectionGroupItem, SectionGroupItemCrud>().ReverseMap();
            CreateMap<VwSectionGroupItemForm, SectionGroupItemCrud>().ReverseMap();
            CreateMap<SqlSectionGroupItem, SectionGroupItemDto>().ReverseMap();

            CreateMap<SqlLinkType, LinkTypeCrud>().ReverseMap();
            CreateMap<VwLinkTypeForm, LinkTypeCrud>().ReverseMap();
            CreateMap<SqlLinkType, LinkTypeDto>().ReverseMap();
            CreateMap<LinkTypeCrud, LinkTypeDto>().ReverseMap();

            CreateMap<SqlSiteMenu, SiteMenuCrud>().ReverseMap();
            CreateMap<VwSiteMenuForm, SiteMenuCrud>().ReverseMap();
            CreateMap<SqlSiteMenu, SiteMenuDto>().ReverseMap();

            CreateMap<SqlContentCategory, ContentCategoryCrud>().ReverseMap();
            CreateMap<VwContentCategoryForm, ContentCategoryCrud>().ReverseMap();
            CreateMap<SqlContentCategory, ContentCategoryDto>().ReverseMap();

            CreateMap<SqlTag, TagCrud>().ReverseMap();
            CreateMap<VwTagForm, TagCrud>().ReverseMap();
            CreateMap<SqlTag, TagDto>().ReverseMap();

            CreateMap<SqlContentItem, ContentItemListDto>()
    .ForMember(
        dest => dest.CategoryName,
        opt => opt.MapFrom(src =>
            src.Category != null
                ? src.Category.Name
                : null))
    .ForMember(
        dest => dest.CategorySlug,
        opt => opt.MapFrom(src =>
            src.Category != null
                ? src.Category.Slug
                : null))
    .ForMember(
        dest => dest.Tags,
        opt => opt.MapFrom(src =>
            src.ContentItemTags
                .Where(x => x.Tag != null)
                .Select(x => x.Tag.Name)
                .ToList()
        ));

            CreateMap<SqlContentItemTag, ContentItemTagDto>().ReverseMap();


            CreateMap<SqlProductCategory, ProductCategoryCrud>().ReverseMap();
            CreateMap<VwProductCategoryForm, ProductCategoryCrud>().ReverseMap();
            CreateMap<SqlProductCategory, ProductCategoryDto>().ReverseMap();

            CreateMap<SqlProductBrand, ProductBrandCrud>().ReverseMap();
            CreateMap<VwProductBrandForm, ProductBrandCrud>().ReverseMap();
            CreateMap<SqlProductBrand, ProductBrandDto>().ReverseMap();


            CreateMap<SqlProductTag, ProductTagCrud>().ReverseMap();
            CreateMap<VwProductTagForm, ProductTagCrud>().ReverseMap();
            CreateMap<SqlProductTag, ProductTagDto>().ReverseMap();


            CreateMap<SqlCategoryAttribute, CategoryAttributeCrud>().ReverseMap();
            CreateMap<VwCategoryAttributeForm, CategoryAttributeCrud>().ReverseMap();
            CreateMap<SqlCategoryAttribute, CategoryAttributeDto>().ReverseMap();


            CreateMap<SqlProductAttribute, ProductAttributeCrud>().ReverseMap();
            CreateMap<VwProductAttributeForm, ProductAttributeCrud>().ReverseMap();
            CreateMap<SqlProductAttribute, ProductAttributeDto>().ReverseMap();

            CreateMap<SqlProductType, ProductTypeCrud>().ReverseMap();
            CreateMap<VwProductTypeForm, ProductTypeCrud>().ReverseMap();
            CreateMap<SqlProductType, ProductTypeDto>().ReverseMap();

            CreateMap<SqlProduct, ProductCrud>().ReverseMap();
            CreateMap<VwProductForm, ProductCrud>().ReverseMap();
            CreateMap<SqlProduct, ProductDto>().ReverseMap();


            CreateMap<SqlInventoryTransactionReason, InventoryTransactionReasonCrud>().ReverseMap();
            CreateMap<VwInventoryTransactionReasonForm, InventoryTransactionReasonCrud>().ReverseMap();
            CreateMap<SqlInventoryTransactionReason, InventoryTransactionReasonDto>().ReverseMap();

            CreateMap<SqlProductFile, ProductFileCrud>().ReverseMap();
            CreateMap<VwProductFileForm, ProductFileCrud>().ReverseMap();
            CreateMap<SqlProductFile, ProductFileDto>().ReverseMap();

            CreateMap<SqlProductAttributeValue, ProductAttributeValueCrud>().ReverseMap();
            CreateMap<VwProductAttributeValueForm, ProductAttributeValueCrud>().ReverseMap();
            CreateMap<SqlProductAttributeValue, ProductAttributeValueDto>().ReverseMap();


            CreateMap<SqlProductVariant, ProductVariantCrud>().ReverseMap();
            CreateMap<VwProductVariantForm, ProductVariantCrud>().ReverseMap();
            CreateMap<SqlProductVariant, ProductVariantDto>().ReverseMap();


            CreateMap<SqlProductInventoryTransaction, ProductInventoryTransactionCrud>().ReverseMap();
            CreateMap<VwProductInventoryTransactionForm, ProductInventoryTransactionCrud>().ReverseMap();
            CreateMap<SqlProductInventoryTransaction, ProductInventoryTransactionDto>().ReverseMap();



            CreateMap<SqlDiscount, DiscountCrud>().ReverseMap();
            CreateMap<VwDiscountForm, DiscountCrud>().ReverseMap();
            CreateMap<SqlDiscount, DiscountDto>().ReverseMap();

            CreateMap<SqlDiscountItem, DiscountItemCrud>().ReverseMap();
            CreateMap<VwDiscountItemForm, DiscountItemCrud>().ReverseMap();
            CreateMap<SqlDiscountItem, DiscountItemDto>().ReverseMap();

            CreateMap<SqlCoupon, CouponCrud>().ReverseMap();
            CreateMap<VwCouponForm, CouponCrud>().ReverseMap();
            CreateMap<SqlCoupon, CouponDto>().ReverseMap();

            CreateMap<SqlShoppingCart, ShoppingCartDto>().ReverseMap();
            CreateMap<SqlShoppingCartItem, ShoppingCartItemDto>().ReverseMap();

            CreateMap<SqlSmsLog, SmsLogCrud>().ReverseMap();
            CreateMap<VwSmsLogForm, SmsLogCrud>().ReverseMap();
            CreateMap<SqlSmsLog, SmsLogDto>().ReverseMap();

            CreateMap<SqlSmsSetting, SmsSettingCrud>().ReverseMap();
            CreateMap<VwSmsSettingForm, SmsSettingCrud>().ReverseMap();
            CreateMap<SqlSmsSetting, SmsSettingDto>().ReverseMap();


            CreateMap<SqlUserOtp, UserOtpCrud>().ReverseMap();
            CreateMap<VwUserOtpForm, UserOtpCrud>().ReverseMap();
            CreateMap<SqlUserOtp, UserOtpDto>().ReverseMap();


            CreateMap<SqlUserAddress, UserAddressCrud>().ReverseMap();
            CreateMap<VwUserAddressForm, UserAddressCrud>().ReverseMap();
            CreateMap<SqlUserAddress, UserAddressDto>().ReverseMap();

            CreateMap<SqlCity, CityCrud>().ReverseMap();
            CreateMap<VwCityForm, CityCrud>().ReverseMap();
            CreateMap<SqlCity, CityDto>().ReverseMap();

            CreateMap<SqlState, StateCrud>().ReverseMap();
            CreateMap<VwStateForm, StateCrud>().ReverseMap();
            CreateMap<SqlState, StateDto>().ReverseMap();
        }


    }
}
