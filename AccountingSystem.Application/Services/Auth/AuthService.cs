using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using AccountingSystem.Domain.Entities;
using System.Security.Cryptography;
using AccountingSystem.Infrastructure.Persistence;
using AccountingSystem.Application.Models;
using Microsoft.EntityFrameworkCore;

namespace AccountingSystem.Application.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _config;
        private readonly ApplicationDbContext _context;

        public AuthService(IConfiguration config, ApplicationDbContext context)
        {
            _config = config;
            _context = context;

        }

        public TokenResponse GenerateTokens(User user)
        {
            var accessToken = GenerateJwtToken(user);
            var refreshToken = GenerateRefreshToken();

            // Save refresh token to database
            var userRefreshToken = new RefreshToken
            {
                Token = refreshToken,
                UserId = user.Id,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                CreatedAt = DateTime.UtcNow
            };

            _context.RefreshToken.Add(userRefreshToken);
            _context.SaveChanges();

            return new TokenResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddHours(2)
            };
        }

        public string GenerateJwtToken(User user)
        {
            var keyBytes = Encoding.UTF8.GetBytes(_config["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key is not configured"));
            if (keyBytes.Length < 32)
            {
                Array.Resize(ref keyBytes, 32);
            }

            var key = new SymmetricSecurityKey(keyBytes);
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim("TenantId", user.TenantId.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Name, user.FullName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email)
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }

        public async Task<TokenResponse> RefreshToken(string refreshToken)
        {
            var storedToken = await _context.RefreshToken
                .Include(rt => rt.User)
                .FirstOrDefaultAsync(rt => rt.Token == refreshToken && !rt.IsRevoked);

            if (storedToken == null || storedToken.ExpiresAt < DateTime.UtcNow)
            {
                throw new Exception("Invalid or expired refresh token");
            }

            var newTokenResponse = GenerateTokens(storedToken.User);

            // Revoke old refresh token
            storedToken.IsRevoked = true;
            await _context.SaveChangesAsync();

            return newTokenResponse;
        }

        public async Task RevokeToken(string refreshToken)
        {
            var storedToken = await _context.RefreshToken.FirstOrDefaultAsync(rt => rt.Token == refreshToken);
            if (storedToken != null)
            {
                storedToken.IsRevoked = true;
                await _context.SaveChangesAsync();
            }
        }

    }
}
