
using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemLayout : MonoBehaviour
{
    private List<GridPositionable> gridItems = new List<GridPositionable>();
    [SerializeField] private Vector3 initPoint;
    [SerializeField] private Vector3 finalPoint;
    public void Add(GridPositionable gridItem)
    {
        gridItem.GetSelfTransform().SetParent(transform);
        gridItems.Add(gridItem);
        Refresh();
    }

    public void Remove(GridPositionable gridItem)
    {
        gridItems.Remove(gridItem);
        Refresh();
    }

    private void Refresh()
    {
        for (int i = 0; i < gridItems.Count; i++)
        {
            gridItems[i].MoveVisually(Vector3.Lerp(transform.position + initPoint,
                transform.position + finalPoint, i * 1.0f / gridItems.Count));
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(initPoint + transform.position, finalPoint + transform.position);
    }
}
