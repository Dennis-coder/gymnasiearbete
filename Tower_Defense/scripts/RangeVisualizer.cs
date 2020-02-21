using Godot;
using System;

public class RangeVisualizer : Control
{
    [Export]
    float circleVisualizerResolution = 0.1f;
    float range = 0;
    Vector2 pos;
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    
    public override void _Draw() {
        DrawEmptyCircle(pos, new Vector2(range, range), Color.ColorN("white"), circleVisualizerResolution);
    }

    public void SetRange(Vector2 p, float r) {
        SetVisible(true);
        pos = p;
        range = r;

        Update();
    }

    void DrawEmptyCircle(Vector2 circleCenter, Vector2 circleRadius, Color color, float resolution) {
        float drawCounter = 1;
        Vector2 lineOrigin = new Vector2();
        Vector2 lineEnd = new Vector2();
        circleRadius.x = Mathf.Cos(Mathf.Pi/4) * circleRadius.x;
        circleRadius.y = Mathf.Sin(Mathf.Pi/4) * circleRadius.y;
        lineOrigin = circleRadius + circleCenter;

        while (drawCounter <= 360) {
            lineEnd = circleRadius.Rotated(Mathf.Deg2Rad(drawCounter)) + circleCenter;
            DrawLine(lineOrigin, lineEnd, color);
            drawCounter += 1 / resolution;
            lineOrigin = lineEnd;
            
        }
        lineEnd = circleRadius.Rotated(Mathf.Deg2Rad(360)) + circleCenter;
        DrawLine(lineOrigin, lineEnd, color);

    }


    
}
