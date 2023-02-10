using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Text;
using VistasCamunda.Models;

namespace VistasCamunda.Pages
{
    public class AprobarSolicitudModel : PageModel
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



        }
        [HttpPost]
        public IActionResult OnPost(string id1, string idinstanced, string Observaciones, string Aprobacion, string Nombres)
        {
            HttpClient client = new HttpClient();

            Variablecomplete newvariable = new Variablecomplete();
            newvariable.variables = new Dictionary<string, AtributeComplete>();
            newvariable.variables.Add("Aprobacion", new AtributeComplete { value = Aprobacion });
            var json = JsonConvert.SerializeObject(newvariable);

            Console.Write(json + "\n");
            Console.Write(newvariable);
            var dataobservacion = new StringContent(json, Encoding.UTF8, "application/json");

            //taskcomplete

            string urlcompletetask = "http://localhost:8080/engine-rest/task/" + id1 + "/complete";
            var responsecompletetask = client.PostAsync(urlcompletetask, dataobservacion).Result.Content.ReadAsStringAsync().Result;


            if (Aprobacion == "true") {            
            //idnewtask
            string UrlNewTask = "http://localhost:8080/engine-rest/task?processInstanceId=" + idinstanced;
            var responseNewTask = client.GetAsync(UrlNewTask).Result.Content.ReadAsStringAsync().Result;
            var newtask = JsonConvert.DeserializeObject<dynamic>(responseNewTask)[0];
            var idnewtask = Convert.ToString(newtask.id);

                //// form key
                string UrlFormKey = "http://localhost:8080/engine-rest/task/" + idnewtask + "/form";
                var responseFormKey = client.GetAsync(UrlFormKey).Result.Content.ReadAsStringAsync().Result;
                var formkey = JsonConvert.DeserializeObject<dynamic>(responseFormKey);
                var formkeytext = Convert.ToString(formkey.key);

                Console.Write(formkeytext);


                return RedirectToPage(formkeytext, new { idtask = idnewtask, idinstanced = idinstanced });
            }
            else
            {
                return RedirectToPage("/ColaboradorNoContratado", new { nombres = Nombres });
            }
        }
    }
}
