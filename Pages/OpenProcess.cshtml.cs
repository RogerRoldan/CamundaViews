using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace VistasCamunda.Pages
{
    public class OpenProcessModel : PageModel
    {
        public string response { get; set; }

        public async void OnGet()
        {

            
        }
        [HttpPost]
        public IActionResult OnPost()
        {

            return RedirectToPage("/FormularioIngreso");
        }
    }
}
