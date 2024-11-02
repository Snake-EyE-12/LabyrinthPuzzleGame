using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseToTileInput : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private float scrollSpeed = 0.1f;
    private float lastScrollTime = 0;

    private void PerformRotation(RotationDirection direction)
    {
        Debug.Log("ROTATED");
        GameManager.Instance.cardToPlace.RotateTile(direction, -1);
        lastScrollTime = Time.time + scrollSpeed;
    }
    private void Update()
    {
        if (GameManager.Instance.GetSelectionType() != SelectableGroupType.Tile) return;
        if (lastScrollTime < Time.time && GameManager.Instance.cardToPlace != null)
        {
            if (Input.mouseScrollDelta.y > 0)
            {
                PerformRotation(RotationDirection.Clockwise);
            }
            else if (Input.mouseScrollDelta.y < 0)
            {
                PerformRotation(RotationDirection.Counterclockwise);
            }
            
        }
        if (Input.GetMouseButtonDown(1))
        {
            Deselect();
            selectedTile = null;
        }
        if (selectedTile == null)
        {
            hoveredTile = CheckHoveringTile();
            if (Input.GetMouseButtonDown(0))
            {
                StartMouseSelection();
            }
        }

        if (selectedTile == null) return;
        if(Input.GetMouseButton(0)) Dragging();
        if (Input.GetMouseButtonUp(0))
        {
            EndMouseSelection();
        }
    }
    private TileDisplay hoveredTile;
    private Vector2 startPosition;
    private TileDisplay selectedTile;
    private TileDisplay CheckHoveringTile()
    {
        Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        TileDisplay tile = ConvertScreenPositionToTile(mousePos);
        if (tile != null)
        {
            if (hoveredTile != tile)
            {
                Deselect();
                hoveredTile = tile;
                tile.Select();
                return tile;
            }

            return tile;
        }
        Deselect();
        return null;
    }
    private void StartMouseSelection()
    {
        if (hoveredTile != null)
        {
            selectedTile = hoveredTile;
            startPosition = cam.ScreenToWorldPoint(Input.mousePosition);
        }
    }
    private TileDisplay ConvertScreenPositionToTile(Vector2 pos)
    {
        Vector2Int gridPos = VisualDataHolder.Instance.PositionToCoords(pos);
        if(gridPos.x < 0 || gridPos.x >= DataHolder.currentMode.GridSize || gridPos.y < 0 || gridPos.y >= DataHolder.currentMode.GridSize) return null;
        return GameManager.Instance.ActiveMap.GetTileAtPosition(gridPos);
    }

    private void Dragging()
    {
        Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        CardinalDirection direction = (mousePos - startPosition).Cardnialize();
        GameManager.Instance.DirectionToSlide = direction;
        GameManager.Instance.DisplayDirection(selectedTile.GetGridPosition(), GameManager.Instance.cardToPlace.GetCard().GetTile().type == "Slide");
    }
    private void EndMouseSelection()
    {
        selectedTile.Activate(null);
        selectedTile = null;
    }

    private void Deselect()
    {
        if(hoveredTile != null) hoveredTile.Deselect();
    }
}
