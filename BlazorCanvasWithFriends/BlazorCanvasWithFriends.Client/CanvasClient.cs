using System.Drawing;
using BlazorWithFriends.Shared;
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
    
    public EventCallback<(Point, Point)> OnDrawLine { get; set; }
    
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

    public async Task Load(List<(Point from, Point to)> lines)
    {
        logger.LogInformation("Loading {Count} lines", lines.Count);
        
        foreach (var (from, to) in lines)
        {
            await OnDrawLine.InvokeAsync((from, to));
        }
    }

    public Task DrawLine(Point from, Point to)
    {
        logger.LogInformation("Drawing line from {from} to {to}", from, to);
        
        return OnDrawLine.InvokeAsync((from, to));
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