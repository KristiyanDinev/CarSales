using CarSales.Models.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CarSales.Services
{
    public class JwtService
    {
        private readonly string _issuer;
        private readonly string _audience;
        private readonly SigningCredentials _creds;

        public JwtService(string key, string issuer, string audience)
        {
            _creds = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)), 
                SecurityAlgorithms.HmacSha256);
            _issuer = issuer;
            _audience = audience;
        }

        public string GenerateToken(IdentityUserModel user)
        {
            return new JwtSecurityTokenHandler().WriteToken(
                new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: new[] {
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName!),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.NameIdentifier, user.Id)
                },
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: _creds));
        }
    }
}
