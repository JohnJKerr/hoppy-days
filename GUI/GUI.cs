using Godot;
using System;

public class GUI : CanvasLayer
{
    private const string LabelPath = "Control/TextureRect/HBoxContainer/{0}";
    private Label LifeCount => GetNode<Label>(string.Format(LabelPath, nameof(LifeCount)));
    private Label CoinCount => GetNode<Label>(string.Format(LabelPath, nameof(CoinCount)));
    
    public override void _Ready()
    {
    }

    internal void UpdateLives(int lives)
        => LifeCount.Text = lives.ToString();

    internal void UpdateCoins(int coins)
        => CoinCount.Text = coins.ToString();
}
