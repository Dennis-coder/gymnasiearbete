using Godot;
using System;

public class UIController : Control
{
    Tower tower;
    RangeVisualizer rangeVisualizer;
    TimerVisualizer timerVisualizer;
    
 

    
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        rangeVisualizer = FindNode("RangeVisualizer") as RangeVisualizer;
        timerVisualizer = FindNode("TimerVisualizer") as TimerVisualizer;
    }

    public override void _Process(float delta) {

        
    }

    

    public void SetTower(Tower t) {
        tower = t;
        timerVisualizer.SetTower(tower);
        rangeVisualizer.SetTower(tower);

        if (t == null) {
            rangeVisualizer.SetVisible(false);
            timerVisualizer.SetVisible(false);
        }

    }
}
