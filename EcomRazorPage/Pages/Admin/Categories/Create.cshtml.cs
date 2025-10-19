using Microsoft.AspNetCore.Mvc;
using EcomRazorPage.Data;
using EcomRazorPage.Models;

namespace EcomRazorPage.Pages.Admin.Categories
{
    public class CreateModel : AdminPageModel
    {
        private readonly ArticlesDBContext _context;

        public CreateModel(ArticlesDBContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Category Category { get; set; } = new();

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Category.CreatedDate = DateTime.UtcNow;
            Category.LastModifiedDate = DateTime.UtcNow;
            Category.Slug = Category.Name.ToLower().Replace(" ", "-");

            _context.Categories.Add(Category);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Category created successfully.";
            return RedirectToPage("./Index");
        }
    }
}
