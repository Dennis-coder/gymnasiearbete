using Godot;
using System;
using System.Collections.Generic;

public class GameController : Node2D
{
    TileMap worldGrid;
    Navigation2D nav2d;
    Node2D spawn;
    Node2D goal;
    Vector2[] path = new Vector2[0];
    Line2D debugLine;
    Line2D debugLineSnapped;

    PackedScene enemyScene;

    public override void _Ready()
    {
        //ASSIGN WORLD GRID
        worldGrid = FindNode("WorldGrid") as TileMap;

        //LOAD ENEMY SCENE
        enemyScene = (PackedScene)ResourceLoader.Load("res://Scenes/Enemies/Enemy.tscn");


        //PATH FINDING
        debugLine = FindNode("DebugLine") as Line2D;
        debugLineSnapped = FindNode("DebugLineSnapped") as Line2D;
        nav2d = GetNode<Navigation2D>("Navigation2D");
        spawn = GetNode<Node2D>("Spawn");
        goal = GetNode<Node2D>("ParasitNiklas");
        path = nav2d.GetSimplePath(spawn.GetGlobalPosition(), goal.GetGlobalPosition(), false);
        debugLine.Points = path;
        
        //ALIGN PATH TO GRID
        GD.Print(path.Length);
        path = PathSnapToGrid(path);
        GD.Print(path.Length);
        debugLineSnapped.Points = path;

    }

    public Vector2[] RequestPath() {
        return path;
        //yeet 
    }

    void SpawnEnemy() {
        Node2D enemy = enemyScene.Instance() as Node2D;
        enemy.SetPosition(spawn.GetGlobalPosition());
        GetTree().GetRoot().GetNode("World").AddChild(enemy);
    }


//  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta) {

        if (Input.IsActionJustPressed("ui_select")) {
            SpawnEnemy();
        }

    }



    Vector2[] PathSnapToGrid(Vector2[] sPath) {
        List<Vector2> newPath = new List<Vector2>();
        for (int i = 0; i < sPath.Length; i++) {
            Vector2 newPos = worldGrid.WorldToMap(sPath[i]);
            Vector2 gridPos = newPos;

            //FIXA FEL EV FEL vid svängarna
            if (i < sPath.Length-1) {
                Vector2 nextGridPos = worldGrid.WorldToMap(sPath[i+1]);

                int xDif = (int)(nextGridPos.x-gridPos.x);
                int yDif = (int)(nextGridPos.y-gridPos.y);


                if (xDif != 0 && yDif != 0) {
                    if (xDif == -1 && yDif == 1) {

                        i++;
                        newPos = worldGrid.WorldToMap(sPath[i]);
                        newPos.x -= 0;
                        newPos.y -= 1;

                    }
                }
                if (xDif == 0 && yDif == 0) {
                    i++;
                    newPos = worldGrid.WorldToMap(sPath[i]);
                }

            }

            newPos = worldGrid.MapToWorld(newPos);

            newPath.Add(newPos);
        }

        //TA BORT RAKSTRÄCKOR
        newPath = RemoveRedundantPoints(newPath);

        return newPath.ToArray();
    }

    //ta bort raksträckor
    List<Vector2> RemoveRedundantPoints(List<Vector2> input) {
        List<Vector2> output = new List<Vector2>();

        output.Add(input[0]);

        for (int i = 1; i < input.Count-1; i++) {
            Vector2 prev = input[i-1];
            Vector2 cur = input[i];
            Vector2 next = input[i+1];

            float x = (prev.x/cur.x) + (cur.x/next.x);
            float y = (prev.y/cur.y) + (cur.y/next.y);
            

            if (!(x == 2 || y == 2)) {
                output.Add(cur);

            }
        }

        output.Add(input[input.Count-1]);

        return output;
    }
}