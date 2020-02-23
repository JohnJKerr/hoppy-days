using Godot;
using System;
using System.Diagnostics;
using hoppydays.Player;

public class SpikeTop : Area2D
{
    public void OnBodyEntered(Node body)
    {
        if (body is Player player)
            GetTree().CallGroup(nameof(GameState), nameof(GameState.Hurt));
    }
}
