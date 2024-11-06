using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventArgs = Guymon.DesignPatterns.EventArgs;
using EventHandler = Guymon.DesignPatterns.EventHandler;

public class TimeSetter : MonoBehaviour
{
    public void SetTimeToNormal()
    {
        Time.timeScale = 1;
    }

    public void StopTime()
    {
        Time.timeScale = 0;
    }
}
