using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] private TMP_Text gameOverText;
    [SerializeField] private Color startColor;
    [SerializeField] private Color endColor;
    [SerializeField] private float time;

    
    private bool isFading;
    private void Awake()
    {
        isFading = true;
    }

    private float elapsedTime = 0;
    private void Update()
    {
        elapsedTime += Time.deltaTime;
        if (isFading)
        {
            Color color = Color.Lerp(startColor, endColor, elapsedTime / time);
            gameOverText.color = color;
            if(elapsedTime >= time) isFading = false;
        }
    }
}
