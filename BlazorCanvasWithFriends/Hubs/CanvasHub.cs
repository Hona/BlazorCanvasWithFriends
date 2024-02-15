using System.Drawing;
using BlazorWithFriends.Shared.Models;
using BlazorWithFriends.Shared.SignalR;
using Microsoft.AspNetCore.SignalR;

namespace BlazorCanvasWithFriends.Hubs;

public class CanvasHub : Hub<ICanvasClient>, ICanvasHub
{
    private ILogger<CanvasHub> _logger;

    private CanvasState _state;

    public CanvasHub(ILogger<CanvasHub> logger, CanvasState state)
    {
        _logger = logger;
        _state = state;
    }

    public async Task RequestState()
    {
        _logger.LogInformation("RequestState from {Caller}, currently have {LineCount}", Context.ConnectionId, _state.Lines.Count);
        
        await Clients.Caller
            .Load(_state.Lines);
    }

    public async Task DrawLine(Line line)
    {
        _logger.LogInformation("DrawLine {Line}", line);
        
        _state.Lines.Add(line);
        
        await Clients.All
            .DrawLine(line);
    }

    public Task Clear()
    {
        _logger.LogInformation("Clear");
        
        _state.Lines.Clear();

        return Clients.All.Clear();
    }
}