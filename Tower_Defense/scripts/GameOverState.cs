using Godot;
using System;

public class GameOverState : Node
{
    [Export]
    PackedScene mainScene;

    public override void _Ready()
    {
        mainScene = (PackedScene)ResourceLoader.Load("res://Scenes/GameStates/World.tscn");
    }

    public override void _Process(float delta)
    {
        if (Input.IsActionJustPressed("ui_select")) {
            GetTree().ChangeSceneTo(mainScene);
        }
    }
}