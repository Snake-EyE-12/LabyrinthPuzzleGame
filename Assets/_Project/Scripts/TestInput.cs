using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInput : MonoBehaviour
{
    [SerializeField] private MapHandler mapHandler;
    private void Update()
    {

        //
        // if (Input.GetKeyDown(KeyCode.LeftArrow))
        // {
        //     unitHandler.GetActiveUnit().Move(Vector2Int.left);
        // }
        //
        // if (Input.GetKeyDown(KeyCode.RightArrow))
        // {
        //     unitHandler.GetActiveUnit().Move(Vector2Int.right);
        // }
        //
        // if (Input.GetKeyDown(KeyCode.UpArrow))
        // {
        //     unitHandler.GetActiveUnit().Move(Vector2Int.up);
        // }
        //
        // if (Input.GetKeyDown(KeyCode.DownArrow))
        // {
        //     unitHandler.GetActiveUnit().Move(Vector2Int.down);
        // }
        //
        
        
        
        
        
        if (Input.GetKeyDown(KeyCode.Q))
        {
            mapHandler.Pressed(0, false);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            mapHandler.Pressed(1, false);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            mapHandler.Pressed(2, false);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            mapHandler.Pressed(3, false);
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            mapHandler.Pressed(4, false);
        }
        
        
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            mapHandler.Pressed(0, true);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            mapHandler.Pressed(1, true);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            mapHandler.Pressed(2, true);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            mapHandler.Pressed(3, true);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            mapHandler.Pressed(4, true);
        }
        
        
        
        
    }
}
