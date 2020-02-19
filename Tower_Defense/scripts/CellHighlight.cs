using Godot;
using System;

public class CellHighlight : Sprite
{
    Tower curTower = null;


    public bool IsCellVacant() {
        return curTower == null;
        
    }

    public Tower GetCurTower() {
        return curTower;
    } 

    public void _on_DetectionArea_area_entered(Area2D area) {
        curTower = area.GetParent() as Tower;
    }

    public void _on_DetectionArea_area_exited(Area2D area) {
        curTower = ((curTower = area.GetParent() as Tower) == curTower) ? null : curTower;
    }


}
