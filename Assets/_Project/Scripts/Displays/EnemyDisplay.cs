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
        choosenAttack = new EnemyAttackVisual(){attack = item.attackLayout.Pick(GetAttackTargetLocations(), gridPosition, item.AttackIQ), user = this};
        AttackIndicator.Instance.AddAttack(choosenAttack);
    }

    private List<Vector2Int> GetAttackTargetLocations()
    {
        List<CharacterDisplay> characters = GameManager.Instance.GetActiveCharacters();
        List<Vector2Int> positions = new();
        foreach (var character in characters)
        {
            positions.Add(character.GetGridPosition());
        }

        return positions;
    }


    public Enemy GetEnemy()
    {
        return item;
    }

    public void Vanish()
    {
        GameManager.Instance.RemoveEnemy(this);
        localMap.RemoveGridPositionable(this);
        AttackIndicator.Instance.RemoveAttack(choosenAttack);
        GameManager.Instance.RemoveSelectable(this, selectionIndicator.type);
        SpawnLootDrop();
        Destroy(this.gameObject);
    }

    [SerializeField] private LootDisplay lootDisplayPrefab;
    private void SpawnLootDrop()
    {
        foreach (var loot in item.loot)
        {
            var lootDisplay = Instantiate(lootDisplayPrefab);
            lootDisplay.Set(loot);
            lootDisplay.SetLocalMap(localMap);
            localMap.SpawnLoot(lootDisplay, gridPosition);
        }
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

    public void OnPassOverLoot(List<LootDisplay> loot)
    {
        
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
        return GameManager.Instance.Phase == GamePhase.UsingActiveAbility && GameManager.Instance.InActiveSelectionRange(gridPosition);
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
        localMap.Move(this, direction);
    }

    public Vector2Int FindSmartDirectionToMove(Vector2Int center, Vector2Int closestOpponent, Vector2Int closestAlly, Vector2Int averageOpponent, Vector2Int averageAlly)
    {//                                          has big attacks           has small attacks            is low health      is big attacks & high health    is enemy helpful
        
        // Should but doesnt - avoid traps
        // closest ally is self
        
        bool lowHealth = CheckLowHealth();
        int averageAttackSize = GetAverageAttackSize();
        bool backline = item.IsInBackLine();
        bool IsMakingSmartMove = PassIQ() || PassIQ();

        if (lowHealth && PassIQ()) return GetDirectionTowards(closestAlly);
        if(averageAttackSize <= 2 && PassIQ()) return GetDirectionTowards(closestOpponent);
        if (averageAttackSize >= 5 && PassIQ())
        {
            if (!lowHealth) return GetDirectionTowards(averageOpponent);
            return GetDirectionTowards(center);
        }
        if (backline && PassIQ()) return GetDirectionTowards(averageAlly);
        return GameUtils.GetRandomDirection();
    }

    private Vector2Int GetDirectionTowards(Vector2Int destination)
    {
        return (destination - gridPosition).Normalize(); // Should use some pathfinding
    }

    private bool PassIQ()
    {
        return GameUtils.PercentChance(item.MovementIQ, DataHolder.currentMode.MaximumIQ);
    }
    private int GetAverageAttackSize()
    {
        if(item.attackLayout.attacks.Count == 0) return 0;
        int sum = 0;
        foreach (Attack a in item.attackLayout.attacks)
        {
            sum += a.GetShapeSize();
        }
        return sum / item.attackLayout.attacks.Count;
    }
    private bool CheckLowHealth()
    {
        return damager.GetHealthPercent() * 100 < DataHolder.currentMode.LowHealthPercent;
    }
    
    public void CheckForDeath()
    {
        item.CheckDeath();
    }
    public Map GetMap()
    {
        return localMap;
    }

    public void GainXP(int amount)
    {
        
    }
}
