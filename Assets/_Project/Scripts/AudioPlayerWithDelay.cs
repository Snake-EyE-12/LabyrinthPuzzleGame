using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayerWithDelay : MonoBehaviour
{
    [SerializeField] private string audioName;
    [SerializeField] private string audioNameOnDelay;
    [SerializeField] private float delay;

    private void Start()
    {
        if(audioName.Length > 0) AudioManager.Instance.Play(audioName);
        if(audioNameOnDelay.Length > 0) AudioManager.Instance.Play(audioNameOnDelay, delay);
    }
}
