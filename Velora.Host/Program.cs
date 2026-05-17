using Autofac;
using Autofac.Core;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MyApp.Orm.EfCore;
using Serilog;
using Serilog.Exceptions;
using System.Data;
using System.Text;
using Velora.Application.Seeds;
using Velora.Application.Services;
using Velora.Application.Shared.Dtos;
using Velora.Application.Shared.Enums;
using Velora.Application.Shared.Repositories;
using Velora.Application.Shared.Services;
using Velora.Application.Validators;
using Velora.EntityFrameworkCore.EntityFramework.PostgreSQL;
using Velora.EntityFrameworkCore.EntityFramework.SqlServer;
using Velora.Host.Middlewares;
using Velora.Host.Settings;
using Velora.Host.swagger;
using Velora.Infrastructure.ORM.Interfaces.MyApp.Orm.Interfaces;


// Configure Serilog logger
Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .Enrich.WithExceptionDetails() // Include detailed exception data
    .WriteTo.Console() // Log to console
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day) // Log to daily rolling file
                                                                         //.WriteTo.Seq("http://localhost:5341") // Optional: log to Seq if running
    .CreateLogger();
var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();
// Add services to the container.

builder.Services.AddControllers();
var databaseSettings = builder.Configuration.GetSection("Database").Get<DatabaseSettings>();
var dbTypeString = builder.Configuration.GetValue<string>("Database:Provider") ?? "PostgreSQL";
// مثال برای ASP.NET Core
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});


// تعیین نوع دیتابیس
var provider = dbTypeString.Equals("SqlServer", StringComparison.OrdinalIgnoreCase)
    ? DatabaseType.SqlServer
    : DatabaseType.PostgreSql;
if (provider == DatabaseType.SqlServer)
{
    builder.Services.AddDbContext<CoreCmsContext>(options =>
        options.UseSqlServer(databaseSettings.ConnectionString), ServiceLifetime.Scoped);
}
else
{
    builder.Services.AddDbContext<VeloraPgContext>(options =>
        options.UseNpgsql(databaseSettings.ConnectionString), ServiceLifetime.Scoped);
}


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' followed by your JWT token in the text input below.\r\nExample: 'Bearer abc123def456'"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            new string[] {}
        }
    });
    c.OperationFilter<SwaggerFileOperationFilter>();
});
// Tell ASP.NET Core to use Autofac as the DI container
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("Database"));

bool FilterTypes(Type t)
{
    return t.Name.EndsWith("Service")
           && t.Namespace != null;
}
// Get all loaded assemblies in the current application domain
// Then filter assemblies whose full name contains the string "Velora.Application"
// Convert the filtered assemblies to an array for registration
var filteredAssemblies = AppDomain.CurrentDomain.GetAssemblies()
    .Where(assembly => assembly.FullName.Contains("Velora.Application"))
    .ToArray();
var applicationAssembly = AppDomain.CurrentDomain.GetAssemblies()
    .FirstOrDefault(a => a.GetName().Name == "Velora.Application");

var sharedAssembly = AppDomain.CurrentDomain.GetAssemblies()
    .FirstOrDefault(a => a.GetName().Name == "Velora.Application.Shared");

if (applicationAssembly == null || sharedAssembly == null)
    throw new InvalidOperationException("Required assemblies are not loaded.");
var assembliesToScan = new[] { applicationAssembly, sharedAssembly };

// Get all loaded assemblies (می‌توانید فقط assembly پروژه خودتان را فیلتر کنید)
var assemblies = AppDomain.CurrentDomain.GetAssemblies()
    .Where(a => a.FullName!.Contains("Velora.Application"))
    .ToArray();

// پیدا کردن همه کلاس‌هایی که اسمشان Resolver است یا IResolver را implement می‌کنند
var resolverTypes = assemblies
    .SelectMany(a => a.GetTypes())
    .Where(t =>
        !t.IsAbstract &&
        !t.IsInterface &&
        (
            typeof(IGqlResolver).IsAssignableFrom(t) ||
            t.Name.EndsWith("GqlResolver", StringComparison.OrdinalIgnoreCase)
        )
    )
    .ToArray();
// ثبت داینامیک در GraphQL
var gqlBuilder = builder.Services
    .AddGraphQLServer()
    .AddAuthorization()
    .AddQueryType()
    //.AddTypeExtension<ComponentTypeGraphQLResolver>()
    .AddFiltering()
    .AddSorting()
    ;


foreach (var resolverType in resolverTypes)
{
    gqlBuilder.AddTypeExtension(resolverType);
}

builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    if (provider == DatabaseType.SqlServer)
    {
        containerBuilder.RegisterType<EfUnitOfWork<CoreCmsContext>>()
            .As<IUnitOfWork>()
            .InstancePerLifetimeScope();
    }
    else
    {
        containerBuilder.RegisterType<EfUnitOfWork<VeloraPgContext>>()
            .As<IUnitOfWork>()
            .InstancePerLifetimeScope();
    }
    // ثبت Resolverها به صورت Scoped
    foreach (var resolverType in resolverTypes)
    {
        containerBuilder.RegisterType(resolverType)
            .InstancePerLifetimeScope(); // ⚠️ مهم
    }
    containerBuilder.RegisterGeneric(typeof(EfCoreRepository<>))
        .As(typeof(ISqlRepository<>))
        .As(typeof(IPosgreSqlRepository<>))
        .InstancePerLifetimeScope();
    containerBuilder.RegisterAssemblyTypes(assembliesToScan)
        .Where(t => typeof(IBaseService).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract
        )
        .AsImplementedInterfaces()
        .InstancePerLifetimeScope();
    containerBuilder.RegisterGeneric(typeof(MemoryCacheService<,,>))
    .As(typeof(IMemoryCacheService<>))
    .InstancePerLifetimeScope();
    containerBuilder.RegisterGeneric(typeof(GenericService<,,>))
    .As(typeof(IGenericService<,,>))
    .InstancePerLifetimeScope();

    containerBuilder.RegisterType<PermissionCacheService>()
       .As<IPermissionCacheService>()
       .InstancePerLifetimeScope();
    containerBuilder.Register(context =>
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddMaps(filteredAssemblies);
        });
        return config.CreateMapper();
    }).As<IMapper>().InstancePerLifetimeScope();
    containerBuilder.RegisterType<HttpContextAccessor>()
       .As<IHttpContextAccessor>()
       .InstancePerLifetimeScope();

});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

builder.Services.AddFluentValidationAutoValidation(c => c.DisableDataAnnotationsValidation = true);
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.AddValidatorsFromAssemblyContaining<RoleDtoValidator>();
builder.Services.AddScoped<DataSeeder>();
builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("Jwt")
);

builder.Services.AddMemoryCache();
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 50_000_000; // 50 MB
});
var app = builder.Build();
// و در pipeline
app.UseCors(builder =>
{
    builder
        .WithOrigins("http://localhost:3000")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials(); // حتما این را داشته باش
});
// ----------------------------
// 2. Seed Data را اجرا کن
using (var scope = app.Services.CreateScope())
{
    var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();
    if (env.IsDevelopment()||env.IsProduction()) // ✅ فقط در حالت Development
    {
        var seeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();
        await seeder.SeedAllAsync();
    }
}

var supportedCultures = new[] { "en", "fa" };
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);

app.UseRequestLocalization(localizationOptions);
app.UseErrorHandlingMiddleware();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseMiddleware<LanguageMiddleware>();
app.UseRouting();           // حتما قبل از authentication/authorization
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapGraphQL(); // حالا HotChocolate Authorization کار می‌کند
});


app.MapControllers();

app.Run();

public partial class Program { }