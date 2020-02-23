using Godot;
using System;

public class Cloud : Node2D
{
    private bool _timeout = false;
    private RayCast2D RayCast2D => GetNode<RayCast2D>($"{nameof(Sprite)}/{nameof(RayCast2D)}");
    private Timer Timer => GetNode<Timer>($"{nameof(Sprite)}/{nameof(Timer)}");
    
    public override void _Process(float delta)
    {
        base._Process(delta);
        if (RayCast2D.IsColliding())
            Fire();
    }

    public void OnTimerTimeout() => _timeout = false;

	private void Fire()
    {
        if (_timeout) return;
        var lightning = GD.Load<PackedScene>("res://Enemies/Lightning.tscn");
        RayCast2D.AddChild(lightning.Instance());
        Timer.Start();
        _timeout = true;
    }
}
