using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AccountingSystem.Infrastructure.Persistence;
using AccountingSystem.Domain.Entities;
using AccountingSystem.Application.Services.Auth;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using AccountingSystem.Application.Models;

namespace AccountingSystem.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ApplicationDbContext _context;

        public AuthController(IAuthService authService, ApplicationDbContext context)
        {
            _authService = authService;
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (await _context.Users.AnyAsync(u => u.Email == request.Email))
            {
                return BadRequest("Email already registered");
            }

            var user = new User
            {
                FullName = request.FullName,
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                TenantId = 1 // You might want to handle this differently
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var tokenResponse = _authService.GenerateTokens(user);
            return Ok(tokenResponse);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return Unauthorized("Invalid credentials");
            }

            var tokenResponse = _authService.GenerateTokens(user);
            return Ok(tokenResponse);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            try
            {
                var tokenResponse = await _authService.RefreshToken(request.RefreshToken);
                return Ok(tokenResponse);
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var user = await _context.Users
                .Select(u => new
                {
                    u.Id,
                    u.FullName,
                    u.Email,
                    u.TenantId
                })
                .FirstOrDefaultAsync(u => u.Email == userId);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            if (!string.IsNullOrEmpty(refreshToken))
            {
                await _authService.RevokeToken(refreshToken);
            }

            Response.Cookies.Delete("refreshToken");
            return Ok(new { message = "Logged out successfully" });
        }
    }
}