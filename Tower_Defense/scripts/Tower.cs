using Godot;
using System;

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
    float level = 1;


    public override void _Ready()
    {
        towerRange = (FindNode("Range") as CollisionShape2D).GetShape() as CircleShape2D;
        towerRange.Radius = range;
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
