using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Velora.EntityFrameworkCore.EntityFramework.SqlServer;

public partial class CoreCmsContext : DbContext
{
    public CoreCmsContext()
    {
    }

    public CoreCmsContext(DbContextOptions<CoreCmsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BankAccount> BankAccounts { get; set; }

    public virtual DbSet<CategoryAttribute> CategoryAttributes { get; set; }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<CmsConfiguration> CmsConfigurations { get; set; }

    public virtual DbSet<ComponentType> ComponentTypes { get; set; }

    public virtual DbSet<ContentCategory> ContentCategories { get; set; }

    public virtual DbSet<ContentItem> ContentItems { get; set; }

    public virtual DbSet<ContentItemTag> ContentItemTags { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<Coupon> Coupons { get; set; }

    public virtual DbSet<CouponUsage> CouponUsages { get; set; }

    public virtual DbSet<Discount> Discounts { get; set; }

    public virtual DbSet<DiscountItem> DiscountItems { get; set; }

    public virtual DbSet<GeneralSetting> GeneralSettings { get; set; }

    public virtual DbSet<InventoryTransactionReason> InventoryTransactionReasons { get; set; }

    public virtual DbSet<LinkType> LinkTypes { get; set; }

    public virtual DbSet<LocalizationKey> LocalizationKeys { get; set; }

    public virtual DbSet<LocalizationTranslation> LocalizationTranslations { get; set; }

    public virtual DbSet<Menu> Menus { get; set; }

    public virtual DbSet<Page> Pages { get; set; }

    public virtual DbSet<PageTemplate> PageTemplates { get; set; }

    public virtual DbSet<PageTemplateComponent> PageTemplateComponents { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<PaymentGateway> PaymentGateways { get; set; }

    public virtual DbSet<Permission> Permissions { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductAttribute> ProductAttributes { get; set; }

    public virtual DbSet<ProductAttributeValue> ProductAttributeValues { get; set; }

    public virtual DbSet<ProductBrand> ProductBrands { get; set; }

    public virtual DbSet<ProductCategory> ProductCategories { get; set; }

    public virtual DbSet<ProductFile> ProductFiles { get; set; }

    public virtual DbSet<ProductInventoryTransaction> ProductInventoryTransactions { get; set; }

    public virtual DbSet<ProductQuestion> ProductQuestions { get; set; }

    public virtual DbSet<ProductReview> ProductReviews { get; set; }

    public virtual DbSet<ProductTag> ProductTags { get; set; }

    public virtual DbSet<ProductTagMapping> ProductTagMappings { get; set; }

    public virtual DbSet<ProductType> ProductTypes { get; set; }

    public virtual DbSet<ProductVariant> ProductVariants { get; set; }

    public virtual DbSet<Resource> Resources { get; set; }

    public virtual DbSet<ResourceLanguage> ResourceLanguages { get; set; }

    public virtual DbSet<ResourceType> ResourceTypes { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<RolePermission> RolePermissions { get; set; }

    public virtual DbSet<Section> Sections { get; set; }

    public virtual DbSet<SectionGroupItem> SectionGroupItems { get; set; }

    public virtual DbSet<SectionItem> SectionItems { get; set; }

    public virtual DbSet<SeedHistory> SeedHistories { get; set; }

    public virtual DbSet<ShippingMethod> ShippingMethods { get; set; }

    public virtual DbSet<ShippingMethodCity> ShippingMethodCities { get; set; }

    public virtual DbSet<ShoppingCart> ShoppingCarts { get; set; }

    public virtual DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }

    public virtual DbSet<SiteMenu> SiteMenus { get; set; }

    public virtual DbSet<SiteSetting> SiteSettings { get; set; }

    public virtual DbSet<SmsLog> SmsLogs { get; set; }

    public virtual DbSet<SmsSetting> SmsSettings { get; set; }

    public virtual DbSet<State> States { get; set; }

    public virtual DbSet<Tag> Tags { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserAddress> UserAddresses { get; set; }

    public virtual DbSet<UserOtp> UserOtps { get; set; }

    public virtual DbSet<UserProfile> UserProfiles { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    public virtual DbSet<VwBankAccountForm> VwBankAccountForms { get; set; }

    public virtual DbSet<VwCategoryAttributeForm> VwCategoryAttributeForms { get; set; }

    public virtual DbSet<VwCityForm> VwCityForms { get; set; }

    public virtual DbSet<VwCmsConfigurationForm> VwCmsConfigurationForms { get; set; }

    public virtual DbSet<VwComponentTypeForm> VwComponentTypeForms { get; set; }

    public virtual DbSet<VwContentCategoryForm> VwContentCategoryForms { get; set; }

    public virtual DbSet<VwContentItemForm> VwContentItemForms { get; set; }

    public virtual DbSet<VwCouponForm> VwCouponForms { get; set; }

    public virtual DbSet<VwDiscountForm> VwDiscountForms { get; set; }

    public virtual DbSet<VwDiscountItemForm> VwDiscountItemForms { get; set; }

    public virtual DbSet<VwInventoryTransactionReasonForm> VwInventoryTransactionReasonForms { get; set; }

    public virtual DbSet<VwLinkTypeForm> VwLinkTypeForms { get; set; }

    public virtual DbSet<VwLocalization> VwLocalizations { get; set; }

    public virtual DbSet<VwPageForm> VwPageForms { get; set; }

    public virtual DbSet<VwPageTemplateComponentForm> VwPageTemplateComponentForms { get; set; }

    public virtual DbSet<VwPageTemplateForm> VwPageTemplateForms { get; set; }

    public virtual DbSet<VwPaymentGatewayForm> VwPaymentGatewayForms { get; set; }

    public virtual DbSet<VwPermissionForm> VwPermissionForms { get; set; }

    public virtual DbSet<VwProductAttributeForm> VwProductAttributeForms { get; set; }

    public virtual DbSet<VwProductAttributeValueForm> VwProductAttributeValueForms { get; set; }

    public virtual DbSet<VwProductBrandForm> VwProductBrandForms { get; set; }

    public virtual DbSet<VwProductCategoryForm> VwProductCategoryForms { get; set; }

    public virtual DbSet<VwProductFileForm> VwProductFileForms { get; set; }

    public virtual DbSet<VwProductForm> VwProductForms { get; set; }

    public virtual DbSet<VwProductInventoryTransactionForm> VwProductInventoryTransactionForms { get; set; }

    public virtual DbSet<VwProductQuestionForm> VwProductQuestionForms { get; set; }

    public virtual DbSet<VwProductReviewForm> VwProductReviewForms { get; set; }

    public virtual DbSet<VwProductTagForm> VwProductTagForms { get; set; }

    public virtual DbSet<VwProductTypeForm> VwProductTypeForms { get; set; }

    public virtual DbSet<VwProductVariantForm> VwProductVariantForms { get; set; }

    public virtual DbSet<VwResource> VwResources { get; set; }

    public virtual DbSet<VwResourceForm> VwResourceForms { get; set; }

    public virtual DbSet<VwSectionForm> VwSectionForms { get; set; }

    public virtual DbSet<VwSectionGroupItemForm> VwSectionGroupItemForms { get; set; }

    public virtual DbSet<VwSectionItemForm> VwSectionItemForms { get; set; }

    public virtual DbSet<VwShippingMethodCityForm> VwShippingMethodCityForms { get; set; }

    public virtual DbSet<VwShippingMethodForm> VwShippingMethodForms { get; set; }

    public virtual DbSet<VwSiteGlobalSetting> VwSiteGlobalSettings { get; set; }

    public virtual DbSet<VwSiteMenuForm> VwSiteMenuForms { get; set; }

    public virtual DbSet<VwSiteSettingForm> VwSiteSettingForms { get; set; }

    public virtual DbSet<VwSmsLogForm> VwSmsLogForms { get; set; }

    public virtual DbSet<VwSmsSettingForm> VwSmsSettingForms { get; set; }

    public virtual DbSet<VwStateForm> VwStateForms { get; set; }

    public virtual DbSet<VwTagForm> VwTagForms { get; set; }

    public virtual DbSet<VwUserAddressForm> VwUserAddressForms { get; set; }

    public virtual DbSet<VwUserForm> VwUserForms { get; set; }

    public virtual DbSet<VwUserOtpForm> VwUserOtpForms { get; set; }

    public virtual DbSet<VwUserRole> VwUserRoles { get; set; }

    public virtual DbSet<VwUsersLite> VwUsersLites { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-JLBIAKI\\AFE;Database=CoreCMS;User Id=sa;Password=77723588;TrustServerCertificate=True;Connect Timeout=180;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BankAccount>(entity =>
        {
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.SiteSetting).WithMany(p => p.BankAccounts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BankAccounts_SiteSettings");
        });

        modelBuilder.Entity<CategoryAttribute>(entity =>
        {
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Attribute).WithMany(p => p.CategoryAttributes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CategoryAttribute_ProductAttribute");

            entity.HasOne(d => d.Category).WithMany(p => p.CategoryAttributes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CategoryAttribute_ProductCategory");
        });

        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Cities__3214EC07B13C2BFF");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.State).WithMany(p => p.Cities)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Cities__StateId__52593CB8");
        });

        modelBuilder.Entity<CmsConfiguration>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CmsConfi__3214EC07CF0C7FBA");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.EnableCache).HasDefaultValue(true);
            entity.Property(e => e.EnableSeo).HasDefaultValue(true);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<ComponentType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Componen__3214EC07378005F4");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<ContentCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Category__3214EC07AE6AB5D9");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<ContentItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ContentI__3214EC0700B10CEE");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.Category).WithMany(p => p.ContentItems)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_ContentItem_Category");

            entity.HasOne(d => d.Page).WithMany(p => p.ContentItems)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_ContentItem_Page");
        });

        modelBuilder.Entity<ContentItemTag>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");

            entity.HasOne(d => d.ContentItem).WithMany(p => p.ContentItemTags).HasConstraintName("FK_ContentItemTags_ContentItems");

            entity.HasOne(d => d.Tag).WithMany(p => p.ContentItemTags).HasConstraintName("FK_ContentItemTags_Tags");
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Countrie__3214EC07DB0AB1CE");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<Coupon>(entity =>
        {
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<CouponUsage>(entity =>
        {
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.UsedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Coupon).WithMany(p => p.CouponUsages)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CouponUsage_Coupon");
        });

        modelBuilder.Entity<Discount>(entity =>
        {
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<DiscountItem>(entity =>
        {
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Discount).WithMany(p => p.DiscountItems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DiscountItem_Discount");

            entity.HasOne(d => d.ProductBrand).WithMany(p => p.DiscountItems).HasConstraintName("FK_DiscountItem_ProductBrand");

            entity.HasOne(d => d.ProductCategory).WithMany(p => p.DiscountItems).HasConstraintName("FK_DiscountItem_ProductCategory");

            entity.HasOne(d => d.Product).WithMany(p => p.DiscountItems).HasConstraintName("FK_DiscountItem_Product");

            entity.HasOne(d => d.ProductVariant).WithMany(p => p.DiscountItems).HasConstraintName("FK_DiscountItem_ProductVariant");
        });

        modelBuilder.Entity<GeneralSetting>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__GeneralS__3214EC07DA747B55");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
        });

        modelBuilder.Entity<InventoryTransactionReason>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<LinkType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__LinkType__3214EC0760657063");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<LocalizationKey>(entity =>
        {
            entity.HasKey(e => e.Code).HasName("PK__Localiza__A25C5AA6683CCCC8");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
        });

