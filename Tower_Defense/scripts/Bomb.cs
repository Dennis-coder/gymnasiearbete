using Godot;
using System;

public class Bomb : Projectile
{
    public Vector2 origin;
    public Vector2 targetPos;
    PackedScene explosionScene;

    Node2D pellet;


    public override void _Ready() {
        base._Ready();
        pellet = GetNode<Node2D>("Pellet");

        explosionScene = (PackedScene)ResourceLoader.Load("res://Scenes/Effects/Explosion.tscn");
    }

    public override void _Process(float delta) {
        Vector2 newPos = (GetPosition() + dir * speed * delta);
        float dist = GetPosition().DistanceTo(newPos);
        SetPosition(newPos);

        if (origin.DistanceTo(targetPos) <= origin.DistanceTo(GetPosition())) {
            Hit();
        }

        float lT = 0f;

        float progress = origin.DistanceTo(GetPosition())/origin.DistanceTo(targetPos);

        

        

        if (origin.DistanceTo(GetPosition()) < origin.DistanceTo(targetPos)) {
            pellet.SetScale(GetScale() + new Vector2(.1f, .1f));
            Vector2 oPos = new Vector2(0,0);
            Vector2 pPos = oPos.LinearInterpolate(oPos + new Vector2(0, -50), progress*2);
            pellet.SetPosition(pPos);
        } else {
            pellet.SetScale(GetScale() - new Vector2(.1f, .1f));
            Vector2 oPos = new Vector2(0,-50);
            Vector2 pPos = oPos.LinearInterpolate(oPos + new Vector2(0, 0), progress*2);
            pellet.SetPosition(pPos);
        }

    }

    protected override void Hit(object obj = null) {
        Explosion explosion = explosionScene.Instance() as Explosion;

        explosion.SetPosition(targetPos);
        explosion.damage = damage;
        explosion.damageFalloff = 5;


        GetTree().GetRoot().GetNode("World").AddChild(explosion);

        QueueFree();
    }
}
