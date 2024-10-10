using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInput : MonoBehaviour
{
    [SerializeField] private InputSelector selector;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            selector.SelectFirst();
        }
        
        
        
    }
}
