using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LayoutDisable : MonoBehaviour
{
    [SerializeField] private LayoutGroup group;

    private bool canDisable = false;
    private bool waitingTick = false;
    private bool currentlyDisabled = false;
    private void Awake()
    {
        group.enabled = true;
        canDisable = true;
        waitingTick = false;
    }

    private void Update()
    {
        if (currentlyDisabled) return;
        if (canDisable)
        {
            if (waitingTick)
            {
                group.enabled = false;
                currentlyDisabled = true;
            }
            waitingTick = true;
        }
    }

    public void AllowDisable()
    {
        canDisable = true;
    }
    
}
