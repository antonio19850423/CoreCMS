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
        }
    }
}
