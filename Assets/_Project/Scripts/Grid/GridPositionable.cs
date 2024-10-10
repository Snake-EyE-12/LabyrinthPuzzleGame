using UnityEngine;

public interface GridPositionable
{
    public Vector2Int GetGridPosition();
    public void SetGridPosition(Vector2Int position);

    public OnTileLocation GetTileLocation();
    public Transform GetSelfTransform();
}


public enum OnTileLocation
{
    None,
    Left,
    Right,
    Bottom
}