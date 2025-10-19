using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using EcomRazorPage.Data;
using EcomRazorPage.Models;

namespace EcomRazorPage.Pages.Admin.Articles
{
    public class IndexModel : AdminPageModel
    {
        private readonly ArticlesDBContext _context;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ArticlesDBContext context, ILogger<IndexModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IList<Article> Articles { get; set; } = new List<Article>();

        [BindProperty(SupportsGet = true)]
        public string? Search { get; set; }

        [BindProperty(SupportsGet = true)]
        public int Page { get; set; } = 1;

        public int PageSize { get; } = 10;
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }

        public async Task OnGetAsync()
        {
            await LoadArticlesAsync(Page, Search);
        }

        // AJAX handler that returns JSON data for the table
        public async Task<JsonResult> OnGetDataAsync(string? search, int page = 1)
        {
            try
            {
                var query = _context.Articles.Include(a => a.Category).AsQueryable();
                if (!string.IsNullOrWhiteSpace(search))
                {
                    query = query.Where(a => a.Title.Contains(search) || (a.Category != null && a.Category.Name.Contains(search)));
                }

                TotalCount = await query.CountAsync();
                TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);
                var items = await query
                    .OrderByDescending(a => a.Title)
                    .Skip((page - 1) * PageSize)
                    .Take(PageSize)
                    .Select(a => new {
                        id = a.Id,
                        title = a.Title,
                        category = a.Category != null ? a.Category.Name : "Uncategorized",
                        price = a.Price,
                        stock = a.StockQuantity
                    })
                    .ToListAsync();

                return new JsonResult(new { items, totalPages = TotalPages, page });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching articles data");
                return new JsonResult(new { items = Array.Empty<object>(), totalPages = 0, page = 1, error = "Unable to load data" });
            }
        }

        private async Task LoadArticlesAsync(int page, string? search)
        {
            var query = _context.Articles.Include(a => a.Category).AsQueryable();
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(a => a.Title.Contains(search) || (a.Category != null && a.Category.Name.Contains(search)));
            }

            TotalCount = await query.CountAsync();
            TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);

            Articles = await query
                .OrderByDescending(a => a.Title)
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();
        }
    }
}