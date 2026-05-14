using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace Velora.IntegrationTests;

public class CustomWebApplicationFactory<TStartup>
    : WebApplicationFactory<TStartup> where TStartup : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // اینجا میتونی سرویس‌ها رو جایگزین کنی، مثل:
            // - دیتابیس InMemory
            // - Mock سرویس‌ها (مثلاً ایمیل)
            // - Seed داده اولیه برای تست

            // مثال دیتابیس InMemory:
            // var descriptor = services.SingleOrDefault(
            //     d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
            // services.Remove(descriptor);
            // services.AddDbContext<AppDbContext>(options =>
            // {
            //     options.UseInMemoryDatabase("TestDb");
            // });
        });
    }
}
