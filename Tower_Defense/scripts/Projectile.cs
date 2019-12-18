using Godot;
using System;

public class Projectile : Sprite
{
    [Export]
    float speed = 1;
    [Export]
    float lifetime = 2;

    public Vector2 dir = new Vector2(1, 0);

    public override void _Ready() {
        SetRotation(GetAngleTo(GetPosition() + dir));

        GetNode<Timer>("Timer").WaitTime = lifetime; 
    }

    public override void _Process(float delta) {
        SetPosition(GetPosition() + dir * speed * delta);
    }

    public void _on_Timer_timeout() {
        GD.Print("Soy muerte");
        QueueFree();
    }
}
