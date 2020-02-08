using Godot;
using System;

public class Carriage : Enemy
{
    [Export]
    string content = "Goblin";
    [Export]
    int contentAmount = 10;

    // Called when the node enters the scene tree for the first time.
    Tuple<Vector2, int> BackTrackPath(Vector2 startPos, float distance) {
        int t = target - 1;
        Vector2 pos = startPos;
        GD.Print("bruh");

        distance += .1f;


        while (distance > 0) {
            GD.Print("ye");

            

            float d = pos.DistanceTo(path[t]);
            GD.Print(pos, " ", d);


            if (d < distance) {
                distance -= d;
                pos = path[t];
                t--;
                

            } else {

                t++;
                float pr = distance/path[t-1].DistanceTo(path[t]);
                distance = 0;

                pos = path[t-1].LinearInterpolate(path[t], pr);
            }

            //Om den backar längre än spawn;
            if (t < 1) {
                
                pos = path[0] + new Vector2(path[0]-path[1]).Normalized()*distance;
                t = 1;
                distance = 0;
            }
        }
    


        return Tuple.Create(pos, t);
    }

    public override void Die() {

        for (int i = 0; i < contentAmount; i++) {
            var tuple = BackTrackPath(GetPosition(), 10*i);

            GD.Print("Spawning Enemy at: ", tuple.Item1, " with target: ", tuple.Item2, " at: ", path[tuple.Item2]);
            gameController.SpawnEnemyInProgress(content, tuple.Item1, tuple.Item2);
        }
        GD.Print("Died at: ", GetPosition(), " with target: ", target, " at: ", path[target]);

        
        base.Die();
    }
}
