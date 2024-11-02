using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tester : MonoBehaviour
{
    [SerializeField] private Floater burnAfterImage;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Burn(0.5f);
        }
    }
    public void Burn(float percent)
    {
        Floater e = Instantiate(burnAfterImage, transform);
        e.Set(percent);
    }
}
