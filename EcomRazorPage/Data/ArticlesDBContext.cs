﻿// Data/ArticlesDBContext.cs
// This file defines the Entity Framework Core database context for the NexaCart application.
// The context manages database connections, entity configurations, and provides access to database sets.

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using EcomRazorPage.Models;

namespace EcomRazorPage.Data
{
    /// <summary>
    /// Database context for the NexaCart e-commerce application.
    /// Inherits from IdentityDbContext to include ASP.NET Core Identity tables and functionality.
    /// Manages all entity relationships and database operations.
    /// </summary>
    public class ArticlesDBContext : IdentityDbContext<ApplicationUser>
    {
        /// <summary>
        /// Constructor that accepts database context options.
        /// Options typically include connection string and database provider configuration.
        /// </summary>
        /// <param name="options">Database context configuration options</param>
        public ArticlesDBContext(DbContextOptions<ArticlesDBContext> options)
           : base(options) { }

        /// <summary>
        /// DbSet for Article entities.
        /// Provides access to article/product data in the database.
        /// Enables CRUD operations on articles through Entity Framework.
        /// </summary>
        public DbSet<Article> Articles { get; set; }

        /// <summary>
        /// DbSet for Category entities.
        /// Provides access to product category data in the database.
        /// Enables organization and filtering of products by category.
        /// </summary>
        public DbSet<Category> Categories { get; set; }

        /// <summary>
        /// Configures entity relationships and database schema.
        /// Called by Entity Framework during model creation.
        /// Override to customize entity configurations and relationships.
        /// </summary>
        /// <param name="modelBuilder">The model builder used to configure entities</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Call base method to ensure Identity tables are configured
            base.OnModelCreating(modelBuilder);

            // Configure Article entity relationships
            modelBuilder.Entity<Article>()
                .HasOne(a => a.Category)           // Each article has one category
                .WithMany(c => c.Articles)         // Each category can have many articles
                .HasForeignKey(a => a.CategoryId)  // Foreign key property
                .OnDelete(DeleteBehavior.SetNull); // Set category to null if deleted

            // Configure default values and constraints
            modelBuilder.Entity<Article>()
                .Property(a => a.CreatedDate)
                .HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<Article>()
                .Property(a => a.LastModifiedDate)
                .HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<Category>()
                .Property(c => c.CreatedDate)
                .HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<Category>()
                .Property(c => c.LastModifiedDate)
                .HasDefaultValueSql("GETUTCDATE()");

            // Create indexes for better query performance
            modelBuilder.Entity<Article>()
                .HasIndex(a => a.Title);

            modelBuilder.Entity<Article>()
                .HasIndex(a => a.CategoryId);

            modelBuilder.Entity<Article>()
                .HasIndex(a => a.IsActive);

            modelBuilder.Entity<Category>()
                .HasIndex(c => c.Name)
                .IsUnique(); // Category names should be unique

            modelBuilder.Entity<Category>()
                .HasIndex(c => c.Slug)
                .IsUnique(); // URL slugs should be unique
        }
    }
}
