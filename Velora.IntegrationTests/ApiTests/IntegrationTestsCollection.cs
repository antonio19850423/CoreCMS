using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.IntegrationTests.ApiTests
{
    [CollectionDefinition("IntegrationTests")]
    public class IntegrationTestsCollection : ICollectionFixture<TestDataCleanupFixture>
    {
        // این کلاس خالی است، فقط برای معرفی CollectionFixture
    }
}
