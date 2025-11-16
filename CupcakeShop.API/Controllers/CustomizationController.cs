using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CupcakeShop.API.Data;

namespace CupcakeShop.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CustomizationController : ControllerBase
{
    private readonly CupcakeDbContext _context;

    public CustomizationController(CupcakeDbContext context)
    {
        _context = context;
    }

    [HttpGet("doughs")]
    public async Task<IActionResult> GetDoughs()
    {
        var doughs = await _context.DoughTypes
            .Where(d => d.IsAvailable)
            .ToListAsync();
        return Ok(doughs);
    }

    [HttpGet("frostings")]
    public async Task<IActionResult> GetFrostings()
    {
        var frostings = await _context.Frostings
            .Where(f => f.IsAvailable)
            .ToListAsync();
        return Ok(frostings);
    }

    [HttpGet("fillings")]
    public async Task<IActionResult> GetFillings()
    {
        var fillings = await _context.Fillings
            .Where(f => f.IsAvailable)
            .ToListAsync();
        return Ok(fillings);
    }
}
