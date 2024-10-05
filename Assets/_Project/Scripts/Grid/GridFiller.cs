public abstract class GridFiller
{
    public abstract Tile GetFillAt(int size, int x, int y);
}

public class RandomFiller : GridFiller
{
    public override Tile GetFillAt(int size, int x, int y)
    {
        return Tile.GenerateRandomTile();
    }
}