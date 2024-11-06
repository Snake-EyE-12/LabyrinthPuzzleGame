
using System;
using UnityEngine;

public class AttackLineDisplay : Display<LineRendererData>
{
    [SerializeField] private LineRenderer lineRenderer;
    public override void Render()
    {
        
    }

    private void Update()
    {
        Vector3[] positions = new[] { item.follower.position, item.start };
        lineRenderer.SetPositions(positions);
    }
}

public class LineRendererData
{
    public Transform follower;
    public Vector3 start;
}