using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventHandler = Guymon.DesignPatterns.EventHandler;

public class EndTurnButton : MonoBehaviour
{
    public void EndTurn()
    {
        //GameObject myEventSystem = GameObject.Find("EventSystem");
        //myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);
        EventHandler.Invoke("Round/EndTurn", null);
    }
    
}
