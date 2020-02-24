using Godot;
using System;
using System.Collections.Generic;

public class Tower : Node2D
{
    [Export]
    public string type = "Tower";
    [Export]
    protected float damage = 20;
    [Export]
    float hp = 10;
    [Export]
    float rateOfFire;
    [Export]
    protected PackedScene projectileType;
    [Export]
    float range = 128;
    [Export]
    CircleShape2D towerRange;

    Vector2 shootDir;
    AnimationPlayer anim;

    float level = 1;
    IList<Enemy> enemiesInRange = new List<Enemy>();

    float shootTimer;

    public override void _Ready()
    {
        towerRange = (FindNode("Range") as CollisionShape2D).GetShape() as CircleShape2D;
        towerRange.Radius = range;

        GD.Print(towerRange.GetRadius(), " vs ", range);

        // shootTimer = rateOfFire;

        anim = FindNode("AnimationPlayer") as AnimationPlayer;
        anim.Play("idle");
    }

    public override void _Process(float delta) {
        if (enemiesInRange.Count > 0) {
            float lowest = enemiesInRange[0].FastDistanceToGoal();
            int t = 0;

            for (int i = 1; i < enemiesInRange.Count; i++) {
                float dist = enemiesInRange[i].FastDistanceToGoal();

                if (dist < lowest) {
                    lowest = dist;
                    t = i;
                }
            }
            if (shootTimer <= 0) {
                shootTimer = rateOfFire;
                Shoot(enemiesInRange[t].GetPosition());
            }

        }

        
        shootTimer = (shootTimer > 0) ? shootTimer - delta : 0;
    }

    private void _on_Detection_Enemy_area_entered(Area2D area) {
        if(area.GetName() == "EnemyHitbox") {
            enemiesInRange.Add(area.GetParent() as Enemy);

            // GD.Print(enemiesInRange.Count);
        }
    }

    private void _on_Detection_Enemy_area_exited(Area2D area) {
        if(area.GetName() == "EnemyHitbox") {
            enemiesInRange.Remove(area.GetParent() as Enemy);

            // GD.Print(enemiesInRange.Count);
        }
    }

    protected virtual void Shoot(Vector2 targetPos) {
        Projectile projectile = projectileType.Instance() as Projectile;
        Vector2 rootPos = GetPosition();
        rootPos.x += 12;
        rootPos.y += 12;
        projectile.SetPosition(rootPos);
        projectile.dir = rootPos.DirectionTo(targetPos + new Vector2(12, 12));
        projectile.damage = damage;
        GetTree().GetRoot().GetNode("World").AddChild(projectile);
    }

    public float GetRange() {
        return range;
    }

    public float GetShootTimer() {
        return shootTimer/rateOfFire;
    }
}
