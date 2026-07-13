using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Dtos;
using Velora.Application.Shared.Constants;

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
        };

        public static Type? GetModelType(string entityName)
        {
            return _map.TryGetValue(entityName, out var type) ? type : null;
        }
    }

}
