using CoffeeShop.Shared.Models;
using CoffeeShop.Shared.DTOs;

namespace CoffeeShop.Server.Services;

public interface IProductService
{
    Task<List<Product>> GetAllProductsAsync();
    Task<List<Product>> GetAvailableProductsAsync();
    Task<List<Product>> GetProductsByCategoryAsync(ProductCategory category);
    Task<Product?> GetProductByIdAsync(int productId);
    Task<Product> CreateProductAsync(Product product);
    Task<Product> UpdateProductAsync(Product product);
    Task<bool> DeleteProductAsync(int productId);
    Task<bool> UpdateStockAsync(int productId, int newStock);
    Task<bool> ToggleAvailabilityAsync(int productId);
    Task<int> GetLowStockProductsCountAsync(int threshold = 10);
}