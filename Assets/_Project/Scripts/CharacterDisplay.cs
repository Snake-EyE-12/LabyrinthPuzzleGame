using System;
using System.Collections;
using System.Collections.Generic;
using Capstone.DataLoad;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using EventHandler = Guymon.DesignPatterns.EventHandler;

public class CharacterDisplay : Display<Character>, GridPositionable, Selectable, Targetable
{
    [SerializeField] private Image coloredImage;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private SelectionDisplay selectionIndicator;
    [SerializeField] private HealthbarDisplay healthBar;
    [SerializeField] private ExperienceDisplay xpBar;
    private Damager damager;
    

    public override void Render()
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

    private Map localMap;
    public void SetLocalMap(Map map)
    {
        localMap = map;
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
        if (data is DirectionalSelectableActivatorData)
        {
            localMap.Move(this, (data as DirectionalSelectableActivatorData).direction);
            return;
        }

        if (data is ConfirmSelectableActivatorData)
        {
            EventHandler.Invoke("Destroy/AbilityList", null);
            Instantiate(characterAbilityDisplayPrefab, GameManager.Instance.GetCanvasParent()).Set(item.abilityList);
            GameManager.Instance.AbilityUser = this;
            GameManager.Instance.SetSelectionMode(SelectableGroupType.Ability);
            return;
        }
    }
    [SerializeField] private CharacterAbilityDisplay characterAbilityDisplayPrefab;

    private void Start()
    {
        GameManager.Instance.AddSelectable(this, selectionIndicator.type);
        damager = new Damager(item);
        healthBar.Set(item.health);
    }

    public void HitByAbility(Ability ability)
    {
        ability.Use(this);
    }

    public void ChangeHealth(int amount)
    {
        if (amount > 0)
        {
            damager.TakeDamage(amount);
        }
        else damager.Heal(-amount);
        healthBar.Render();
    }

    public void ApplyEffect(ActiveEffect effect)
    {
        damager.ApplyEffect(effect);
    }
    public void Move(Vector2Int direction)
    {
        SetGridPosition(GetGridPosition() + direction);
    }
}

