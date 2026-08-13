using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Constants;
using Velora.Application.Shared.Dtos;
using Velora.EntityFrameworkCore.EntityFramework.SqlServer;

namespace Velora.Application.Shared.Infrastructure
{
    public static class ModelMapping
    {
        private static readonly Dictionary<string, Type> _map =
            new(StringComparer.OrdinalIgnoreCase)
        {
            { LookupEntities.Resource, typeof(ResourceCrud) },
            { LookupEntities.User, typeof(UserCrud) },
            { LookupEntities.ResourceType, typeof(ResourceTypeCrud) },
            { LookupEntities.Role, typeof(RoleCrud) },
            { LookupEntities.ComponentType, typeof(ComponentTypeCrud) },
            { LookupEntities.PageTemplate, typeof(PageTemplateCrud) },
            { LookupEntities.PageTemplateComponent, typeof(PageTemplateComponentCrud) },
            { LookupEntities.Page, typeof(PageCrud) },
            { LookupEntities.Section, typeof(SectionCrud) },
            { LookupEntities.SectionItem, typeof(SectionItemCrud) },
            { LookupEntities.CmsConfiguration, typeof(CmsConfigurationCrud) },
            { LookupEntities.SiteSetting, typeof(SiteSettingCrud) },
            { LookupEntities.ContentItem, typeof(ContentItemCrud) },
            { LookupEntities.SectionGroupItem, typeof(SectionGroupItemCrud) },
            { LookupEntities.SiteMenu, typeof(SiteMenuCrud) },
            { LookupEntities.ContentCategory, typeof(ContentCategoryCrud) },
            { LookupEntities.Tag, typeof(TagCrud) },
            { LookupEntities.ProductBrand, typeof(ProductBrandCrud) },
            { LookupEntities.ProductCategory, typeof(ProductCategoryCrud) },
            { LookupEntities.ProductTag, typeof(ProductTagCrud) },
            { LookupEntities.ProductAttribute, typeof(ProductAttributeCrud) },
            { LookupEntities.CategoryAttribute, typeof(CategoryAttributeCrud) },
            { LookupEntities.ProductType, typeof(ProductTypeCrud) },
            { LookupEntities.Product, typeof(ProductCrud) },
            { LookupEntities.InventoryTransactionReason, typeof(InventoryTransactionReasonCrud) },
            { LookupEntities.ProductFile, typeof(ProductFileCrud) },
            { LookupEntities.ProductAttributeValue, typeof(ProductAttributeValueCrud) },
            { LookupEntities.ProductVariant, typeof(ProductVariantCrud) },
            { LookupEntities.ProductInventoryTransaction, typeof(ProductInventoryTransactionCrud) },
            { LookupEntities.Discount, typeof(DiscountCrud) },
            { LookupEntities.DiscountItem, typeof(DiscountItemCrud) },
            { LookupEntities.Coupon, typeof(CouponCrud) },
            { LookupEntities.SmsLog, typeof(SmsLogCrud) },
            { LookupEntities.SmsSetting, typeof(SmsSettingCrud) },
            { LookupEntities.UserOtp, typeof(UserOtpCrud) },
            { LookupEntities.UserAddress, typeof(UserAddressCrud) },
            { LookupEntities.City, typeof(CityCrud) },
            { LookupEntities.State, typeof(StateCrud) },
            { LookupEntities.ShippingMethod, typeof(ShippingMethodCrud) },
            { LookupEntities.ShippingMethodCity, typeof(ShippingMethodCityCrud) },
            { LookupEntities.ProductReview, typeof(ProductReviewCrud) },
            { LookupEntities.ProductQuestion, typeof(ProductQuestionCrud) },
            { LookupEntities.PaymentGateway, typeof(PaymentGatewayCrud) },
            { LookupEntities.BankAccount, typeof(BankAccountCrud) },
            { LookupEntities.CouponUsage, typeof(CouponUsageCrud) },
            { LookupEntities.ShoppingCart, typeof(ShoppingCartCrud) },
            { LookupEntities.ShoppingCartItem, typeof(ShoppingCartItem) },
        };

        public static Type? GetModelType(string entityName)
        {
            return _map.TryGetValue(entityName, out var type) ? type : null;
        }
    }

}
