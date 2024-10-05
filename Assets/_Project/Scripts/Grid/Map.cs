
using UnityEngine;

public class Map
{
    public readonly int size;
    private GridUser gridHandler;

    public GridBox GetMapLayout()
    {
        return gridHandler.GetGridBox();
    }
    

    public Map(int size, GridFiller filler)
    {
        this.size = size;
        gridHandler = new GridUser();
        gridHandler.InitializeGrid(size, filler);
        
        //Temporary
        Unit hero = new Unit();
        hero.SetMap(this);
        SpawnUnit(hero, new Vector2Int(0, 0));
        hero.Move(Vector2Int.up);
        hero.Move(Vector2Int.up);
        Debug.Log(gridHandler.GetGridBox().Find(hero));
    }
    
    public void Slide(bool row, bool positive, int number)
    {
        if (row) gridHandler.SlideRow(number, positive);
        else gridHandler.SlideColumn(number, positive);
    }

    public void Move(GridPositionable entity, Vector2Int direction)
    {
        gridHandler.Move(entity, direction);
    }

    public void SpawnUnit(Unit unit, Vector2Int position)
    {
        gridHandler.Place(unit, position);
    }

    public Vector2Int? GetUnitLocation(Unit unit)
    {
        return gridHandler.Find(unit);
    }

}