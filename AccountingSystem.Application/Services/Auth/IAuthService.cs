using AccountingSystem.Application.Models;
using AccountingSystem.Domain.Entities;

namespace AccountingSystem.Application.Services.Auth
{
    public interface IAuthService
    {
        TokenResponse GenerateTokens(User user);
        Task<TokenResponse> RefreshToken(string refreshToken);
        Task RevokeToken(string refreshToken);
    }
}
