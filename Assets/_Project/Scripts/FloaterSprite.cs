using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloaterSprite : MonoBehaviour
{
    [SerializeField] private float speed = 20;
    [SerializeField] private float lifetime = 3;
    [SerializeField] private float shrinkPercent = 0.99f;
    private void Start()
    {
        Destroy(this.gameObject, lifetime);
    }

    private void Update()
    {
        transform.Translate(Vector3.up * Time.deltaTime * speed);
        transform.localScale *= shrinkPercent;
    }

}
