using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Text;
using VistasCamunda.Models;

namespace VistasCamunda.Pages
{
    public class AceptacionContratoModel : PageModel
    {
        public string IdTask { get; set; }
        public string IdInstanced { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public int Edad { get; set; }
        public string Cargo { get; set; }
        public string Ciudad { get; set; }
        public string Email { get; set; }
        public int Telefono { get; set; }
        public string Observaciones { get; set; }
        public string Aprobacion { get; set; }
        public string CodContrato { get; set; }
        public string ValorContrato { get; set; }
        public async Task OnGet(string idtask, string idinstanced)
        {
            IdTask = idtask;
            IdInstanced = idinstanced;

            HttpClient client = new HttpClient();
            var response = await client.GetAsync("https://localhost:7176/cargarvariables/" + IdTask).Result.Content.ReadAsStringAsync();
            var variables = JsonConvert.DeserializeObject<dynamic>(response);
            Nombres = variables.Nombres.value;
            Apellidos = variables.Apellidos.value;
            Edad = variables.Edad.value;
            Cargo = variables.Cargo.value;
            Ciudad = variables.Ciudad.value;
            Email = variables.Email.value;
            Telefono = variables.Telefono.value;
            Observaciones = variables.Observaciones.value;
            Aprobacion = variables.Aprobacion.value;
            CodContrato = variables.CodContrato.value;
            ValorContrato = variables.ValorContrato.value;

        }
        [HttpPost]
        public IActionResult OnPost(string idtask, string idinstanced, string AceptarContrato, string Nombres )
        {
            HttpClient client = new HttpClient();

            Variablecomplete newvariable = new Variablecomplete();
            newvariable.variables = new Dictionary<string, AtributeComplete>();
            newvariable.variables.Add("AceptarContrato", new AtributeComplete { value = AceptarContrato });
            var json = JsonConvert.SerializeObject(newvariable);

            Console.Write(json + "\n");
            Console.Write(newvariable);
            var dataobservacion = new StringContent(json, Encoding.UTF8, "application/json");

            //taskcomplete

            string urlcompletetask = "http://localhost:8080/engine-rest/task/" + idtask + "/complete";
            var responsecompletetask = client.PostAsync(urlcompletetask, dataobservacion).Result.Content.ReadAsStringAsync().Result;

            if (AceptarContrato == "true")
            {
            return RedirectToPage("/ColaboradorContratado", new { nombres = Nombres });
                
            }
            else
            {
                return RedirectToPage("/ColaboradorNoContratado", new  { nombres = Nombres });
            }
        }
    }
}
