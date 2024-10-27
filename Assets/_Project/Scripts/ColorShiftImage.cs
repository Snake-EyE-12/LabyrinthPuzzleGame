using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorShiftImage : ColorShift
{
    [SerializeField] private Image image;
    private void Update()
    {
        base.Update();
        image.color = GetColor(lightColor, darkColor, t / duration);
    }
}
