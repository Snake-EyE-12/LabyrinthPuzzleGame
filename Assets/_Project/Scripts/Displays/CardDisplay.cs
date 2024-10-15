using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardDisplay : Display<Card>, Selectable
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private SelectionDisplay selectionIndicator;
    [SerializeField] private TileOnCardDisplay tileForDisplay;
    public override void Render()
    {
        text.text = "Loaded";
        tileForDisplay.Set(item.GetTile());
    }

    public void RotateTile(RotationDirection direction, int amount)
    {
        item.GetTile().rotation.Rotate(direction, amount);
        Render();
    }

    public Card GetCard()
    {
        return item;
    }

    public Tile GetTile()
    {
        return item.GetTile();
    }

    public void Select()
    {
        selectionIndicator.StartSelection();
    }

    public void Deselect()
    {
        selectionIndicator.EndSelection();
    }

    public int GetOrderValue()
    {
        return 0;
    }

    public bool IsCurrentlySelectable()
    {
        return true;
    }

    public void Activate(SelectableActivatorData data)
    {
        GameManager.Instance.SetCardToPlace(this);
    }
    private void Start()
    {
        GameManager.Instance.AddSelectable(this, selectionIndicator.type);
    }

    public void RemoveFromPlay()
    {
        GameManager.Instance.RemoveSelectable(this, selectionIndicator.type);
        Destroy(this.gameObject);
    }
}