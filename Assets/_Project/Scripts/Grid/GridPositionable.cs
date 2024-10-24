using System.Collections.Generic;
using UnityEngine;

public interface GridPositionable
{
    public Vector2Int GetGridPosition();
    public void SetGridPosition(Vector2Int position);

    public OnTileLocation GetTileLocation();
    public Transform GetSelfTransform();
    public void SetLocalMap(Map map);
    public void OnPassOverLoot(List<LootDisplay> loot);
}


public enum OnTileLocation
{
    None,
    Left,
    Right,
    Bottom
}