using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid
{
    private int size;
    private Dictionary<Vector2Int, GridSpace> grid = new Dictionary<Vector2Int, GridSpace>();
    public Grid(int size)
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
    
    
    
    
    
    public void PlaceTileAt(int x, int y, GridPositionable tile)
    {
        Vector2Int position = ToGridSpace(x, y);
        EnsurePosition(position);
        grid[position].SetTile(tile);
    }
    private void EnsurePosition(Vector2Int position)
    {
        if (!grid.ContainsKey(position))
        {
            grid[position] = new GridSpace();
        }
    }

    public List<GridSpace> GetAllSpaces()
    {
        return new List<GridSpace>(grid.Values);
        
    }
}