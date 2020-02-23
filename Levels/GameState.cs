using Godot;
using System;
using System.Threading.Tasks;
using hoppydays.Player;

public class GameState : Node2D
{
    private int _lives = 3;
    private int _coins = 0;
    private const int CoinTarget = 10;

    public override void _Ready()
    {
        AddToGroup(nameof(GameState));
        UpdateGUI();
    }

    private Player Player => GetNode<Player>(nameof(Player));
    private bool IsLevelUp => _coins % CoinTarget == 0;

    public void OnPlayerOutOfBounds()
        => EndGame();

    internal async void Hurt()
    {
        await Player.Hurt();
        _lives--;
        if(_lives < 0) EndGame();
        UpdateGUI();
    }

    internal void CoinUp()
    {
        _coins++;
        if (IsLevelUp) _lives++; 
        UpdateGUI();
    }

    private void UpdateGUI()
    {
        GetTree().CallGroup(nameof(GUI), nameof(GUI.UpdateLives), _lives);
        GetTree().CallGroup(nameof(GUI), nameof(GUI.UpdateCoins), _coins);
    }

    private void EndGame()
    {
        GetTree().ChangeScene("res://Levels/GameOver.tscn");
    }

    internal void WinGame()
    {
        GetTree().ChangeScene("res://Levels/Victory.tscn");
    }
}
