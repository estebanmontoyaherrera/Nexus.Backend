using FluentAssertions;
using Identity.Tests.Fixtures;
using Identity.Tests.TestUtilities;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Identity.Tests.IntegrationTests.API;

public class UserAndAuthorizationIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public UserAndAuthorizationIntegrationTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task UserCreate_WithValidRequest_ReturnsSuccess()
    {
        var email = $"new-user-{Guid.NewGuid():N}@test.com";
        var response = await _client.PostAsync("/api/User/Create", JsonContent.Create(new
        {
            firstName = "New",
            lastName = "User",
            email,
            password = "P@ssw0rd!",
            state = "1"
        }));

        response.IsSuccessStatusCode.Should().BeTrue();
        var payload = await response.Content.ReadAsStringAsync();
        payload.Should().Contain("isSuccess");
    }

    [Fact]
    public async Task UserRoleCreate_WithAuthenticatedUser_AssignsRole()
    {
        var token = await LoginAndGetTokenAsync();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var createResponse = await _client.PostAsync("/api/UserRole/Create", JsonContent.Create(new
        {
            userId = 1,
            roleId = 1,
            state = "1"
        }));

        createResponse.IsSuccessStatusCode.Should().BeTrue();
    }

    [Fact]
    public async Task GetPermissionsByRoleId_WithAuthenticatedRequest_ReturnsPermissions()
    {
        var token = await LoginAndGetTokenAsync();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync("/api/Permission/PermissionByRoleId/1");

        response.IsSuccessStatusCode.Should().BeTrue();
        var payload = await response.Content.ReadAsStringAsync();
        payload.Should().Contain("data");
    }

    private async Task<string> LoginAndGetTokenAsync()
    {
        var response = await _client.PostAsync("/api/Auth/Login", JsonContent.Create(new
        {
            email = "integration@test.com",
            password = "P@ssw0rd!"
        }));
        response.EnsureSuccessStatusCode();

        var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        return json.RootElement.GetProperty("accessToken").GetString()!;
    }
}
