using Godot;
using System;

public class Portal : Node2D
{
    public void OnBodyEntered(Node body)
        => GetTree().CallGroup(nameof(GameState), nameof(GameState.WinGame));
}
