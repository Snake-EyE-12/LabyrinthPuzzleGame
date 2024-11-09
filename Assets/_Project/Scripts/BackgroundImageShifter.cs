using System;
using System.Collections.Generic;
using Guymon.DesignPatterns;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundImageShifter : MonoBehaviour
{
    [SerializeField] private List<Sprite> _backgroundImages;
    [SerializeField] private Image _backgroundImage;
    [SerializeField] private float speed;


    private int currentIndex = 0;
    private float elapsedTime = 0;
    
    private void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= speed)
        {
            currentIndex = (currentIndex + 1) % _backgroundImages.Count;
            _backgroundImage.sprite = _backgroundImages[currentIndex];
            elapsedTime = 0;
            
        }
    }
}
