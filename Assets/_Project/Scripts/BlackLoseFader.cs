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


    private void Awake()
    {
        EventHandler.AddListener("OnGameLost", Fade);
    }

    private void Fade(EventArgs args)
    {
        EventHandler.RemoveListenerLate("OnGameLost", Fade);
        destinator.MoveTo(finalPos.position, false);
        shifter.enabled = true;
    }
}
