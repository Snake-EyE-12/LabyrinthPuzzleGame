using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowTile : Display<ShowTileData>
{
    [SerializeField] private List<WallDisplay> wallDisplays = new List<WallDisplay>();
    [SerializeField] private Destinator destinator;
    public override void Render()
    {
        int orientation = item.tile.GetOrientation();
        for (int i = 0, j = 1; i < 4; i++, j *= 2)
        {
            wallDisplays[i].SetVisibility((orientation & j) == 0);
        }
        destinator.MoveTo(new DestinationData(new Vector3(item.pos.x * 2, item.pos.y * 2, 0), 0.1f, false));
    }

    public void Move(Vector2Int newPos, bool instant)
    {
        if(instant) destinator.MoveTo(new DestinationData(new Vector3(newPos.x * 2, newPos.y * 2, 0), 0.001f, false));
        destinator.MoveTo(new Vector3(newPos.x * 2, newPos.y * 2, 0), false);
    }
}
