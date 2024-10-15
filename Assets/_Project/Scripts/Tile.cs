using System;
using System.Collections.Generic;
using Capstone.DataLoad;
using UnityEngine;
using Random = UnityEngine.Random;

public class Tile
{
    public Path path;
    public Rotation rotation;


    public bool IsOpen(Vector2Int direction)
    {
        int bitDirection = directionToBits(direction);
        //Debug.Log("IsOpenDirection: " + Convert.ToString(bitDirection, 2) + " =? " + Convert.ToString((GetOrientation() & bitDirection), 2));
        return (GetOrientation() & bitDirection) == bitDirection;
    }

    private int directionToBits(Vector2Int direction)
    {
        if(direction == Vector2Int.up) return 1;
        if(direction == Vector2Int.right) return 2;
        if(direction == Vector2Int.down) return 4;
        if(direction == Vector2Int.left) return 8;
        return 0;
    }
    public int GetOrientation()
    {
        int openPaths = path.GetOpenPathBits();
        return (openPaths << rotation.GetRotationValue()) | (openPaths >> (4 - rotation.GetRotationValue()));
    }
    
    public static Tile GenerateRandomTile()
    {
        Tile tile = new Tile();
        tile.path = new Path();
        tile.path.SetOpenPaths((CardinalDirection)Random.Range(0, 16));
        tile.rotation = new Rotation();
        tile.rotation.Rotate(RotationDirection.Clockwise, Random.Range(0, 4));
        return tile;
    }

    

    

    public Tile()
    {
    }
    
    public Tile(TileData data)
    {
        path = new Path(data.Path);
        rotation = new Rotation();
        //type
    }

}

