using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using CupcakeShop.API.Data;
using CupcakeShop.API.DTOs;
using CupcakeShop.API.Models;

namespace CupcakeShop.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OrdersController : ControllerBase
{
    private readonly CupcakeDbContext _context;

    public OrdersController(CupcakeDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto orderDto)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var order = new Order
        {
            UserId = userId,
            OrderDate = DateTime.UtcNow,
            Status = "Pendente",
            DeliveryAddress = orderDto.DeliveryAddress,
            PaymentMethod = orderDto.PaymentMethod
        };

        decimal totalAmount = 0;

        foreach (var item in orderDto.Items)
        {
            var dough = await _context.DoughTypes.FindAsync(item.DoughId);
            var frosting = await _context.Frostings.FindAsync(item.FrostingId);
            
            if (dough == null || frosting == null)
                return BadRequest(new { message = "Produto não encontrado" });

            decimal unitPrice = dough.Price + frosting.Price;

            if (item.FillingId.HasValue)
            {
                var filling = await _context.Fillings.FindAsync(item.FillingId.Value);
                if (filling != null)
                    unitPrice += filling.Price;
            }

            var orderItem = new OrderItem
            {
                DoughId = item.DoughId,
                FrostingId = item.FrostingId,
                FillingId = item.FillingId,
                Quantity = item.Quantity,
                UnitPrice = unitPrice,
                Subtotal = unitPrice * item.Quantity
            };

            totalAmount += orderItem.Subtotal;
            order.OrderItems.Add(orderItem);
        }

        order.TotalAmount = totalAmount;
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        return Ok(new { orderId = order.OrderId, message = "Pedido criado com sucesso" });
    }

    [HttpGet]
    public async Task<IActionResult> GetMyOrders()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var orders = await _context.Orders
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.DoughType)
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Frosting)
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Filling)
            .Where(o => o.UserId == userId)
            .OrderByDescending(o => o.OrderDate)
            .Select(o => new
            {
                o.OrderId,
                o.UserId,
                o.OrderDate,
                o.TotalAmount,
                o.Status,
                o.DeliveryAddress,
                o.PaymentMethod,
                o.UpdatedAt,
                OrderItems = o.OrderItems.Select(oi => new
                {
                    oi.ItemId,
                    oi.Quantity,
                    oi.UnitPrice,
                    oi.Subtotal,
                    DoughType = new { oi.DoughType.DoughId, oi.DoughType.Name, oi.DoughType.Price },
                    Frosting = new { oi.Frosting.FrostingId, oi.Frosting.Name, oi.Frosting.Color, oi.Frosting.Description, oi.Frosting.Price },
                    Filling = oi.Filling != null ? new { oi.Filling.FillingId, oi.Filling.Name, oi.Filling.Price } : null
                })
            })
            .ToListAsync();

        return Ok(orders);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrder(int id)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var order = await _context.Orders
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.DoughType)
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Frosting)
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Filling)
            .Where(o => o.OrderId == id && o.UserId == userId)
            .Select(o => new
            {
                o.OrderId,
                o.UserId,
                o.OrderDate,
                o.TotalAmount,
                o.Status,
                o.DeliveryAddress,
                o.PaymentMethod,
                o.UpdatedAt,
                OrderItems = o.OrderItems.Select(oi => new
                {
                    oi.ItemId,
                    oi.Quantity,
                    oi.UnitPrice,
                    oi.Subtotal,
                    DoughType = new { oi.DoughType.DoughId, oi.DoughType.Name, oi.DoughType.Price },
                    Frosting = new { oi.Frosting.FrostingId, oi.Frosting.Name, oi.Frosting.Color, oi.Frosting.Description, oi.Frosting.Price },
                    Filling = oi.Filling != null ? new { oi.Filling.FillingId, oi.Filling.Name, oi.Filling.Price } : null
                })
            })
            .FirstOrDefaultAsync();

        if (order == null)
            return NotFound(new { message = "Pedido não encontrado" });

        return Ok(order);
    }

    [HttpPatch("{id}/status")]
    public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] UpdateStatusDto dto)
    {
        var order = await _context.Orders.FindAsync(id);
        if (order == null)
            return NotFound(new { message = "Pedido não encontrado" });

        order.Status = dto.Status;
        order.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return Ok(new { message = "Status atualizado com sucesso" });
    }
}
