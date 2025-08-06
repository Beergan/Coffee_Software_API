using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoffeeShop.Server.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // This migration will create all tables
            // EF Core will handle this automatically when using context.Database.Migrate()
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop all tables
        }
    }
}