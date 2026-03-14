using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace Identity.Tests.TestUtilities;

public static class JsonContent
{
    private static readonly JsonSerializerOptions Options = new(JsonSerializerDefaults.Web);

    public static StringContent Create(object payload) =>
        new(JsonSerializer.Serialize(payload, Options), Encoding.UTF8, "application/json");

    public static async Task<T?> ReadAsAsync<T>(HttpResponseMessage response)
        => await response.Content.ReadFromJsonAsync<T>(Options);
}
