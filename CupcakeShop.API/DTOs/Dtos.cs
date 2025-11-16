using System.ComponentModel.DataAnnotations;

namespace CupcakeShop.API.DTOs;

public class LoginDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}

public class RegisterDto
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(100, MinimumLength = 6)]
    public string Password { get; set; } = string.Empty;

    [Phone]
    public string? Phone { get; set; }
}

public class OrderItemDto
{
    public int DoughId { get; set; }
    public int FrostingId { get; set; }
    public int? FillingId { get; set; }
    public int Quantity { get; set; }
}

public class CreateOrderDto
{
    [Required]
    public List<OrderItemDto> Items { get; set; } = new();

    [Required]
    public string DeliveryAddress { get; set; } = string.Empty;

    [Required]
    public string PaymentMethod { get; set; } = string.Empty;
}

public class UpdateStatusDto
{
    [Required]
    public string Status { get; set; } = string.Empty;
}
