using Godot;
using System;

public class Shotgun : Tower
{
    [Export(new PropertyHint(), "bruh")]
    float spreadDegrees;
    float minValue;
    float maxValue;
    [Export]
    float accuracy;
    [Export]
    float FragmentAmounts;

    override public void _Ready() {
        base._Ready();

        minValue = -spreadDegrees/2;
        maxValue = spreadDegrees/2;

        GD.Print(Vector2.Right);
        GD.Print(Vector2.Right.Rotated(Mathf.Pi/2));
    }

    protected override void Shoot(Vector2 targetPos) {
        Random rand = new Random();
        for (int i = 0; i < FragmentAmounts; i++) {
            Projectile projectile = projectileType.Instance() as Projectile;
            Vector2 rootPos = GetPosition();
            rootPos.x += 12;
            rootPos.y += 12;
            projectile.SetPosition(rootPos);
            Vector2 pDir = rootPos.DirectionTo(targetPos + new Vector2(12, 12));
            projectile.dir = pDir.Rotated(
                Mathf.Deg2Rad(
                    (float) (rand.NextDouble() * (maxValue - minValue) + minValue)
            ));
            projectile.damage = damage;
            GetTree().GetRoot().GetNode("World").AddChild(projectile);
        }

    }

    public float GetSpreadDegrees() {
        return spreadDegrees;
    }

}
