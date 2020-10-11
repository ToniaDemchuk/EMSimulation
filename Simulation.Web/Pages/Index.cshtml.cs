using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Simulation.Web.Pages
{
    public class IndexModel: PageModel
    {
        public IActionResult OnGet()
        {
            return RedirectToPage("DDA");
        }
    }
}
