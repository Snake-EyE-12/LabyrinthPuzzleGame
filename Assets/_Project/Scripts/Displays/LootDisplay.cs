using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LootDisplay : Display<Loot>, GridPositionable
{
    [SerializeField] private Image image;
    [SerializeField] private TMP_Text text;
    public override void Render()
    {
        image.sprite = Resources.Load<Sprite>("KeynamedSprites/Loot/" + item.GetImageName());
        text.text = item.GetDisplayValue();
    }

    public void Collect(CharacterDisplay collector)
    {
        item.Collect(collector);
        //remove self
        localMap.RemoveGridPositionable(this);
        Destroy(this.gameObject);
    }

    private Vector2Int gridPosition;
    

    public Vector2Int GetGridPosition()
    {
        return gridPosition;
    }

    public void SetGridPosition(Vector2Int value, bool wrapping = false)
    {
        gridPosition = value;
    }

    public OnTileLocation GetTileLocation()
    {
        return OnTileLocation.Bottom;
    }

    public Transform GetSelfTransform()
    {
        return gameObject.transform;
    }

    private Map localMap;
    public void SetLocalMap(Map map)
    {
        localMap = map;
    }

    public void OnPassOverLoot(List<LootDisplay> loot)
    {
        
    }
}