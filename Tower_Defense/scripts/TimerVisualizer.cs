using Godot;
using System;

public class TimerVisualizer : Control
{
    [Export]
    float TimerVisualizerHeight = 5;

    float prevTimerVal;

    Tower tower;
    NodePath towerPath;


    public override void _Process(float delta)
    {
        if (tower != null && prevTimerVal != tower.GetShootTimer()) {            
            Update();
            prevTimerVal = tower.GetShootTimer();
        }

    }

    public override void _Draw() {
        if (towerPath != null) {
        if (GetTree().GetRoot().GetNode("World").HasNode(towerPath)) {
            if (tower != null) {
                DrawRect(new Rect2(tower.GetPosition() + new Vector2(0, 24), tower.GetShootTimer()*24, TimerVisualizerHeight), Color.ColorN("white"));
            }
        }}
    }

    public void SetTower(Tower t) {
        SetVisible(true);
        tower = t;

        if (tower != null) {
            towerPath = tower.GetPath();

        }

    }
}
