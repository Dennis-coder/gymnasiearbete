using Godot;
using System;

public class Enemy : Node2D
{
    [Export]
    float speed = 10;
    [Export]
    float hp = 1000;
    [Export]
    float value = 20;
    protected GameController gameController;
    protected Vector2[] path = new Vector2[0];
    Vector2 startPos;
    protected int target = 1;
    
    AnimationPlayer anim;


    public override void _Ready() {

        gameController = GetTree().GetRoot().GetNode("World") as GameController; 
        path = gameController.RequestPath();

        anim = FindNode("AnimationPlayer") as AnimationPlayer;

        startPos = GetPosition();

        UpdateAnimation();
    }

    public override void _Process(float delta)
    {
        float moveDist = speed * delta;
        pathFollow(moveDist);

        if (Input.IsActionJustPressed("ui_select")) {
            Die();
        }
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
        if (GetPosition().DistanceTo(startPos) >= path[target].DistanceTo(startPos)) {
            startPos = path[target];
            SetPosition(startPos);
            target++;

            UpdateAnimation();
        }
    }

    public void SetTarget(int t) {
        target = t;
    }

    void Arrived() {
        gameController.UpdateHealth(-hp);
        GD.Print("ARRIVED");
        Die();
    }

    public void RegisterHit(float dmg) {
        hp -= dmg;

        if (hp <= 0) {
            gameController.EarnMoney(value);
            Die();
        }
    }

    public virtual void Die() {
        gameController.EnemyKilled();
        QueueFree();
    }

    private void UpdateAnimation() {
        if (anim == null) {
            return;
        }

        if (target >= path.Length) {
            return;
        }

        Vector2 animDir = new Vector2(path[target]-startPos).Normalized();

        if (animDir == Vector2.Right) {
            anim.Play("run_right");
        } else if (animDir == Vector2.Left) {
            anim.Play("run_left");
        } else if (animDir == Vector2.Down) {
            anim.Play("run_down");
        } else if (animDir == Vector2.Up) {
            anim.Play("run_up");
        }

    }
}
