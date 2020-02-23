using Godot;
using System;
using hoppydays.Player;

public class JumpPad : Area2D
{
    private AnimationPlayer AnimationPlayer => GetNode<AnimationPlayer>(nameof(AnimationPlayer));

    public override void _Ready()
    {
        
    }

    public async void OnBodyEntered(Player player)
    {
        AnimationPlayer.Play("boost");
        await player.Boost();
    }
}
