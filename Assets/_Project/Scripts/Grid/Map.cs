
using System.Collections.Generic;
using Capstone.DataLoad;
using UnityEngine;

public class Map
{
    private Grid grid;

    public Map(Fight fight)
    {
        grid = new Grid(DataHolder.currentMode.GridSize);
        
        //Temporary
        // Unit hero = new Unit();
        // hero.SetMap(this);
        // SpawnUnit(hero, new Vector2Int(0, 0));
        // hero.Move(Vector2Int.up);
        // hero.Move(Vector2Int.up);
        // Debug.Log(gridHandler.GetGridBox().Find(hero));
    }
    public void SpawnTile(GridPositionable tile, Vector2Int position)
    {
        grid.PlaceTileAt(position.x, position.y, tile);
    }
    
    public void Slide(bool row, bool positive, int number)
    {
        // if (row) gridHandler.SlideRow(number, positive);
        // else gridHandler.SlideColumn(number, positive);
    }

    public void Move(GridPositionable entity, Vector2Int direction)
    {
        //gridHandler.Move(entity, direction);
    }

    public void SpawnUnit(Unit unit, Vector2Int position)
    {
        //gridHandler.Place(unit, position);
    }

    public Vector2Int? GetUnitLocation(Unit unit)
    {
        return null;
        //return gridHandler.Find(unit);
    }

    public TileDisplay GetRandomBorderTile()
    {
        List<GridSpace> gridSpaces = GetBorderTiles();
        return gridSpaces[Random.Range(0, gridSpaces.Count)].GetTile() as TileDisplay; 
    }
    private List<GridSpace> GetBorderTiles()
    {
        List<GridSpace> gridSpaces = new List<GridSpace>();
        for(int i = 0; i < grid.GetSize(); i++)
        {
            for(int j = 0; j < grid.GetSize(); j++)
            {
                if(IsBorder(i, j)) gridSpaces.Add(grid.Get(i, j));
            }
        }

        return gridSpaces;
    }
    private List<GridSpace> GetCornerTiles()
    {
        List<GridSpace> gridSpaces = new List<GridSpace>();
        for(int i = 0; i < grid.GetSize(); i++)
        {
            for(int j = 0; j < grid.GetSize(); j++)
            {
                if(IsCorner(i, j)) gridSpaces.Add(grid.Get(i, j));
            }
        }

        return gridSpaces;
    }
    private List<GridSpace> GetCenterTiles()
    {
        List<GridSpace> gridSpaces = new List<GridSpace>();
        for(int i = 0; i < grid.GetSize(); i++)
        {
            for(int j = 0; j < grid.GetSize(); j++)
            {
                if(!IsBorder(i, j)) gridSpaces.Add(grid.Get(i, j));
            }
        }

        return gridSpaces;
    }
    private bool IsBorder(int x, int y)
    {
        return x == 0 || y == 0 || x == grid.GetSize() - 1 || y == grid.GetSize() - 1;
    }
    private bool IsCorner(int x, int y)
    {
        return ((x == 0 || x == grid.GetSize() - 1) && (y == 0 || y == grid.GetSize() - 1));
    }

}