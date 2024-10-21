using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventHandler = Guymon.DesignPatterns.EventHandler;

public class EndTurnButton : MonoBehaviour
{
    public void EndTurn()
    {
        EventHandler.Invoke("Round/EndTurn", null);
    }
    
}
