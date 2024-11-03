using System.Collections.Generic;
using UnityEngine;

public interface GridPositionable
{
    public Vector2Int GetGridPosition();
    public void SetGridPosition(Vector2Int position, bool wrapping = false);

    public OnTileLocation GetTileLocation();
    public Transform GetSelfTransform();
    public void SetLocalMap(Map map);
    public void OnPassOverLoot(List<LootDisplay> loot);
    public void MoveVisually(Vector3 position);
}


public enum OnTileLocation
{
    None,
    Left,
    Right,
    Bottom
}