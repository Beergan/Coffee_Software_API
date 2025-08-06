using CoffeeShop.Shared.Models;
using CoffeeShop.Shared.DTOs;

namespace CoffeeShop.Server.Services;

public interface IOrderService
{
    Task<List<Order>> GetAllOrdersAsync();
    Task<List<Order>> GetOrdersByUserAsync(string userId);
    Task<Order?> GetOrderByIdAsync(int orderId);
    Task<Order> CreateOrderAsync(CreateOrderRequest request, string userId);
    Task<Order> UpdateOrderStatusAsync(int orderId, OrderStatus status);
    Task<List<Order>> GetTodaysOrdersAsync();
    Task<decimal> GetTodaysRevenueAsync();
    Task<int> GetPendingOrdersCountAsync();
    Task<string> GenerateOrderNumberAsync();
}