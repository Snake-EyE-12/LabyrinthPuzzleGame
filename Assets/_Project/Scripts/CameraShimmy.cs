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
        cam.orthographicSize = Mathf.PingPong(Time.time * sizeSpeed, maxSize - minSize) + minSize;
        transform.position = new Vector3(startingPosition.x + Mathf.PingPong(Time.time * distanceSpeed, maxDistance - minDistance) + minDistance,
            startingPosition.y + Mathf.PingPong(Time.time * distanceSpeed, maxDistance - minDistance) + minDistance,
            transform.position.z);
        transform.rotation = Quaternion.Euler(0, 0, Mathf.PingPong(Time.time * angleSpeed, maxAngle));
    }
}
