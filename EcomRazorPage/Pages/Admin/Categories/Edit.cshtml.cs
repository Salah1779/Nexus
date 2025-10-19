using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EcomRazorPage.Data;
using EcomRazorPage.Models;

namespace EcomRazorPage.Pages.Admin.Categories
{
    public class EditModel : AdminPageModel
    {
        private readonly ArticlesDBContext _context;

        public EditModel(ArticlesDBContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Category Category { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
                return NotFound();

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return NotFound();

            Category = category;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var categoryToUpdate = await _context.Categories.FindAsync(Category.Id);
            if (categoryToUpdate == null)
                return NotFound();

            categoryToUpdate.Name = Category.Name;
            categoryToUpdate.Description = Category.Description;
            categoryToUpdate.DisplayOrder = Category.DisplayOrder;
            categoryToUpdate.IsActive = Category.IsActive;
            categoryToUpdate.LastModifiedDate = DateTime.UtcNow;
            categoryToUpdate.Slug = Category.Name.ToLower().Replace(" ", "-");

            await _context.SaveChangesAsync();
            TempData["Success"] = "Category updated successfully.";
            return RedirectToPage("./Index");
        }
    }
}
