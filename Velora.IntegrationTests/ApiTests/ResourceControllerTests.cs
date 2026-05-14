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
    // ✅ Resource Tests
    public class ResourceControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public ResourceControllerTests(CustomWebApplicationFactory<Program> factory)
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

        [Fact(DisplayName = "Resource CRUD should succeed (Create, Get, Update, Delete)")]
        public async Task Resource_CRUD_Should_Succeed()
        {
            await AuthenticateAsync();

            // ✅ اول باید ResourceType بسازیم
            var resTypeDto = new
            {
                Id = Guid.NewGuid(),
                Code = "RES_TYPE"+ Guid.NewGuid(),
                Name = "Test ResourceType",
                DisplayName = "RT Display",
                Description = "RT Description"
                ,IsTest=true
            };
            var resTypeResponse = await _client.PostAsJsonAsync("/api/resourcetype", resTypeDto);
            resTypeResponse.EnsureSuccessStatusCode();
            var resType = await resTypeResponse.Content.ReadFromJsonAsync<ResultDto<ResourceTypeDto>>();

            var dto = new
            {
                Id = Guid.NewGuid(),
                ResourceTypeId = resType!.Data.Id,
                ParentId = (Guid?)null,
                Code = "TEST_RES",
                Name = "Test Resource",
                DisplayName = "Test Display",
                Description = "Test Description",
                Order = 1,
                IsActive = true
                ,IsTest=true
            };

            var createResponse = await _client.PostAsJsonAsync("/api/resource", dto);
            createResponse.EnsureSuccessStatusCode();
            var created = await createResponse.Content.ReadFromJsonAsync<ResultDto<ResourceDto>>();
            var id = created!.Data.Id;

            // ✅ Update
            var updatedDto = new
            {
                Id = id,
                ResourceTypeId = created.Data.ResourceTypeId,
                ParentId = (Guid?)null,
                Code = "UPDATED_CODE",
                Name = "Updated Resource",
                DisplayName = "Updated Display",
                Description = "Updated Description",
                Order = 2,
                IsActive = false
                ,IsTest=true
            };
            var updateResponse = await _client.PutAsJsonAsync($"/api/resource/{id}", updatedDto);
            updateResponse.EnsureSuccessStatusCode();

            // ✅ Delete
            var deleteResponse = await _client.DeleteAsync($"/api/resource/{id}");
            deleteResponse.EnsureSuccessStatusCode();
        }
        [Fact(DisplayName = "GetAllResources should return paginated results using cursor pagination")]
        public async Task GetAllResources_ReturnsCursorPaginatedResults()
        {
            // Arrange
            await AuthenticateAsync();
            var builder = new GraphQLQueryBuilder<ResourceDto>("getAllResources")
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

            var result = await response.Content.ReadFromJsonAsync<GraphQLResponse<Dictionary<string, GraphQLConnection<ResourceDto>>>>();
            var rolesConnection = result!.Data["getAllResources"];

            // Assertions
            rolesConnection.Should().NotBeNull();
            rolesConnection.Nodes.Should().NotBeNullOrEmpty();
            rolesConnection.TotalCount.Should().BeGreaterThan(0);
            rolesConnection.PageInfo.Should().NotBeNull();
            rolesConnection.PageInfo.EndCursor.Should().NotBeNullOrEmpty(); // مثال اضافه
        }
    }
}
