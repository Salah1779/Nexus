using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using EcomRazorPage.Data;
using EcomRazorPage.Models;

namespace EcomApp.Pages.Articles
{
   
    public class DeleteModel : PageModel
    {
        private readonly ArticlesDBContext _context;
        private readonly ILogger<DeleteModel> _logger;

        public DeleteModel(ArticlesDBContext context, ILogger<DeleteModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        // The entity model is used as a display-only ViewModel property
        public Article Article { get; set; } = default!;

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

            Article = article;
            return Page();
        }

        // POST Handler
        public async Task<IActionResult> OnPostAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _context.Articles.FindAsync(id);

            if (article == null)
            {
                TempData["Error"] = "Article not found during deletion.";
                return RedirectToPage("./Index");
            }

            try
            {
                _context.Articles.Remove(article);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Article deleted successfully!";
                return RedirectToPage("./Index");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error deleting article {ArticleId}", id);
                TempData["Error"] = "Cannot delete article.";
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting article {ArticleId}", id);
                TempData["Error"] = "An unexpected error occurred. Please try again.";
                return RedirectToPage("./Index");
            }
        }
    }
}