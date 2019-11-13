using Godot;
using System;

public class Tower : Node
{

    [Export]
    float damage;
    [Export]
    float hp;
    [Export]
    float rateOfFire;
    [Export]
    PackedScene projectileType;
    [Export]
    float range;
    [Export]
    CircleShape2D towerRange;
    float level;


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
