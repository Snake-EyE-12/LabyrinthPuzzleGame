using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class ScaleSizeByCanvas : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private float scale = 0.1f;
    [SerializeField] private Camera cam;

    private void Update()
    {
        SetPositionToCanvasCenterAndScale();
    }
    [Button]
    private void SetPositionToCanvasCenterAndScale()
    {
        transform.position = canvas.transform.position;
        cam.orthographicSize = (canvas.transform.lossyScale * scale).x;
    }
}
