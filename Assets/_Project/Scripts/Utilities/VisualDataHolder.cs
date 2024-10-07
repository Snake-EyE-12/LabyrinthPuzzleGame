using System.Collections;
using System.Collections.Generic;
using Guymon.DesignPatterns;
using UnityEngine;

public class VisualDataHolder : Singleton<VisualDataHolder>
{
    public int gridSize = 5;
    public int spacing = 2;
    public Vector2 origin = Vector2.zero;
}
