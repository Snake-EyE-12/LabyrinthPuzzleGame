using System;
using System.Collections;
using System.Collections.Generic;
using Guymon.DesignPatterns;
using NaughtyAttributes;
using UnityEngine;

public class VisualDataHolder : Singleton<VisualDataHolder>
{
    public int spacing = 2;
    public Vector2 origin = Vector2.zero;
    [SerializeField] private Camera cam;
    public Vector3 CoordsToPosition(Vector2Int coords)
    {
        return new Vector3(coords.x * spacing, coords.y * spacing, 0) + new Vector3(origin.x, origin.y, 0);
    }
    public Vector2Int PositionToCoords(Vector3 position)
    {
        return new Vector2Int(Mathf.RoundToInt(position.x / spacing), Mathf.RoundToInt(position.y / spacing));
    }

    public Vector3 Center(int size, int z = 0)
    {
        return new Vector3((size - 1) / 2f, (size - 1) / 2f) * spacing + new Vector3(origin.x, origin.y, z);
    }

    private void Awake()
    {
        SetCameraPosition();
    }

    [Button]
    private void SetCameraPosition()
    {
        int x = -3 - ((GetSize() - 3));
        Vector3 offset = new Vector3(x, 0, 0);
        cam.transform.position = Center(GetSize(), -10) + offset;
        cam.orthographicSize = GetSize() * 1.5f;
    }

    private int GetSize()
    {
        if (useSpecificSize) return specificSize;
        return DataHolder.currentMode.GridSize;
    }
    
    [SerializeField] private bool useSpecificSize = false;
    [SerializeField] private int specificSize;

}
