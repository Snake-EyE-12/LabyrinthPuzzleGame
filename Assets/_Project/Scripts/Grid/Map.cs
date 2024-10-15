
using System.Collections.Generic;
using Capstone.DataLoad;
using UnityEngine;

public class Map
{
    private Grid grid;

    private Fight activeFight;
    public Map(Fight fight)
    {
        grid = new Grid(DataHolder.currentMode.GridSize);
        activeFight = fight;

        //Temporary
        // Unit hero = new Unit();
        // hero.SetMap(this);
        // SpawnUnit(hero, new Vector2Int(0, 0));
        // hero.Move(Vector2Int.up);
        // hero.Move(Vector2Int.up);
        // Debug.Log(gridHandler.GetGridBox().Find(hero));
    }

    public string[] GetStartingEnemyNames()
    {
        return activeFight.Enemies;
    }
    public void SpawnTile(TileDisplay tile, Vector2Int position)
    {
        grid.PlaceTileAt(position.x, position.y, tile);
    }
    
    public void Slide(bool row, bool positive, int number)
    {
        if(row) SlideRow(number, positive, GameManager.Instance.cardToPlace.GetTile());
        else SlideColumn(number, positive, GameManager.Instance.cardToPlace.GetTile());
        
    }
    private void SlideColumn(int x, bool upwards, Tile replacer)
    {
        if (upwards)
        {
            GridSpace fallOffSpace = grid.Get(x, grid.GetSize() - 1);
            for (int y = grid.GetSize() - 1; y > 0; y--)
            {
                grid.Get(x, y - 1).GetTile().SetGridPosition(new Vector2Int(x, y));
                grid.Set(grid.Get(x, y - 1), x, y);
            }

            Vector2Int replacePos = new Vector2Int(x, 0);
            grid.Set(fallOffSpace, replacePos.x, replacePos.y);
            fallOffSpace.GetTile().Set(replacer);
            fallOffSpace.GetTile().SetGridPosition(replacePos);
            Debug.Log("UPWARDS COLUMN");
        }
        else
        {
            GridSpace fallOffSpace = grid.Get(x, 0);
            for (int y = 0; y < grid.GetSize() - 1; y++)
            {
                grid.Get(x, y + 1).GetTile().SetGridPosition(new Vector2Int(x, y));
                grid.Set(grid.Get(x, y + 1), x, y);
            }
            
            Vector2Int replacePos = new Vector2Int(x, grid.GetSize() - 1);
            grid.Set(fallOffSpace, replacePos.x, replacePos.y);
            fallOffSpace.GetTile().Set(replacer);
            fallOffSpace.GetTile().SetGridPosition(replacePos);
            Debug.Log("DOWN COLUMN");
        }
        
    }
    private void SlideRow(int y, bool upwards, Tile replacer)
    {
        if (upwards)
        {
            GridSpace fallOffSpace = grid.Get(grid.GetSize() - 1, y);
            for (int x = grid.GetSize() - 1; x > 0; x--)
            {
                grid.Get(x - 1, y).GetTile().SetGridPosition(new Vector2Int(x, y));
                grid.Set(grid.Get(x - 1, y), x, y);
            }

            Vector2Int replacePos = new Vector2Int(0, y);
            grid.Set(fallOffSpace, replacePos.x, replacePos.y);
            fallOffSpace.GetTile().Set(replacer);
            fallOffSpace.GetTile().SetGridPosition(replacePos);
            Debug.Log("right ROW");
        }
        else
        {
            GridSpace fallOffSpace = grid.Get(0, y);
            for (int x = 0; x < grid.GetSize() - 1; x++)
            {
                grid.Get(x + 1, y).GetTile().SetGridPosition(new Vector2Int(x, y));
                grid.Set(grid.Get(x + 1, y), x, y);
            }

            Vector2Int replacePos = new Vector2Int(grid.GetSize() - 1, y);
            grid.Set(fallOffSpace, replacePos.x, replacePos.y);
            fallOffSpace.GetTile().Set(replacer);
            fallOffSpace.GetTile().SetGridPosition(replacePos);
            Debug.Log("Left ROW");
        }
    }

    public void Move(GridPositionable entity, Vector2Int direction)
    {
        Vector2Int currentPos = entity.GetGridPosition();
        Vector2Int newPosition = currentPos + direction;

        if (grid.IsConnectedDirection(currentPos, direction))
        {
            grid.Get(currentPos).GetTile().LoseControl(entity);
            grid.Get(newPosition).GetTile().GainControl(entity);
        }
    }
    //
    // public void SpawnUnit(Unit unit, Vector2Int position)
    // {
    //     //gridHandler.Place(unit, position);
    // }
    //
    // public Vector2Int? GetUnitLocation(Unit unit)
    // {
    //     return null;
    //     //return gridHandler.Find(unit);
    // }
    public TileDisplay GetRandomCenterTile()
    {
        List<GridSpace> gridSpaces = GetCenterTiles();
        return gridSpaces[Random.Range(0, gridSpaces.Count)].GetTile() as TileDisplay;
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