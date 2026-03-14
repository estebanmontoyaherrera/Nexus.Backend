using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Nexus.Identity.SecurityTests.Utilities;

public static class ApiSecurityFactory
{
    public const string ValidIssuer = "Identity";
    public const string ValidAudience = "Identity";
    public const string ValidSecret = "=guucg=-z_0g%)l7uw-a5#h3-gf%(92e73z(x_rn*-#g11jtvj";

    public static TokenValidationParameters BuildTokenValidationParameters(string? overrideSecret = null)
        => new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ClockSkew = TimeSpan.Zero,
            ValidIssuer = ValidIssuer,
            ValidAudience = ValidAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(overrideSecret ?? ValidSecret))
        };
}
