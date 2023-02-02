using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Text;
using VistasCamunda.Models;

namespace VistasCamunda.Pages
{
    public class RevisarSolicitudModel : PageModel
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
 

        }
        [HttpPost]
        public IActionResult OnPost(string id1, string idinstanced, string Observaciones)
        {
            HttpClient client = new HttpClient();
            
            Variablecomplete newvariable = new Variablecomplete();
            newvariable.variables = new Dictionary<string, AtributeComplete>();
            newvariable.variables.Add("Observaciones", new AtributeComplete { value = Observaciones });
            var json = JsonConvert.SerializeObject(newvariable);

            Console.Write(json+"\n");
            Console.Write(newvariable);
            var dataobservacion = new StringContent(json, Encoding.UTF8, "application/json");

            //taskcomplete

            string urlcompletetask = "http://localhost:8080/engine-rest/task/" + id1 + "/complete";
            var responsecompletetask = client.PostAsync(urlcompletetask, dataobservacion).Result.Content.ReadAsStringAsync().Result;
            Console.Write(urlcompletetask+"\n");

            //idnewtask
            string UrlNewTask = "http://localhost:8080/engine-rest/task?processInstanceId=" + idinstanced;
            var responseNewTask = client.GetAsync(UrlNewTask).Result.Content.ReadAsStringAsync().Result;
            var newtask = JsonConvert.DeserializeObject<dynamic>(responseNewTask)[0];
            var idnewtask = Convert.ToString(newtask.id);

            return RedirectToPage("/AprobarSolicitud", new { idtask = idnewtask, idinstanced = idinstanced });
            }
        }
}
