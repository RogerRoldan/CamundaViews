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
        //startProcess
        HttpClient client = new HttpClient();
        var responseStartProcess = client.GetAsync("https://localhost:7176/startprocess").Result.Content.ReadAsStringAsync().Result;
        var idexecution = JsonConvert.DeserializeObject<dynamic>(responseStartProcess);
        var idexecutiontext = Convert.ToString(idexecution.id);
        //idprocess
        string Urlprocess = "http://localhost:8080/engine-rest/task?executionId=" + idexecutiontext;
        var responseIdProcess = client.GetAsync(Urlprocess).Result.Content.ReadAsStringAsync().Result;
            var process = JsonConvert.DeserializeObject<dynamic>(responseIdProcess)[0];
            var idtask = Convert.ToString(process.id);
            var idinstanced = Convert.ToString(process.processInstanceId);
          


            return RedirectToPage("/FormularioIngreso", new { id = idtask, idinstanced = idinstanced });
        }
    }
}
