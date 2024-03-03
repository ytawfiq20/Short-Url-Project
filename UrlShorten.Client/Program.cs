using CurrieTechnologies.Razor.Clipboard;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using UrlShorten.Client;
using UrlShorten.Client.Service.ShortenUrlService;
using UrlShorten.Client.Service.ThirdPartyService;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7116/") });

builder.Services.AddScoped<IUrl, UrlService>();
builder.Services.AddScoped<IShortenUrlThirdParty, ShortenUrlThirfPartyService>();


builder.Services.AddClipboard();

await builder.Build().RunAsync();
