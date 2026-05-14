using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Velora.Application.Shared.Dtos;
using Velora.Application.Shared.Dtos.GraphQL;
using Velora.Application.Shared.Enums;
using Velora.Application.Shared.GraphQL;
using Velora.IntegrationTests;

namespace Velora.IntegrationTests.Controllers
{
    [Collection("IntegrationTests")]
    public class UserRoleControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly DatabaseType _dbType;
        

        public UserRoleControllerTests(CustomWebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
            var configuration = factory.Services.GetRequiredService<IConfiguration>();
            var provider = configuration.GetValue<string>("Database:Provider") ?? "PostgreSql";

            _dbType = provider.Equals("SqlServer", StringComparison.OrdinalIgnoreCase)
                ? DatabaseType.SqlServer
                : DatabaseType.PostgreSql;
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

        [Fact(DisplayName = "UserRole CRUD should succeed (Create, Get, Update, Delete)")]
        public async Task UserRole_CRUD_Should_Succeed()
        {
            await AuthenticateAsync();

            // 1️⃣ اول یک Role بسازیم
            var roleDto = new { Id = Guid.NewGuid(), Name = "TestRole_" + Guid.NewGuid() ,Code = "TEST_ROLE" + Guid.NewGuid(), IsTest = true };
            var roleResponse = await _client.PostAsJsonAsync("/api/role", roleDto);
            roleResponse.EnsureSuccessStatusCode();
            var roleCreated = await roleResponse.Content.ReadFromJsonAsync<ResultDto<RoleDto>>();
            var roleId = roleCreated!.Data.Id;

            // 2️⃣ سپس یک User بسازیم
            var userDto = new { Id = Guid.NewGuid(), UserName = "testuser_" + Guid.NewGuid(), PasswordHash = "123456", IsTest = true };
            var userResponse = await _client.PostAsJsonAsync("/api/user", userDto);
            userResponse.EnsureSuccessStatusCode();
            var userCreated = await userResponse.Content.ReadFromJsonAsync<ResultDto<UserDto>>();
            var userId = userCreated!.Data.Id;

            // 3️⃣ حالا UserRole
            var dto = new { Id = Guid.NewGuid(), Userid = userId, Roleid = roleId, IsTest = true };

            var createResponse = await _client.PostAsJsonAsync("/api/userrole", dto);
            createResponse.EnsureSuccessStatusCode();

            var created = await createResponse.Content.ReadFromJsonAsync<ResultDto<UserRoleDto>>();
            created!.Data.Roleid.Should().Be(roleId);

            var id = created.Data.Id;

            // 4️⃣ Update
            var newRoleDto = new { Id = Guid.NewGuid(), Name = "TestRole_" + Guid.NewGuid() ,Code = "TEST_ROLE" + Guid.NewGuid(), IsTest = true };
            var newRoleResponse = await _client.PostAsJsonAsync("/api/role", newRoleDto);
            newRoleResponse.EnsureSuccessStatusCode();
            var newRole = await newRoleResponse.Content.ReadFromJsonAsync<ResultDto<RoleDto>>();

            var updatedDto = new { Id = id, Userid = userId, Roleid = newRole!.Data.Id };
            var updateResponse = await _client.PutAsJsonAsync($"/api/userrole/{id}", updatedDto);
            updateResponse.EnsureSuccessStatusCode();

            var updated = await updateResponse.Content.ReadFromJsonAsync<ResultDto<UserRoleDto>>();
            updated!.Data.Roleid.Should().Be(newRole.Data.Id);

            // 5️⃣ Delete
            var deleteResponse = await _client.DeleteAsync($"/api/userrole/{id}");
            deleteResponse.EnsureSuccessStatusCode();
        }

        [Fact(DisplayName = "GetAllUserRoles should return paginated results using cursor pagination")]
        public async Task GetAllUserRoles_ReturnsCursorPaginatedResults()
        {
            // Arrange
            await AuthenticateAsync();
            var builder = new GraphQLQueryBuilder<UserRoleDto>("getAllUserRoles")
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

            var result = await response.Content.ReadFromJsonAsync<GraphQLResponse<Dictionary<string, GraphQLConnection<UserRoleDto>>>>();
            var rolesConnection = result!.Data["getAllUserRoles"];

            // Assertions
            rolesConnection.Should().NotBeNull();
            rolesConnection.Nodes.Should().NotBeNullOrEmpty();
            rolesConnection.TotalCount.Should().BeGreaterThan(0);
            rolesConnection.PageInfo.Should().NotBeNull();
            rolesConnection.PageInfo.EndCursor.Should().NotBeNullOrEmpty(); // مثال اضافه
        }

        [Fact(DisplayName = "GetAllUserRolesView should return results for the current DB provider")]
        public async Task GetAllUserRolesView_ReturnsCorrectResults()
        {
            await AuthenticateAsync();
            string graphQlField;
            string queryString;

            if (_dbType == DatabaseType.SqlServer)
            {
                graphQlField = "GetSqlUserRolesView";
                var builder = new GraphQLQueryBuilder<Velora.EntityFrameworkCore.EntityFramework.SqlServer.VwUserRole>(graphQlField)
                    .WithArgument("first", 5)
                    .WithArgument("order", new { UserId = GraphQLEnum.Of("ASC") });

                queryString = builder.BuildQuery().Replace("\r", "").Replace("\n", " ").Trim();
            }
            else
            {
                graphQlField = "GetPgUserRolesView";
                var builder = new GraphQLQueryBuilder<Velora.EntityFrameworkCore.EntityFramework.PostgreSQL.VwUserRole>(graphQlField)
                    .WithArgument("first", 5)
                    .WithArgument("order", new { UserId = GraphQLEnum.Of("ASC") });

                queryString = builder.BuildQuery().Replace("\r", "").Replace("\n", " ").Trim();
            }

            var requestBody = new { query = queryString };
            var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/graphql", content);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<
                GraphQLResponse<Dictionary<string, GraphQLConnection<dynamic>>>>();

            var rolesConnection = result!.Data[graphQlField];

            rolesConnection.Should().NotBeNull();
            rolesConnection.Nodes.Should().NotBeNullOrEmpty();
            rolesConnection.TotalCount.Should().BeGreaterThan(0);
            rolesConnection.PageInfo.Should().NotBeNull();
            rolesConnection.PageInfo.EndCursor.Should().NotBeNullOrEmpty();
        }
    }
}