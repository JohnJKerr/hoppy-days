using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using hoppydays.Player;

public class Lightning : Node2D
{
    private const int Speed = 300;
    private Area2D Area2D => GetNode<Area2D>(nameof(Area2D));
    
    public override void _Ready()
    {
        SetAsToplevel(true);
        GlobalPosition = GetParent<Node2D>().GlobalPosition;
    }

    public override void _Process(float delta)
    {
        base._Process(delta);
        Position = new Vector2(Position.x, Position.y + (Speed * delta));
        ManageCollision();
    }

    public void OnScreenExited() => QueueFree();

    private void ManageCollision()
    {
        var collisions = Area2D.GetOverlappingBodies();
        if (collisions.Count == 0) return;
        var isCollidingWithPlayer = collisions.OfType<Player>().Any();
        if (isCollidingWithPlayer)
            GetTree().CallGroup(nameof(GameState), nameof(GameState.Hurt));
        QueueFree();

    }
}
