using System;
using Guymon.DesignPatterns;
using UnityEngine;

public class RollOut : MonoBehaviour
{
    [SerializeField] private Destinator destinator;
    [SerializeField] private Transform destination;
    [SerializeField] private bool useLastPos;

    private void Awake()
    {

        if (useLastPos)
        {
            destinator.MoveTo(Vector3.zero, true);
        }
        else destinator.MoveTo(destination.position, false);
    }
}
