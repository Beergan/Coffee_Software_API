using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoffeeShop.Shared.Models;

public class Product
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Description { get; set; }

    [Required]
    [Column(TypeName = "decimal(10,2)")]
    public decimal Price { get; set; }

    [Required]
    public ProductCategory Category { get; set; }

    [StringLength(200)]
    public string? ImageUrl { get; set; }

    public bool IsAvailable { get; set; } = true;

    [Required]
    public int StockQuantity { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Navigation properties
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}

public enum ProductCategory
{
    Coffee = 0,
    Tea = 1,
    Pastry = 2,
    Sandwich = 3,
    Snack = 4,
    Other = 5
}