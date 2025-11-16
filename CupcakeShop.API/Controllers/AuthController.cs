using Microsoft.AspNetCore.Mvc;
using CupcakeShop.API.DTOs;
using CupcakeShop.API.Services;

namespace CupcakeShop.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var (success, token, user) = await _authService.LoginAsync(loginDto);

        if (!success)
        {
            return Unauthorized(new { message = "Email ou senha inválidos" });
        }

        return Ok(new
        {
            token,
            user = new
            {
                user!.UserId,
                user.Name,
                user.Email,
                user.Phone
            }
        });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        var (success, message, user) = await _authService.RegisterAsync(registerDto);

        if (!success)
        {
            return BadRequest(new { message });
        }

        var token = _authService.GenerateJwtToken(user!);

        return Ok(new
        {
            message,
            token,
            user = new
            {
                user!.UserId,
                user.Name,
                user.Email,
                user.Phone
            }
        });
    }
}
