using System.Collections.Generic;
using UnityEngine;

public class Unit : GridPositionable
{
     private Map localMap;
    // private string unitName;
    // private int degree;
    // private Health health;
    // private List<ActiveEffect> activeEffects = new List<ActiveEffect>();
    // private List<Modifier> modifiers = new List<Modifier>();
    
    

    public void SetMap(Map map)
    {
        localMap = map;
    }
    public void Move(Vector2Int direction)
    {
        localMap.Move(this, direction);
    }

    public void Spawn(Vector2Int pos)
    {
        localMap.SpawnUnit(this, pos);
    }

    public Vector2Int? GetPosition()
    {
        return localMap.GetUnitLocation(this);
        
    }
}



