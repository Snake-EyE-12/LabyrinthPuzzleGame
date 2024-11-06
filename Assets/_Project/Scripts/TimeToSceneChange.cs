using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventArgs = Guymon.DesignPatterns.EventArgs;
using EventHandler = Guymon.DesignPatterns.EventHandler;

public class TimeToSceneChange : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] private float time;
    private void Awake()
    {
        Transition(null);
    }

    private bool change;
    private void Transition(EventArgs args)
    {
        change = true;
    }

    private float elpasedTime;
    private void Update()
    {
        if(change) elpasedTime += Time.deltaTime;
        if (elpasedTime >= time)
        {
            SceneChanger.LoadScene(sceneName);
        }
    }
}
