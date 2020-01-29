using Godot;
using System;
using System.Collections.Generic;

public class GameController : Node2D
{
    //SCENES
    [Export]
    PackedScene gameOverScene;


    //HEALTH
    [Export]
    float startingHealth = 100;
    float health;
    Camera2D ye;
    

    //GRID AND PATHFINDING
    TileMap worldGrid;
    Navigation2D nav2d;
    Node2D spawn;
    Node2D goal;
    Vector2[] path = new Vector2[0];
    Line2D debugLine;
    Line2D debugLineSnapped;

    //MONEY
    float balance;

    //TOWERS
    float resellFactor = 0.6f;
    Dictionary<string, PackedScene> towerScenes = new Dictionary<string, PackedScene>();
    Dictionary<string, float> towerCosts = new Dictionary<string, float>();


    //ENEMIES
    Dictionary<string, PackedScene> enemyScenes = new Dictionary<string, PackedScene>();

    public override void _Ready()
    {
        
        //START HEALTH
        health = startingHealth;

        //ASSIGN WORLD GRID
        worldGrid = FindNode("WorldGrid") as TileMap;

        //LOAD TOWER SCENE
        towerScenes.Add("Tower", (PackedScene)ResourceLoader.Load("res://Scenes/Towers/Tower.tscn"));
        towerCosts.Add("Tower", 40);

        //LOAD ENEMY SCENE
        enemyScenes.Add("Enemy", (PackedScene)ResourceLoader.Load("res://Scenes/Enemies/Enemy.tscn"));


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

        EarnMoney(100);
    }

    public Vector2[] RequestPath() {
        return path;
        //yeet 
    }

    void SpawnEnemy(string enemyType) {
        PackedScene scene = enemyScenes[enemyType];

        Node2D enemy = scene.Instance() as Node2D;
        enemy.SetPosition(spawn.GetGlobalPosition());
        AddChild(enemy);
    }


//  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta) {

        if (Input.IsActionJustPressed("ui_select")) {
            SpawnEnemy("Enemy");
        }

    }

    //-------------------------PRIVATE FUNCTIONS------------------------------------------------


    //PATH-----------------------------------------------
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

    //GAME OVER
    void GameOver() {
        GD.Print("GAME OVER");
        // GetTree().ChangeScene("res://Scenes/Game States/GameOverState.tscn");
        GetTree().ChangeSceneTo(gameOverScene);
    }

    //-------------------------------------------------PUBLIC FUNCTIONS--------------------------------------------------------
    //DAMAGE OSV---------------------------------------------------------
    public void TakeDamage(float amount) {
        health -= amount;
        
        GD.Print("Damage Taken: ", amount, ". Remaining Health: ", health);
        
        if (health <= 0)
        {
            GameOver();
        }
    }


    //TOWER------------------------------------------------------------
    public void PlaceTower(string towerType, Vector2 gridPos) {
        PackedScene scene = towerScenes[towerType];
        float cost = towerCosts[towerType];
        //checka marktyp
        int cellType = worldGrid.GetCellv(gridPos);
        
        if (cellType > 2) { 
            return;
        }

        
        //checka para
        if (!SpendMoney(cost)) {
            return;
        }
        
        

        Vector2 gridWorldPos = worldGrid.MapToWorld(gridPos);
        
        Node2D tower = scene.Instance() as Node2D;
        tower.SetPosition(gridWorldPos);
        AddChild(tower);
    }

    public void SellTower(Tower tower) {
        float value = towerCosts[tower.type] * resellFactor;
        EarnMoney(value);

        tower.QueueFree();
    }


    //BALANCE------------------------------------------------------------
    public bool SpendMoney(float amount) {
        if (balance >= amount) {
            balance -= amount;

            //EV UPPDATERA UI
            GD.Print("Success. New balance: ", balance);

            return true;
        }

        GD.Print("Not enough para");
        return false;
    }

    public void EarnMoney(float amount) {
        balance += amount;

        //EV UPPDATERA UI
        GD.Print("New balance: ", balance);
    }
}