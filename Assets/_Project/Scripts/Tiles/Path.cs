using System;

public class Path
{
    private CardinalDirection openPathways;

    public Path()
    {
    }
    public static Path Copy(Path path)
    {
        Path newPath = new Path();
        newPath.SetOpenPaths(path.openPathways);
        return newPath;
    }

    public Path(string paths)
    {
        foreach (var letter in paths.ToCharArray())
        {
            switch (letter)
            {
                case 'N':
                    AddPathway(CardinalDirection.North);
                    break;
                case 'E':
                    AddPathway(CardinalDirection.East);
                    break;
                case 'S':
                    AddPathway(CardinalDirection.South);
                    break;
                case 'W':
                    AddPathway(CardinalDirection.West);
                    break;
                
            }
        }
    }

    public bool Equals(string pathways)
    {
        return (openPathways == new Path(pathways).openPathways);
    }

    public int GetOpenPathCount()
    {
        int x = (int)openPathways;
        int count = 0;
        while(x > 0){   
            count += x & 1;
            x = x >> 1;
        }

        return count;
    }

    public void SetOpenPaths(CardinalDirection paths)
    {
        openPathways = paths;
    }

    public void AddPathway(CardinalDirection direction)
    {
        openPathways |= direction;
    }
    public void RemovePathway(CardinalDirection direction)
    {
        openPathways &= ~direction;
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