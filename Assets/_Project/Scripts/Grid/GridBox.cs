using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBox
{
    private int size;
    private Dictionary<Vector2Int, GridSpace> grid = new Dictionary<Vector2Int, GridSpace>();
    public GridBox(int size)
    {
        this.size = size;
    }
    public int GetSize()
    {
        return size;
    }

    public void Set(GridSpace space, int x, int y)
    {
        grid[ToGridSpace(x, y)] = space;
    }
    public GridSpace Get(int x, int y)
    {
        return grid[ToGridSpace(x, y)];
    }

    public Vector2Int? Find(GridPositionable entity)
    {
        foreach (var space in grid)
        {
            if(space.Value.Contains(entity)) return space.Key;
        }

        return null;
    }

    private Vector2Int ToGridSpace(int x, int y)
    {
        return new Vector2Int(GameUtils.ModPositive(x, size), GameUtils.ModPositive(y, size));
    }
}