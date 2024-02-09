using System.Drawing;

namespace BlazorCanvasWithFriends.Hubs;

public class CanvasState
{
    public List<(Point from, Point to)> Lines { get; set; } = new();
}