using Godot;
using System;

public class MouseController : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    GameController gameController;
    CellHighlight cellHighlight;
    public TileMap worldGrid;
    
    string curTower = "Tower";

    Vector2 gridPos;
    Vector2 gridWorldPos;
    Vector2 mousePos;

    public override void _Ready()
    {
        gc = GetParent() as GameController;
        cellHighlight = GetNode<CellHighlight>("Cell Highlight");
        // get_tree().get_root().get_node("myRootNode").find_node("desiredNode")
        worldGrid = GetTree().GetRoot().GetNode("World").FindNode("WorldGrid") as TileMap;
        
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        gridPos = worldGrid.WorldToMap(mousePos);
        gridWorldPos = worldGrid.MapToWorld(gridPos);

        cellHighlight.SetPosition(gridWorldPos);
    }

    public override void _UnhandledInput(InputEvent @event) {
    	InputEventMouseButton e = @event as InputEventMouseButton;
    	if (e != null) {
            if (e.IsActionPressed("mouse_left")) {
                if (cellHighlight.IsCellVacant()) {
                    gameController.PlaceTower(curTower, gridPos);
                } else {
                    //SELECT TOWER
                    gameController.SellTower(cellHighlight.GetCurTower());
                    
                }
            }
    	}
        InputEventMouseMotion eM = @event as InputEventMouseMotion;

        if (eM != null) {
            mousePos = eM.Position;
        }

    }

    
}   
