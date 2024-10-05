using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapHandler : MonoBehaviour
{
    private Map map;
    [SerializeField] private MapDisplay mapDisplay;

    private void Awake()
    {
        map = new Map(VisualDataHolder.Instance.gridSize, new RandomFiller());
        mapDisplay.Render(map);
    }

    public void Pressed(int number, bool row)
    {
        map.Slide(row, true, number);
    }

    public Map GetMap()
    {
        return map;
        
    }
    
}