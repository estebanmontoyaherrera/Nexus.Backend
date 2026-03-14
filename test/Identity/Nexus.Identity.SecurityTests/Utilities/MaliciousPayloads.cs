namespace Nexus.Identity.SecurityTests.Utilities;

public static class MaliciousPayloads
{
    public const string SqlInjectionClassic = "' OR 1=1 --";
    public const string SqlInjectionStacked = "\"; DROP TABLE Users;";
    public const string XssScriptTag = "<script>alert('xss')</script>";
    public const string JwtTamperedSegment = "tampered";
}
