﻿// Models/Article.cs
// This file defines the Article entity model for the NexaCart e-commerce platform.
// The Article represents a product in the catalog with pricing and image information.

namespace EcomRazorPage.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Represents a product/article in the e-commerce catalog.
    /// This entity stores product information including title, description, pricing, and media.
    /// </summary>
    [Table("Articles")]
    public class Article
    {
        /// <summary>
        /// Unique identifier for the article/product.
        /// Uses GUID to ensure global uniqueness across distributed systems.
        /// </summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// The title/name of the article.
        /// Required field with maximum length constraint for database optimization.
        /// </summary>
        [Required(ErrorMessage = "Article title is required.")]
        [MaxLength(100, ErrorMessage = "Title cannot exceed 100 characters.")]
        [Display(Name = "Product Title")]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Detailed description of the article/product.
        /// Required field with length constraints for SEO and display purposes.
        /// </summary>
        [Required(ErrorMessage = "Article description is required.")]
        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        [Display(Name = "Product Description")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// The selling price of the article.
        /// Stored as decimal with high precision for accurate financial calculations.
        /// </summary>
        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, 999999.99, ErrorMessage = "Price must be between 0.01 and 999,999.99.")]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Price")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        /// <summary>
        /// URL path to the article's image.
        /// Optional field that can store relative or absolute image paths.
        /// </summary>
        [MaxLength(200, ErrorMessage = "Image URL cannot exceed 200 characters.")]
        [Display(Name = "Product Image")]
        [DataType(DataType.ImageUrl)]
        public string ImageUrl { get; set; } = string.Empty;

        /// <summary>
        /// Quantity available in stock.
        /// Added for e-commerce functionality to track inventory levels.
        /// </summary>
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Stock quantity cannot be negative.")]
        [Display(Name = "Stock Quantity")]
        public int StockQuantity { get; set; }

        /// <summary>
        /// Foreign key reference to the product category.
        /// Enables categorization and filtering of products.
        /// </summary>
        [Display(Name = "Category")]
        public Guid? CategoryId { get; set; }

        /// <summary>
        /// Navigation property to the associated category.
        /// Enables lazy loading of category information when needed.
        /// </summary>
        [ForeignKey("CategoryId")]
        public virtual Category? Category { get; set; }

        /// <summary>
        /// Timestamp for when the article was created.
        /// Automatically set during creation for audit purposes.
        /// </summary>
        [Display(Name = "Created Date")]
        [DataType(DataType.DateTime)]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Timestamp for the last modification of the article.
        /// Updated automatically on changes for audit trail.
        /// </summary>
        [Display(Name = "Last Modified")]
        [DataType(DataType.DateTime)]
        public DateTime LastModifiedDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Indicates whether the article is currently active/available for purchase.
        /// Allows soft deletion and temporary product deactivation.
        /// </summary>
        [Display(Name = "Is Active")]
        public bool IsActive { get; set; } = true;
    }
}
