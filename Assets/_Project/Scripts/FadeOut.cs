using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EventArgs = Guymon.DesignPatterns.EventArgs;
using EventHandler = Guymon.DesignPatterns.EventHandler;

public class FadeOut : MonoBehaviour
{
    [SerializeField] private float time;
    [SerializeField] private Image image;


    private float elapsedTime;
    private void Update()
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, (time - elapsedTime) / time);
        elapsedTime += Time.deltaTime;
        
        if (elapsedTime > time)
        {
            gameObject.SetActive(false);
        }
    }
}
