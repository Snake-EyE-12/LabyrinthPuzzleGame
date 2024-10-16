using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyDisplay : Display<Enemy>, GridPositionable, Selectable, Targetable
{
    [SerializeField] private Image coloredImage;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private SelectionDisplay selectionIndicator;
    [SerializeField] private HealthbarDisplay healthBar;
    private Damager damager;
    
    public override void Render()
    {
        //coloredImage.color = DataHolder.characterColorEquivalenceTable.GetColor(item.type);
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
        return OnTileLocation.Right;
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
        GameManager.Instance.AbilityInUse.Use(this);
    }

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
        Debug.Log("Health Lost: " + amount);
        if (amount < 0)
        {
            damager.TakeDamage(-amount);
        }
        else damager.Heal(amount);
        healthBar.Render();
    }

    public void ApplyEffect(ActiveEffect effect)
    {
        damager.ApplyEffect(effect);
    }

    public void Move(Vector2Int direction)
    {
        
    }
}
