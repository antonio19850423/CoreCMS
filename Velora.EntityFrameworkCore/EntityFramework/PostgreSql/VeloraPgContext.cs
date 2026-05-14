using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Velora.EntityFrameworkCore.EntityFramework.PostgreSQL;

public partial class VeloraPgContext : DbContext
{
    public VeloraPgContext()
    {
    }

    public VeloraPgContext(DbContextOptions<VeloraPgContext> options)
        : base(options)
    {
    }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<GeneralSetting> GeneralSettings { get; set; }

    public virtual DbSet<LocalizationKey> LocalizationKeys { get; set; }

    public virtual DbSet<LocalizationTranslation> LocalizationTranslations { get; set; }

    public virtual DbSet<LocalizationView> LocalizationViews { get; set; }

    public virtual DbSet<Permission> Permissions { get; set; }

    public virtual DbSet<Resource> Resources { get; set; }

    public virtual DbSet<ResourceLanguage> ResourceLanguages { get; set; }

    public virtual DbSet<ResourceType> ResourceTypes { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<RolePermission> RolePermissions { get; set; }

    public virtual DbSet<SeedHistory> SeedHistories { get; set; }

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
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=VeloraDB;Username=postgres;Password=Elham#09123507322#");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("cities_pkey");

            entity.ToTable("Cities", "gen");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.IsTest).HasDefaultValue(false);
            entity.Property(e => e.Name).HasMaxLength(100);

            entity.HasOne(d => d.State).WithMany(p => p.Cities)
                .HasForeignKey(d => d.StateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("cities_stateid_fkey");
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("countries_pkey");

            entity.ToTable("Countries", "gen");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CeatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.IsTest).HasDefaultValue(false);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<GeneralSetting>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("generalsettings_pkey");

            entity.ToTable("GeneralSettings", "gen");

