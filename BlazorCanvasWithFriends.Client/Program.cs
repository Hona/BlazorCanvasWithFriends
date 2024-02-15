using BlazorCanvasWithFriends.Client;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddSingleton<CanvasClient>();

await builder.Build().RunAsync();
