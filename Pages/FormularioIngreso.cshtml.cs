using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using VistasCamunda.Models;
using System.Net.Http;



using static Iana.MediaTypes;
using System.Threading.Tasks;

namespace VistasCamunda.Pages
{
    public class FormularioIngresoModel : PageModel
    {

        public string IdInstanced { get; set; }
        public string IdTask { get; set; }

        public async Task OnGet(string id, string idinstanced)
        {
            IdTask = id;
            IdInstanced = idinstanced;

        }
        [HttpPost]
        public IActionResult OnPost(string id1, string IdInstanced, string Nombres, string Apellidos, int Edad, string Cargo, string Ciudad, string Email, int Telefono, IFormFile HojaVida)
        {
            HttpClient client = new HttpClient();
            string NombreArchivo = "HojaVida"; //Nombre del archivo


            //-------------------------------------
            string urlbinary = "http://localhost:8080/engine-rest/task/" + id1 + "/variables/" + NombreArchivo +"/data";
            client.BaseAddress = new Uri(urlbinary);
            //se debe agregar el  enctype="multipart/form-data" en el form
            var file = HojaVida.OpenReadStream();
            var content = new MultipartFormDataContent();
            //Para agregar un stream file
            content.Add(new StreamContent(file), "data", "HojaVida"+ Nombres + ".pdf");
            content.Add(new StringContent("File"), "valueType");

            var response = client.PostAsync("", content).Result.Content.ReadAsStringAsync().Result;
            

            //-------------------------------------------------

            var idprocess = id1;

            Variable variables = new Variable();
            variables.modifications = new Dictionary<string, Atribute>();
            variables.Add("Nombres", Nombres, "String");
            variables.Add("Apellidos", Apellidos, "String");
            variables.Add("Edad", Edad, "integer");
            variables.Add("Cargo", Cargo, "String");
            variables.Add("Ciudad", Ciudad, "String");
            variables.Add("Email", Email, "String");
            variables.Add("Telefono", Telefono, "integer");
            //variables.Add("HojaVida.pdf", HojaVida, "Bytes");
            
            
            string json = JsonConvert.SerializeObject(variables);
            var contentVariables = new StringContent(json, Encoding.UTF8, "application/json");

            string Url = "http://localhost:8080/engine-rest/task/" + idprocess + "/variables";
            var responseVariables = client.PostAsync(Url, contentVariables).Result.Content.ReadAsStringAsync().Result;

            //complete task
            var jsonComplete = new{ variables = new { }       };
            string jsonCompleteSerialized = JsonConvert.SerializeObject(jsonComplete);


            var contentcomplete = new StringContent(json, Encoding.UTF8, "application/json");
            string UrlCompleteTask = "http://localhost:8080/engine-rest/task/" + idprocess + "/complete";
            var responseCompleteTask = client.PostAsync(UrlCompleteTask, contentcomplete).Result.Content.ReadAsStringAsync().Result;

            //idnewtask
            string UrlNewTask = "http://localhost:8080/engine-rest/task?processInstanceId=" + IdInstanced;
            var responseNewTask = client.GetAsync(UrlNewTask).Result.Content.ReadAsStringAsync().Result;
            var newtask = JsonConvert.DeserializeObject<dynamic>(responseNewTask)[0];
            var idnewtask = Convert.ToString(newtask.id);

            //// form key
            string UrlFormKey = "http://localhost:8080/engine-rest/task/" + idnewtask + "/form";
            var responseFormKey = client.GetAsync(UrlFormKey).Result.Content.ReadAsStringAsync().Result;
            var formkey = JsonConvert.DeserializeObject<dynamic>(responseFormKey);
            var formkeytext = Convert.ToString(formkey.key);

            Console.Write(formkeytext);


            return RedirectToPage(formkeytext, new { idtask = idnewtask, idinstanced = IdInstanced });
        }
    }


}

            