            entity.HasIndex(e => e.Key, "generalsettings_Key_key").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Key).HasMaxLength(100);
            entity.Property(e => e.Value).HasMaxLength(500);
        });

        modelBuilder.Entity<LocalizationKey>(entity =>
        {
            entity.HasKey(e => e.Code).HasName("LocalizationKey_pkey");

            entity.ToTable("LocalizationKey", "gen");

            entity.Property(e => e.Code).HasMaxLength(200);
            entity.Property(e => e.CreatedAt)
                .HasPrecision(6)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.IsTest).HasDefaultValue(false);
            entity.Property(e => e.Type).HasMaxLength(50);
            entity.Property(e => e.UpdatedAt).HasPrecision(6);
        });

        modelBuilder.Entity<LocalizationTranslation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("LocalizationTranslation_pkey");

            entity.ToTable("LocalizationTranslation", "gen");

            entity.HasIndex(e => e.LanguageCode, "IX_LocalizationTranslation_LanguageCode");

            entity.HasIndex(e => new { e.LocalizationKeyCode, e.LanguageCode }, "UQ_LocalizationTranslation_Key_Language").IsUnique();

            entity.Property(e => e.CreatedAt)
                .HasPrecision(6)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.IsTest).HasDefaultValue(false);
            entity.Property(e => e.LanguageCode).HasMaxLength(10);
            entity.Property(e => e.LocalizationKeyCode).HasMaxLength(200);
            entity.Property(e => e.UpdatedAt).HasPrecision(6);
            entity.Property(e => e.Value).HasMaxLength(500);

            entity.HasOne(d => d.LocalizationKeyCodeNavigation).WithMany(p => p.LocalizationTranslations)
                .HasForeignKey(d => d.LocalizationKeyCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("LocalizationTranslation_LocalizationKeyCode_fkey");
        });

        modelBuilder.Entity<LocalizationView>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("LocalizationView", "gen");

            entity.Property(e => e.LanguageCode).HasMaxLength(10);
            entity.Property(e => e.LocalizationKeyCode).HasMaxLength(200);
            entity.Property(e => e.Type).HasMaxLength(50);
            entity.Property(e => e.Value).HasMaxLength(500);
        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("permissions_pkey");

            entity.ToTable("Permissions", "auth");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Actions).HasDefaultValue(0);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.Description).HasMaxLength(300);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.Istest)
                .HasDefaultValue(false)
                .HasColumnName("istest");

            entity.HasOne(d => d.Resource).WithMany(p => p.Permissions)
                .HasForeignKey(d => d.ResourceId)
                .HasConstraintName("permissions_resourceid_fkey");
        });

        modelBuilder.Entity<Resource>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("resources_pkey");

            entity.ToTable("Resources", "auth");

            entity.HasIndex(e => e.Code, "resources_code_key").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Code).HasMaxLength(100);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.Description).HasMaxLength(300);
            entity.Property(e => e.DisplayName).HasMaxLength(250);
            entity.Property(e => e.EntityName).HasMaxLength(100);
            entity.Property(e => e.FieldType).HasMaxLength(50);
            entity.Property(e => e.InputMask).HasMaxLength(20);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.IsRequired).HasDefaultValue(false);
            entity.Property(e => e.Istest)
                .HasDefaultValue(false)
                .HasColumnName("istest");
            entity.Property(e => e.LinkedFieldCode).HasMaxLength(200);
            entity.Property(e => e.Order).HasDefaultValue(0);
            entity.Property(e => e.Route).HasMaxLength(255);
            entity.Property(e => e.SelectDisplayFields).HasMaxLength(300);
            entity.Property(e => e.ServiceName).HasMaxLength(100);
            entity.Property(e => e.ShowInForm).HasDefaultValue(true);
            entity.Property(e => e.ShowInGrid).HasDefaultValue(true);
            entity.Property(e => e.ShowInSelectBox).HasDefaultValue(false);

            entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent)
                .HasForeignKey(d => d.ParentId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("resources_parentid_fkey");

            entity.HasOne(d => d.ResourceType).WithMany(p => p.Resources)
                .HasForeignKey(d => d.ResourceTypeId)
                .HasConstraintName("resources_resourcetypeid_fkey");
        });

        modelBuilder.Entity<ResourceLanguage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ResourceLanguage_pkey");

            entity.ToTable("ResourceLanguage", "auth");

            entity.HasIndex(e => new { e.ResourceId, e.LanguageCode }, "UQ_ResourceLanguage").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.LanguageCode).HasMaxLength(10);
            entity.Property(e => e.Name).HasMaxLength(200);

            entity.HasOne(d => d.Resource).WithMany(p => p.ResourceLanguages)
                .HasForeignKey(d => d.ResourceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ResourceLanguage_ResourceId_fkey");
        });

        modelBuilder.Entity<ResourceType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("resourcetypes_pkey");

            entity.ToTable("ResourceTypes", "auth");

            entity.HasIndex(e => e.Code, "resourcetypes_code_key").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Code).HasMaxLength(50);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.Description).HasMaxLength(300);
            entity.Property(e => e.DisplayName).HasMaxLength(200);
            entity.Property(e => e.Isdeleted).HasDefaultValue(false);
            entity.Property(e => e.Istest)
                .HasDefaultValue(false)
                .HasColumnName("istest");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("roles_pkey");

            entity.ToTable("Roles", "auth");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Code)
                .HasMaxLength(50)
                .HasDefaultValueSql("''::character varying");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.Istest)
                .HasDefaultValue(false)
                .HasColumnName("istest");
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<RolePermission>(entity =>
        {
            entity.HasKey(e => new { e.RoleId, e.PermissionId }).HasName("rolepermissions_pkey");

            entity.ToTable("RolePermissions", "auth");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.Istest)
                .HasDefaultValue(false)
                .HasColumnName("istest");

            entity.HasOne(d => d.Permission).WithMany(p => p.RolePermissions)
                .HasForeignKey(d => d.PermissionId)
                .HasConstraintName("rolepermissions_permissionid_fkey");

            entity.HasOne(d => d.Role).WithMany(p => p.RolePermissions)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("rolepermissions_roleid_fkey");
        });

        modelBuilder.Entity<SeedHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SeedHistory_pkey");

            entity.ToTable("SeedHistory", "auth");

            entity.HasIndex(e => e.Name, "UQ_SeedHistory_Name").IsUnique();

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.Name).HasMaxLength(150);
        });

        modelBuilder.Entity<State>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("states_pkey");

            entity.ToTable("States", "gen");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.IsTest).HasDefaultValue(false);
            entity.Property(e => e.Name).HasMaxLength(100);

            entity.HasOne(d => d.Country).WithMany(p => p.States)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("states_countryid_fkey");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("users_pkey");

            entity.ToTable("Users", "auth");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.Email).HasMaxLength(200);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.Istest)
                .HasDefaultValue(false)
                .HasColumnName("istest");
            entity.Property(e => e.MobileNumber).HasMaxLength(20);
            entity.Property(e => e.NationalCode).HasMaxLength(10);
            entity.Property(e => e.Password).HasMaxLength(256);
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);
            entity.Property(e => e.UserName).HasMaxLength(100);
        });

        modelBuilder.Entity<UserProfile>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("userprofiles_pkey");

            entity.ToTable("UserProfiles", "auth");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Address).HasMaxLength(500);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.Istest)
                .HasDefaultValue(false)
                .HasColumnName("istest");
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.NationalCode).HasMaxLength(20);
            entity.Property(e => e.ProfileImage).HasMaxLength(255);

            entity.HasOne(d => d.User).WithMany(p => p.UserProfiles)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("userprofiles_userid_fkey");
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("UserRoles_pkey");

            entity.ToTable("UserRoles", "auth");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.Istest)
                .HasDefaultValue(false)
                .HasColumnName("istest");

            entity.HasOne(d => d.Role).WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("userroles_roleid_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("userroles_userid_fkey");
        });

        modelBuilder.Entity<VwLocalization>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("VwLocalization", "gen");

            entity.Property(e => e.LanguageCode).HasMaxLength(10);
            entity.Property(e => e.LocalizationKeyCode).HasMaxLength(200);
            entity.Property(e => e.Type).HasMaxLength(50);
            entity.Property(e => e.Value).HasMaxLength(500);
        });

        modelBuilder.Entity<VwPermissionForm>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("VwPermissionForm", "auth");

            entity.Property(e => e.ResourceCode).HasMaxLength(100);
            entity.Property(e => e.ResourceName).HasMaxLength(250);
            entity.Property(e => e.ResourceTypeCode).HasMaxLength(50);
        });

        modelBuilder.Entity<VwResource>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("VwResources", "auth");

            entity.Property(e => e.DefaultDisplayName).HasMaxLength(250);
            entity.Property(e => e.EntityName).HasMaxLength(100);
            entity.Property(e => e.FieldType).HasMaxLength(50);
            entity.Property(e => e.InputMask).HasMaxLength(20);
            entity.Property(e => e.LanguageCode).HasMaxLength(10);
            entity.Property(e => e.LinkedFieldCode).HasMaxLength(200);
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.ResourceCode).HasMaxLength(100);
            entity.Property(e => e.ResourceTypeCode).HasMaxLength(50);
            entity.Property(e => e.Route).HasMaxLength(255);
            entity.Property(e => e.SelectDisplayFields).HasMaxLength(300);
            entity.Property(e => e.ServiceName).HasMaxLength(100);
        });

        modelBuilder.Entity<VwResourceForm>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("VwResourceForm", "auth");

            entity.Property(e => e.Code).HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(300);
            entity.Property(e => e.DisplayName).HasMaxLength(250);
            entity.Property(e => e.EntityName).HasMaxLength(100);
            entity.Property(e => e.FieldType).HasMaxLength(50);
            entity.Property(e => e.InputMask).HasMaxLength(20);
            entity.Property(e => e.LinkedFieldCode).HasMaxLength(200);
            entity.Property(e => e.ParentDisplayName).HasMaxLength(250);
            entity.Property(e => e.ResourceTypeTitle).HasMaxLength(50);
            entity.Property(e => e.Route).HasMaxLength(255);
            entity.Property(e => e.ServiceName).HasMaxLength(100);
        });

        modelBuilder.Entity<VwUserForm>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("VwUserForm", "auth");

            entity.Property(e => e.Address).HasMaxLength(500);
            entity.Property(e => e.CityName).HasMaxLength(100);
            entity.Property(e => e.CountryName).HasMaxLength(100);
            entity.Property(e => e.Email).HasMaxLength(200);
            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.MobileNumber).HasMaxLength(20);
            entity.Property(e => e.NationalCode).HasMaxLength(10);
            entity.Property(e => e.Password).HasMaxLength(256);
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);
            entity.Property(e => e.ProfileImage).HasMaxLength(255);
            entity.Property(e => e.StateName).HasMaxLength(100);
            entity.Property(e => e.UserName).HasMaxLength(100);
        });

        modelBuilder.Entity<VwUserRole>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("VwUserRoles", "auth");

            entity.Property(e => e.Email).HasMaxLength(200);
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);
            entity.Property(e => e.RoleCode).HasMaxLength(50);
            entity.Property(e => e.RoleDescription).HasMaxLength(500);
            entity.Property(e => e.RoleName).HasMaxLength(100);
            entity.Property(e => e.UserName).HasMaxLength(100);
        });

        modelBuilder.Entity<VwUsersLite>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("VwUsersLite", "auth");

            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.UserName).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
