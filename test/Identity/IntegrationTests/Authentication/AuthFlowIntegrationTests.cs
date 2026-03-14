using FluentAssertions;
using Identity.Tests.Fixtures;
using Identity.Tests.TestUtilities;

namespace Identity.Tests.IntegrationTests.Authentication;

public class AuthFlowIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public AuthFlowIntegrationTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Login_WithValidCredentials_ReturnsJwtToken()
    {
        var response = await _client.PostAsync("/api/Auth/Login", JsonContent.Create(new
        {
            email = "integration@test.com",
            password = "P@ssw0rd!"
        }));

        response.IsSuccessStatusCode.Should().BeTrue();
        var payload = await response.Content.ReadAsStringAsync();
        payload.Should().Contain("accessToken");
        payload.Should().Contain("refreshToken");
    }

    [Fact]
    public async Task LoginRefreshToken_WithValidRefreshToken_ReturnsRotatedToken()
    {
        var loginResponse = await _client.PostAsync("/api/Auth/Login", JsonContent.Create(new
        {
            email = "integration@test.com",
            password = "P@ssw0rd!"
        }));

        var loginBody = await loginResponse.Content.ReadAsStringAsync();
        loginBody.Should().Contain("refreshToken");

        var refreshToken = System.Text.Json.JsonDocument.Parse(loginBody).RootElement.GetProperty("refreshToken").GetString();

        var refreshResponse = await _client.PostAsync("/api/Auth/LoginRefreshToken", JsonContent.Create(new
        {
            refreshToken
        }));

        refreshResponse.IsSuccessStatusCode.Should().BeTrue();
        var refreshPayload = await refreshResponse.Content.ReadAsStringAsync();
        refreshPayload.Should().Contain("accessToken");
        refreshPayload.Should().Contain("refreshToken");
    }

    [Fact]
    public async Task RevokeRefreshToken_WithExistingUser_ReturnsSuccess()
    {
        var response = await _client.DeleteAsync("/api/Auth/RevokeRefreshToken/1");

        response.IsSuccessStatusCode.Should().BeTrue();
        var payload = await response.Content.ReadAsStringAsync();
        payload.Should().Contain("isSuccess");
    }
}
