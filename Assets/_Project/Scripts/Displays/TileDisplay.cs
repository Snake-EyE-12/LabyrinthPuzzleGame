using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EventArgs = Guymon.DesignPatterns.EventArgs;
using EventHandler = Guymon.DesignPatterns.EventHandler;
using Random = UnityEngine.Random;

public class TileDisplay : Display<Tile>, GridPositionable, Selectable
{
    [SerializeField] private List<WallDisplay> wallDisplays = new List<WallDisplay>();
    [SerializeField] private MappableOrganizer mappableOrganizer;
    [SerializeField] private SelectionDisplay selectionIndicator;
    public override void Render()
    {
        int orientation = item.GetOrientation();
        for (int i = 0, j = 1; i < 4; i++, j *= 2)
        {
            wallDisplays[i].SetVisibility((orientation & j) == 0);
        }

        
    }


    public void LoseControl(GridPositionable unit)
    {
        mappableOrganizer.Remove(unit);
    }
    public void GainControl(GridPositionable unit)
    {
        mappableOrganizer.Add(unit);
        unit.SetGridPosition(GetGridPosition());
    }
    

    private Vector2Int gridPosition;
    public Vector2Int GetGridPosition()
    {
        return gridPosition;
    }
    

    public void SetGridPosition(Vector2Int value)
    {
        transform.position = VisualDataHolder.Instance.CoordsToPosition(value);
        gridPosition = value;
        mappableOrganizer.ResetPositions(gridPosition);
    }

    

    public OnTileLocation GetTileLocation()
    {
        return OnTileLocation.None;
    }

    public Transform GetSelfTransform()
    {
        return gameObject.transform;
    }

    public void SetLocalMap(Map map)
    {
        localMap = map;
    }

    private Map localMap;
    public void SetOntoMap(Map map, Vector2Int position)
    {
        SetGridPosition(position);
        map.SpawnTile(this, position);
        SetLocalMap(map);

    }

    public bool IsOpen(Vector2Int direction)
    {
        return item.IsOpen(direction);
    }
    private void Start()
    {
        GameManager.Instance.AddSelectable(this, selectionIndicator.type);
    }

    public void Select()
    {
        selectionIndicator.StartSelection();
    }

    public void Deselect()
    {
        selectionIndicator.EndSelection();
    }

    public int GetOrderValue()
    {
        return gridPosition.y * DataHolder.currentMode.GridSize - gridPosition.x;
    }

    public bool IsCurrentlySelectable()
    {
        return true;
    }

    public void Activate(SelectableActivatorData data)
    {
        bool onRow = GameManager.Instance.DirectionToSlide == CardinalDirection.West ||
                     GameManager.Instance.DirectionToSlide == CardinalDirection.East;
        bool posDirection = GameManager.Instance.DirectionToSlide == CardinalDirection.North ||
                            GameManager.Instance.DirectionToSlide == CardinalDirection.East;
        localMap.Slide(onRow, posDirection, ((onRow) ? gridPosition.y : gridPosition.x));
        EventHandler.Invoke("CardPlaced", new CardEventArgs(GameManager.Instance.cardToPlace.GetCard()));
        GameManager.Instance.cardToPlace = null;
    }
}

