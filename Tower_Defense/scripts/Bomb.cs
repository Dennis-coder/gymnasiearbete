using Godot;
using System;

public class Bomb : Projectile
{
    public Vector2 origin;
    public Vector2 targetPos;

    PackedScene explosionScene;
    Sprite pellet;
    float yDir;

    public override void _Ready() {
        base._Ready();

        pellet = GetNode<Sprite>("Pellet");

        yDir = (dir.x < 0) ? 1 : -1;

        explosionScene = (PackedScene)ResourceLoader.Load("res://Scenes/Effects/Explosion.tscn");
    }

    public override void _Process(float delta) {
        Vector2 newPos = (GetPosition() + dir * speed * delta);
        float dist = GetPosition().DistanceTo(newPos);
        SetPosition(newPos);

        
        float progress = origin.DistanceTo(GetPosition())/origin.DistanceTo(targetPos);

        pellet.SetPosition(QuadraticBezier(new Vector2(0, 0), new Vector2(0, 50 * yDir), new Vector2(0, 0), progress));
        pellet.SetScale(QuadraticBezier(new Vector2(1, 1), new Vector2(1.5f, 1.5f), new Vector2(1, 1), progress));


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

    private Vector2 QuadraticBezier(Vector2 p0, Vector2 p1, Vector2 p2, float t) {
        Vector2 q0 = p0.LinearInterpolate(p1, t);
        Vector2 q1 = p1.LinearInterpolate(p2, t);

        Vector2 r = q0.LinearInterpolate(q1, t);

        return r;
    }

}
