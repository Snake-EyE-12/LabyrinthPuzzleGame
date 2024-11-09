using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorShift : MonoBehaviour
{
    [SerializeField] protected Color lightColor;
    [SerializeField] protected Color darkColor;
    [SerializeField] protected float duration = 100f;
    
    protected float t;
    private bool isReverse = false;
    protected void Update()
    {
        if(isReverse)
        {
            t -= Time.deltaTime;
            if(t < 0) isReverse = false;
        }
        else
        {
            t += Time.deltaTime;
            if(t > duration) isReverse = true;
        }
    }
    protected Color GetColor(Color light, Color dark, float t)
    {
        return Color.Lerp(light, dark, t);
    }

    public void SetColorSet(Color light, Color dark)
    {
        lightColor = light;
        darkColor = dark;
    }
    
}
