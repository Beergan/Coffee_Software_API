namespace CoffeeShop.Server.Services;

public interface IDatabaseUpdateService
{
    Task UpdateDatabaseAsync();
    Task SeedDataAsync();
    Task<bool> IsDatabaseUpToDateAsync();
    Task<List<string>> GetPendingMigrationsAsync();
}