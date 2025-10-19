using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using EcomRazorPage.Models;
using EcomApp.Pages.Articles.ViewModels;
using EcomRazorPage.Data;

namespace EcomApp.Pages.Articles
{
    
    public class EditModel : PageModel
    {
        private readonly ArticlesDBContext _context;
        private readonly ILogger<EditModel> _logger;

        public EditModel(ArticlesDBContext context, ILogger<EditModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        
        [BindProperty]
        public ArticleFormModel ArticleForm { get; set; } = new ArticleFormModel();

        // GET Handler
        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _context.Articles.FirstOrDefaultAsync(m => m.Id == id);

            if (article == null)
            {
                TempData["Error"] = "Article not found.";
                return RedirectToPage("./Index");
            }

            // Map the Entity Model to the ViewModel
            ArticleForm = new ArticleFormModel
            {
                Id = article.Id,
                Title = article.Title,
                Description = article.Description,
                Price = article.Price,
                ImageUrl = article.ImageUrl ?? string.Empty
            };

            return Page();
        }

        // POST Handler
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Find the existing article
            var articleToUpdate = await _context.Articles.FindAsync(ArticleForm.Id);

            if (articleToUpdate == null)
            {
                TempData["Error"] = "Article not found during update.";
                return RedirectToPage("./Index");
            }

            // Map the ViewModel back to the Entity Model
            articleToUpdate.Title = ArticleForm.Title;
            articleToUpdate.Description = ArticleForm.Description;
            articleToUpdate.Price = ArticleForm.Price;
            articleToUpdate.ImageUrl = ArticleForm.ImageUrl;

            _context.Attach(articleToUpdate).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                TempData["Success"] = "Article updated successfully!";
                return RedirectToPage("./Index");
            }
           
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating article {ArticleId}", articleToUpdate.Id);
                TempData["Error"] = "An unexpected error occurred. Please try again.";
                return Page();
            }
        }

        public async Task<IActionResult> OnPostBulkDeleteAsync(List<Guid> selectedIds)
        {
            if (selectedIds == null || !selectedIds.Any())
            {
                TempData["Error"] = "No articles selected for deletion.";
                return RedirectToPage("./Index");
            }

            try
            {
                var articlesToDelete = await _context.Articles
                    .Where(a => selectedIds.Contains(a.Id))
                    .ToListAsync();

                _context.Articles.RemoveRange(articlesToDelete);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Selected articles deleted successfully!";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting articles: {ArticleIds}", string.Join(", ", selectedIds));
                TempData["Error"] = "An unexpected error occurred while deleting articles. Please try again.";
            }

            return RedirectToPage("./Index");
        }
    }
}