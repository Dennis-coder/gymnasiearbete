using Godot;
using System;

public class Explosion : Node2D
{
    public float damageFalloff;
    public float damage;
    float range = 48;
    Area2D explosionArea;
    CircleShape2D radius;

    bool wait = true;

    public override void _Ready()
    {
        radius = (FindNode("Range") as CollisionShape2D).GetShape() as CircleShape2D;
        radius.Radius = range;

        explosionArea = GetNode<Area2D>("ExplosionArea");

        
    }

    public override void _PhysicsProcess(float delta) {
        if (wait) {
            wait = false;
            return;
        }

        Godot.Collections.Array areas = explosionArea.GetOverlappingAreas();
        GD.Print(areas.Count);
        foreach (Area2D area in areas) {
            Enemy enemy = area.GetParent() as Enemy;

            if (enemy != null) {
                enemy.RegisterHit(damage);
            }
        }

        GD.Print("Kabom");
        QueueFree();
    }

    

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
