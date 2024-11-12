using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wiggle : MonoBehaviour
{
    [SerializeField] private float minDistance;
    [SerializeField] private float maxDistance;
    [SerializeField] private float minAngle;
    [SerializeField] private float maxAngle;
    [SerializeField] private float distanceSpeed;
    [SerializeField] private float angleSpeed;
    
    private Vector3 startingPosition;
    [SerializeField] private float delayBeforeStart;
    

    private float thisTime;
    private bool canWiggle;
    private void Update()
    {
        thisTime += Time.deltaTime;
        if (thisTime < delayBeforeStart) return;
        else
        {
            if(!canWiggle) startingPosition = transform.position;
            canWiggle = true;
        }
        
        transform.position = new Vector3(startingPosition.x + Mathf.SmoothStep(minDistance, maxDistance, Mathf.PingPong(thisTime * distanceSpeed, 1)),
            startingPosition.y + Mathf.SmoothStep(minDistance, maxDistance,Mathf.PingPong(thisTime * distanceSpeed, 1)),
            transform.position.z);
        transform.rotation = Quaternion.Euler(0, 0, Mathf.SmoothStep(minAngle, maxAngle,Mathf.PingPong(thisTime * angleSpeed, 1)));
    }
}