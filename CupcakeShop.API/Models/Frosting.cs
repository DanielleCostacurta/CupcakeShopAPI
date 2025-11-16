using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CupcakeShop.API.Models;

[Table("frostings")]
public class Frosting
{
    [Key]
    [Column("frosting_id")]
    public int FrostingId { get; set; }

    [Required]
    [StringLength(50)]
    [Column("name")]
    public string Name { get; set; } = string.Empty;

    [StringLength(50)]
    [Column("color")]
    public string? Color { get; set; }

    [Column("description")]
    public string? Description { get; set; }

    [Column("price")]
    public decimal Price { get; set; }

    [Column("is_available")]
    public bool IsAvailable { get; set; } = true;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
