using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace VistasCamunda.Pages.Shared
{
    public class ColaboradoNoContratadoModel : PageModel
    {
        public string Nombres { get; set; }
        public void OnGet(string nombres)
        {
            Nombres = nombres;
        }

        [HttpPost]
        public IActionResult OnPost(string nombres)
        {
            return RedirectToPage("/OpenProcess");
        }
    }
}
