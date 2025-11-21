using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Xliften2.Auth
{
    /// <summary>
    /// Implementation of <see cref="IJwtTokenService"/> producing signed JWTs.
    /// </summary>
    public class JwtTokenService : IJwtTokenService
    {
        private readonly JwtSettings _settings;

        /// <summary>
        /// Creates the service using configured <see cref="JwtSettings"/>.
        /// </summary>
        /// <param name="settings">Options wrapper for JWT configuration.</param>
        public JwtTokenService(IOptions<JwtSettings> settings)
        {
            _settings = settings.Value;
        }

        /// <summary>
        /// Generates a signed JWT containing username and role claims.
        /// </summary>
        /// <param name="username">The username to include in the token subject/claims.</param>
        /// <param name="role">The role to include as a claim (e.g., "Admin").</param>
        /// <returns>A signed JWT compact token string.</returns>
        public string GenerateToken(string username, string role)
        {
            // Define claims placed in the token.
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // Create signing credentials using the configured symmetric key.
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.SigningKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Build the token.
            var token = new JwtSecurityToken(
                issuer: _settings.Issuer,
                audience: _settings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds);

            // Return the compact serialized token.
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
