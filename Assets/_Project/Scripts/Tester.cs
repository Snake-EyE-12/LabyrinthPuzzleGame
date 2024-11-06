using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventHandler = Guymon.DesignPatterns.EventHandler;

public class Tester : MonoBehaviour
{
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.L)) EventHandler.Invoke("OnGameLost", null);
    }
}
