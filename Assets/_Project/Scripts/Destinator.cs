using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destinator : MonoBehaviour
{
    private DestinationData activeDestination;
    private Vector3 startingPosition;
    private float elapsedTime;
    private float duration;
    private bool moving;
    private Queue<DestinationData> forwardDestinations = new Queue<DestinationData>();
    [SerializeField] private float baseTime;
    public void MoveTo(List<DestinationData> locations)
    {
        foreach (var location in locations)
        {
            MoveTo(location);
        }
    }
    public void MoveTo(DestinationData location)
    {
        forwardDestinations.Enqueue(location);
    }

    public void MoveTo(Vector3 position, bool local)
    {
        MoveTo(new DestinationData(position, baseTime, local));
    }
    
    private void Update()
    {
        if (activeDestination != null)
        {
            if(activeDestination.useLocal) transform.localPosition = Vector3.Lerp(startingPosition, activeDestination.position, elapsedTime / duration);
            else transform.position = Vector3.Lerp(startingPosition, activeDestination.position, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= duration)
            {
                if(activeDestination.useLocal) transform.localPosition = activeDestination.position;
                else transform.position = activeDestination.position;
                activeDestination = null;
            }

            return;
        }
        if (forwardDestinations.Count > 0)
        {
            activeDestination = forwardDestinations.Dequeue();
            elapsedTime = 0;
            duration = activeDestination.time;
            startingPosition = (activeDestination.useLocal) ? transform.localPosition : transform.position;
        }
    }
}

public class DestinationData
{
    public DestinationData(Vector3 position, float time, bool local) { this.position = position; this.time = time; this.useLocal = local; }
    public Vector3 position;
    public bool useLocal;
    public float time;
}
