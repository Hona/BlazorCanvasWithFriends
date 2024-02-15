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
    
    Task DrawLine(Line line);
    
    Task Clear();
}