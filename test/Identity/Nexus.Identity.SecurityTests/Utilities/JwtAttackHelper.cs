using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Nexus.Identity.SecurityTests.Utilities;

public static class JwtAttackHelper
{
    public static string BuildToken(string secret, string issuer, string audience, DateTime expiresUtc)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer,
            audience,
            claims: [new Claim(JwtRegisteredClaimNames.Sub, "1"), new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())],
            expires: expiresUtc,
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public static string TamperPayload(string jwt)
    {
        var parts = jwt.Split('.');
        parts[1] = Base64UrlEncoder.Encode("{\"sub\":\"999\",\"role\":\"admin\"}");
        return string.Join('.', parts);
    }
}
