using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using EcomRazorPage.Data;
using EcomRazorPage.Models;

namespace EcomRazorPage.Pages.Admin
{
    public class IndexModel : PageModel
    {
        private readonly ArticlesDBContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public IndexModel(ArticlesDBContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public int TotalArticles { get; set; }
        public int TotalCategories { get; set; }
        public List<Article> LowStockArticles { get; set; } = new();
        public decimal TotalValue { get; set; }
        public List<Category> Categories { get; set; } = new();

        public async Task OnGetAsync()
        {
            TotalArticles = await _context.Articles.CountAsync();
            TotalCategories = await _context.Categories.CountAsync();

            LowStockArticles = await _context.Articles
                .Where(a => a.StockQuantity < 10)
                .Include(a => a.Category)
                .OrderBy(a => a.StockQuantity)
                .Take(5)
                .ToListAsync();

            TotalValue = await _context.Articles.SumAsync(a => a.Price * a.StockQuantity);

            Categories = await _context.Categories
                .OrderBy(c => c.DisplayOrder)
                .ToListAsync();
        }
    }
}