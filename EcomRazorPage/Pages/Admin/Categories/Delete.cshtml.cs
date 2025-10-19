using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EcomRazorPage.Data;
using EcomRazorPage.Models;

namespace EcomRazorPage.Pages.Admin.Categories
{
    public class DeleteModel : AdminPageModel
    {
        private readonly ArticlesDBContext _context;

        public DeleteModel(ArticlesDBContext context)
        {
            _context = context;
        }

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

        public async Task<IActionResult> OnPostAsync(Guid? id)
        {
            if (id == null)
                return NotFound();

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return NotFound();

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Category deleted successfully.";
            return RedirectToPage("./Index");
        }
    }
}
