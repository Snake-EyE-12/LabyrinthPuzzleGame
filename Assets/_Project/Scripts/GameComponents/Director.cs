using System.Collections;
using System.Collections.Generic;
using Guymon.DesignPatterns;
using UnityEngine;
using EventArgs = Guymon.DesignPatterns.EventArgs;

public class Director : MonoBehaviour
{
    [SerializeField] private Transform pivot;
    [SerializeField] private Transform boundary;
    [SerializeField] private Transform swapper;

    private void Awake()
    {
        //Guymon.DesignPatterns.EventHandler.AddListener("Round/FightOver", Hide);
        Guymon.DesignPatterns.EventHandler.AddListener("CardPlaced", Hide);
        //DontDestroyOnLoad(this.gameObject);
    }

    private void SetSize(int size)
    {
        boundary.localScale = new Vector3(VisualDataHolder.Instance.spacing,
            size * VisualDataHolder.Instance.spacing, 1);
        swapper.localScale = Vector3.one * VisualDataHolder.Instance.spacing;
    }

    private void PositionAtRow(int row, int size)
    {
        transform.position = new Vector3(VisualDataHolder.Instance.Center(size).x, VisualDataHolder.Instance.CoordsToPosition(new Vector2Int(0, row)).y, 0);
    }
    private void PositionAtColumn(int col, int size)
    {
        transform.position = new Vector3(VisualDataHolder.Instance.CoordsToPosition(new Vector2Int(col, 0)).x, VisualDataHolder.Instance.Center(size).y, 0);
    }

    private void PositionAll(int row, int col)
    {
        transform.position = VisualDataHolder.Instance.CoordsToPosition(new Vector2Int(col, row));
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
    private bool isSlider;
    public void Display(CardinalDirection direction, int size, Vector2Int pos, bool slider)
    {
        lastSize = size;
        lastPos = pos;
        SetSize(size);
        isSlider = slider;
        if (slider)
        {
            if(GameUtils.IsDirectionRow(direction)) PositionAtRow(pos.y, size);
            else PositionAtColumn(pos.x, size);
            FaceAt(direction);
        }
        else PositionAll(pos.y, pos.x);

        boundary.gameObject.SetActive(slider);
        swapper.gameObject.SetActive(!slider);
    }

    public void Hide(EventArgs args)
    {
        boundary.gameObject.SetActive(false);
        swapper.gameObject.SetActive(false);
    }

    public void ChangeRotation(CardinalDirection direction)
    {
        if(isSlider) Display(direction, lastSize, lastPos, true);
    }

    private void OnDisable()
    {
        EventHandler.RemoveListener("CardPlaced", Hide);
    }
}
