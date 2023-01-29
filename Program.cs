using Camunda.Api.Client;
using Camunda.Api.Client.UserTask;
using Newtonsoft.Json;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

string baseUrl = "http://localhost:8080/engine-rest";
CamundaClient camunda = CamundaClient.Create(baseUrl);

app.MapGet("/startprocess", async () =>
{
    HttpClient client = new HttpClient();


    //httpclient post enviando un json

    var jsonObject = new
    {
        variables = new { }
    };
    string json = JsonConvert.SerializeObject(jsonObject);


    var content = new StringContent(json, Encoding.UTF8, "application/json");
    var response = await client.PostAsync("http://localhost:8080/engine-rest/process-definition/key/Process_18azszf/start", content);
    var responseString = await response.Content.ReadAsStringAsync();

    
    var respuesta = JsonConvert.DeserializeObject<dynamic>(responseString);
    var id = respuesta.id;
    

    return JsonConvert.SerializeObject(new { id = respuesta.id });

});

app.MapGet("/cargarvariables/{id}", async (string id) =>
{
    HttpClient client = new HttpClient();
    var Url = baseUrl + "/task/" + id + "/variables";
    var response = await client.GetAsync(Url);
    var datos = await response.Content.ReadAsStringAsync();
    Console.Write(Url + "\n");
    Url = " ";
    return datos;
});




// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{

    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
