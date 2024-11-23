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
    
    public bool Break(int amount)
    {
        int breakages = 0;
        bool brokeSomething = false;
        if (!IsOpen(Vector2Int.up) && amount > breakages)
        {
            breakages++;
            OpenPath(Vector2Int.up);
            brokeSomething = true;
        }

        if (!IsOpen(Vector2Int.right) && amount > breakages)
        {
            breakages++;
            OpenPath(Vector2Int.right);
            brokeSomething = true;
        }

        if (!IsOpen(Vector2Int.down) && amount > breakages)
        {
            breakages++;
            OpenPath(Vector2Int.down);
            brokeSomething = true;
        }

        if (!IsOpen(Vector2Int.left) && amount > breakages)
        {
            breakages++;
            OpenPath(Vector2Int.left);
            brokeSomething = true;
        }

        return brokeSomething;
    }
    
    public bool Build(int amount)
    {
        int placements = 0;
        bool placedSomething = false;
        if (IsOpen(Vector2Int.up) && amount > placements)
        {
            placements++;
            ClosePath(Vector2Int.up);
            placedSomething = true;
        }

        if (IsOpen(Vector2Int.right) && amount > placements)
        {
            placements++;
            ClosePath(Vector2Int.right);
            placedSomething = true;
        }

        if (IsOpen(Vector2Int.down) && amount > placements)
        {
            placements++;
            ClosePath(Vector2Int.down);
            placedSomething = true;
        }

        if (IsOpen(Vector2Int.left) && amount > placements)
        {
            placements++;
            ClosePath(Vector2Int.left);
            placedSomething = true;
        }

        return placedSomething;
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

