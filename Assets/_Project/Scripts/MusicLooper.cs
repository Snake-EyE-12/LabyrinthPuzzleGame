using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MusicLooper : MonoBehaviour
{
    [SerializeField] private AudioSource source;

    private void Update()
    {
        if (!source.isPlaying)
        {
            source.Play();
        }
    }
}
