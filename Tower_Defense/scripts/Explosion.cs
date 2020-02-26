using Godot;
using System;

public class Explosion : Node2D {
    public float damageFalloff;
    public float damage;
    [Export]
    float range = 48;
    CircleShape2D radius;
    Area2D explosionArea;
    Sprite sprite;

    AnimationPlayer anim;

    public override void _Ready() {
        radius = (FindNode("Range") as CollisionShape2D).GetShape() as CircleShape2D;
        radius.Radius = range;

        anim = FindNode("AnimationPlayer") as AnimationPlayer;

        sprite = FindNode("Sprite") as Sprite;

        explosionArea = FindNode("Explosion Area") as Area2D;

        Random rand = new Random();
        sprite.Rotation =  Mathf.Deg2Rad((float) (rand.Next(0, 359)));
        sprite.Scale = new Vector2(range / 24, range / 24);
    }

    public override void _Process(float delta) {
        anim.Play("explode");
    }

    private void _on_AnimationPlayer_animation_finished(AnimationPlayer animation) {
        QueueFree();
    }

    public void _on_Explosion_Area_area_entered(Area2D area) {
        Enemy enemy = area.GetParent() as Enemy;

        if (enemy != null) {
            enemy.RegisterHit(damage);
        }

        explosionArea.QueueFree();
    }
}
