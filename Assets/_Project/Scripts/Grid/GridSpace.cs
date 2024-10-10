using System.Collections.Generic;
using UnityEngine;

public class GridSpace
{
    // private Vector2Int position;
    // public void SetPosition(int x, int y)
    // {
    //     position = new Vector2Int(x, y);
    // }
    //
    // public Vector2Int GetPosition()
    // {
    //     return position;
    // }
    //
    private GridPositionable tile;
    //private List<GridPositionable> objects = new List<GridPositionable>();
    public GridPositionable GetTile()
    {
        return tile;
    }
    public void SetTile(GridPositionable tile)
    {
        this.tile = tile;
    }
    // public void AddObject(GridPositionable obj)
    // {
    //     objects.Add(obj);
    // }
    // public void RemoveObject(GridPositionable obj)
    // {
    //     objects.Remove(obj);
    // }
    //
    // public bool Contains(GridPositionable entity)
    // {
    //     foreach (var obj in objects)
    //     {
    //         if (obj == entity) return true;
    //     }
    //
    //     return false;
    // }
}