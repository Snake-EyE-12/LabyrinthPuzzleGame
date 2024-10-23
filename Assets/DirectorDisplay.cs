using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventArgs = Guymon.DesignPatterns.EventArgs;

public class DirectorDisplay : MonoBehaviour
{
    [SerializeField] private Transform pivot;
    [SerializeField] private Transform boundary;

    private void Awake()
    {
        Guymon.DesignPatterns.EventHandler.AddListener("Round/FightOver", Hide);
    }

    private void SetSize(int size)
    {
        boundary.localScale = new Vector3(VisualDataHolder.Instance.spacing,
            size * VisualDataHolder.Instance.spacing, 1);
    }

    private void PositionAtRow(int row, int size)
    {
        transform.position = new Vector3(VisualDataHolder.Instance.Center(size).x, VisualDataHolder.Instance.CoordsToPosition(new Vector2Int(0, row)).y, 0);
    }
    private void PositionAtColumn(int col, int size)
    {
        transform.position = new Vector3(VisualDataHolder.Instance.CoordsToPosition(new Vector2Int(col, 0)).x, VisualDataHolder.Instance.Center(size).y, 0);
    }

    private void FaceAt(CardinalDirection direction)
    {
        switch (direction)
        {
            case CardinalDirection.North:
                pivot.transform.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case CardinalDirection.West:
                pivot.transform.rotation = Quaternion.Euler(0, 0, 90);
                break;
            case CardinalDirection.South:
                pivot.transform.rotation = Quaternion.Euler(0, 0, 180);
                break;
            case CardinalDirection.East:
                pivot.transform.rotation = Quaternion.Euler(0, 0, 270);
                break;
            
        }
    }

    private int lastSize;
    private Vector2Int lastPos;
    public void Display(CardinalDirection direction, int size, Vector2Int pos)
    {
        lastSize = size;
        lastPos = pos;
        SetSize(size);
        if(GameUtils.IsDirectionRow(direction)) PositionAtRow(pos.y, size);
        else PositionAtColumn(pos.x, size);
        FaceAt(direction);
        gameObject.SetActive(true);
    }

    public void Hide(EventArgs args)
    {
        gameObject.SetActive(false);
    }

    public void ChangeRotation(CardinalDirection direction)
    {
        Display(direction, lastSize, lastPos);
    }
}
