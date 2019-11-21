using Godot;
using System;

public class MouseController : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    Node2D cellHighlight;
    public TileMap worldGrid;
    PackedScene towerScene;
    

    Vector2 gridPos;
    Vector2 gridWorldPos;
    Vector2 mousePos;

    public override void _Ready()
    {
        cellHighlight = GetNode<Node2D>("Cell Highlight");
        // get_tree().get_root().get_node("myRootNode").find_node("desiredNode")
        worldGrid = GetTree().GetRoot().GetNode("World").FindNode("WorldGrid") as TileMap;
        towerScene = (PackedScene)ResourceLoader.Load("res://Scenes/Towers/Tower.tscn");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        gridPos = worldGrid.WorldToMap(mousePos);
        gridWorldPos = worldGrid.MapToWorld(gridPos);

        cellHighlight.SetPosition(gridWorldPos);
    }

    public override void _Input(InputEvent @event) {
    	InputEventMouseButton e = @event as InputEventMouseButton;
    	if (e != null) {
            if (e.IsActionPressed("mouse_left")) {
                placeTower();
            }
    	}
        InputEventMouseMotion eM = @event as InputEventMouseMotion;

        if (eM != null) {
            mousePos = eM.Position;
        }

    }

    void placeTower() {

        int cellType = worldGrid.GetCellv(gridPos);
        GD.Print(cellType);
        //checka om pengar finns


        //checka om torn


        //checka marktyp
        if (cellType < 2) {
            GD.Print("Place");

            
            Node2D tower = towerScene.Instance() as Node2D;
            GD.Print(tower);
            GD.Print(gridWorldPos);
            tower.SetPosition(gridWorldPos);
            AddChild(tower);
        }
    }
}   
