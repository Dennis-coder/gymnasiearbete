using Godot;
using System;
using System.Collections.Generic;
using System.Text;

public class GameController : Node2D
{

    //WAVE CONTROLLER
    public class SpawnAction {
        public string EnemyType;
        public float PauseTime;

        public SpawnAction(string enemyType, float pauseTime) {
            this.EnemyType = enemyType;
            this.PauseTime = pauseTime;
        }
    }

    string jsonFilePath = "res://scripts/waves.json";
    Timer wavePauseTimer;
    bool waveInAction;
    int waveCount = 1;
    float RemainingEnemies;
    
    List<SpawnAction> wave = new List<SpawnAction>();


    //GRIDS
    public TileMap worldGrid;
    Navigation2D nav2d;
    Node2D spawn;
    Node2D goal;
    Vector2[] path = new Vector2[0];
    Line2D debugLine;
    Line2D debugLineSnapped;
    Line2D bDL;

    float balance;
    float healthPoints;

    //TOWERS
    float resellFactor = 0.6f;
    Dictionary<string, PackedScene> towerScenes = new Dictionary<string, PackedScene>();
    Dictionary<string, float> towerCosts = new Dictionary<string, float>();


    //ENEMIES
    Dictionary<string, PackedScene> enemyScenes = new Dictionary<string, PackedScene>();


    // GUI
    public Label money;
    public Label health;

    public override void _Ready()
    {
        
        wavePauseTimer = GetNode<Timer>("WavePauseTimer");

        wave = LoadWave(waveCount-1);


        //ASSIGN WORLD GRID
        worldGrid = FindNode("WorldGrid") as TileMap;

        //LOAD TOWER SCENE
        towerScenes.Add("Tower", (PackedScene)ResourceLoader.Load("res://Scenes/Towers/Tower.tscn"));
        towerCosts.Add("Tower", 40);
        towerScenes.Add("Shoot", (PackedScene)ResourceLoader.Load("res://Scenes/Towers/Shoot.tscn"));
        towerCosts.Add("Shoot", 40);
        towerScenes.Add("Shotgun", (PackedScene)ResourceLoader.Load("res://Scenes/Towers/Shotgun.tscn"));
        towerCosts.Add("Shotgun", 60);

        //LOAD ENEMY SCENE
        enemyScenes.Add("Enemy", (PackedScene)ResourceLoader.Load("res://Scenes/Enemies/Enemy.tscn"));
        enemyScenes.Add("EnemyBig", (PackedScene)ResourceLoader.Load("res://Scenes/Enemies/EnemyBig.tscn"));
        enemyScenes.Add("Goblin", (PackedScene)ResourceLoader.Load("res://Scenes/Enemies/Goblin.tscn"));
        enemyScenes.Add("Elephant", (PackedScene)ResourceLoader.Load("res://Scenes/Enemies/Elephant.tscn"));
        enemyScenes.Add("Fast", (PackedScene)ResourceLoader.Load("res://Scenes/Enemies/Fast.tscn"));
        enemyScenes.Add("Carriage", (PackedScene)ResourceLoader.Load("res://Scenes/Enemies/Carriage.tscn"));

        //PATH FINDING
        debugLine = FindNode("DebugLine") as Line2D;
        debugLineSnapped = FindNode("DebugLineSnapped") as Line2D;
        nav2d = GetNode<Navigation2D>("Navigation2D");
        spawn = GetNode<Node2D>("Spawn");
        goal = GetNode<Node2D>("ParasitNiklas");
        path = nav2d.GetSimplePath(spawn.GetGlobalPosition(), goal.GetGlobalPosition(), false);
        debugLine.Points = path;

        bDL = FindNode("bDL") as Line2D;
        
        //ALIGN PATH TO GRID
        GD.Print(path.Length);
        path = PathSnapToGrid(path);
        GD.Print(path.Length);
        debugLineSnapped.Points = path;


        money = GetTree().GetRoot().GetNode("World").FindNode("Money") as Label;
        GD.Print(money.GetName());

        health = GetTree().GetRoot().GetNode("World").FindNode("Health") as Label;
        GD.Print(health.GetName());

        EarnMoney(100);
        UpdateHealth(1500);

        Tuple<Vector2, int> tuple = BackTrackPath(path, worldGrid.MapToWorld(new Vector2(8,6)), 24, 3);
        GD.Print("btpi: ", tuple.Item2);
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

    public void SpawnEnemyInProgress(string enemyType, Vector2 pos, int target) {
        PackedScene scene = enemyScenes[enemyType];

        Enemy enemy = scene.Instance() as Enemy;
        enemy.SetPosition(pos);
        enemy.SetTarget(target);

        RemainingEnemies++;

        AddChild(enemy);
    }


//  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta) {
        if (Input.IsActionJustPressed("ui_select")) {
            StartWave();
        }
        
    }

    //-------------------------PRIVATE FUNCTIONS------------------------------------------------


