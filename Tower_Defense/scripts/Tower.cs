using Godot;
using System;
using System.Collections.Generic;

public class Tower : Node2D
{

    [Export]
    float damage = 1;
    [Export]
    float hp = 10;
    [Export]
    float rateOfFire = 95;
    [Export]
    PackedScene projectileType;
    [Export]
    float range = 128;
    [Export]
    CircleShape2D towerRange;

    Vector2 shootDir;
    
    float level = 1;
    IList<Node2D> enemiesInRange = new List<Node2D>();

    public override void _Ready()
    {
        towerRange = (FindNode("Range") as CollisionShape2D).GetShape() as CircleShape2D;
        towerRange.Radius = range;
    }

    public override void _Process(float delta) {
        // Shoot da first enemy in list (temporary)
        if (enemiesInRange.Count > 0) {
            GD.Print(enemiesInRange[0].GetName());


        }
        // When builds are merged check which enemy has the smallest distance to goal thru variablel (final)
    }

    private void _on_Detection_Enemy_area_entered(Area2D area) {
        if(area.GetName() == "EnemyHitbox") {
            enemiesInRange.Add(area.GetParent() as Node2D);

            GD.Print(enemiesInRange.Count);
        }
    }

    private void _on_Detection_Enemy_area_exited(Area2D area) {
        if(area.GetName() == "EnemyHitbox") {
            enemiesInRange.Remove(area.GetParent() as Enemy);

            GD.Print(enemiesInRange.Count);
        }
    }
}
