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

    private EnemyAttackVisual choosenAttack;
    public void ChooseAttack()
    {
        choosenAttack = new EnemyAttackVisual(){attack = item.attackLayout.Pick(), user = this};
        AttackIndicator.Instance.AddAttack(choosenAttack);
    }


    public Enemy GetEnemy()
    {
        return item;
    }

    public void Vanish()
    {
        GameManager.Instance.RemoveEnemy(this);
        localMap.RemoveUnit(this);
        AttackIndicator.Instance.RemoveAttack(choosenAttack);
        GameManager.Instance.RemoveSelectable(this, selectionIndicator.type);
        Destroy(this.gameObject);
    }
    
    
    private Vector2Int gridPosition;
    public Vector2Int GetGridPosition()
    {
        return gridPosition;
    }

    public void BecomeUsed()
    {
        
    }

    public void SetGridPosition(Vector2Int value)
    {
        gridPosition = value;
        AttackIndicator.Instance.Recalculate();
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
        GameManager.Instance.UseActiveCharacterAbility(this);
    }

    private void Start()
    {
        GameManager.Instance.AddSelectable(this, selectionIndicator.type);
        damager = new Damager(item);
        healthBar.Set(item.health);
        GameManager.Instance.AddEnemey(this);
    }


    public void HitByAbility(Ability ability)
    {
        ability.Use(this);
    }

    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            damager.TakeDamage(-amount);
            if (item.health.isDead) item.Die();
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
