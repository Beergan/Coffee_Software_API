using CoffeeShop.Server.Data;
using CoffeeShop.Shared.Models;
using CoffeeShop.Shared.DTOs;
using Microsoft.EntityFrameworkCore;

namespace CoffeeShop.Server.Services;

public class OrderService : IOrderService
{
    private readonly ApplicationDbContext _context;

    public OrderService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Order>> GetAllOrdersAsync()
    {
        return await _context.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .Include(o => o.User)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<Order>> GetOrdersByUserAsync(string userId)
    {
        return await _context.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .Where(o => o.UserId == userId)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync();
    }

    public async Task<Order?> GetOrderByIdAsync(int orderId)
    {
        return await _context.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .Include(o => o.User)
            .FirstOrDefaultAsync(o => o.Id == orderId);
    }

    public async Task<Order> CreateOrderAsync(CreateOrderRequest request, string userId)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var order = new Order
            {
                OrderNumber = await GenerateOrderNumberAsync(),
                UserId = userId,
                CustomerName = request.CustomerName,
                Notes = request.Notes,
                Status = OrderStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            decimal totalAmount = 0;

            foreach (var itemRequest in request.Items)
            {
                var product = await _context.Products.FindAsync(itemRequest.ProductId);
                if (product == null)
                    throw new ArgumentException($"Product with ID {itemRequest.ProductId} not found");

                if (!product.IsAvailable)
                    throw new InvalidOperationException($"Product {product.Name} is not available");

                if (product.StockQuantity < itemRequest.Quantity)
                    throw new InvalidOperationException($"Insufficient stock for {product.Name}. Available: {product.StockQuantity}");

                var orderItem = new OrderItem
                {
                    OrderId = order.Id,
                    ProductId = itemRequest.ProductId,
                    Quantity = itemRequest.Quantity,
                    UnitPrice = product.Price,
                    Notes = itemRequest.Notes
                };

                _context.OrderItems.Add(orderItem);

                // Update stock
                product.StockQuantity -= itemRequest.Quantity;
                totalAmount += orderItem.TotalPrice;
            }

            order.TotalAmount = totalAmount;
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();

            return await GetOrderByIdAsync(order.Id) ?? order;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<Order> UpdateOrderStatusAsync(int orderId, OrderStatus status)
    {
        var order = await _context.Orders.FindAsync(orderId);
        if (order == null)
            throw new ArgumentException($"Order with ID {orderId} not found");

        order.Status = status;
        
        if (status == OrderStatus.Completed)
        {
            order.CompletedAt = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync();
        return await GetOrderByIdAsync(orderId) ?? order;
    }

    public async Task<List<Order>> GetTodaysOrdersAsync()
    {
        var today = DateTime.UtcNow.Date;
        return await _context.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .Where(o => o.CreatedAt.Date == today)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync();
    }

    public async Task<decimal> GetTodaysRevenueAsync()
    {
        var today = DateTime.UtcNow.Date;
        return await _context.Orders
            .Where(o => o.CreatedAt.Date == today && o.Status == OrderStatus.Completed)
            .SumAsync(o => o.TotalAmount);
    }

    public async Task<int> GetPendingOrdersCountAsync()
    {
        return await _context.Orders
            .CountAsync(o => o.Status == OrderStatus.Pending || o.Status == OrderStatus.Preparing);
    }

    public async Task<string> GenerateOrderNumberAsync()
    {
        var today = DateTime.UtcNow.Date;
        var prefix = today.ToString("yyyyMMdd");
        
        var lastOrder = await _context.Orders
            .Where(o => o.OrderNumber.StartsWith(prefix))
            .OrderByDescending(o => o.OrderNumber)
            .FirstOrDefaultAsync();

        int nextNumber = 1;
        if (lastOrder != null)
        {
            var lastNumberStr = lastOrder.OrderNumber.Substring(prefix.Length);
            if (int.TryParse(lastNumberStr, out int lastNumber))
            {
                nextNumber = lastNumber + 1;
            }
        }

        return $"{prefix}{nextNumber:D3}";
    }
}