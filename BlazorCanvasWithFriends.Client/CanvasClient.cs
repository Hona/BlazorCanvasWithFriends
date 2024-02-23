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
    public HubConnection? HubConnection { get; private set; }
    
    public ICanvasHub? Hub { get; private set; }
    private IDisposable? _clientSubscription;
    
    public EventCallback<Line> OnDrawLine { get; set; }
    public EventCallback OnClear { get; set; }
    
    public Action? OnConnected { get; set; }
    public Action? OnDisconnected { get; set; }
    
    public bool IsConnected =>
        HubConnection?.State == HubConnectionState.Connected;
    
    public async Task InitializeAsync()
    {
        HubConnection = new HubConnectionBuilder()
            .WithUrl(navigation.ToAbsoluteUri(Constants.CanvasHubRoute))
            .AddMessagePackProtocol()
            .Build();
        
        HubConnection.Closed += error =>
        {
            logger.LogWarning("Connection closed: {Error}", error);
            OnDisconnected?.Invoke();

            return Task.CompletedTask;
        };

        HubConnection.Reconnected += connectionId =>
        {
            InvokeOnConnected();
            
            return Task.CompletedTask;
        };
        
        Hub = HubConnection.CreateHubProxy<ICanvasHub>();
        _clientSubscription = HubConnection.Register<ICanvasClient>(this);

        await HubConnection.StartAsync();

        InvokeOnConnected();
    }

    private void InvokeOnConnected()
    {
        logger.LogInformation("SignalR connected");
        OnConnected?.Invoke();
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
        if (HubConnection != null) await HubConnection.DisposeAsync();
        
        if (_clientSubscription is IAsyncDisposable clientSubscriptionAsyncDisposable)
            await clientSubscriptionAsyncDisposable.DisposeAsync();
        else
        {
            _clientSubscription?.Dispose();
        }
    }
}