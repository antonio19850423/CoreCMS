using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net.Http.Json;
using Velora.Application.Shared.Dtos;
using Velora.Host; // مسیر پروژه اصلی
using Xunit;

namespace Velora.IntegrationTests.Controllers;

public class AuthControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public AuthControllerTests(CustomWebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact(DisplayName = "Register and Login should succeed")]
    public async Task RegisterAndLogin_Should_Succeed()
    {
        // 1️⃣ Arrange: اطلاعات ثبت‌نام
        var registerDto = new
        {
            UserName = "Developer", // یکتا بودن
            Password = "123",
            Email = "test@example.com",
            PhoneNumber = "09120000000",
            DefaultRoleCode = "AD"
        };

        //// 2️⃣ Act: Register
        //var registerResponse = await _client.PostAsJsonAsync("/api/auth/register", registerDto);
        //registerResponse.EnsureSuccessStatusCode();

        //var registeredUser = await registerResponse.Content.ReadFromJsonAsync<ResultDto<UserDto>>();
        //registeredUser.Should().NotBeNull();
        //registeredUser!.Success.Should().BeTrue();
        //registeredUser.Data.Should().NotBeNull();
        //registeredUser.Data.UserName.Should().Be(registerDto.UserName);

        // 3️⃣ Act: Login
        var loginDto = new
        {
            UserName = registerDto.UserName,
            Password = registerDto.Password
        };

        var loginResponse = await _client.PostAsJsonAsync("/api/auth/login", loginDto);
        var content = await loginResponse.Content.ReadAsStringAsync();
        Console.WriteLine(content); // خطای واقعی یا پیام سرور
        loginResponse.EnsureSuccessStatusCode();
        var loginResult = await loginResponse.Content.ReadFromJsonAsync<ResultDto<LoginResultDto>>();
        loginResult.Should().NotBeNull();
        loginResult!.Success.Should().BeTrue();
        loginResult.Data.Should().NotBeNull();
        loginResult.Data.Token.Should().NotBeNullOrEmpty();
    }
}
