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
    public class PermissionControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public PermissionControllerTests(CustomWebApplicationFactory<Program> factory)
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

        [Fact(DisplayName = "Permission CRUD should succeed (Create, Get, Update, Delete)")]
        public async Task Permission_CRUD_Should_Succeed()
        {
            await AuthenticateAsync();

            // ✅ اول ResourceType
            var resTypeDto = new { Id = Guid.NewGuid(), Code = "RT_CODE"+Guid.NewGuid(), Name = "ResType", DisplayName = "ResType Display", Description = "ResType Desc", IsTest=true };
            var resTypeResp = await _client.PostAsJsonAsync("/api/resourcetype", resTypeDto);
            resTypeResp.EnsureSuccessStatusCode();
            var resType = await resTypeResp.Content.ReadFromJsonAsync<ResultDto<ResourceTypeDto>>();

            // ✅ بعد Resource
            var resDto = new { Id = Guid.NewGuid(), ResourceTypeId = resType!.Data.Id, ParentId = (Guid?)null, Code = "RES_CODE" + Guid.NewGuid(), Name = "Resource", DisplayName = "Res Display", Description = "Res Desc", Order = 1, IsActive = true, IsTest = true };
            var resResp = await _client.PostAsJsonAsync("/api/resource", resDto);
            resResp.EnsureSuccessStatusCode();
            var resource = await resResp.Content.ReadFromJsonAsync< ResultDto<ResourceDto>>();

            // ✅ حالا Permission
            var dto = new { Id = Guid.NewGuid(), ResourceId = resource!.Data.Id, Description = "Test permission", IsActive = true, IsTest = true };
            var createResponse = await _client.PostAsJsonAsync("/api/permission", dto);
            createResponse.EnsureSuccessStatusCode();
            var created = await createResponse.Content.ReadFromJsonAsync<ResultDto<PermissionDto>>();
            var id = created!.Data.Id;

            // ✅ Update
            var updatedDto = new { Id = id, ResourceId = resource.Data.Id, Description = "Updated permission", IsActive = false, IsTest = true };
            var updateResponse = await _client.PutAsJsonAsync($"/api/permission/{id}", updatedDto);
            updateResponse.EnsureSuccessStatusCode();

            // ✅ Delete
            var deleteResponse = await _client.DeleteAsync($"/api/permission/{id}");
            deleteResponse.EnsureSuccessStatusCode();
        }
        [Fact(DisplayName = "GetAllPermissions should return paginated results using cursor pagination")]
        public async Task GetAllPermissions_ReturnsCursorPaginatedResults()
        {
            // Arrange
            await AuthenticateAsync();
            var builder = new GraphQLQueryBuilder<PermissionDto>("getAllPermissions")
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

            var result = await response.Content.ReadFromJsonAsync<GraphQLResponse<Dictionary<string, GraphQLConnection<PermissionDto>>>>();
            var rolesConnection = result!.Data["getAllPermissions"];

            // Assertions
            rolesConnection.Should().NotBeNull();
            rolesConnection.Nodes.Should().NotBeNullOrEmpty();
            rolesConnection.TotalCount.Should().BeGreaterThan(0);
            rolesConnection.PageInfo.Should().NotBeNull();
            rolesConnection.PageInfo.EndCursor.Should().NotBeNullOrEmpty(); // مثال اضافه
        }
    }
}
