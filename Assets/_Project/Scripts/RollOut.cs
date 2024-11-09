using System;
using Guymon.DesignPatterns;
using UnityEngine;

public class RollOut : MonoBehaviour
{
    [SerializeField] private Destinator destinator;
    [SerializeField] private Transform destination;

    private void Awake()
    {
        
        destinator.MoveTo(destination.position, false);
    }
}
