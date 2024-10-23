using System.Collections.Generic;
using Capstone.DataLoad;
using UnityEngine;

public abstract class GridFiller
{
    public abstract Tile GetFillAt(int size, Vector2Int pos, FloorTile[] set);
    protected Tile TileAtIndex(int index, FloorTile[] set)
    {
        index = GameUtils.ModPositive(index, set.Length);
        FloorTile chosen = set[index];
        Tile tile = Card.Load(chosen.Tile).GetTile();
        tile.rotation.SetStringRotation(chosen.Rotation);
        return tile;
    }
    
    public static GridFiller GetTypeOf(string type)
    {
        switch (type)
        {
            case "Random": return new RandomFromSetFiller();
            case "All": return new AllFiller();
            case "Expand": return new ExpandFiller();
            case "Row": return new RowFiller();
            case "Column": return new ColumnFiller();
            default: return new RandomFiller();
        }
    }
}

public class RandomFiller : GridFiller
{
    public override Tile GetFillAt(int size, Vector2Int pos, FloorTile[] set)
    {
        return Tile.GenerateRandomTile();
    }
}
public class RandomFromSetFiller : GridFiller
{
    public override Tile GetFillAt(int size, Vector2Int pos, FloorTile[] set)
    {
        int index = GameUtils.IndexByWeightedRandom(new List<Weighted>(set));
        return TileAtIndex(index, set);
    }
}

public class AllFiller : GridFiller
{
    public override Tile GetFillAt(int size, Vector2Int pos, FloorTile[] set)
    {
        int index = 0;
        return TileAtIndex(index, set);
    }
}

public class ExpandFiller : GridFiller
{
    public override Tile GetFillAt(int size, Vector2Int pos, FloorTile[] set)
    {
        return Tile.GenerateRandomTile();
    }
}

public class RowFiller : GridFiller
{
    public override Tile GetFillAt(int size, Vector2Int pos, FloorTile[] set)
    {
        int index = pos.y;
        return TileAtIndex(index, set);
    }
}

public class ColumnFiller : GridFiller
{
    public override Tile GetFillAt(int size, Vector2Int pos, FloorTile[] set)
    {
        int index = pos.x;
        return TileAtIndex(index, set);
    }
}