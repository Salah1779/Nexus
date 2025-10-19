using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcomApp.Pages.Articles.ViewModels
{
    public class ArticleFormModel
    {
        
        public Guid? Id { get; set; }

        [Required]
        [MaxLength(100)]
        [Display(Name = "Title")]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MaxLength(500)]
        [Display(Name = "Description")]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [DataType(DataType.Currency)]
        [Range(0.01, (double)decimal.MaxValue, ErrorMessage = "The price must be greater than zero.")]
        [Display(Name = "Price")]

        public decimal Price { get; set; }

        [MaxLength(200)]
        [Display(Name = "Image URL")]
        public string ImageUrl { get; set; } = string.Empty;
    }
}