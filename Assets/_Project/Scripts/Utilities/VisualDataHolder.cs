using System.Collections;
using System.Collections.Generic;
using Guymon.DesignPatterns;
using UnityEngine;

public class VisualDataHolder : Singleton<VisualDataHolder>
{
    public int spacing = 2;
    public Vector2 origin = Vector2.zero;
    public Vector3 CoordsToPosition(Vector2Int coords)
    {
        return new Vector3(coords.x * spacing, coords.y * spacing, 0) + new Vector3(origin.x, origin.y, 0);
    }

    public Vector3 Center(int size)
    {
        return new Vector3((size - 1) / 2f, (size - 1) / 2f) * spacing + new Vector3(origin.x, origin.y, 0);
    }
}
