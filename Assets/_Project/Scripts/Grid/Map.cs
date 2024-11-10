
using System.Collections.Generic;
using Capstone.DataLoad;
using Guymon.DesignPatterns;
using UnityEngine;

public class Map
{
    protected Grid grid;

    private Fight activeFight;

    public Map()
    {
    }

    public int GetSize()
    {
        return grid.GetSize();
    }

    public Map(Fight fight)
    {
        grid = new Grid(DataHolder.currentMode.GridSize);
        activeFight = fight;
    }
    public Fight GetFight() { return activeFight; }

    public void SpawnLoot(GridPositionable loot, Vector2Int position)
    {
        grid.Get(position).GetTile().GainControl(loot);
    }
    
    public string[] GetStartingEnemyNames()
    {
        return activeFight.GetEnemies();
    }
    public void SpawnTile(TileDisplay tile, Vector2Int position)
    {
        grid.PlaceTileAt(position.x, position.y, tile);
    }

    public void RotateTile(Vector2Int pos, int rotationAmount)
    {
        TileDisplay tileAtPos = grid.Get(pos).GetTile();
        tileAtPos.GetTile().rotation.Rotate(RotationDirection.Clockwise, rotationAmount);
        tileAtPos.Render();
    }
    
    public void Slide(bool row, bool positive, int number)
    {
        CommandHandler.Execute(new SlideCommand(row, positive, number, this, GameManager.Instance.cardToPlace.GetCard(), GameManager.Instance.activeDeck));
        AudioManager.Instance.Play("HeavySlide");
    }

    public void Swap(Vector2Int gridPos, Tile tile)
    {
        SwapPosWith(gridPos, Tile.Copy(tile));
        AudioManager.Instance.Play("Swapping");
    }
    private void SwapPosWith(Vector2Int pos, Tile replacer)
    {
        grid.Get(pos).GetTile().Set(replacer);
    }
    public Tile SlideColumn(int x, bool upwards, Tile replacer)
    {
        GridSpace fallOffSpace = null;
        Tile fallOffTile = null;
        if (upwards)
        {
            fallOffSpace = grid.Get(x, grid.GetSize() - 1);
            fallOffTile = Tile.Copy(fallOffSpace.GetTile().GetTile());
            for (int y = grid.GetSize() - 1; y > 0; y--)
            {
                grid.Get(x, y - 1).GetTile().SetGridPosition(new Vector2Int(x, y));
                grid.Set(grid.Get(x, y - 1), x, y);
            }

            Vector2Int replacePos = new Vector2Int(x, 0);
            grid.Set(fallOffSpace, replacePos.x, replacePos.y);
            fallOffSpace.GetTile().Set(replacer);
            fallOffSpace.GetTile().SetGridPosition(replacePos, true);
        }
        else
        {
            fallOffSpace = grid.Get(x, 0);
            fallOffTile = Tile.Copy(fallOffSpace.GetTile().GetTile());
            for (int y = 0; y < grid.GetSize() - 1; y++)
            {
                grid.Get(x, y + 1).GetTile().SetGridPosition(new Vector2Int(x, y));
                grid.Set(grid.Get(x, y + 1), x, y);
            }
            
            Vector2Int replacePos = new Vector2Int(x, grid.GetSize() - 1);
            grid.Set(fallOffSpace, replacePos.x, replacePos.y);
            fallOffSpace.GetTile().Set(replacer);
            fallOffSpace.GetTile().SetGridPosition(replacePos, true);
        }

        return fallOffTile;
    }
    public Tile SlideRow(int y, bool upwards, Tile replacer)
    {
        Tile fallOffTile = null;
        GridSpace fallOffSpace = null;
        if (upwards)
        {
            fallOffSpace = grid.Get(grid.GetSize() - 1, y);
            fallOffTile = Tile.Copy(fallOffSpace.GetTile().GetTile());
            for (int x = grid.GetSize() - 1; x > 0; x--)
            {
                grid.Get(x - 1, y).GetTile().SetGridPosition(new Vector2Int(x, y));
                grid.Set(grid.Get(x - 1, y), x, y);
            }

            Vector2Int replacePos = new Vector2Int(0, y);
            grid.Set(fallOffSpace, replacePos.x, replacePos.y);
            fallOffSpace.GetTile().Set(replacer);
            fallOffSpace.GetTile().SetGridPosition(replacePos, true);
        }
        else
        {
            fallOffSpace = grid.Get(0, y);
            fallOffTile = Tile.Copy(fallOffSpace.GetTile().GetTile());
            for (int x = 0; x < grid.GetSize() - 1; x++)
            {
                grid.Get(x + 1, y).GetTile().SetGridPosition(new Vector2Int(x, y));
                grid.Set(grid.Get(x + 1, y), x, y);
            }

            Vector2Int replacePos = new Vector2Int(grid.GetSize() - 1, y);
            grid.Set(fallOffSpace, replacePos.x, replacePos.y);
            fallOffSpace.GetTile().Set(replacer);
            fallOffSpace.GetTile().SetGridPosition(replacePos, true);
        }
        return fallOffTile;
    }
    public TileDisplay GetTileAtPosition(Vector2Int pos) => grid.Get(pos).GetTile();

    public void Move(GridPositionable entity, Vector2Int direction)
    {
        AudioManager.Instance.Play("Footsteps");
        Vector2Int currentPos = entity.GetGridPosition();
        Vector2Int newPosition = currentPos + direction;

        if (grid.IsConnectedDirection(currentPos, direction))
        {
            grid.Get(currentPos).GetTile().LoseControl(entity);
            grid.Get(newPosition).GetTile().GainControl(entity);
        }
    }

    public void RemoveGridPositionable(GridPositionable unit)
    {
        grid.Get(unit.GetGridPosition()).GetTile().LoseControl(unit);
    }
    
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