using System;

public class Path
{
    private CardinalDirection openPathways;

    public void SetOpenPaths(CardinalDirection paths)
    {
        openPathways = paths;
    }

    public int GetOpenPathBits()
    {
        return (int)openPathways;
    }
}

[Flags]
public enum CardinalDirection
{
    None = 0,
    North = 1,
    East = 2,
    South = 4,
    West = 8
}