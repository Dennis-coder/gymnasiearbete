using Godot;
using System;

public class Enemy : Node2D
{
    [Export]
    float speed = 10;
    GameController gameController;
    Vector2[] path = new Vector2[0];
    Vector2 startPos;
    int target = 1;


    public override void _Ready() {

        gameController = GetTree().GetRoot().GetNode("World") as GameController; 
        path = gameController.RequestPath();


        startPos = GetPosition();
        
    }

    public override void _Process(float delta)
    {
        float moveDist = speed * delta;
        pathFollow(moveDist);

    }

    public float DistanceToGoal() {
        if (target >= path.Length) {
            return 0;
        }

        float total = GetPosition().DistanceTo(path[target]);

        for (int i = target; i < (path.Length - 1); i++) {
            total += path[i].DistanceTo(path[i+1]);
        }

        return total;
    }

    public float FastDistanceToGoal() {
        if (target >= path.Length) {
            return 0;
        }

        float total = GetPosition().DistanceTo(path[target]);
        total += (path.Length - target)*1000;

        return total;
    }


    void pathFollow(float dist) {

        if (target >= path.Length) {
            Arrived();
            return;
        }

        float distToNext = startPos.DistanceTo(path[target]);
        Vector2 movement = startPos.DirectionTo(path[target]) * dist;
        SetPosition(GetPosition() + movement);
        if (GetPosition().DistanceTo(startPos) > path[target].DistanceTo(startPos)) {
            startPos = path[target];
            SetPosition(startPos);
            target++;
        }
    }

    void Arrived() {
        GD.Print("ARRIVED");
    }
}
