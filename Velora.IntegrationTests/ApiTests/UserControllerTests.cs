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
    public class UserControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public UserControllerTests(CustomWebApplicationFactory<Program> factory)
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
        [Fact(DisplayName = "User CRUD should succeed (Create, Get, Update, Delete)")]
        public async Task User_CRUD_Should_Succeed()
        {

            Console.WriteLine("Authorization header: " + _client.DefaultRequestHeaders.Authorization);
            await AuthenticateAsync();
            // 1️⃣ Arrange
            var dto = new
            {
                Id = Guid.NewGuid(),
                UserName = "testuser"+Guid.NewGuid(),
                PasswordHash = "hashedpass",
                Email = "test@test.com",
                PhoneNumber = "09120000000",
                IsActive = true,
                Roles = new object[] { } // خالی چون وابستگی داریم
                ,IsTest=true
            };

            // 2️⃣ Create
            var createResponse = await _client.PostAsJsonAsync("/api/user", dto);
            createResponse.EnsureSuccessStatusCode();

            var created = await createResponse.Content.ReadFromJsonAsync<ResultDto<UserDto>>();
            created.Should().NotBeNull();
            created!.Data.Id.Should().NotBe(Guid.Empty);
            created.Data.UserName.Should().Be(dto.UserName);

            var id = created.Data.Id;

            // 3️⃣ GetById
            var getResponse = await _client.GetAsync($"/api/user/{id}");
            getResponse.EnsureSuccessStatusCode();

            var fetched = await getResponse.Content.ReadFromJsonAsync<ResultDto<UserDto>>();
            fetched.Should().NotBeNull();
            fetched!.Data.Id.Should().Be(id);

            // 4️⃣ Update
            var updatedDto = new
            {
                Id = id,
                UserName = "updateduser"+Guid.NewGuid(),
                PasswordHash = "newhashedpass",
                Email = "updated@test.com",
                PhoneNumber = "09350000000",
                IsActive = false,
                Roles = new object[] { }
                ,IsTest=true
            };

            var updateResponse = await _client.PutAsJsonAsync($"/api/user/{id}", updatedDto);
            updateResponse.EnsureSuccessStatusCode();

            var updated = await updateResponse.Content.ReadFromJsonAsync<ResultDto<UserDto>>();
            updated.Should().NotBeNull();
            updated!.Data.UserName.Should().Be(updatedDto.UserName);

            // 5️⃣ Delete
            var deleteResponse = await _client.DeleteAsync($"/api/user/{id}");
            deleteResponse.EnsureSuccessStatusCode();

            // 6️⃣ Get after delete
            var getAfterDelete = await _client.GetAsync($"/api/user/{id}");
            getAfterDelete.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }
        [Fact(DisplayName = "GetAllUsers should return paginated results using cursor pagination")]
        public async Task GetAllUsers_ReturnsCursorPaginatedResults()
        {
            // Arrange
            await AuthenticateAsync();
            var builder = new GraphQLQueryBuilder<UserDto>("getAllUsers")
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

            var result = await response.Content.ReadFromJsonAsync<GraphQLResponse<Dictionary<string, GraphQLConnection<UserDto>>>>();
            var rolesConnection = result!.Data["getAllUsers"];

            // Assertions
            rolesConnection.Should().NotBeNull();
            rolesConnection.Nodes.Should().NotBeNullOrEmpty();
            rolesConnection.TotalCount.Should().BeGreaterThan(0);
            rolesConnection.PageInfo.Should().NotBeNull();
            rolesConnection.PageInfo.EndCursor.Should().NotBeNullOrEmpty(); // مثال اضافه
        }
    }
}
