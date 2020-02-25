using Godot;
using System;

public class Bomb : Projectile
{
    public Vector2 origin;
    public Vector2 targetPos;

    PackedScene explosionScene;

    public override void _Ready() {
        base._Ready();

        explosionScene = (PackedScene)ResourceLoader.Load("res://Scenes/Effects/Explosion.tscn");
    }

    public override void _Process(float delta) {
        Vector2 newPos = (GetPosition() + dir * speed * delta);
        float dist = GetPosition().DistanceTo(newPos);
        SetPosition(newPos);

        if (origin.DistanceTo(targetPos) <= origin.DistanceTo(GetPosition())) {
            Hit();
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
