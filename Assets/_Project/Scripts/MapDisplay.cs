using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventArgs = Guymon.DesignPatterns.EventArgs;

public class MapDisplay : Display<Map>
{
    
    private Dictionary<Vector2Int, TileDisplay> tileDisplayGrid = new Dictionary<Vector2Int, TileDisplay>();


    private bool built;
    private void BuildMap(Map map)
    {
        built = true;
        for (int i = 0; i < map.size; i++)
        {
            for (int j = 0; j < map.size; j++)
            {
                BuildNewTile(new Vector2Int(i, j));
            }
        }
    }

    private void BuildNewTile(Vector2Int coords)
    {
        TileDisplay newTile = ObjectFactory.Instance.GetTileDisplay();
        newTile.transform.SetParent(transform);
        newTile.transform.position = CoordsToPosition(coords);
        tileDisplayGrid[coords] = newTile;
    }

    private Vector3 CoordsToPosition(Vector2Int coords)
    {
        return new Vector3(coords.x * VisualDataHolder.Instance.spacing, coords.y * VisualDataHolder.Instance.spacing, 0) + new Vector3(VisualDataHolder.Instance.origin.x, VisualDataHolder.Instance.origin.y, 0);
    }

    public override void Render(Map map)
    {
        if(!built) BuildMap(map);
        GridBox grid = map.GetMapLayout();
        for (int i = 0; i < map.size; i++)
        {
            for (int j = 0; j < map.size; j++)
            {
                tileDisplayGrid[new Vector2Int(i, j)].Render(grid.Get(i, j).GetTile());
            }
        }
    }
}