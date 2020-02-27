using Godot;
using System;

public class MouseController : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    GameController gameController;
    UIController uiController;
    CellHighlight cellHighlight;
	public Camera2D camera2D; 
    public TileMap worldGrid;
    
    
    string curTower = "";

    Node2D shootHighlight;
    Node2D mortarHighlight;
    Node2D shotgunHighlight;

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
        uiController = GetTree().GetRoot().GetNode("World").FindNode("UIController") as UIController;

        shootHighlight = FindNode("ShootHighlight") as Node2D;
        mortarHighlight = FindNode("MortarHighlight") as Node2D;
        shotgunHighlight = FindNode("ShotgunHighlight") as Node2D;

        shootHighlight.SetModulate(new Color(1,1,1,.5f));
        mortarHighlight.SetModulate(new Color(1,1,1,.5f));
        shotgunHighlight.SetModulate(new Color(1,1,1,.5f));
        
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        gridPos.x = Mathf.Clamp(worldGrid.WorldToMap(mousePos).x, 0, gridSize.x);
        gridPos.y = Mathf.Clamp(worldGrid.WorldToMap(mousePos).y, 0, gridSize.y);
        
        gridWorldPos = worldGrid.MapToWorld(gridPos);

        cellHighlight.SetPosition(gridWorldPos);

        if (Input.IsKeyPressed(49)) {
            ChangeSelectedTower("Shoot");
        } else if (Input.IsKeyPressed(50)) {
            ChangeSelectedTower("Mortar");
        } else if (Input.IsKeyPressed(51)) {
            ChangeSelectedTower("Shotgun");
        } else if (Input.IsKeyPressed(16777217)) {
            ChangeSelectedTower("");
        }
    }

    public override void _UnhandledInput(InputEvent @event) {
    	InputEventMouseButton e = @event as InputEventMouseButton;
    	if (e != null) {
            if (e.IsActionPressed("mouse_left")) {
                if (cellHighlight.IsCellVacant()) {
                    gameController.PlaceTower(curTower, gridPos);
                    ChangeSelectedTower("");
                } else {
                    //SELECT TOWER
                    // uiController.SetTower(null);
                    gameController.SellTower(cellHighlight.GetCurTower());
                    
                }
            }
    	}
        
        InputEventMouseMotion eM = @event as InputEventMouseMotion;

        if (eM != null) {
            uiController.SetTower(cellHighlight.GetCurTower());
            // if (!cellHighlight.IsCellVacant()) {

            //     uiController.SetTower(cellHighlight.GetCurTower());
            // } else {
            //     uiController.Set();
            //     // GD.Print(cellHighlight.GetCurTower(), "empty");
            // }
            

            mousePos = eM.GlobalPosition - new Vector2(camera2D.GetOffset().x, 0);
        }

    }

    public void ChangeSelectedTower(string towerType) {
        switch (towerType) {
            case "Shoot":
                curTower = "Shoot";
                shootHighlight.SetVisible(true);
                mortarHighlight.SetVisible(false);
                shotgunHighlight.SetVisible(false);
            break;
            case "Mortar":
                curTower = "Mortar";
                
                shootHighlight.SetVisible(false);
                mortarHighlight.SetVisible(true);
                shotgunHighlight.SetVisible(false);

            break;
            case "Shotgun":
                curTower = "Shotgun";
                
                shootHighlight.SetVisible(false);
                mortarHighlight.SetVisible(false);
                shotgunHighlight.SetVisible(true);
            break;
            default:
                curTower = "";

                
                shootHighlight.SetVisible(false);
                mortarHighlight.SetVisible(false);
                shotgunHighlight.SetVisible(false);
            break;
        }


    }

    
}   
