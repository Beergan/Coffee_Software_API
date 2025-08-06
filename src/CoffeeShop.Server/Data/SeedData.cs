using CoffeeShop.Shared.Models;
using Microsoft.AspNetCore.Identity;

namespace CoffeeShop.Server.Data;

public static class SeedData
{
    public static async Task SeedAsync(ApplicationDbContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
    {
        // Seed Roles
        await SeedRolesAsync(roleManager);
        
        // Seed Users
        await SeedUsersAsync(userManager);
        
        // Seed Products
        await SeedProductsAsync(context);
    }

    private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
    {
        var roles = new[] { "Admin", "Manager", "Staff" };
        
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }

    private static async Task SeedUsersAsync(UserManager<User> userManager)
    {
        // Admin User
        if (await userManager.FindByEmailAsync("admin@coffeeshop.com") == null)
        {
            var admin = new User
            {
                UserName = "admin@coffeeshop.com",
                Email = "admin@coffeeshop.com",
                FirstName = "Admin",
                LastName = "User",
                Role = UserRole.Admin,
                CreatedAt = DateTime.UtcNow,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(admin, "Admin123!");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(admin, "Admin");
            }
        }

        // Manager User
        if (await userManager.FindByEmailAsync("manager@coffeeshop.com") == null)
        {
            var manager = new User
            {
                UserName = "manager@coffeeshop.com",
                Email = "manager@coffeeshop.com",
                FirstName = "John",
                LastName = "Manager",
                Role = UserRole.Manager,
                CreatedAt = DateTime.UtcNow,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(manager, "Manager123!");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(manager, "Manager");
            }
        }

        // Staff User
        if (await userManager.FindByEmailAsync("staff@coffeeshop.com") == null)
        {
            var staff = new User
            {
                UserName = "staff@coffeeshop.com",
                Email = "staff@coffeeshop.com",
                FirstName = "Jane",
                LastName = "Staff",
                Role = UserRole.Staff,
                CreatedAt = DateTime.UtcNow,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(staff, "Staff123!");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(staff, "Staff");
            }
        }
    }

    private static async Task SeedProductsAsync(ApplicationDbContext context)
    {
        if (!context.Products.Any())
        {
            var products = new List<Product>
            {
                // Coffee
                new Product
                {
                    Name = "Espresso",
                    Description = "Strong and concentrated coffee shot",
                    Price = 2.50m,
                    Category = ProductCategory.Coffee,
                    IsAvailable = true,
                    StockQuantity = 100,
                    CreatedAt = DateTime.UtcNow
                },
                new Product
                {
                    Name = "Americano",
                    Description = "Espresso with hot water",
                    Price = 3.00m,
                    Category = ProductCategory.Coffee,
                    IsAvailable = true,
                    StockQuantity = 100,
                    CreatedAt = DateTime.UtcNow
                },
                new Product
                {
                    Name = "Latte",
                    Description = "Espresso with steamed milk",
                    Price = 4.50m,
                    Category = ProductCategory.Coffee,
                    IsAvailable = true,
                    StockQuantity = 80,
                    CreatedAt = DateTime.UtcNow
                },
                new Product
                {
                    Name = "Cappuccino",
                    Description = "Espresso with steamed milk and foam",
                    Price = 4.00m,
                    Category = ProductCategory.Coffee,
                    IsAvailable = true,
                    StockQuantity = 75,
                    CreatedAt = DateTime.UtcNow
                },
                new Product
                {
                    Name = "Mocha",
                    Description = "Espresso with chocolate and steamed milk",
                    Price = 5.00m,
                    Category = ProductCategory.Coffee,
                    IsAvailable = true,
                    StockQuantity = 60,
                    CreatedAt = DateTime.UtcNow
                },

                // Tea
                new Product
                {
                    Name = "Green Tea",
                    Description = "Premium green tea leaves",
                    Price = 2.75m,
                    Category = ProductCategory.Tea,
                    IsAvailable = true,
                    StockQuantity = 50,
                    CreatedAt = DateTime.UtcNow
                },
                new Product
                {
                    Name = "Earl Grey",
                    Description = "Classic black tea with bergamot",
                    Price = 3.00m,
                    Category = ProductCategory.Tea,
                    IsAvailable = true,
                    StockQuantity = 45,
                    CreatedAt = DateTime.UtcNow
                },
                new Product
                {
                    Name = "Chamomile Tea",
                    Description = "Relaxing herbal tea",
                    Price = 2.75m,
                    Category = ProductCategory.Tea,
                    IsAvailable = true,
                    StockQuantity = 40,
                    CreatedAt = DateTime.UtcNow
                },

                // Pastries
                new Product
                {
                    Name = "Croissant",
                    Description = "Buttery, flaky pastry",
                    Price = 3.50m,
                    Category = ProductCategory.Pastry,
                    IsAvailable = true,
                    StockQuantity = 25,
                    CreatedAt = DateTime.UtcNow
                },
                new Product
                {
                    Name = "Chocolate Muffin",
                    Description = "Rich chocolate chip muffin",
                    Price = 4.00m,
                    Category = ProductCategory.Pastry,
                    IsAvailable = true,
                    StockQuantity = 20,
                    CreatedAt = DateTime.UtcNow
                },
                new Product
                {
                    Name = "Blueberry Scone",
                    Description = "Fresh blueberry scone",
                    Price = 3.75m,
                    Category = ProductCategory.Pastry,
                    IsAvailable = true,
                    StockQuantity = 18,
                    CreatedAt = DateTime.UtcNow
                },

                // Sandwiches
                new Product
                {
                    Name = "Ham & Cheese Sandwich",
                    Description = "Classic ham and cheese on fresh bread",
                    Price = 6.50m,
                    Category = ProductCategory.Sandwich,
                    IsAvailable = true,
                    StockQuantity = 15,
                    CreatedAt = DateTime.UtcNow
                },
                new Product
                {
                    Name = "Turkey Club",
                    Description = "Turkey, bacon, lettuce, tomato",
                    Price = 7.50m,
                    Category = ProductCategory.Sandwich,
                    IsAvailable = true,
                    StockQuantity = 12,
                    CreatedAt = DateTime.UtcNow
                },

                // Snacks
                new Product
                {
                    Name = "Granola Bar",
                    Description = "Healthy oats and nuts bar",
                    Price = 2.50m,
                    Category = ProductCategory.Snack,
                    IsAvailable = true,
                    StockQuantity = 30,
                    CreatedAt = DateTime.UtcNow
                },
                new Product
                {
                    Name = "Cookies (3 pack)",
                    Description = "Fresh baked chocolate chip cookies",
                    Price = 4.25m,
                    Category = ProductCategory.Snack,
                    IsAvailable = true,
                    StockQuantity = 25,
                    CreatedAt = DateTime.UtcNow
                }
            };

            context.Products.AddRange(products);
            await context.SaveChangesAsync();
        }
    }
}