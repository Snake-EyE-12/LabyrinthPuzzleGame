using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Heart : MonoBehaviour
{
    [SerializeField] private Image heartImage;
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite emptyHeart;
    
    public void Fill(Color color)
    {
        heartImage.sprite = fullHeart;
        heartImage.color = color;
    }

    public void Empty()
    {
        heartImage.sprite = emptyHeart;
        heartImage.color = Color.white;

    }
}
