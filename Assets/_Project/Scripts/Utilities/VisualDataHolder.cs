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
        cam.transform.position = Center(DataHolder.currentMode.GridSize, -10) + new Vector3(-3, 0, 0);
        cam.orthographicSize = DataHolder.currentMode.GridSize * 1.5f;
    }

}
