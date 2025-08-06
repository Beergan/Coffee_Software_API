using CoffeeShop.Server.Data;
using CoffeeShop.Shared.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CoffeeShop.Server.Services;

public class DatabaseUpdateService : IDatabaseUpdateService
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ILogger<DatabaseUpdateService> _logger;

    public DatabaseUpdateService(
        ApplicationDbContext context,
        UserManager<User> userManager,
        RoleManager<IdentityRole> roleManager,
        ILogger<DatabaseUpdateService> logger)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
        _logger = logger;
    }

    public async Task UpdateDatabaseAsync()
    {
        try
        {
            _logger.LogInformation("Checking for pending migrations...");
            
            var pendingMigrations = await GetPendingMigrationsAsync();
            if (pendingMigrations.Any())
            {
                _logger.LogInformation($"Applying {pendingMigrations.Count} pending migrations...");
                await _context.Database.MigrateAsync();
                _logger.LogInformation("Database migration completed successfully.");
            }
            else
            {
                _logger.LogInformation("Database is up to date.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating database");
            throw;
        }
    }

    public async Task SeedDataAsync()
    {
        try
        {
            _logger.LogInformation("Seeding initial data...");
            await SeedData.SeedAsync(_context, _userManager, _roleManager);
            _logger.LogInformation("Data seeding completed successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error seeding data");
            throw;
        }
    }

    public async Task<bool> IsDatabaseUpToDateAsync()
    {
        var pendingMigrations = await GetPendingMigrationsAsync();
        return !pendingMigrations.Any();
    }

    public async Task<List<string>> GetPendingMigrationsAsync()
    {
        var pendingMigrations = await _context.Database.GetPendingMigrationsAsync();
        return pendingMigrations.ToList();
    }

    /// <summary>
    /// Force update database schema - Use with caution in production
    /// </summary>
    public async Task ForceUpdateSchemaAsync()
    {
        try
        {
            _logger.LogWarning("Force updating database schema...");
            
            // Delete existing database
            await _context.Database.EnsureDeletedAsync();
            
            // Recreate with current schema
            await _context.Database.EnsureCreatedAsync();
            
            // Reseed data
            await SeedDataAsync();
            
            _logger.LogInformation("Force schema update completed.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during force schema update");
            throw;
        }
    }
}