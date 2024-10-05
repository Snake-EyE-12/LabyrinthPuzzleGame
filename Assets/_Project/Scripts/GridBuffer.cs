using System.Collections.Generic;
using UnityEngine;

public class GridBuffer<T> where T : class
{
    private Dictionary<Vector2Int, T> bufferA = new Dictionary<Vector2Int, T>();
    private Dictionary<Vector2Int, T> bufferB = new Dictionary<Vector2Int, T>();

    private bool current = false;

    public Dictionary<Vector2Int, T> GetActiveBuffer()
    {
        return current ? bufferA : bufferB;
    }

    public void Swap()
    {
        current = !current;
    }
}