using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTimer : MonoBehaviour
{
    [SerializeField] private float lifetime;
    private void Start()
    {
        Destroy(gameObject, lifetime);
    }
}
