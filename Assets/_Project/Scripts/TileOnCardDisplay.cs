using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileOnCardDisplay : Display<Tile>
{
    [SerializeField] private List<WallDisplay> wallDisplays = new List<WallDisplay>();

    public override void Render()
    {
        int orientation = item.GetOrientation();
        for (int i = 0, j = 1; i < 4; i++, j *= 2)
        {
            wallDisplays[i].SetVisibility((orientation & j) == 0);
        }
    }
}
