using Godot;
using System;

public class RangeVisualizer : Control
{
    [Export]
    float circleVisualizerResolution = 0.1f;
    Tower tower;
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    
    public override void _Draw() {
        if (tower != null) {
            DrawEmptyCircle(tower.GetPosition() + new Vector2(12, 12), new Vector2(tower.GetRange(), tower.GetRange()), Color.ColorN("white"), circleVisualizerResolution);
            if (tower is Shotgun) {
                Shotgun shot = tower as Shotgun;
                float spread = shot.GetSpreadDegrees();

                Vector2 dir = Vector2.Right;

                DrawLine(
                        tower.GetPosition() + new Vector2(12, 12),
                        tower.GetPosition() + new Vector2(12, 12) + tower.GetRange() * dir.Rotated(Mathf.Deg2Rad(-spread/2)),
                        Color.ColorN("white")
                    );
                DrawLine(
                        tower.GetPosition() + new Vector2(12, 12),
                        tower.GetPosition() + new Vector2(12, 12) + tower.GetRange() * dir.Rotated(Mathf.Deg2Rad(spread/2)),
                        Color.ColorN("white")
                    );

            }
        }
    }

    public void SetTower(Tower t) {
        SetVisible(true);

        tower = t;

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
