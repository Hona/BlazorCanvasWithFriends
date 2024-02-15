using BlazorCanvasWithFriends.Client;
using BlazorCanvasWithFriends.Components;
using BlazorCanvasWithFriends.Hubs;
using BlazorWithFriends.Shared;
using Microsoft.AspNetCore.ResponseCompression;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        new[] { "application/octet-stream" });
});

builder.Services.AddScoped<CanvasClient>();
builder.Services.AddSingleton<CanvasState>();

builder.Services.AddSignalR()
    .AddMessagePackProtocol();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseResponseCompression();

if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(BlazorCanvasWithFriends.Client._Imports).Assembly);

app.MapHub<CanvasHub>(Constants.CanvasHubRoute);

app.Run();
