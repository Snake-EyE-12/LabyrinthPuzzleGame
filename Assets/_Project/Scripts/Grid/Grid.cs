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
    public GridSpace Get(Vector2Int xy)
    {
        return grid[ToGridSpace(xy)];
    }
    private Vector2Int ToGridSpace(int x, int y)
    {
        return new Vector2Int(GameUtils.ModPositive(x, size), GameUtils.ModPositive(y, size));
    }
    private Vector2Int ToGridSpace(Vector2Int xy)
    {
        return new Vector2Int(GameUtils.ModPositive(xy.x, size), GameUtils.ModPositive(xy.y, size));
    }
    
    
    
    
    
    public void PlaceTileAt(int x, int y, TileDisplay tile)
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

    public bool IsConnectedDirection(Vector2Int start, Vector2Int direction)
    {
        bool canMove = Get(start.x, start.y).GetTile().IsOpen(direction) && Get(start.x + direction.x, start.y + direction.y).GetTile().IsOpen(-direction);
        Debug.Log("Can move: " + canMove + " | From: " + start + " | Direction: " + direction);
        return canMove;
    }
}