using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace VistasCamunda.Pages
{
    public class FormularioIngresoModel : PageModel
    {
        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            
            return RedirectToPage("/CargarVariables");
        }
    }
}
