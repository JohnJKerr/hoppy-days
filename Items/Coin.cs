using Godot;
using System;
using System.Linq;

public class Coin : Node2D
{
    private bool _isTaken = false;
    private AnimationPlayer AnimationPlayer => GetNode<AnimationPlayer>(nameof(AnimationPlayer));
    private AudioStreamPlayer2D AudioStreamPlayer2D => GetNode<AudioStreamPlayer2D>(nameof(AudioStreamPlayer2D));
    
    public override void _Ready()
    {
        
    }

    public void OnArea2DBodyEntered(Node body)
    {
        if (_isTaken) return;
        _isTaken = true;
        AnimationPlayer.Play("die");
        AudioStreamPlayer2D.Play();
        GetTree().CallGroup(nameof(GameState), nameof(GameState.CoinUp));
    }

    public void Die()
        => QueueFree();
}
