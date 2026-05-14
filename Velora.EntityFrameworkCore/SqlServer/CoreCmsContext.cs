using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Velora.EntityFrameworkCore.SqlServer;

public partial class CoreCmsContext : DbContext
{
    public CoreCmsContext()
    {
    }

    public CoreCmsContext(DbContextOptions<CoreCmsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<ComponentFieldMap> ComponentFieldMaps { get; set; }

    public virtual DbSet<ComponentType> ComponentTypes { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<FooterColumn> FooterColumns { get; set; }

    public virtual DbSet<FooterLink> FooterLinks { get; set; }

    public virtual DbSet<GeneralSetting> GeneralSettings { get; set; }

    public virtual DbSet<LocalizationKey> LocalizationKeys { get; set; }

    public virtual DbSet<LocalizationTranslation> LocalizationTranslations { get; set; }

    public virtual DbSet<Menu> Menus { get; set; }

    public virtual DbSet<MenuItem> MenuItems { get; set; }

    public virtual DbSet<Page> Pages { get; set; }

    public virtual DbSet<Permission> Permissions { get; set; }

    public virtual DbSet<Resource> Resources { get; set; }

    public virtual DbSet<ResourceLanguage> ResourceLanguages { get; set; }

    public virtual DbSet<ResourceType> ResourceTypes { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<RolePermission> RolePermissions { get; set; }

    public virtual DbSet<Section> Sections { get; set; }

    public virtual DbSet<SectionItem> SectionItems { get; set; }

    public virtual DbSet<SeedHistory> SeedHistories { get; set; }

    public virtual DbSet<SiteSetting> SiteSettings { get; set; }

    public virtual DbSet<State> States { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserProfile> UserProfiles { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    public virtual DbSet<VwLocalization> VwLocalizations { get; set; }

    public virtual DbSet<VwPermissionForm> VwPermissionForms { get; set; }

    public virtual DbSet<VwResource> VwResources { get; set; }

    public virtual DbSet<VwResourceForm> VwResourceForms { get; set; }

    public virtual DbSet<VwUserForm> VwUserForms { get; set; }

    public virtual DbSet<VwUserRole> VwUserRoles { get; set; }

    public virtual DbSet<VwUsersLite> VwUsersLites { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-JLBIAKI\\AFE;Database=CoreCMS;User Id=sa;Password=77723588;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Cities__3214EC07B13C2BFF");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.State).WithMany(p => p.Cities)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Cities__StateId__52593CB8");
        });

        modelBuilder.Entity<ComponentFieldMap>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Componen__3214EC07F390E769");

            entity.Property(e => e.IsVisible).HasDefaultValue(true);

            entity.HasOne(d => d.ComponentType).WithMany(p => p.ComponentFieldMaps)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ComponentFieldMaps_ComponentType");
        });

        modelBuilder.Entity<ComponentType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Componen__3214EC07D91D48BE");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Countrie__3214EC07DB0AB1CE");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<FooterColumn>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__FooterCo__3214EC07D496A314");

            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<FooterLink>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__FooterLi__3214EC07141D8853");

            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.FooterColumn).WithMany(p => p.FooterLinks)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FooterLinks_Column");
        });

        modelBuilder.Entity<GeneralSetting>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__GeneralS__3214EC07DA747B55");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
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
            entity.HasKey(e => e.Id).HasName("PK__Menus__3214EC072B888AD6");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<MenuItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__MenuItem__3214EC07D6CE7F74");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.Menu).WithMany(p => p.MenuItems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MenuItems_Menu");

            entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent).HasConstraintName("FK_MenuItems_Parent");
        });

        modelBuilder.Entity<Page>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Pages__3214EC0767D9E890");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsPublished).HasDefaultValue(true);
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
            entity.HasKey(e => e.Id).HasName("PK__Sections__3214EC0746A07F01");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.ComponentType).WithMany(p => p.Sections)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Sections_ComponentType");

            entity.HasOne(d => d.Page).WithMany(p => p.Sections)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Sections_Page");
        });

        modelBuilder.Entity<SectionItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SectionI__3214EC077F63EF7B");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.ComponentType).WithMany(p => p.SectionItems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SectionItems_ComponentType");

            entity.HasOne(d => d.Section).WithMany(p => p.SectionItems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SectionItems_Section");
        });

        modelBuilder.Entity<SeedHistory>(entity =>
        {
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
        });

        modelBuilder.Entity<SiteSetting>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SiteSett__3214EC07D91DB6DE");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
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

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC079F63A5A8");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
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

        modelBuilder.Entity<VwLocalization>(entity =>
        {
            entity.ToView("VwLocalization", "gen");
        });

        modelBuilder.Entity<VwPermissionForm>(entity =>
        {
            entity.ToView("VwPermissionForm", "auth");
        });

        modelBuilder.Entity<VwResource>(entity =>
        {
            entity.ToView("VwResources", "auth");
        });

        modelBuilder.Entity<VwResourceForm>(entity =>
        {
            entity.ToView("VwResourceForm", "auth");
        });

        modelBuilder.Entity<VwUserForm>(entity =>
        {
            entity.ToView("VwUserForm", "auth");
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
