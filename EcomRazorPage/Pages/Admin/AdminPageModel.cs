using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EcomRazorPage.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public abstract class AdminPageModel : PageModel
    {
        [TempData]
        public string? StatusMessage { get; set; }

        protected void SetSuccessMessage(string message)
        {
            StatusMessage = $"Success: {message}";
        }

        protected void SetErrorMessage(string message)
        {
            StatusMessage = $"Error: {message}";
        }

        protected IActionResult RedirectToIndex(string? message = null)
        {
            if (!string.IsNullOrEmpty(message)) SetSuccessMessage(message);
            return RedirectToPage("./Index");
        }

        protected JsonResult JsonResult(object data, bool success = true, string? message = null)
        {
            return new JsonResult(new { success, message, data });
        }

        protected bool IsAjaxRequest()
        {
            return Request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }
    }
}
