using System.Collections.Generic;
using Capstone.DataLoad;
using UnityEngine;

public abstract class Unit
{
    protected Map localMap;
     
     
    public string unitName { get; protected set; }
    public int degree { get; protected set; }
    public Health health { get; protected set; }
    public ActiveEffect activeEffects { get; protected set; }
    
    

    public void SetMap(Map map)
    {
        localMap = map;
    }
    public void Move(Vector2Int direction)
    {
        //localMap.Move(this, direction);
    }

    public void Spawn(Vector2Int pos)
    {
        localMap.SpawnUnit(this, pos);
    }

    // public Vector2Int? GetPosition()
    // {
    //     return localMap.GetUnitLocation(this);
    //     
    // }
    //
    //
    //
    // public abstract OnTileLocation GetTileLocation();
    //
    // public Vector2Int GridPosition { get; set; }
    //
}

