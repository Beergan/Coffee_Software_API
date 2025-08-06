using CoffeeShop.Server.Data;
using CoffeeShop.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace CoffeeShop.Server.Services;

public class ProductService : IProductService
{
    private readonly ApplicationDbContext _context;

    public ProductService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Product>> GetAllProductsAsync()
    {
        return await _context.Products
            .OrderBy(p => p.Category)
            .ThenBy(p => p.Name)
            .ToListAsync();
    }

    public async Task<List<Product>> GetAvailableProductsAsync()
    {
        return await _context.Products
            .Where(p => p.IsAvailable && p.StockQuantity > 0)
            .OrderBy(p => p.Category)
            .ThenBy(p => p.Name)
            .ToListAsync();
    }

    public async Task<List<Product>> GetProductsByCategoryAsync(ProductCategory category)
    {
        return await _context.Products
            .Where(p => p.Category == category)
            .OrderBy(p => p.Name)
            .ToListAsync();
    }

    public async Task<Product?> GetProductByIdAsync(int productId)
    {
        return await _context.Products.FindAsync(productId);
    }

    public async Task<Product> CreateProductAsync(Product product)
    {
        product.CreatedAt = DateTime.UtcNow;
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return product;
    }

    public async Task<Product> UpdateProductAsync(Product product)
    {
        product.UpdatedAt = DateTime.UtcNow;
        _context.Products.Update(product);
        await _context.SaveChangesAsync();
        return product;
    }

    public async Task<bool> DeleteProductAsync(int productId)
    {
        var product = await _context.Products.FindAsync(productId);
        if (product == null)
            return false;

        // Check if product is used in any orders
        var hasOrders = await _context.OrderItems
            .AnyAsync(oi => oi.ProductId == productId);

        if (hasOrders)
        {
            // Don't delete if used in orders, just mark as unavailable
            product.IsAvailable = false;
            product.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
        else
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }

        return true;
    }

    public async Task<bool> UpdateStockAsync(int productId, int newStock)
    {
        var product = await _context.Products.FindAsync(productId);
        if (product == null)
            return false;

        product.StockQuantity = newStock;
        product.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ToggleAvailabilityAsync(int productId)
    {
        var product = await _context.Products.FindAsync(productId);
        if (product == null)
            return false;

        product.IsAvailable = !product.IsAvailable;
        product.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<int> GetLowStockProductsCountAsync(int threshold = 10)
    {
        return await _context.Products
            .CountAsync(p => p.IsAvailable && p.StockQuantity <= threshold);
    }
}