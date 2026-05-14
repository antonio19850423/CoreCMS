using FluentAssertions;
using Microsoft.AspNetCore.Http.Extensions;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Velora.Application.Shared.Dtos;
using Velora.Application.Shared.Dtos.GraphQL;
using Velora.Application.Shared.GraphQL;
using Velora.IntegrationTests;
using Xunit;
namespace Velora.IntegrationTests.Controllers
{
    [Collection("IntegrationTests")]
    public class RoleControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public RoleControllerTests(CustomWebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }
        private async Task AuthenticateAsync()
        {
            var loginDto = new { UserName = "Developer", Password = "123" };

            var response = await _client.PostAsJsonAsync("/api/auth/login", loginDto);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<ResultDto<LoginResultDto>>();
            result!.Success.Should().BeTrue();
            result.Data.Should().NotBeNull();

            _client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", result.Data.Token);
        }
        [Fact(DisplayName = "Role CRUD should succeed (Create, Get, Update, Delete)")]
        public async Task Role_CRUD_Should_Succeed()
        {
            Console.WriteLine("Authorization header: " + _client.DefaultRequestHeaders.Authorization);
            await AuthenticateAsync();
            // 1️⃣ Arrange: دیتای اولیه
            var roleDto = new
            {
                Id = Guid.NewGuid(), // در Create نیازی نیست ولی باید وجود داشته باشه برای JSON
                Name = "Test Role" + Guid.NewGuid(),
                Code = "TEST_ROLE" + Guid.NewGuid()
            };

            // 2️⃣ Act: Create
            var createResponse = await _client.PostAsJsonAsync("/api/role", roleDto);
            createResponse.EnsureSuccessStatusCode();

            var createdRole = await createResponse.Content.ReadFromJsonAsync<ResultDto<RoleDto>>();
            createdRole.Should().NotBeNull();
            createdRole!.Data.Id.Should().NotBe(Guid.Empty);
            createdRole.Data.Name.Should().Be(roleDto.Name);

            var roleId = createdRole.Data.Id;

            // 3️⃣ Act: GetById
            var getResponse = await _client.GetAsync($"/api/role/{roleId}");
            getResponse.EnsureSuccessStatusCode();

            var fetchedRole = await getResponse.Content.ReadFromJsonAsync<ResultDto<RoleDto>>();
            fetchedRole.Should().NotBeNull();
            fetchedRole!.Data.Id.Should().Be(roleId);

            // 4️⃣ Act: Update
            var updatedDto = new
            {
                Id = roleId,
                Name = "Updated Role",
                Code = "UPDATED_ROLE"
                ,IsTest=true
            };

            var updateResponse = await _client.PutAsJsonAsync($"/api/role/{roleId}", updatedDto);
            updateResponse.EnsureSuccessStatusCode();

            var updatedRole = await updateResponse.Content.ReadFromJsonAsync<ResultDto<RoleDto>>();
            updatedRole.Should().NotBeNull();
            updatedRole!.Data.Name.Should().Be("Updated Role");

            // 5️⃣ Act: Delete
            var deleteResponse = await _client.DeleteAsync($"/api/role/{roleId}");
            deleteResponse.EnsureSuccessStatusCode();

            // 6️⃣ Act: GetById (باید NotFound برگردد)
            var getAfterDeleteResponse = await _client.GetAsync($"/api/role/{roleId}");
            getAfterDeleteResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);

        }
        [Fact(DisplayName = "GetAllRoles should return paginated results using cursor pagination")]
        public async Task GetAllRoles_ReturnsCursorPaginatedResults()
        {
            // Arrange
            await AuthenticateAsync();
            var builder = new GraphQLQueryBuilder<RoleDto>("getAllRoles")
                .WithArgument("first", 2)
                .WithArgument("order", new { name = GraphQLEnum.Of("ASC") });

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

            var result = await response.Content.ReadFromJsonAsync<GraphQLResponse<Dictionary<string, GraphQLConnection<RoleDto>>>>();
            var rolesConnection = result!.Data["getAllRoles"];

            // Assertions
            rolesConnection.Should().NotBeNull();
            rolesConnection.Nodes.Should().NotBeNullOrEmpty();
            rolesConnection.TotalCount.Should().BeGreaterThan(0);
            rolesConnection.PageInfo.Should().NotBeNull();
            rolesConnection.PageInfo.EndCursor.Should().NotBeNullOrEmpty(); // مثال اضافه
        }

    }
}
