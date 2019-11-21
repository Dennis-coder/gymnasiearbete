using Godot;
using System;

public class GameController : Node2D
{
    Navigation2D nav2d;
    Node2D spawn;
    Node2D goal;
    Vector2[] path;

    public override void _Ready()
    {
        nav2d = GetNode<Navigation2D>("Navigation2D");
        spawn = GetNode<Node2D>("Spawn");
        goal = GetNode<Node2D>("ParasitNiklas");
        var path = nav2d.GetSimplePath(spawn.GetGlobalPosition(), goal.GetGlobalPosition());

    }

    public Vector2[] RequestPath() {
        return path;
        //yeet 
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
    //     
//  }
}
