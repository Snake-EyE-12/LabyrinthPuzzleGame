using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventHandler = Guymon.DesignPatterns.EventHandler;
using Random = UnityEngine.Random;

public class Flee : MonoBehaviour
{
    [SerializeField] private GameObject menu;
    public void FleeGame()
    {
        menu.SetActive(false);
        EventHandler.Invoke("OnGameLost", null);
    }
}
