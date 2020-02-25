using Godot;
using System;

public class Mortar : Tower
{
    AnimationPlayer anim;

    public override void _Ready()
    {
        base._Ready();   

        anim = FindNode("AnimationPlayer") as AnimationPlayer;
    }

    protected override void Shoot(Vector2 targetPos) {
        anim.Play("shoot");

        Bomb projectile = projectileType.Instance() as Bomb;
        Vector2 rootPos = GetPosition();
        rootPos.x += 12;
        rootPos.y += 12;
        projectile.SetPosition(rootPos);
        projectile.origin = rootPos;
        projectile.targetPos = targetPos;
        projectile.dir = rootPos.DirectionTo(targetPos + new Vector2(12, 12));
        projectile.damage = damage;
        GetTree().GetRoot().GetNode("World").AddChild(projectile);
    }
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
