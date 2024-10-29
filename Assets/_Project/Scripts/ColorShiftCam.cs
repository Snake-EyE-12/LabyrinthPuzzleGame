using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorShiftCam : ColorShift
{
    [SerializeField] private Camera cam;
    private new void Update()
    {
        base.Update();
        cam.backgroundColor = GetColor(lightColor, darkColor, t / duration);
    }
}
