using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MultiTenant.ProjectManagement.API.Models;

namespace MultiTenant.ProjectManagement.API.Helpers
{
    public class JwtTokenGenerator
    {
        private readonly JwtSettings _jwtSettings;
        public JwtTokenGenerator(IOptions<JwtSettings> jwtOptions)
        {
            _jwtSettings = jwtOptions.Value;
        }

        public (string token, DateTime expiresAt) GenerateToken(User user)
        {
            var claims = new[]
            {
             new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
             new Claim(ClaimTypes.Email, user.Email),
             new Claim("tenantId", user.TenantId.ToString()),
             new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes);

            var token = new JwtSecurityToken(
                            issuer:_jwtSettings.Issuer,
                            audience:_jwtSettings.Audience,
                            claims: claims,
                            expires: expires,
                            signingCredentials: creds
                            );

            return(
                new JwtSecurityTokenHandler().WriteToken(token),
                expires
            );

        } 
    }
}
