using System;
using UnityEngine;

public class AfterEffectSprite : MonoBehaviour
{
    [SerializeField] private float distance;
    [SerializeField] private int amount;
    [SerializeField] private float delay;

    private float elapsedTime;
    private int movements;
    private void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime > delay)
        {
            transform.Translate(Vector3.left * distance / amount);
            elapsedTime = 0;
            movements++;
            if (movements >= amount)
            {
                Destroy(gameObject);
            }
        }
    }
}