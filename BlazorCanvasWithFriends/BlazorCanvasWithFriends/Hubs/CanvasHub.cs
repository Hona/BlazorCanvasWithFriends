using System.Drawing;
using BlazorWithFriends.Shared.SignalR;
using Microsoft.AspNetCore.SignalR;

namespace BlazorCanvasWithFriends.Hubs;

public class CanvasHub : Hub<ICanvas>, ICanvas
{
    public async Task DrawLine(Point from, Point to)
    {
        await Clients.All
            .DrawLine(from, to);
    }
}