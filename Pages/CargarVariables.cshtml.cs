using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Collections.Immutable;
using static Camunda.Api.Client.Filter.FilterInfo;

namespace VistasCamunda.Pages
{
    public class CargarVariablesModel : PageModel
    {
        public string name { get; set; }
        public string ejemplo { get; set; }
        public string response { get; set; }


        public async Task OnGet(string id)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync("https://localhost:7176/cargarvariables/" + id);
            var datos = await response.Content.ReadAsStringAsync();
            var variables =  JsonConvert.DeserializeObject<dynamic>(datos);
            var variables2 = variables.var1.value;
            name = variables2;
            ejemplo = "hola";
            Console.Write(name);
            Console.Write(ejemplo);

            
        }
        [HttpPost]
        public IActionResult OnPost()
        {
            HttpClient client = new HttpClient();
            var response = client.GetAsync("https://localhost:7176/startprocess").Result.Content.ReadAsStringAsync().Result;
            var idprocess = JsonConvert.DeserializeObject<dynamic>(response);
            var id = Convert.ToString(idprocess.id);

            return RedirectToPage("/CargarVariables", new { id = id });
        }
    }
}
