// Models/Category.cs
// This file defines the Category entity model for the NexaCart e-commerce platform.
// Categories are used to organize and group products for better navigation and filtering.

namespace EcomRazorPage.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Represents a product category in the e-commerce catalog.
    /// Categories help organize products and improve user navigation and search.
    /// </summary>
    [Table("Categories")]
    public class Category
    {
        /// <summary>
        /// Unique identifier for the category.
        /// Uses GUID for consistency with other entities in the system.
        /// </summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// The display name of the category.
        /// Required field that users will see in navigation and filters.
        /// </summary>
        [Required(ErrorMessage = "Category name is required.")]
        [MaxLength(50, ErrorMessage = "Category name cannot exceed 50 characters.")]
        [Display(Name = "Category Name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Optional description of what this category contains.
        /// Helps users understand the category's purpose and contents.
        /// </summary>
        [MaxLength(200, ErrorMessage = "Description cannot exceed 200 characters.")]
        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// URL-friendly version of the category name.
        /// Used for SEO-friendly URLs and routing.
        /// </summary>
        [MaxLength(50)]
        [Display(Name = "URL Slug")]
        public string Slug { get; set; } = string.Empty;

        /// <summary>
        /// Display order for sorting categories in navigation.
        /// Lower numbers appear first in lists and menus.
        /// </summary>
        [Display(Name = "Display Order")]
        [Range(0, int.MaxValue, ErrorMessage = "Display order must be a positive number.")]
        public int DisplayOrder { get; set; }

        /// <summary>
        /// Indicates whether the category is currently active and visible.
        /// Allows hiding categories without deleting them.
        /// </summary>
        [Display(Name = "Is Active")]
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Navigation property for articles in this category.
        /// Enables lazy loading of related products.
        /// </summary>
        public virtual ICollection<Article> Articles { get; set; } = new List<Article>();

        /// <summary>
        /// Timestamp for when the category was created.
        /// Automatically set for audit trail purposes.
        /// </summary>
        [Display(Name = "Created Date")]
        [DataType(DataType.DateTime)]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Timestamp for the last modification of the category.
        /// Updated automatically for audit purposes.
        /// </summary>
        [Display(Name = "Last Modified")]
        [DataType(DataType.DateTime)]
        public DateTime LastModifiedDate { get; set; } = DateTime.UtcNow;
    }
}
