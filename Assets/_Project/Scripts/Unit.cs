using UnityEngine;

public class Unit : GridPositionable
{
    private Map localMap;

    public void SetMap(Map map)
    {
        localMap = map;
    }
    public void Move(Vector2Int direction)
    {
        localMap.Move(this, direction);
    }

    public Vector2Int? GetPosition()
    {
        return localMap.GetUnitLocation(this);
        
    }
}