using System.Collections.Generic;

namespace EcomRazorPage.Pages.Public.ViewModels
{
    /// <summary>
    /// ViewModel for the public article search and filter page.
    /// </summary>
    public class ArticleSearchViewModel
    {
        public string SearchTerm { get; set; }
        public string SelectedCategory { get; set; }
        public List<ArticleCardViewModel> Articles { get; set; } = new();
        public List<string> Categories { get; set; } = new();
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