        modelBuilder.Entity<LocalizationTranslation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Localiza__3214EC0725BE609D");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.LocalizationKeyCodeNavigation).WithMany(p => p.LocalizationTranslations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Localizat__Local__2AD55B43");
        });

        modelBuilder.Entity<Menu>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Menus__3214EC07ACE71E14");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Link1Target).WithMany(p => p.Menus).HasConstraintName("FK_Menus_Page");

            entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent).HasConstraintName("FK_Menus_Parent");
        });

        modelBuilder.Entity<Page>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Pages__3214EC07C5495B28");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsPublished).HasDefaultValue(true);

            entity.HasOne(d => d.PageTemplate).WithMany(p => p.Pages).HasConstraintName("FK_Pages_Template");
        });

        modelBuilder.Entity<PageTemplate>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PageTemp__3214EC078446304C");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<PageTemplateComponent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PageTemp__3214EC07851A59F8");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsEditable).HasDefaultValue(true);

            entity.HasOne(d => d.ComponentType).WithMany(p => p.PageTemplateComponents)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TemplateComponents_Type");

            entity.HasOne(d => d.PageTemplate).WithMany(p => p.PageTemplateComponents)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TemplateComponents_Template");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<PaymentGateway>(entity =>
        {
            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Permissi__3214EC07DFD2A7B1");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.Resource).WithMany(p => p.Permissions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Permissions_Resources");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsFeatured).HasDefaultValue(false);
            entity.Property(e => e.IsPublished).HasDefaultValue(true);
            entity.Property(e => e.IsTest).HasDefaultValue(false);
            entity.Property(e => e.SaleCount).HasDefaultValue(0);
            entity.Property(e => e.ViewCount).HasDefaultValue(0);

            entity.HasOne(d => d.Brand).WithMany(p => p.Products).HasConstraintName("FK_Product_ProductBrand");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Product_ProductCategory");

            entity.HasOne(d => d.ProductType).WithMany(p => p.Products)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Product_ProductType");
        });

        modelBuilder.Entity<ProductAttribute>(entity =>
        {
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<ProductAttributeValue>(entity =>
        {
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.ProductAttribute).WithMany(p => p.ProductAttributeValues)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductAttributeValue_ProductAttribute");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductAttributeValues)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductAttributeValue_Product");
        });

        modelBuilder.Entity<ProductBrand>(entity =>
        {
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<ProductCategory>(entity =>
        {
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent).HasConstraintName("FK_ProductCategory_Parent");
        });

        modelBuilder.Entity<ProductFile>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_ProductMedia");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.Product).WithMany(p => p.ProductFiles)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductMedia_Product");
        });

        modelBuilder.Entity<ProductInventoryTransaction>(entity =>
        {
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.ChangeQuantity).HasComment("تعداد تغییر یافته موجودی");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("زمان انجام تراکنش موجودی");
            entity.Property(e => e.CreatedBy).HasComment("کاربر ایجاد کننده تراکنش");
            entity.Property(e => e.Note).HasComment("توضیحات تکمیلی تراکنش");
            entity.Property(e => e.OperationType).HasComment("نوع عملیات موجودی - 1 افزایش، 2 کاهش");
            entity.Property(e => e.ProductId).HasComment("شناسه محصول");
            entity.Property(e => e.ProductVariantId).HasComment("شناسه واریانت محصول (در صورت وجود)");
            entity.Property(e => e.ReferenceDetailId).HasComment("شناسه جزئیات سند مرتبط مانند OrderItemId");
            entity.Property(e => e.ReferenceId).HasComment("شناسه سند اصلی مرتبط مانند OrderId");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductInventoryTransactions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductInventoryTransaction_Product");

            entity.HasOne(d => d.ProductVariant).WithMany(p => p.ProductInventoryTransactions).HasConstraintName("FK_ProductInventoryTransaction_ProductVariant");

            entity.HasOne(d => d.Reason).WithMany(p => p.ProductInventoryTransactions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductInventoryTransaction_Reason");
        });

        modelBuilder.Entity<ProductQuestion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ProductQ__3214EC070EAD1FDE");

            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<ProductReview>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ProductR__3214EC07BC04F2E3");

            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<ProductTag>(entity =>
        {
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<ProductTagMapping>(entity =>
        {
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductTagMappings)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductTagMapping_Product");

            entity.HasOne(d => d.ProductTag).WithMany(p => p.ProductTagMappings)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductTagMapping_ProductTag");
        });

        modelBuilder.Entity<ProductType>(entity =>
        {
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<ProductVariant>(entity =>
        {
            entity.HasIndex(e => e.Barcode, "UX_ProductVariant_Barcode")
                .IsUnique()
                .HasFilter("([Barcode] IS NOT NULL)");

            entity.HasIndex(e => e.Sku, "UX_ProductVariant_Sku")
                .IsUnique()
                .HasFilter("([Sku] IS NOT NULL)");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name).HasDefaultValue("");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductVariants)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductVariant_Product");
        });

        modelBuilder.Entity<Resource>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Resource__3214EC077EA1D68C");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.ShowInForm).HasDefaultValue(true);
            entity.Property(e => e.ShowInGrid).HasDefaultValue(true);

            entity.HasOne(d => d.ResourceType).WithMany(p => p.Resources)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Resources_ResourceTypes");
        });

        modelBuilder.Entity<ResourceLanguage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Resource__3214EC070E75EB39");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.Resource).WithMany(p => p.ResourceLanguages)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ResourceL__Resou__55BFB948");
        });

        modelBuilder.Entity<ResourceType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Resource__3214EC079376CBCF");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Roles__3214EC07CCC76ECF");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Code).HasDefaultValue("");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<RolePermission>(entity =>
        {
            entity.HasKey(e => new { e.RoleId, e.PermissionId }).HasName("PK__RolePerm__6400A1A80B44C278");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.Permission).WithMany(p => p.RolePermissions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RolePermissions_Permissions");

            entity.HasOne(d => d.Role).WithMany(p => p.RolePermissions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RolePermissions_Roles");
        });

        modelBuilder.Entity<Section>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Sections__3214EC07EC1B5639");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.ComponentType).WithMany(p => p.Sections)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Sections_Type");

            entity.HasOne(d => d.Link1Target).WithMany(p => p.SectionLink1Targets).HasConstraintName("FK_Section_Link1Target_Page");

            entity.HasOne(d => d.Link1Type).WithMany(p => p.SectionLink1Types).HasConstraintName("FK_Sections_Link1Type_Page");

            entity.HasOne(d => d.Link2Target).WithMany(p => p.SectionLink2Targets).HasConstraintName("FK_Sections_Link2Target_Page");

            entity.HasOne(d => d.Link2Type).WithMany(p => p.SectionLink2Types).HasConstraintName("FK_Section_Link2Type_LinkType");

            entity.HasOne(d => d.Link3Target).WithMany(p => p.SectionLink3Targets).HasConstraintName("FK_Section_Link3Target_Page");

            entity.HasOne(d => d.Link3Type).WithMany(p => p.SectionLink3Types).HasConstraintName("FK_Sections_Link3Type_Page");

            entity.HasOne(d => d.Link4Target).WithMany(p => p.SectionLink4Targets).HasConstraintName("FK_Sections_Link4Target_Page");

            entity.HasOne(d => d.Link4Type).WithMany(p => p.SectionLink4Types).HasConstraintName("FK_Section_Link4Type_LinkType");

            entity.HasOne(d => d.Page).WithMany(p => p.SectionPages)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Sections_Page");
        });

        modelBuilder.Entity<SectionGroupItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SectionG__3214EC074F261DAF");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<SectionItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SectionI__3214EC071187D84D");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.Link1Target).WithMany(p => p.SectionItemLink1Targets).HasConstraintName("FK_SectionItems_Link1Target_Page");

            entity.HasOne(d => d.Link1Type).WithMany(p => p.SectionItemLink1Types).HasConstraintName("FK_SectionItems_Link1Type_Page");

            entity.HasOne(d => d.Link2Target).WithMany(p => p.SectionItemLink2Targets).HasConstraintName("FK_SectionItems_Link2Target_Page");

            entity.HasOne(d => d.Link2Type).WithMany(p => p.SectionItemLink2Types).HasConstraintName("FK_SectionItems_Link2Type_Page");

            entity.HasOne(d => d.Link3Target).WithMany(p => p.SectionItemLink3Targets).HasConstraintName("FK_SectionItems_Link3Target_Page");

            entity.HasOne(d => d.Link3Type).WithMany(p => p.SectionItemLink3Types).HasConstraintName("FK_SectionItems_Link3Type_Page");

            entity.HasOne(d => d.Link4Target).WithMany(p => p.SectionItemLink4Targets).HasConstraintName("FK_SectionItems_Link4Target_Page");

            entity.HasOne(d => d.Link4Type).WithMany(p => p.SectionItemLink4Types).HasConstraintName("FK_SectionItems_Link4Type_Page");

            entity.HasOne(d => d.SectionGroupItem).WithMany(p => p.SectionItems).HasConstraintName("FK_SectionItem_SectionGroupItem");

            entity.HasOne(d => d.Section).WithMany(p => p.SectionItems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SectionItems_Section");
        });

        modelBuilder.Entity<SeedHistory>(entity =>
        {
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
        });

        modelBuilder.Entity<ShippingMethod>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsNationwide).HasDefaultValue(true);
        });

        modelBuilder.Entity<ShippingMethodCity>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.ShippingMethod).WithMany(p => p.ShippingMethodCities)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ShippingMethodCities_ShippingMethods");
        });

        modelBuilder.Entity<ShoppingCart>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<ShoppingCartItem>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Product).WithMany(p => p.ShoppingCartItems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ShoppingCartItems_Product");

            entity.HasOne(d => d.ShoppingCart).WithMany(p => p.ShoppingCartItems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ShoppingCartItems_ShoppingCarts");

            entity.HasOne(d => d.Variant).WithMany(p => p.ShoppingCartItems).HasConstraintName("FK_ShoppingCartItems_ProductVariant");
        });

        modelBuilder.Entity<SiteMenu>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SiteMenu__3214EC0723E0C5A6");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Link1Target).WithMany(p => p.SiteMenus).HasConstraintName("FK_SiteMenus_Page");

            entity.HasOne(d => d.Link1Type).WithMany(p => p.SiteMenus).HasConstraintName("FK_SiteMenus_LinkType");

            entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent).HasConstraintName("FK_SiteMenus_Parent");
        });

        modelBuilder.Entity<SiteSetting>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SiteSett__3214EC07FD9B15D6");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<SmsLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SmsLogs__3214EC077600A246");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<SmsSetting>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SmsSetti__3214EC075CC304D9");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<State>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__States__3214EC07719326B1");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Country).WithMany(p => p.States)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__States__CountryI__4D94879B");
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_cms_Tags");

            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC079F63A5A8");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<UserAddress>(entity =>
        {
            entity.HasIndex(e => new { e.UserId, e.PostalCode }, "UX_UserAddresses_UserId_PostalCode")
                .IsUnique()
                .HasFilter("([IsDeleted]=(0))");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.User).WithMany(p => p.UserAddresses).OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<UserOtp>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.MaxAttempts).HasDefaultValue(5);
        });

        modelBuilder.Entity<UserProfile>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserProf__3214EC07E311A7CF");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.User).WithMany(p => p.UserProfiles)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserProfi__UserI__3B75D760");
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Role).WithMany(p => p.UserRoles)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserRoles__RoleI__44FF419A");

            entity.HasOne(d => d.User).WithMany(p => p.UserRoles)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserRoles__UserI__440B1D61");
        });

        modelBuilder.Entity<VwBankAccountForm>(entity =>
        {
            entity.ToView("VwBankAccountForm", "cms");
        });

        modelBuilder.Entity<VwCategoryAttributeForm>(entity =>
        {
            entity.ToView("VwCategoryAttributeForm", "cms");
        });

        modelBuilder.Entity<VwCityForm>(entity =>
        {
            entity.ToView("VwCityForm", "cms");
        });

        modelBuilder.Entity<VwCmsConfigurationForm>(entity =>
        {
            entity.ToView("VwCmsConfigurationForm", "cms");
        });

        modelBuilder.Entity<VwComponentTypeForm>(entity =>
        {
            entity.ToView("VwComponentTypeForm", "cms");
        });

        modelBuilder.Entity<VwContentCategoryForm>(entity =>
        {
            entity.ToView("VwContentCategoryForm", "cms");
        });

        modelBuilder.Entity<VwContentItemForm>(entity =>
        {
            entity.ToView("VwContentItemForm", "cms");
        });

        modelBuilder.Entity<VwCouponForm>(entity =>
        {
            entity.ToView("VwCouponForm", "cms");
        });

        modelBuilder.Entity<VwDiscountForm>(entity =>
        {
            entity.ToView("VwDiscountForm", "cms");
        });

        modelBuilder.Entity<VwDiscountItemForm>(entity =>
        {
            entity.ToView("VwDiscountItemForm", "cms");
        });

        modelBuilder.Entity<VwInventoryTransactionReasonForm>(entity =>
        {
            entity.ToView("VwInventoryTransactionReasonForm", "cms");
        });

        modelBuilder.Entity<VwLinkTypeForm>(entity =>
        {
            entity.ToView("VwLinkTypeForm", "cms");
        });

        modelBuilder.Entity<VwLocalization>(entity =>
        {
            entity.ToView("VwLocalization", "gen");
        });

        modelBuilder.Entity<VwPageForm>(entity =>
        {
            entity.ToView("VwPageForm", "cms");
        });

        modelBuilder.Entity<VwPageTemplateComponentForm>(entity =>
        {
            entity.ToView("VwPageTemplateComponentForm", "cms");
        });

        modelBuilder.Entity<VwPageTemplateForm>(entity =>
        {
            entity.ToView("VwPageTemplateForm", "cms");
        });

        modelBuilder.Entity<VwPaymentGatewayForm>(entity =>
        {
            entity.ToView("VwPaymentGatewayForm", "cms");
        });

        modelBuilder.Entity<VwPermissionForm>(entity =>
        {
            entity.ToView("VwPermissionForm", "auth");
        });

        modelBuilder.Entity<VwProductAttributeForm>(entity =>
        {
            entity.ToView("VwProductAttributeForm", "cms");
        });

        modelBuilder.Entity<VwProductAttributeValueForm>(entity =>
        {
            entity.ToView("VwProductAttributeValueForm", "cms");
        });

        modelBuilder.Entity<VwProductBrandForm>(entity =>
        {
            entity.ToView("VwProductBrandForm", "cms");
        });

        modelBuilder.Entity<VwProductCategoryForm>(entity =>
        {
            entity.ToView("VwProductCategoryForm", "cms");
        });

        modelBuilder.Entity<VwProductFileForm>(entity =>
        {
            entity.ToView("VwProductFileForm", "cms");
        });

        modelBuilder.Entity<VwProductForm>(entity =>
        {
            entity.ToView("VwProductForm", "cms");
        });

        modelBuilder.Entity<VwProductInventoryTransactionForm>(entity =>
        {
            entity.ToView("VwProductInventoryTransactionForm", "cms");
        });

        modelBuilder.Entity<VwProductQuestionForm>(entity =>
        {
            entity.ToView("VwProductQuestionForm", "cms");
        });

        modelBuilder.Entity<VwProductReviewForm>(entity =>
        {
            entity.ToView("VwProductReviewForm", "cms");
        });

        modelBuilder.Entity<VwProductTagForm>(entity =>
        {
            entity.ToView("VwProductTagForm", "cms");
        });

        modelBuilder.Entity<VwProductTypeForm>(entity =>
        {
            entity.ToView("VwProductTypeForm", "cms");
        });

        modelBuilder.Entity<VwProductVariantForm>(entity =>
        {
            entity.ToView("VwProductVariantForm", "cms");
        });

        modelBuilder.Entity<VwResource>(entity =>
        {
            entity.ToView("VwResources", "auth");
        });

        modelBuilder.Entity<VwResourceForm>(entity =>
        {
            entity.ToView("VwResourceForm", "auth");
        });

        modelBuilder.Entity<VwSectionForm>(entity =>
        {
            entity.ToView("VwSectionForm", "cms");
        });

        modelBuilder.Entity<VwSectionGroupItemForm>(entity =>
        {
            entity.ToView("VwSectionGroupItemForm", "cms");
        });

        modelBuilder.Entity<VwSectionItemForm>(entity =>
        {
            entity.ToView("VwSectionItemForm", "cms");
        });

        modelBuilder.Entity<VwShippingMethodCityForm>(entity =>
        {
            entity.ToView("VwShippingMethodCityForm", "cms");
        });

        modelBuilder.Entity<VwShippingMethodForm>(entity =>
        {
            entity.ToView("VwShippingMethodForm", "cms");
        });

        modelBuilder.Entity<VwSiteGlobalSetting>(entity =>
        {
            entity.ToView("VwSiteGlobalSettings", "cms");
        });

        modelBuilder.Entity<VwSiteMenuForm>(entity =>
        {
            entity.ToView("VwSiteMenuForm", "cms");
        });

        modelBuilder.Entity<VwSiteSettingForm>(entity =>
        {
            entity.ToView("VwSiteSettingForm", "cms");
        });

        modelBuilder.Entity<VwSmsLogForm>(entity =>
        {
            entity.ToView("VwSmsLogForm", "cms");
        });

        modelBuilder.Entity<VwSmsSettingForm>(entity =>
        {
            entity.ToView("VwSmsSettingForm", "cms");
        });

        modelBuilder.Entity<VwStateForm>(entity =>
        {
            entity.ToView("VwStateForm", "cms");
        });

        modelBuilder.Entity<VwTagForm>(entity =>
        {
            entity.ToView("VwTagForm", "cms");
        });

        modelBuilder.Entity<VwUserAddressForm>(entity =>
        {
            entity.ToView("VwUserAddressForm", "cms");
        });

        modelBuilder.Entity<VwUserForm>(entity =>
        {
            entity.ToView("VwUserForm", "auth");
        });

        modelBuilder.Entity<VwUserOtpForm>(entity =>
        {
            entity.ToView("VwUserOtpForm", "cms");
        });

        modelBuilder.Entity<VwUserRole>(entity =>
        {
            entity.ToView("VwUserRoles", "auth");
        });

        modelBuilder.Entity<VwUsersLite>(entity =>
        {
            entity.ToView("VwUsersLite", "auth");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
