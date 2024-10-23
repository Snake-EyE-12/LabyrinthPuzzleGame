using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AbilityDisplay : Display<Ability>, Selectable
{
    [SerializeField] private TMP_Text target;
    [SerializeField] private TMP_Text value;
    [SerializeField] private TMP_Text keys;
    [SerializeField] private SelectionDisplay selectionIndicator;
    private void Start()
    {
        GameManager.Instance.AddSelectable(this, selectionIndicator.type);
    }

    private void OnDestroy()
    {
        GameManager.Instance.RemoveSelectable(this, selectionIndicator.type);
    }

    public override void Render()
    {
        target.text = "Target: " + item.targetDescription;
        value.text = "Value: " + item.value;
        keys.text = "Keys: " + item.GetKeywordsString();
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
        GameManager.Instance.AbilityInUse = item;
        item.PrepareTarget();
        GameManager.Instance.Phase = GamePhase.UsingActiveAbility;
    }
}
