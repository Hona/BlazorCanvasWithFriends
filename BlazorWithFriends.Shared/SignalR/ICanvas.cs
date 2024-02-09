using System.Drawing;

namespace BlazorWithFriends.Shared.SignalR;

/// <summary>
/// Common interface for server & client
/// Client is able to ...
/// Server is able to ...
/// </summary>
public interface ICanvas
{
    Task DrawLine(Point from, Point to);
}