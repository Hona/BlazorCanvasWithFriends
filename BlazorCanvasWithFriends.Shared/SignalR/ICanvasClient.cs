using BlazorWithFriends.Shared.Models;

namespace BlazorWithFriends.Shared.SignalR;

public interface ICanvasClient
{
    Task Load(List<Line> lines);
    
    Task DrawLine(Line line);
    
    Task Clear();
}