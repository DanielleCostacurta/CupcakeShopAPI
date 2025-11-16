using CupcakeShop.API.DTOs;
using CupcakeShop.API.Models;

namespace CupcakeShop.API.Services;

public interface IAuthService
{
    Task<(bool Success, string Token, User? User)> LoginAsync(LoginDto loginDto);
    Task<(bool Success, string Message, User? User)> RegisterAsync(RegisterDto registerDto);
    string GenerateJwtToken(User user);
}
