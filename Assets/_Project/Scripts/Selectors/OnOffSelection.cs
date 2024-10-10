using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnOffSelection : SelectionDisplay
{
    [SerializeField] private GameObject onDisplay;

    private void Awake()
    {
        EndSelection();
    }

    public override void StartSelection()
    {
        onDisplay.SetActive(true);
    }

    public override void EndSelection()
    {
        onDisplay.SetActive(false);
    }
}
