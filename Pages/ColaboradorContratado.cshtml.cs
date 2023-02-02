using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace VistasCamunda.Pages
{
    public class ColaboradorContratadoModel : PageModel
    {
        public string Nombres { get; set; }
        public void OnGet(string nombres)
        {
            Nombres = nombres;
        }
        [HttpPost]
        public IActionResult OnPost()
        {
            return RedirectToPage("/OpenProcess");
        }

    }
}
