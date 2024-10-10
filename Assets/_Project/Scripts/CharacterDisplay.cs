using System;
using System.Collections;
using System.Collections.Generic;
using Capstone.DataLoad;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterDisplay : Display<Character>, GridPositionable, Selectable
{
    [SerializeField] private Image coloredImage;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private SelectionDisplay selectionIndicator;
    public override void Render(Character item)
    {
        coloredImage.color = DataHolder.characterColorEquivalenceTable.GetColor(item.characterType);
        nameText.text = item.unitName;
    }


    private Vector2Int gridPosition;
    public Vector2Int GetGridPosition()
    {
        return gridPosition;
    }

    public void SetGridPosition(Vector2Int value)
    {
        gridPosition = value;
    }

    public OnTileLocation GetTileLocation()
    {
        return OnTileLocation.Left;
    }

    public Transform GetSelfTransform()
    {
        return gameObject.transform;
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

    private void Start()
    {
        GameManager.Instance.AddSelectable(this, SelectableGroupType.Team);
    }
}