    //PATH-----------------------------------------------
    Vector2[] PathSnapToGrid(Vector2[] sPath) {
        List<Vector2> newPath = new List<Vector2>();


        //LÄGG TILL FÖRSTA
        newPath.Add(worldGrid.MapToWorld(worldGrid.WorldToMap(path[0])));


        for (int i = 1; i < sPath.Length-1; i++) {
            Vector2 newPos = worldGrid.WorldToMap(sPath[i]);

            Vector2 pPos = sPath[i];

            if (i < sPath.Length-1) {
                float xDif = sPath[i+1].x - sPath[i].x;
                float yDif = sPath[i+1].y - sPath[i].y;

                if (xDif != 0 || yDif != 0) {
                    pPos = worldGrid.MapToWorld(worldGrid.WorldToMap(pPos));
                    //RAKSTRÄCKA     ^ == xor

                    if (xDif != 0 ^ yDif != 0) {

                        newPath.Add(pPos);
                    } else {
                        //VID SVÄNG


                        //OM DUBBLETT, lägg ej till. funkar ej dock helt
                        if (newPath[newPath.Count-1] != pPos) {

                            newPath.Add(pPos);
                        } else {
                            // GD.Print("SAMMA");
                        }
                        



                        Vector2 nextPos = sPath[i+1];
                        nextPos.x -= xDif/2;
                        nextPos.y -= yDif/2;

                        nextPos = worldGrid.MapToWorld(worldGrid.WorldToMap(nextPos));

                        newPath.Add(nextPos);
                    }


                }

            }

            //Om en dubblett missades att tas bort
            if (newPath[newPath.Count-1] == newPath[newPath.Count-2]) {
                // GD.Print("UPPTÄCKTE SAMMA");
                newPath.RemoveAt(newPath.Count-1);
            }
        }
    
        //LÄGG TILL SISTA
        newPath.Add(worldGrid.MapToWorld(worldGrid.WorldToMap(sPath[sPath.Length-1])));


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
                if (cur != prev) {
                    output.Add(cur);
                }
                
            }
        }

        output.Add(input[input.Count-1]);

        return output;
    }
    public Tuple<Vector2, int> BackTrackPath(Vector2[] inPath, Vector2 startPos, float remainingDistance, int targeted) {
        int t = targeted;

        Vector2 pos = startPos;


        float dist = pos.DistanceTo(inPath[t-1]);


        while (remainingDistance > dist) {
            if (t <= 1) {
                    pos = inPath[0] + new Vector2(inPath[0]-inPath[1]).Normalized()*Math.Abs(remainingDistance);
                    t = 1;

                    return Tuple.Create(pos, t); 
            }
        
        
            remainingDistance -= dist;
            t--;
            pos = path[t];
            dist = pos.DistanceTo(inPath[t-1]);

            
        }


        Vector2 spawnPos;

        float pr = remainingDistance/(pos.DistanceTo(path[t-1]));
        spawnPos = pos.LinearInterpolate(path[t-1], pr);
        
        
        
        return Tuple.Create(spawnPos, t);
        
    }



    //WAVECONTROLLER-------------------------------------------------------
    public void _on_WavePauseTimer_timeout() {
        if (!waveInAction) {
            return;
        }

        if (wave.Count <= 0) {
            return;
        }
    
        SpawnEnemy(wave[0].EnemyType);
        wavePauseTimer.WaitTime = wave[0].PauseTime;
        wave.RemoveAt(0);
    }

    void StartWave() {
        if (waveInAction) {
            GD.Print("Wave already in action!");
            return;
        }

        
        waveInAction = true;
        wavePauseTimer.SetAutostart(true);        
        wavePauseTimer.WaitTime = 1f;
        RemainingEnemies = wave.Count;

        GD.Print("Wave #", waveCount, " started!");
    }

    void EndWave() {
        GD.Print("Wave ended");
        waveCount++;
        waveInAction = false;
        wave = LoadWave(waveCount-1);
    }

    List<SpawnAction> LoadWave(int whichWave) {
        //Godot.Collections.Dictionary
        //Godot.Collections.Array
        List<SpawnAction> loadedWave = new List<SpawnAction>();
        File jsonFile = new File();
        jsonFile.Open(jsonFilePath, 1);
        JSONParseResult parse = JSON.Parse(jsonFile.GetAsText());
        jsonFile.Close();

        Godot.Collections.Array resultAll = parse.Result as Godot.Collections.Array;

        if (whichWave >= resultAll.Count) {
            GD.Print("NO MORE WAVES");
            return new List<SpawnAction>();
        }



        Godot.Collections.Array result = resultAll[whichWave] as Godot.Collections.Array;


        foreach (Godot.Collections.Dictionary action in result) {
            loadedWave.Add(new SpawnAction((string) action["EnemyType"], (float) action["PauseTime"]));
        }


        return loadedWave;
    }

    //-------------------------------------------------PUBLIC FUNCTIONS--------------------------------------------------------
    public void EnemyKilled() {
        RemainingEnemies--;

        if (RemainingEnemies <= 0) {
            EndWave();
        }

        GD.Print("Remaining: ", RemainingEnemies);

    }

    //TOWER------------------------------------------------------------
    public void PlaceTower(string towerType, Vector2 gridPos) {
        PackedScene scene = towerScenes[towerType];
        float cost = towerCosts[towerType];
        //checka marktyp
        int cellType = worldGrid.GetCellv(gridPos);
        
        if (cellType > 0) { 
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

            UpdateUIMoney();
            GD.Print("Success. New balance: ", balance);

            return true;
        }

        GD.Print("Not enough para");
        return false;
    }

    public void EarnMoney(float amount) {
        balance += amount;

        UpdateUIMoney();
        GD.Print("New balance: ", balance);
    }

    public void UpdateUIMoney() {
        money.SetText("$: " + balance);
    }

    public void UpdateHealth(float amount) {
        healthPoints += amount;
        health.SetText("Hp: " + healthPoints);
    }
}