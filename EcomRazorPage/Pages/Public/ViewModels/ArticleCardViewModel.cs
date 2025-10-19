using System;

namespace EcomRazorPage.Pages.Public.ViewModels
{
    /// <summary>
    /// ViewModel for displaying article cards in the public area.
    /// </summary>
    public class ArticleCardViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public string CategoryName { get; set; }
        public int StockQuantity { get; set; }
    }
}
