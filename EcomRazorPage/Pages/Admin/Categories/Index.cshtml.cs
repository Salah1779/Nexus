using Microsoft.EntityFrameworkCore;
using EcomRazorPage.Data;
using EcomRazorPage.Models;
using Microsoft.AspNetCore.Mvc;

namespace EcomRazorPage.Pages.Admin.Categories
{
    public class IndexModel : AdminPageModel
    {
        private readonly ArticlesDBContext _context;

        public IndexModel(ArticlesDBContext context)
        {
            _context = context;
        }

        public IList<Category> Categories { get; set; } = new List<Category>();

        [BindProperty]
        public List<Guid> SelectedIds { get; set; } = new();

        public async Task OnGetAsync()
        {
            Categories = await _context.Categories
                .Include(c => c.Articles)
                .OrderBy(c => c.DisplayOrder)
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostBulkDeleteAsync()
        {
            if (SelectedIds != null && SelectedIds.Any())
            {
                var categories = await _context.Categories.Where(c => SelectedIds.Contains(c.Id)).ToListAsync();
                _context.Categories.RemoveRange(categories);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Selected categories deleted successfully.";
            }
            else
            {
                TempData["Error"] = "No categories selected.";
            }

            return RedirectToPage();
        }
    }
}
