using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitDisplay : Display<Unit>
{
    public override void Render(Unit item)
    {
        
    }

    public void Move(Unit unit)
    {
        Vector2Int? unitPosition = unit.GetPosition();
        if(!unitPosition.HasValue) return;
        SetWorldPosition(unitPosition.Value);
    }

    private void SetWorldPosition(Vector2Int coords)
    {
        transform.position = new Vector3(coords.x * VisualDataHolder.Instance.spacing, coords.y * VisualDataHolder.Instance.spacing, 0) + new Vector3(VisualDataHolder.Instance.origin.x, VisualDataHolder.Instance.origin.y, 0);
    }
}
