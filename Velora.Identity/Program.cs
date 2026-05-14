using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using MyApp.Orm.EfCore;
using Serilog;
using Serilog.Exceptions;
using Velora.Application.Shared.Repositories;
using Velora.Application.Shared.Services;
using Velora.Application.Validators;
using Velora.EntityFrameworkCore.EntityFramework.PostgreSQL;
using Velora.EntityFrameworkCore.EntityFramework.SqlServer;
using Velora.Identity.Middlewares;
using Velora.Identity.Settings;
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


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<VeloraPgContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("PostgresConnection")));
// Tell ASP.NET Core to use Autofac as the DI container
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
var databaseSettings = builder.Configuration.GetSection("Database").Get<DatabaseSettings>();

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
if (databaseSettings.Provider == "PostgreSql")
{
    builder.Services.AddDbContext<VeloraPgContext>(options =>
        options.UseNpgsql(databaseSettings.ConnectionString));

    builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
    {
        containerBuilder.RegisterType<EfUnitOfWork<VeloraPgContext>>()
    .As<IUnitOfWork>()
    .InstancePerLifetimeScope();
        containerBuilder.RegisterGeneric(typeof(EfCoreRepository<>))
            .As(typeof(ISqlRepository<>))
            .InstancePerLifetimeScope();


        containerBuilder.RegisterAssemblyTypes(assembliesToScan)
            .Where(t => typeof(IBaseService).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();
        containerBuilder.Register(context =>
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddMaps(filteredAssemblies);
            });
            return config.CreateMapper();
        }).As<IMapper>().InstancePerLifetimeScope();
    });

}
else if (databaseSettings.Provider == "SqlServer")
{
    builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
    {
        containerBuilder.RegisterType<EfUnitOfWork<VeloraDbContext>>()
    .As<IUnitOfWork>()
    .InstancePerLifetimeScope();

        containerBuilder.RegisterGeneric(typeof(EfCoreRepository<>))
            .As(typeof(ISqlRepository<>))
            .InstancePerLifetimeScope();
        containerBuilder.RegisterAssemblyTypes(assembliesToScan)
            .Where(t => typeof(IBaseService).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();
        // ✅ AutoMapper registration
        containerBuilder.Register(context =>
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddMaps(filteredAssemblies);
            });
            return config.CreateMapper();
        }).As<IMapper>().InstancePerLifetimeScope();
    });
}

//builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
//{
//    // Get the assembly containing the validators (usually Velora.Application)
//    var assembly = AppDomain.CurrentDomain.GetAssemblies()
//        .FirstOrDefault(a => a.GetName().Name == "Velora.Application");

//    if (assembly != null)
//    {
//        // Register all classes implementing IValidator<T> from this assembly
//        containerBuilder.RegisterAssemblyTypes(assembly)
//            .Where(t => t.IsClosedTypeOf(typeof(IValidator<>)))
//            .AsImplementedInterfaces()
//            .InstancePerLifetimeScope();
//    }
//});
builder.Services.AddFluentValidationAutoValidation(c => c.DisableDataAnnotationsValidation = true);
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.AddValidatorsFromAssemblyContaining<RoleDtoValidator>();

var app = builder.Build();
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


app.UseAuthorization();

app.MapControllers();
app.Run();
