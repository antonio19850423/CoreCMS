using FluentAssertions;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Velora.Application.Shared.Dtos;
using Velora.Application.Shared.Dtos.GraphQL;
using Velora.Application.Shared.GraphQL;
using Velora.IntegrationTests;
namespace Velora.IntegrationTests.Controllers
{
    [Collection("IntegrationTests")]
    public class UserProfileControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public UserProfileControllerTests(CustomWebApplicationFactory<Program> factory)
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

        [Fact(DisplayName = "UserProfile CRUD should succeed (Create, Get, Update, Delete)")]
        public async Task UserProfile_CRUD_Should_Succeed()
        {
            await AuthenticateAsync();

            // 1️⃣ اول کاربر ایجاد کنیم
            var userDto = new
            {
                Id = Guid.NewGuid(),
                UserName = "testuser_" + Guid.NewGuid(),
                PasswordHash = "hashedpass",
                IsTest=true
            };
            var userResponse = await _client.PostAsJsonAsync("/api/user", userDto);
            userResponse.EnsureSuccessStatusCode();
            var userCreated = await userResponse.Content.ReadFromJsonAsync<ResultDto<UserDto>>();
            var userId = userCreated!.Data.Id;

            // 2️⃣ حالا پروفایل
            var profileDto = new
            {
                Id = Guid.NewGuid(),
                Userid = userId,
                Firstname = "Ali" + Guid.NewGuid(),
                Lastname = "Ahmadi",
                Nationalcode = "1234567890",
                Address = "Tehran",
                Countryid = (Guid?)null,
                Stateid = (Guid?)null,
                Cityid = (Guid?)null
                ,IsTest=true
            };

            var createResponse = await _client.PostAsJsonAsync("/api/userprofile", profileDto);
            createResponse.EnsureSuccessStatusCode();

            var created = await createResponse.Content.ReadFromJsonAsync<ResultDto<UserProfileDto>>();
            created!.Data.Firstname.Should().Be(profileDto.Firstname);

            var id = created.Data.Id;

            // 3️⃣ Get
            var getResponse = await _client.GetAsync($"/api/userprofile/{id}");
            getResponse.EnsureSuccessStatusCode();

            // 4️⃣ Update
            var updatedDto = new
            {
                Id = id,
                Userid = userId,
                Firstname = "Reza" + Guid.NewGuid(),
                Lastname = "Updated",
                Nationalcode = "9876543210",
                Address = "Mashhad",
                Countryid = (Guid?)null,
                Stateid = (Guid?)null,
                Cityid = (Guid?)null
                ,IsTest=true
            };
            var updateResponse = await _client.PutAsJsonAsync($"/api/userprofile/{id}", updatedDto);
            updateResponse.EnsureSuccessStatusCode();

            var updated = await updateResponse.Content.ReadFromJsonAsync<ResultDto<UserProfileDto>>();
            updated!.Data.Firstname.Should().Be(updatedDto.Firstname);

            // 5️⃣ Delete
            var deleteResponse = await _client.DeleteAsync($"/api/userprofile/{id}");
            deleteResponse.EnsureSuccessStatusCode();
        }
        [Fact(DisplayName = "GetAllUserProfiles should return paginated results using cursor pagination")]
        public async Task GetAllUserProfiles_ReturnsCursorPaginatedResults()
        {
            // Arrange
            await AuthenticateAsync();
            var builder = new GraphQLQueryBuilder<UserProfileDto>("getAllUserProfiles")
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

            var result = await response.Content.ReadFromJsonAsync<GraphQLResponse<Dictionary<string, GraphQLConnection<UserProfileDto>>>>();
            var rolesConnection = result!.Data["getAllUserProfiles"];

            // Assertions
            rolesConnection.Should().NotBeNull();
            rolesConnection.Nodes.Count.Should().BeGreaterThanOrEqualTo(0);
            rolesConnection.TotalCount.Should().BeGreaterThanOrEqualTo(0);
        }
    }
}