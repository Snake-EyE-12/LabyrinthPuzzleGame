using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GridUser
{
    private GridBox gridBox;

    public GridBox GetGridBox()
    {
        return gridBox;
    }
    public void InitializeGrid(int size, GridFiller fillType)
    {
        gridBox = new GridBox(size);
        FillGrid(fillType);
    }

    private void FillGrid(GridFiller fillType)
    {
            for (int i = 0; i < gridBox.GetSize(); i++)
            {
                for (int j = 0; j < gridBox.GetSize(); j++)
                {
                    GridSpace space = new GridSpace();
                    Tile tile = fillType.GetFillAt(gridBox.GetSize(), i, j);
                    space.SetTile(tile);
                    gridBox.Set(space, i, j);
                }
            }
    }
    
    public void SlideColumn(int row, bool upwards)
    {
        if (upwards)
        {
            GridSpace temp = gridBox.Get(row, gridBox.GetSize() - 1);
            for (int i = gridBox.GetSize() - 1; i > 0; i--)
            {
                gridBox.Set(gridBox.Get(row, i - 1), row, i);
            }
            gridBox.Set(temp, row, 0);
        }
        //TODO: else opposite
    }
    public void SlideRow(int column, bool upwards)
    {
        if (upwards)
        {
            GridSpace temp = gridBox.Get(gridBox.GetSize() - 1, column);
            for (int i = gridBox.GetSize() - 1; i > 0; i--)
            {
                gridBox.Set(gridBox.Get(i - 1, column), i, column);
            }
            gridBox.Set(temp, 0, column);
        }
        //TODO: else opposite
    }
    public void Place(GridPositionable entity, Vector2Int position)
    {
        gridBox.Get(position.x, position.y).AddObject(entity);
    }

    public bool Move(GridPositionable entity, Vector2Int direction)
    {
        Vector2Int? foundEntityPosition = gridBox.Find(entity);
        if(foundEntityPosition == null) return false;
        if (!IsConnectedPath(foundEntityPosition.Value, direction)) return false;
        
        
        ChangePosition(entity, foundEntityPosition.Value, (foundEntityPosition + direction).Value);
        return true;
    }

    private bool IsConnectedPath(Vector2Int start, Vector2Int direction)
    {
        return gridBox.Get(start.x, start.y).GetTile().IsOpen(direction) && gridBox.Get(start.x + direction.x, start.y + direction.y).GetTile().IsOpen(-direction);
    }

    private void ChangePosition(GridPositionable entity, Vector2Int oldPosition, Vector2Int newPosition)
    {
        gridBox.Get(oldPosition.x, oldPosition.y).RemoveObject(entity);
        gridBox.Get(newPosition.x, newPosition.y).AddObject(entity);
    }

    public Vector2Int? Find(GridPositionable entity)
    {
        return gridBox.Find(entity);
    }
}
