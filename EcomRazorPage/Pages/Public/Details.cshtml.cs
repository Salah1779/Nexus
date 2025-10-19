using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EcomRazorPage.Pages.Public.ViewModels;
using EcomRazorPage.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace EcomRazorPage.Pages.Public
{
    /// <summary>
    /// PageModel for the public-facing article details page.
    /// </summary>
    public class DetailsModel : PageModel
    {
        private readonly ArticlesDBContext _db;
        [BindProperty]
        public ArticleCardViewModel Article { get; set; }

        public DetailsModel(ArticlesDBContext db)
        {
            _db = db;
        }

        public IActionResult OnGet(Guid id)
        {
            var article = _db.Articles.Include(a => a.Category).FirstOrDefault(a => a.Id == id);
            if (article == null)
                return NotFound();
            Article = new ArticleCardViewModel
            {
                Id = article.Id,
                Title = article.Title,
                Description = article.Description,
                Price = article.Price,
                ImageUrl = article.ImageUrl,
                CategoryName = article.Category?.Name,
                StockQuantity = article.StockQuantity
            };
            return Page();
        }
    }
}
