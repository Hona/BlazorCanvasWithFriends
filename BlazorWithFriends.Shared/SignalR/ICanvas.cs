using System.Drawing;
using BlazorWithFriends.Shared.Models;

namespace BlazorWithFriends.Shared.SignalR;

/// <summary>
/// Common interface for server & client
/// Client is able to ...
/// Server is able to ...
/// </summary>
public interface ICanvasHub
{
    Task RequestState();
    
    Task DrawLine(Point2D from, Point2D to);
}

public interface ICanvasClient
{
    Task Load(List<(Point2D from, Point2D to)> lines);
    
    Task DrawLine(Point2D from, Point2D to);
}