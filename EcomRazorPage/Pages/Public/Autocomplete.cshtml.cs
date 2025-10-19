using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EcomRazorPage.Data;
using System.Linq;
using System.Collections.Generic;

namespace EcomRazorPage.Pages.Public
{
    public class AutocompleteModel : PageModel
    {
        private readonly ArticlesDBContext _db;
        public AutocompleteModel(ArticlesDBContext db)
        {
            _db = db;
        }

        public JsonResult OnGet(string term)
        {
            var results = _db.Articles
                .Where(a => a.Title.Contains(term))
                .OrderBy(a => a.Title)
                .Select(a => a.Title)
                .Take(8)
                .ToList();
            return new JsonResult(results);
        }
    }
}
