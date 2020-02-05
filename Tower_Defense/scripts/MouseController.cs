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
	public Camera2D camera2D; 
    public TileMap worldGrid;
    
    string curTower = "Tower";

    Vector2 gridPos;
    Vector2 gridWorldPos;
    Vector2 mousePos;
    Vector2 gridSize = new Vector2(13, 8);

    public override void _Ready()
    {
        gameController = GetParent() as GameController;
        cellHighlight = GetNode<CellHighlight>("Cell Highlight");
		camera2D = GetTree().GetRoot().GetNode("World").FindNode("Camera2D") as Camera2D;
        GD.Print("Cam:" + camera2D.GetOffset().x);
        // get_tree().get_root().get_node("myRootNode").find_node("desiredNode")
        worldGrid = GetTree().GetRoot().GetNode("World").FindNode("WorldGrid") as TileMap;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        gridPos.x = Mathf.Clamp(worldGrid.WorldToMap(mousePos).x, 0, gridSize.x);
        gridPos.y = Mathf.Clamp(worldGrid.WorldToMap(mousePos).y, 0, gridSize.y);
        
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
            mousePos = eM.GlobalPosition - new Vector2(camera2D.GetOffset().x, 0);
        }

    }

    
}   
