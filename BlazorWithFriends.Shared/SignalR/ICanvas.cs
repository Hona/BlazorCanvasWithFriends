using System.Drawing;

namespace BlazorWithFriends.Shared.SignalR;

/// <summary>
/// Common interface for server & client
/// Client is able to ...
/// Server is able to ...
/// </summary>
public interface ICanvasHub
{
    Task RequestState();
    
    Task DrawLine(Point from, Point to);
}

public interface ICanvasClient
{
    Task Load(List<(Point from, Point to)> lines);
    
    Task DrawLine(Point from, Point to);
}