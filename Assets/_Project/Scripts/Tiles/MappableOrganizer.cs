using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MappableOrganizer : MonoBehaviour
{
    [SerializeField] private ItemLayout left;
    [SerializeField] private ItemLayout right;
    [SerializeField] private ItemLayout bottom;
    
    public void Add(GridPositionable unit)
    {
        mapItems.Add(unit);
        switch (unit.GetTileLocation())
        {
            case OnTileLocation.Left:
                left.Add(unit);
                break;
            case OnTileLocation.Right:
                right.Add(unit);
                break;
            case OnTileLocation.Bottom:
                bottom.Add(unit);
                break;
            
        }
    }

    public List<LootDisplay> GetLoot()
    {
        List<LootDisplay> lootDisplays = new List<LootDisplay>();
        foreach (var i in mapItems)
        {
            if(i is LootDisplay) lootDisplays.Add(i as LootDisplay);
        }

        return lootDisplays;
    }

    private List<GridPositionable> mapItems = new List<GridPositionable>();
    public void Remove(GridPositionable unit)
    {
        mapItems.Remove(unit);
        switch (unit.GetTileLocation())
        {
            case OnTileLocation.Left:
                left.Remove(unit);
                break;
            case OnTileLocation.Right:
                right.Remove(unit);
                break;
            case OnTileLocation.Bottom:
                bottom.Remove(unit);
                break;
            
        }
    }

    public void ResetPositions(Vector2Int pos)
    {
        foreach (var i in mapItems)
        {
            i.SetGridPosition(pos);
        }
    }

    
}
