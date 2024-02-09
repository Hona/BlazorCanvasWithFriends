using System.Drawing;
using BlazorWithFriends.Shared;
using BlazorWithFriends.Shared.SignalR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using TypedSignalR.Client;

namespace BlazorCanvasWithFriends.Client;

public class CanvasClient(NavigationManager navigation) : ICanvas, IAsyncDisposable
{
    private HubConnection? _hubConnection;
    public ICanvas? Hub { get; private set; }
    private IDisposable? _clientSubscription;
    
    public EventCallback<(Point, Point)> OnDrawLine { get; set; }
    
    public bool IsConnected =>
        _hubConnection?.State == HubConnectionState.Connected;
    
    public async Task InitializeAsync()
    {
        _hubConnection = new HubConnectionBuilder()
            .WithUrl(navigation.ToAbsoluteUri(Constants.CanvasHubRoute)) // TODO: Refactor
            .Build();
        
        Hub = _hubConnection.CreateHubProxy<ICanvas>();
        _clientSubscription = _hubConnection.Register<ICanvas>(this);

        await _hubConnection.StartAsync();
    }
    public Task DrawLine(Point from, Point to)
    {
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