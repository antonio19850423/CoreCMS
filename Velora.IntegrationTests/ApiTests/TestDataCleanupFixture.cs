using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.IntegrationTests.ApiTests
{
    public class TestDataCleanupFixture : IAsyncLifetime
    {
        private readonly HttpClient _client;

        public TestDataCleanupFixture()
        {
            var factory = new CustomWebApplicationFactory<Program>();
            _client = factory.CreateClient();
        }

        public Task InitializeAsync() => Task.CompletedTask; // قبل از همه تست‌ها

        public async Task DisposeAsync() // بعد از همه تست‌ها
        {
            // پاکسازی تمام داده‌های تست
            var resResp = await _client.DeleteAsync("/api/DataCleanup/CleanupTestData");
            resResp.EnsureSuccessStatusCode();
        }
    }

}
