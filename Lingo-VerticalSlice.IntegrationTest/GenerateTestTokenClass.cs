using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ISystemClock = Microsoft.Extensions.Internal.ISystemClock;

namespace Lingo_VerticalSlice.IntegrationTest;

public class GenerateTestTokenClass 
{
    public string GenerateTestToken()
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("DFDGERsjsfjepoeoe@@#$$@$@123112sdaaadasQEWw"));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, "test-user"),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Name, "TestUser"),
            new Claim(ClaimTypes.NameIdentifier, "3"),
            new Claim(ClaimTypes.Role, "Admin")
        };
        var token = new JwtSecurityToken(
            issuer: "TestApi",
            audience: "TestApp",
            claims: claims,
            expires: DateTime.UtcNow.AddSeconds(10),
            signingCredentials: credentials
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    
    }
}

