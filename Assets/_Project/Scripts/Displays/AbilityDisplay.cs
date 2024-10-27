using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityDisplay : Display<Ability>, Selectable
{
    [SerializeField] private Image colorRingImageRenderer;
    [SerializeField] private Transform valuePointsParent;
    [SerializeField] private GameObject valuePointPrefab;
    [SerializeField] private TMP_Text descriptionTextbox;
    
    [Header("Non-Display")]
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
        colorRingImageRenderer.color = DataHolder.characterColorEquivalenceTable.GetColor(item.owner);
        
    
        descriptionTextbox.gameObject.SetActive(true);
        var descriptionBuilder = "";
        foreach (var keyword in item.keys)
        {
            descriptionBuilder += keyword.GetKeywordName().ConvertToString() + ", ";
        }
        descriptionTextbox.text = descriptionBuilder.Substring(0, descriptionBuilder.Length - 2);
        
        valuePointsParent.gameObject.SetActive(item.value > 0);
        foreach (Transform child in valuePointsParent)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < item.value; i++)
        {
            var valuePoint = Instantiate(valuePointPrefab, valuePointsParent);
        }
    
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
        GameManager.Instance.Phase = GamePhase.UsingActiveAbility;
        item.PrepareTarget();
    }
}
