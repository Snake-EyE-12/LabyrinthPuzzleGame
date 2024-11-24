using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventArgs = Guymon.DesignPatterns.EventArgs;
using EventHandler = Guymon.DesignPatterns.EventHandler;

public class BlackLoseFader : MonoBehaviour
{
    [SerializeField] private Destinator destinator;
    [SerializeField] private ColorShift shifter;
    [SerializeField] private Transform finalPos;
    [SerializeField] private string eventName;


    private void Awake()
    {
        EventHandler.AddListener(eventName, Fade);
    }

    private void Fade(EventArgs args)
    {
        EventHandler.RemoveListener(eventName, Fade);
        destinator.MoveTo(finalPos.position, false);
        shifter.enabled = true;
    }
}
