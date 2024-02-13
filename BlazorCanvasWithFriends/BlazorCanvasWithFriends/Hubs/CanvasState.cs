using System.Drawing;
using BlazorWithFriends.Shared.Models;

namespace BlazorCanvasWithFriends.Hubs;

public class CanvasState
{
    public List<(Point2D from, Point2D to)> Lines { get; set; } = new();
}