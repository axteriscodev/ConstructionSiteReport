using ClientWebApp;
using ClientWebApp.Services;
using ConstructionSiteLibrary.Interfaces;
using ConstructionSiteLibrary.Managers;
using ConstructionSiteLibrary.Repositories;
using ConstructionSiteLibrary.Services;
using ConstructionSiteLibrary.Utility;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Radzen;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
//screen manager per catturare il resize della view
builder.Services.AddScoped<HttpManager>();

builder.Services.AddSingleton<ScreenManager>();
//notify manager per notificare all'utente il risultato delle operazioni compiute
builder.Services.AddScoped<NotificationManager>();
//repository per le categorie e argomenti
builder.Services.AddScoped<CategoriesRepository>();
//repository per le domande e le risposte
builder.Services.AddScoped<QuestionRepository>();
//repository per i documenti
builder.Services.AddScoped<DocumentsRepository>();
//repository per i template
builder.Services.AddScoped<TemplatesRepository>();
//repository per i cantieri
builder.Services.AddScoped<ConstructorSitesRepository>();
//repository per i clienti
builder.Services.AddScoped<ClientsRepository>();
//repository per le aziende
builder.Services.AddScoped<CompaniesRepository>();
//Global Variables
builder.Services.AddSingleton<GlobalVariables>();

builder.Services.AddScoped<ICameraService, CameraService>();
builder.Services.AddScoped<IndexedDBService>();

/* Servizi per la navigazione delle pagine */
builder.Services.AddScoped<NavigationService>();
builder.Services.AddScoped<InvokeJSRuntime>();


//componenti radzen
builder.Services.AddRadzenComponents();

await builder.Build().RunAsync();
