using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScalePingPong : MonoBehaviour
{
    [SerializeField] private float minScale;
    [SerializeField] private float maxScale;
    [SerializeField] private float speed;

    private void Update()
    {
        float scale = Mathf.SmoothStep(minScale, maxScale, Mathf.PingPong(Time.time * speed, 1));
        transform.localScale = new Vector3(scale, scale, scale);
    }
}
