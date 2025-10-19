using Microsoft.AspNetCore.Mvc.RazorPages;
using EcomRazorPage.Pages.Public.ViewModels;
using EcomRazorPage.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace EcomRazorPage.Pages.Public
{
    /// <summary>
    /// PageModel for the public-facing article browse/search page.
    /// </summary>
    public class IndexModel : PageModel
    {
        private readonly ArticlesDBContext _db;
        public ArticleSearchViewModel ViewModel { get; set; } = new();

        public IndexModel(ArticlesDBContext db)
        {
            _db = db;
        }

        public void OnGet(string search, string category, int page = 1)
        {
            var query = _db.Articles.Include(a => a.Category).AsQueryable();
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(a => a.Title.Contains(search) || a.Description.Contains(search));
                ViewModel.SearchTerm = search;
            }
            if (!string.IsNullOrWhiteSpace(category))
            {
                query = query.Where(a => a.Category.Name == category);
                ViewModel.SelectedCategory = category;
            }
            ViewModel.Categories = _db.Categories.Select(c => c.Name).ToList();
            int pageSize = 12;
            int totalArticles = query.Count();
            ViewModel.TotalPages = (int)System.Math.Ceiling(totalArticles / (double)pageSize);
            ViewModel.CurrentPage = page;
            var articles = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            ViewModel.Articles = articles.Select(a => new ArticleCardViewModel
            {
                Id = a.Id,
                Title = a.Title,
                Description = a.Description,
                Price = a.Price,
                ImageUrl = a.ImageUrl,
                CategoryName = a.Category?.Name,
                StockQuantity = a.StockQuantity
            }).ToList();
        }
    }
}
