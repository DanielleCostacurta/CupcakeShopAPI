using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CupcakeShop.API.Models;

[Table("order_items")]
public class OrderItem
{
    [Key]
    [Column("item_id")]
    public int ItemId { get; set; }

    [Column("order_id")]
    public int OrderId { get; set; }

    [ForeignKey("OrderId")]
    public Order Order { get; set; } = null!;

    [Column("dough_id")]
    public int DoughId { get; set; }

    [ForeignKey("DoughId")]
    public DoughType DoughType { get; set; } = null!;

    [Column("frosting_id")]
    public int FrostingId { get; set; }

    [ForeignKey("FrostingId")]
    public Frosting Frosting { get; set; } = null!;

    [Column("filling_id")]
    public int? FillingId { get; set; }

    [ForeignKey("FillingId")]
    public Filling? Filling { get; set; }

    [Column("quantity")]
    public int Quantity { get; set; } = 1;

    [Column("unit_price")]
    public decimal UnitPrice { get; set; }

    [Column("subtotal")]
    public decimal Subtotal { get; set; }
}
