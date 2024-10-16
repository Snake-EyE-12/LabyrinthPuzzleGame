using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Heart : MonoBehaviour
{
    [SerializeField] private Image heartImage;
    
    public void SetColor(Color color)
    {
        heartImage.color = color;
    }
}
