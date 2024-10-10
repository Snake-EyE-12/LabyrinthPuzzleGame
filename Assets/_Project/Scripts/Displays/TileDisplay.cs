using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventArgs = Guymon.DesignPatterns.EventArgs;

public class TileDisplay : Display<Tile>, GridPositionable
{
    [SerializeField] private List<WallDisplay> wallDisplays = new List<WallDisplay>();
    [SerializeField] private MappableOrganizer mappableOrganizer;
    public override void Render(Tile tile)
    {
        int orientation = tile.GetOrientation();
        for (int i = 0, j = 1; i < 4; i++, j *= 2)
        {
            wallDisplays[i].SetVisibility((orientation & j) == 0);
        }
    }

    public void GainControl(GridPositionable unit)
    {
        mappableOrganizer.Add(unit);
    }
    

    private Vector2Int gridPosition;
    public Vector2Int GetGridPosition()
    {
        return gridPosition;
    }

    public void SetGridPosition()
    {
        throw new NotImplementedException();
    }

    public void SetGridPosition(Vector2Int value)
    {
        transform.position = VisualDataHolder.Instance.CoordsToPosition(value);
        gridPosition = value;
    }

    

    public OnTileLocation GetTileLocation()
    {
        return OnTileLocation.None;
    }

    public Transform GetSelfTransform()
    {
        return gameObject.transform;
    }

    private Map localMap;
    public void SetOntoMap(Map map, Vector2Int position)
    {
        SetGridPosition(position);
        map.SpawnTile(this, position);
        localMap = map;
        
    }
}

