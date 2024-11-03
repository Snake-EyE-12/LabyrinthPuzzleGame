using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShimmy : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private float minSize;
    [SerializeField] private float maxSize;
    [SerializeField] private float sizeSpeed;
    [SerializeField] private float minDistance;
    [SerializeField] private float maxDistance;
    [SerializeField] private float minAngle;
    [SerializeField] private float maxAngle;
    [SerializeField] private float distanceSpeed;
    [SerializeField] private float angleSpeed;
    
    private Vector3 startingPosition;

    private void Awake()
    {
        startingPosition = transform.position;
    }

    private void Update()
    {
        
        cam.orthographicSize = Mathf.SmoothStep(minSize, maxSize, Mathf.PingPong(Time.time * sizeSpeed, 1));
        transform.position = new Vector3(startingPosition.x + Mathf.SmoothStep(minDistance, maxDistance, Mathf.PingPong(Time.time * distanceSpeed, 1)),
            startingPosition.y + Mathf.SmoothStep(minDistance, maxDistance,Mathf.PingPong(Time.time * distanceSpeed, 1)),
            transform.position.z);
        transform.rotation = Quaternion.Euler(0, 0, Mathf.SmoothStep(minAngle, maxAngle,Mathf.PingPong(Time.time * angleSpeed, 1)));
    }
}
