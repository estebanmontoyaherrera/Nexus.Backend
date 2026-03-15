using System.Text.Json;
using FluentAssertions;
using Nexus.Identity.SecurityTests.Utilities;
using Xunit;

namespace Nexus.Identity.SecurityTests.SecurityMisconfiguration;

public class SecurityMisconfigurationTests
{
    [Fact]
    public void SecurityMisconfiguration_CorsWildcardOrigin_ShouldBeDetected()
    {
        var programFile = Path.Combine(
            TestPathHelper.RepoRoot(),
            "src", "API", "Api", "Program.cs");

        var source = File.ReadAllText(programFile);

        source.Should().Contain("WithOrigins(\"*\")");
    }

    [Fact]
    public void SecurityMisconfiguration_AppSettings_ContainsHardcodedJwtSecret_ShouldBeDetected()
    {
        var appSettingsFile = Path.Combine(
            TestPathHelper.RepoRoot(),
            "src", "API", "Api", "appsettings.json");

        using var doc = JsonDocument.Parse(File.ReadAllText(appSettingsFile));

        var secret = doc.RootElement
            .GetProperty("JwtSettings")
            .GetProperty("Secret")
            .GetString();

        secret.Should().NotBeNullOrWhiteSpace();
    }
}