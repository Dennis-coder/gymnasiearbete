using Godot;
using System;

public class Projectile : Sprite
{
    [Export]
    protected float speed = 1;
    [Export]
    float lifetime = 2;
    RayCast2D ray;

    Timer t;

    public Vector2 dir = new Vector2(0.5f,0.5f);  
    public float damage;
    
    public override void _Ready() {
        SetRotation(GetAngleTo(GetPosition() + dir));

        t = GetNode<Timer>("Timer");
        t.WaitTime = lifetime;
        t.Start();
        ray = GetNode<RayCast2D>("Ray");

    }

    public override void _Process(float delta) {
        Vector2 newPos = (GetPosition() + dir * speed * delta);
        float dist = GetPosition().DistanceTo(newPos);
        SetPosition(newPos);
        ray.SetPosition(new Vector2(-dist,0));
        ray.CastTo = new Vector2(dist, 0);

        if (ray.IsColliding()) {
            Hit(ray.GetCollider());
        }
    }

    protected virtual void Hit(object obj = null) {
        Area2D area = obj as Area2D;
        if (area == null)
            return;
        Enemy enemy = area.GetParent() as Enemy;
        if (enemy == null)
            return;
        
        
        enemy.RegisterHit(damage);

        QueueFree();
    }

    public void _on_Timer_timeout() {
        // GD.Print("Soy muerte");
        QueueFree();
    }
}
