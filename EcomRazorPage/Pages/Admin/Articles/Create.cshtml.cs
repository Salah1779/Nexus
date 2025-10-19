using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EcomRazorPage.Data;
using EcomRazorPage.Models;
using EcomApp.Pages.Articles.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace EcomApp.Pages.Articles
{
  
    public class CreateModel : PageModel
    {
        private readonly ArticlesDBContext _context;
        private readonly ILogger<CreateModel> _logger;

        public CreateModel(ArticlesDBContext context, ILogger<CreateModel> logger)
        {
            _context = context;
            _logger = logger;
        }

     
        [BindProperty]
        public ArticleFormModel ArticleForm { get; set; } = new ArticleFormModel();

        public IActionResult OnGet()
        {
            return Page();
        }

        // POST Handler
        public async Task<IActionResult> OnPostAsync()
        {
           
            if (!ModelState.IsValid)
            {
                return Page();
            }

            
            var article = new Article
            {
                Title = ArticleForm.Title,
                Description = ArticleForm.Description,
                Price = ArticleForm.Price,
                ImageUrl = ArticleForm.ImageUrl
            };

            try
            {
                _context.Articles.Add(article);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Article created successfully!";
                return RedirectToPage("./Index");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error creating article");
                TempData["Error"] = "Cannot create article. Please check the data and try again.";
                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating article");
                TempData["Error"] = "An unexpected error occurred. Please try again.";
                return Page();
            }
        }
    }
}