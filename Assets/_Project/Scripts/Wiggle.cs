using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Wiggle : MonoBehaviour
{
    [SerializeField] private bool x;
    [SerializeField] private bool y;
    [SerializeField] private float minDistance;
    [SerializeField] private float maxDistance;
    [SerializeField] private float minAngle;
    [SerializeField] private float maxAngle;
    [SerializeField] private float distanceSpeed;
    [SerializeField] private float angleSpeed;
    [SerializeField] private float offsetAmount;
    
    private Vector3 startingPosition;
    [SerializeField] private float delayBeforeStart;
    [SerializeField] private bool useOffset;


    private void Start()
    {
        thisTime -= delayBeforeStart;
        if (!useOffset) return;
        thisTime -= Random.Range(0, offsetAmount);
    }

    private float thisTime;
    private bool canWiggle;
    private void Update()
    {
        thisTime += Time.deltaTime;
        if (thisTime < 0) return;
        else
        {
            if(!canWiggle) startingPosition = transform.position;
            canWiggle = true;
        }

        float xOffset = (startingPosition.x +
                         Mathf.SmoothStep(minDistance, maxDistance,
                             Mathf.PingPong(thisTime * distanceSpeed, 1)));
        float yOffset = (startingPosition.y + 
                         Mathf.SmoothStep(minDistance, maxDistance,
                             Mathf.PingPong(thisTime * distanceSpeed, 1)));
        transform.position = new Vector3(x ? xOffset : transform.position.x, y ? yOffset : transform.position.y, transform.position.z);
        transform.rotation = Quaternion.Euler(0, 0, Mathf.SmoothStep(minAngle, maxAngle,Mathf.PingPong(thisTime * angleSpeed, 1)));
    }
}
