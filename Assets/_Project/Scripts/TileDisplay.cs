using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventArgs = Guymon.DesignPatterns.EventArgs;

public class TileDisplay : Display<Tile>, Poolable<TileDisplay>
{
    [SerializeField] private List<WallDisplay> wallDisplays = new List<WallDisplay>();
    public override void Render(Tile tile)
    {
        int orientation = tile.GetOrientation();
        for (int i = 0, j = 1; i < 4; i++, j *= 2)
        {
            wallDisplays[i].SetVisibility((orientation & j) == 0);
        }
    }

    private void OnDisable()
    {
        available = true;
    }

    public TileDisplay GetSelf()
    {
        return this;
    }

    public void Reset()
    {
        available = false;
        gameObject.SetActive(true);
    }
    private bool available = false;
    public bool IsAvailable()
    {
        return available;
    }
}

