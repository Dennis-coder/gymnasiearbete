using Godot;
using System;

public class UIController : Control
{
    Tower tower;
    [Export]
    float circleVisualizerResolution = 0.1f;
    [Export]
    float TimerVisualizerHeight = 5;

    float prevTimerVal;
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

    public override void _Process(float delta) {
        if (tower != null && prevTimerVal != tower.GetShootTimer()) {
            
            Update();
            prevTimerVal = tower.GetShootTimer();
        }

        
    }
    public override void _Draw() {
        if (tower != null) {
            DrawEmptyCircle(tower.GetPosition() + new Vector2(12, 12), new Vector2(tower.GetRange(),tower.GetRange()), Color.ColorN("white"), circleVisualizerResolution);
            DrawRect(new Rect2(tower.GetPosition() + new Vector2(0, 24), tower.GetShootTimer()*24, TimerVisualizerHeight), Color.ColorN("white"));
            GD.Print(tower.GetRange());
        }
        
        
    }

    public void DrawEmptyCircle(Vector2 circleCenter, Vector2 circleRadius, Color color, float resolution) {
        float drawCounter = 1;
        Vector2 lineOrigin = new Vector2();
        Vector2 lineEnd = new Vector2();
        lineOrigin = circleRadius + circleCenter;

        while (drawCounter <= 360) {
            lineEnd = circleRadius.Rotated(Mathf.Deg2Rad(drawCounter)) + circleCenter;
            DrawLine(lineOrigin, lineEnd, color);
            drawCounter += 1 / resolution;
            lineOrigin = lineEnd;
            
        }
        DrawLine(circleCenter, circleCenter + new Vector2(36,36), Color.ColorN("white"));
        lineEnd = circleRadius.Rotated(Mathf.Deg2Rad(360)) + circleCenter;
        DrawLine(lineOrigin, lineEnd, color);

    }

    public void SetTower(Tower t) {
        tower = t;
        Update();
    }
    public void DisableTowerVizualiser() {
        tower = null;
    }

    // func draw_empty_circle(Vector 2 circle_center, Vector2 circle_radius, Color color, int resolution):
	// var draw_counter = 1
	// var line_origin = Vector2()
	// var line_end = Vector2()
	// line_origin = circle_radius + circle_center

	// while draw_counter <= 360:
	// 	line_end = circle_radius.rotated(deg2rad(draw_counter)) + circle_center
	// 	draw_line(line_origin, line_end, Color)
	// 	draw_counter += 1 / resolution
	// 	line_origin = line_end

	// line_end = circle_radius.rotated(deg2rad(360)) + circle_center
	// draw_line(line_origin, line_end, Color

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
