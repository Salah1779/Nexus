// Data/DbInitializer.cs
// This file contains database initialization and seeding logic for the NexaCart application.
// It creates default roles, users, and sample data for development and testing.

using EcomRazorPage.Models;
using Microsoft.AspNetCore.Identity;
using EcomRazorPage.Data;

namespace EcomRazorPage.Data
{
    /// <summary>
    /// Static class responsible for initializing and seeding the database.
    /// Creates default roles, admin user, and sample data for development.
    /// </summary>
    public static class DbInitializer
    {
        /// <summary>
        /// Initializes the database with default data.
        /// Creates roles, admin user, and sample categories/articles.
        /// </summary>
        /// <param name="context">The database context</param>
        /// <param name="userManager">User manager for creating users</param>
        /// <param name="roleManager">Role manager for creating roles</param>
        public static async Task InitializeAsync(ArticlesDBContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            // Ensure database is created
            context.Database.EnsureCreated();

            // Create default roles
            await CreateRolesAsync(roleManager);

            // Create default admin user
            await CreateAdminUserAsync(userManager);

            // Seed sample data
            await SeedSampleDataAsync(context);
        }

        /// <summary>
        /// Creates the default roles for the application.
        /// </summary>
        /// <param name="roleManager">Role manager instance</param>
        private static async Task CreateRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            string[] roleNames = { "Admin", "Manager", "Customer" };

            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }

        /// <summary>
        /// Creates a default admin user for the application.
        /// </summary>
        /// <param name="userManager">User manager instance</param>
        private static async Task CreateAdminUserAsync(UserManager<ApplicationUser> userManager)
        {
            var adminEmail = "admin@nexacart.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    FirstName = "System",
                    LastName = "Administrator",
                    ShippingAddress = "123 Admin Street, Admin City, AC 12345",
                    BillingAddress = "123 Admin Street, Admin City, AC 12345",
                    PhoneNumber = "+1-555-ADMIN",
                    IsActive = true,
                    PreferredLanguage = "en",
                    PreferredCurrency = "USD"
                };

                var result = await userManager.CreateAsync(adminUser, "Admin@123456");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
        }

        /// <summary>
        /// Seeds the database with sample categories and articles.
        /// </summary>
        /// <param name="context">The database context</param>
        private static async Task SeedSampleDataAsync(ArticlesDBContext context)
        {
            // Only seed if no categories exist
            if (!context.Categories.Any())
            {
                var categories = new List<Category>
                {
                    new Category
                    {
                        Name = "Electronics",
                        Description = "Electronic devices and gadgets",
                        Slug = "electronics",
                        DisplayOrder = 1,
                        IsActive = true
                    },
                    new Category
                    {
                        Name = "Books",
                        Description = "Books and publications",
                        Slug = "books",
                        DisplayOrder = 2,
                        IsActive = true
                    },
                    new Category
                    {
                        Name = "Clothing",
                        Description = "Fashion and apparel",
                        Slug = "clothing",
                        DisplayOrder = 3,
                        IsActive = true
                    },
                    new Category
                    {
                        Name = "Home & Garden",
                        Description = "Home improvement and garden supplies",
                        Slug = "home-garden",
                        DisplayOrder = 4,
                        IsActive = true
                    }
                };

                context.Categories.AddRange(categories);
                await context.SaveChangesAsync();

                // Seed sample articles
                var electronicsCategory = categories.First(c => c.Slug == "electronics");
                var booksCategory = categories.First(c => c.Slug == "books");

                var articles = new List<Article>
                {
                    new Article
                    {
                        Title = "Wireless Bluetooth Headphones",
                        Description = "High-quality wireless headphones with noise cancellation and premium sound quality.",
                        Price = 199.99m,
                        ImageUrl = "/images/products/headphones.jpg",
                        StockQuantity = 50,
                        CategoryId = electronicsCategory.Id,
                        IsActive = true
                    },
                    new Article
                    {
                        Title = "Smart Watch Series X",
                        Description = "Advanced smartwatch with health monitoring, GPS, and long battery life.",
                        Price = 349.99m,
                        ImageUrl = "/images/products/smartwatch.jpg",
                        StockQuantity = 30,
                        CategoryId = electronicsCategory.Id,
                        IsActive = true
                    },
                    new Article
                    {
                        Title = "The Art of Programming",
                        Description = "Comprehensive guide to modern programming practices and techniques.",
                        Price = 49.99m,
                        ImageUrl = "/images/products/programming-book.jpg",
                        StockQuantity = 100,
                        CategoryId = booksCategory.Id,
                        IsActive = true
                    },
                    new Article
                    {
                        Title = "ASP.NET Core in Action",
                        Description = "Learn to build modern web applications with ASP.NET Core and Razor Pages.",
                        Price = 39.99m,
                        ImageUrl = "/images/products/aspnet-book.jpg",
                        StockQuantity = 75,
                        CategoryId = booksCategory.Id,
                        IsActive = true
                    }
                };

                context.Articles.AddRange(articles);
                await context.SaveChangesAsync();
            }
        }
    }
}
