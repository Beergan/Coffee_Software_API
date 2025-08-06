using System.ComponentModel.DataAnnotations;

namespace CoffeeShop.Shared.DTOs;

public class CreateOrderRequest
{
    [StringLength(100)]
    public string? CustomerName { get; set; }

    [StringLength(500)]
    public string? Notes { get; set; }

    [Required]
    public List<CreateOrderItemRequest> Items { get; set; } = new();
}

public class CreateOrderItemRequest
{
    [Required]
    public int ProductId { get; set; }

    [Required]
    [Range(1, 100)]
    public int Quantity { get; set; }

    [StringLength(200)]
    public string? Notes { get; set; }
}