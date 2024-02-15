using System.Drawing;
using BlazorWithFriends.Shared;
using BlazorWithFriends.Shared.Models;
using BlazorWithFriends.Shared.SignalR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using TypedSignalR.Client;

namespace BlazorCanvasWithFriends.Client;

public class CanvasClient(NavigationManager navigation, ILogger<CanvasClient> logger) 
    : ICanvasClient, IAsyncDisposable
{
    private HubConnection? _hubConnection;
    public ICanvasHub? Hub { get; private set; }
    private IDisposable? _clientSubscription;
    
    public EventCallback<Line> OnDrawLine { get; set; }
    public EventCallback OnClear { get; set; }
    
    public bool IsConnected =>
        _hubConnection?.State == HubConnectionState.Connected;
    
    public async Task InitializeAsync()
    {
        _hubConnection = new HubConnectionBuilder()
            .WithUrl(navigation.ToAbsoluteUri(Constants.CanvasHubRoute))
            .AddMessagePackProtocol()
            .Build();
        
        Hub = _hubConnection.CreateHubProxy<ICanvasHub>();
        _clientSubscription = _hubConnection.Register<ICanvasClient>(this);

        await _hubConnection.StartAsync();
    }

    public async Task Load(List<Line> lines)
    {
        logger.LogInformation("Loading {Count} lines", lines.Count);
        
        foreach (var line in lines)
        {
            await OnDrawLine.InvokeAsync(line);
        }
    }

    public Task DrawLine(Line line)
    {
        logger.LogInformation("Drawing line {Line}", line);
        
        return OnDrawLine.InvokeAsync(line);
    }

    public Task Clear()
    {
        logger.LogInformation("Clearing");
        
        return OnClear.InvokeAsync();
    }

    public async ValueTask DisposeAsync()
    {
        if (_hubConnection != null) await _hubConnection.DisposeAsync();
        if (_clientSubscription is IAsyncDisposable clientSubscriptionAsyncDisposable)
            await clientSubscriptionAsyncDisposable.DisposeAsync();
        else
        {
            _clientSubscription?.Dispose();
        }
    }
}