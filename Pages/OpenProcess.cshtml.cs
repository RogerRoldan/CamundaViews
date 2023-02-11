using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Text;
using VistasCamunda.Models;

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
            //startProcess
            HttpClient client = new HttpClient();

            VariableInitial variableInitial = new VariableInitial();
            variableInitial.variables = new Dictionary<string, AtributeInitial>();
            variableInitial.Add("Nombres", "", "String");
            variableInitial.Add("Apellidos", "", "String");
            variableInitial.Add("Edad", "", "integer");
            variableInitial.Add("Cargo", "", "String");
            variableInitial.Add("Ciudad", "", "String");
            variableInitial.Add("Email", "", "String");
            variableInitial.Add("Telefono", "", "integer");
            variableInitial.Add("HojaVida", "", "String");
            variableInitial.Add("Aprobacion", "", "String");
            variableInitial.Add("Observaciones", "", "String");
            variableInitial.Add("CodContrato", "", "String");
            variableInitial.Add("ValorContrato", "", "string");
            variableInitial.Add("AceptarContrato", "", "String");

            var json = JsonConvert.SerializeObject(variableInitial);
            var dataInitialVariables = new StringContent(json, Encoding.UTF8, "application/json");
            var url = "http://localhost:8080/engine-rest/process-definition/key/SasoftcoColaborators/start";
            var response = client.PostAsync(url, dataInitialVariables ).Result.Content.ReadAsStringAsync().Result;
            var idexecutiontext = Convert.ToString(JsonConvert.DeserializeObject<dynamic>(response).id);



            //var responseStartProcess = client.GetAsync("https://localhost:7176/startprocess").Result.Content.ReadAsStringAsync().Result;
            //var idexecution = JsonConvert.DeserializeObject<dynamic>(responseStartProcess);
            //var idexecutiontext = Convert.ToString(idexecution.id);


            //idprocess
            string Urlprocess = "http://localhost:8080/engine-rest/task?executionId=" + idexecutiontext;
            var responseIdProcess = client.GetAsync(Urlprocess).Result.Content.ReadAsStringAsync().Result;
            var process = JsonConvert.DeserializeObject<dynamic>(responseIdProcess)[0];
            var idtask = Convert.ToString(process.id);
            var idinstanced = Convert.ToString(process.processInstanceId);

            // Next Form
            string UrlFormKey = "http://localhost:8080/engine-rest/task/" + idtask + "/form";
            var responseFormKey = client.GetAsync(UrlFormKey).Result.Content.ReadAsStringAsync().Result;
            var formkey = JsonConvert.DeserializeObject<dynamic>(responseFormKey);
            var formkeytext = Convert.ToString(formkey.key);

            Console.Write(formkeytext);


            return RedirectToPage(formkeytext, new { id = idtask, idinstanced = idinstanced });
        }
    }
}
