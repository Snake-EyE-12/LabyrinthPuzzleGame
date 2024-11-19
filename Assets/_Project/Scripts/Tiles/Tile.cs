using System;
using System.Collections.Generic;
using Capstone.DataLoad;
using UnityEngine;
using Random = UnityEngine.Random;

public class Tile
{
    public Path path;
    public string type;
    public Rotation rotation;
    public Ability ability;


    public bool IsOpen(Vector2Int direction)
    {
        int bitDirection = directionToBits(direction);
        return (GetOrientation() & bitDirection) == bitDirection;
    }

    public void OpenPath(Vector2Int direction)
    {
        
        path.AddPathway((CardinalDirection)directionToBits(rotation.RotateDirectionAccoundingly(direction)));
    }
    public void ClosePath(Vector2Int direction)
    {
        
        path.RemovePathway((CardinalDirection)directionToBits(rotation.RotateDirectionAccoundingly(direction)));
    }
    

    public static Tile Copy(Tile tile)
    {
        Tile newTile = new Tile();
        newTile.path = Path.Copy(tile.path);
        newTile.rotation = Rotation.Copy(tile.rotation);
        newTile.type = tile.type;
        if(tile.ability != null) newTile.ability = Ability.Copy(tile.ability);
        return newTile;
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
    
    public Tile(TileData data, AbilityData abilityData)
    {
        path = new Path(data.Path);
        rotation = new Rotation();
        type = data.Type;
        //type
        if(!(abilityData == null || abilityData.Target == null || abilityData.Keys == null)) ability = new Ability(abilityData);
    }

}

