using FluentAssertions;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Velora.Application.Shared.Dtos;
using Velora.Application.Shared.Dtos.GraphQL;
using Velora.Application.Shared.GraphQL;
using Xunit;

namespace Velora.IntegrationTests.Controllers
{
    [Collection("IntegrationTests")]
    public class ResourceTypeControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public ResourceTypeControllerTests(CustomWebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        private async Task AuthenticateAsync()
        {
            var loginDto = new { UserName = "Developer", Password = "123" };
            var response = await _client.PostAsJsonAsync("/api/auth/login", loginDto);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<ResultDto<LoginResultDto>>();
            _client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", result!.Data.Token);
        }

        [Fact(DisplayName = "ResourceType CRUD should succeed (Create, Get, Update, Delete)")]
        public async Task ResourceType_CRUD_Should_Succeed()
        {
            await AuthenticateAsync();

            var dto = new
            {
                Id = Guid.NewGuid(),
                Code = "TEST_CODE",
                Name = "Test ResourceType",
                DisplayName = "Test Display",
                Description = "Test description"
                ,IsTest=true
            };

            var createResponse = await _client.PostAsJsonAsync("/api/resourcetype", dto);
            createResponse.EnsureSuccessStatusCode();

            var created = await createResponse.Content.ReadFromJsonAsync<ResultDto<ResourceTypeDto>>();
            var id = created!.Data.Id;

            // ✅ Get
            var getResponse = await _client.GetAsync($"/api/resourcetype/{id}");
            getResponse.EnsureSuccessStatusCode();

            // ✅ Update
            var updatedDto = new
            {
                Id = id,
                Code = "UPDATED_CODE",
                Name = "Updated ResourceType",
                DisplayName = "Updated Display",
                Description = "Updated description"
                ,IsTest=true
            };
            var updateResponse = await _client.PutAsJsonAsync($"/api/resourcetype/{id}", updatedDto);
            updateResponse.EnsureSuccessStatusCode();

            // ✅ Delete
            var deleteResponse = await _client.DeleteAsync($"/api/resourcetype/{id}");
            deleteResponse.EnsureSuccessStatusCode();
        }
        [Fact(DisplayName = "GetAllResourceTypes should return paginated results using cursor pagination")]
        public async Task GetAllResourceTypes_ReturnsCursorPaginatedResults()
        {
            // Arrange
            await AuthenticateAsync();
            var builder = new GraphQLQueryBuilder<ResourceTypeDto>("getAllResourceTypes")
                .WithArgument("first", 2)
                .WithArgument("order", new { id = GraphQLEnum.Of("ASC") });

            // گرفتن رشته query واقعی
            var queryString = builder.BuildQuery()
                .Replace("\r", "")
                .Replace("\n", " ")
                .Trim();
            // آماده کردن بدنه برای ارسال به GraphQL
            var requestBody = new
            {
                query = queryString
            };

            // سریالایز به JSON
            var jsonString = JsonSerializer.Serialize(requestBody, new JsonSerializerOptions { WriteIndented = true });

            // HttpContent
            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

            // Send request
            var response = await _client.PostAsync("/graphql", content);
            response.EnsureSuccessStatusCode();

            // Read response
            var responseString = await response.Content.ReadAsStringAsync();
            Console.WriteLine(responseString);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<GraphQLResponse<Dictionary<string, GraphQLConnection<ResourceTypeDto>>>>();
            var rolesConnection = result!.Data["getAllResourceTypes"];

            // Assertions
            rolesConnection.Should().NotBeNull();
            rolesConnection.Nodes.Should().NotBeNullOrEmpty();
            rolesConnection.TotalCount.Should().BeGreaterThan(0);
            rolesConnection.PageInfo.Should().NotBeNull();
            rolesConnection.PageInfo.EndCursor.Should().NotBeNullOrEmpty(); // مثال اضافه
        }
    }
}
