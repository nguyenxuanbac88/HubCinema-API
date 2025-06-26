using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace API_Project.Helpers
{
    public class JwtTokenGenerator
    {
        private readonly IConfiguration _configuration;

        public JwtTokenGenerator(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(string username, string role = "User")
        {
            var jwtSection = _configuration.GetSection("Jwt");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection["Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var issuedAt = DateTime.UtcNow;
            var expiresInMinutes = int.TryParse(jwtSection["ExpiresInMinutes"], out var exp) ? exp : 60;

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role),
                new Claim(JwtRegisteredClaimNames.Iat,
                    new DateTimeOffset(issuedAt).ToUnixTimeSeconds().ToString(),
                    ClaimValueTypes.Integer64)
            };

            var token = new JwtSecurityToken(
                issuer: jwtSection["Issuer"],
                audience: jwtSection["Audience"],
                claims: claims,
                notBefore: issuedAt,
                expires: issuedAt.AddMinutes(expiresInMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public IDictionary<string, string> ValidateOtpToken(string token)
        {
            var jwtSection = _configuration.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection["Key"]));

            try
            {
                JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
                var tokenHandler = new JwtSecurityTokenHandler();

                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                return principal.Claims.ToDictionary(c => c.Type, c => c.Value);
            }
            catch
            {
                return null; // Token hết hạn hoặc không hợp lệ
            }
        }
        public string GenerateOtpToken(string username, string otp,string purpose = "reset-password",int expiresInMinutes = 15,IDictionary<string, string> additionalClaims = null)
        {
            var jwtSection = _configuration.GetSection("Jwt");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection["Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var issuedAt = DateTime.UtcNow;

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim("otp", otp),
                new Claim("purpose", purpose),
                new Claim(JwtRegisteredClaimNames.Iat,
                    new DateTimeOffset(issuedAt).ToUnixTimeSeconds().ToString(),
                    ClaimValueTypes.Integer64)
            };

            if (additionalClaims != null)
            {
                foreach (var pair in additionalClaims)
                {
                    claims.Add(new Claim(pair.Key, pair.Value));
                }
            }

            var token = new JwtSecurityToken(
                issuer: jwtSection["Issuer"],
                audience: jwtSection["Audience"],
                claims: claims,
                notBefore: issuedAt,
                expires: issuedAt.AddMinutes(expiresInMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }

}
