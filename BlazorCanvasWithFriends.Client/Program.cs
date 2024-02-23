using BlazorCanvasWithFriends.Client;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.FluentUI.AspNetCore.Components;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddFluentUIComponents();
builder.Services.AddSingleton<CanvasClient>();

await builder.Build().RunAsync();
