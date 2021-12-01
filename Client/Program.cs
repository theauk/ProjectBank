using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ProjectBank.Client;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient("ProjectBank.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

// Supply HttpClient instances that include access tokens when making requests to the server project
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("ProjectBank.ServerAPI"));

builder.Services.AddAuthorizationCore(config =>
{
    config.AddPolicy("Student", policy => policy.RequireClaim("Roles","[\"Student\"]"));
    config.AddPolicy("Supervisor", policy => policy.RequireClaim("Roles","[\"Supervisor\"]"));
    config.AddPolicy("Admin", policy => policy.RequireClaim("Roles","[\"Admin\"]"));
});

builder.Services.AddMsalAuthentication(options =>
{
    builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);
    options.ProviderOptions.DefaultAccessTokenScopes.Add("api://23674461-24c3-4eb4-bcf7-48940c96d1cf/API.Access");
});

await builder.Build().RunAsync();
