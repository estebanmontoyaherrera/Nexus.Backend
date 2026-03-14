using System.Net.Http.Headers;
using System.Text.Json;
using Identity.Tests.TestUtilities;

namespace Identity.Tests.Fixtures;

public sealed class ApiTestFixture : IClassFixture<CustomWebApplicationFactory>
{
    public ApiTestFixture(CustomWebApplicationFactory factory)
    {
        Factory = factory;
        Client = factory.CreateClient();
    }

    public CustomWebApplicationFactory Factory { get; }
    public HttpClient Client { get; }

    public async Task<string> GetAccessTokenAsync(string email = "integration@test.com", string password = "P@ssw0rd!")
    {
        var response = await Client.PostAsync("/api/Auth/Login", JsonContent.Create(new { email, password }));
        response.EnsureSuccessStatusCode();

        using var document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        var token = document.RootElement.GetProperty("accessToken").GetString();
        return token!;
    }

    public async Task AuthenticateClientAsync()
    {
        var token = await GetAccessTokenAsync();
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }
}
