using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using EventHandler = Guymon.DesignPatterns.EventHandler;

public class Tester : MonoBehaviour
{
    [SerializeField] private Vector3 targetPosition;
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.L)) EventHandler.Invoke("OnGameLost", null);
        if (Input.GetKeyDown(KeyCode.K))
        {
            EventHandler.Invoke("OnGameWin", null);
        }

        
        if (Input.GetKeyDown(KeyCode.P))
        {
            EffectHolder.Instance.SpawnEffect("Death", targetPosition);
        }
        
    }
}
