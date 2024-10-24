using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MappableOrganizer : MonoBehaviour
{
    [SerializeField] private Transform left;
    [SerializeField] private Transform right;
    [SerializeField] private Transform bottom;
    
    public void Add(GridPositionable unit)
    {
        mapItems.Add(unit);
        switch (unit.GetTileLocation())
        {
            case OnTileLocation.Left:
                unit.GetSelfTransform().SetParent(left);
                break;
            case OnTileLocation.Right:
                unit.GetSelfTransform().SetParent(right);
                break;
            case OnTileLocation.Bottom:
                unit.GetSelfTransform().SetParent(bottom);
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
    }

    public void ResetPositions(Vector2Int pos)
    {
        foreach (var i in mapItems)
        {
            i.SetGridPosition(pos);
        }
    }

    
}
