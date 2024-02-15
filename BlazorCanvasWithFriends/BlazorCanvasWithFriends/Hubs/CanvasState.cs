using System.Drawing;
using BlazorWithFriends.Shared.Models;

namespace BlazorCanvasWithFriends.Hubs;

public class CanvasState
{
    public List<Line> Lines { get; set; } = new();
}