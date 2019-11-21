using Godot;
using System;

public class Enemy : Node2D
{
    [Export]
    float speed = 10;
    GameController gameController;
    Vector2[] path;

    public override void _Ready() {
        gameController = GetTree().GetRoot().GetNode("World") as GameController; 
        path = gameController.RequestPath();
    }

    public override void _Process(float delta)
    {
        float moveDist = speed * delta;
        pathFollow(moveDist);

    }

    void setPath(Vector2[] value) {
        path = value;
        if (value.GetLength(0) == 0) {
            return;
        } 

        SetProcess(true);
    }


    void pathFollow(float dist) {
        Vector2 pos = GetPosition();
        for (int i = 0; i < path.GetLength(0); i++) {
            float distToNext = pos.DistanceTo(path[i]);
            if (dist <= distToNext && dist >= 0.0) {
                SetPosition(pos.LinearInterpolate(path[i], dist / distToNext));
                break;
            }
            else if (dist < 0.0) {
                SetPosition(path[i]);
                SetProcess(false);
                break;
            }
            dist -= distToNext;
            pos = path[i];
        }
    }
}
