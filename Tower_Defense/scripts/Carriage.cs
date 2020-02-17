using Godot;
using System;

public class Carriage : Enemy
{
    [Export]
    string content = "Goblin";
    [Export]
    int contentAmount = 10;

    // Called when the node enters the scene tree for the first time.
    

    public override void Die() {

        Vector2 pos = GetPosition();
        int nT = target;
        for (int i = 0; i < contentAmount; i++) {
            var tuple = gameController.BackTrackPath(pos, path, 24, nT);

            GD.Print("Spawning Enemy at: ", gameController.worldGrid.WorldToMap(tuple.Item1), " with target: ", tuple.Item2, " at: ", gameController.worldGrid.WorldToMap(path[tuple.Item2]));
            gameController.SpawnEnemyInProgress(content, tuple.Item1, tuple.Item2);
            pos = tuple.Item1;
            nT = tuple.Item2;
        }
        GD.Print("Died at: ", gameController.worldGrid.WorldToMap(GetPosition()), " with target: ", target, " at: ", gameController.worldGrid.WorldToMap(path[target]));

        
        base.Die();
    }
}
