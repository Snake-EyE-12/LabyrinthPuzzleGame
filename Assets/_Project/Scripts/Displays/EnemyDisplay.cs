using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyDisplay : Display<Enemy>, GridPositionable, Selectable, Targetable
{
    [SerializeField] private Image faceImage;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private Image overlayImage;
    
    [SerializeField] private SelectionDisplay selectionIndicator;
    [SerializeField] private HealthbarDisplay healthBar;
    private Damager damager;


    public int GetXPValue()
    {
        return 0;
    }
    public void UseAttackNow()
    {
        AttackIndicator.Instance.UseSpecificEnemyAttack(this);
    }
    public void ApplyDamagePhaseEffects()
    {
        item.ActiveEffectsList.ApplyDamage(this);
        CheckForDeath();
    }
    public void ApplyEndOfTurnEffectChanges()
    {
        item.ActiveEffectsList.EndOfTurn(this);
        SetHealthBar();
    }
    public ActiveEffectList GetEffects()
    {
        return item.ActiveEffectsList;
    }

    public Health GetHealthBar()
    {
        return item.health;
    }
    public override void Render()
    {
        //coloredImage.color = DataHolder.characterColorEquivalenceTable.GetColor(item.type);
        nameText.text = item.unitName;
        faceImage.sprite = Resources.Load<Sprite>("KeynamedSprites/Faces/Villains/" + item.unitName);
        overlayImage.sprite = Resources.Load<Sprite>("KeynamedSprites/Overlays/" + item.type);
    }

    private EnemyAttackVisual choosenAttack;
    public void ChooseAttack()
    {
        choosenAttack = new EnemyAttackVisual(){attack = item.attackLayout.Pick(GetAttackTargetLocations(), gridPosition, item.AttackIQ), user = this};
        AttackIndicator.Instance.AddAttack(choosenAttack);
    }
    public void SelectViaClick()
    {
        if(IsCurrentlySelectable()) Activate(null);
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
        EffectHolder.Instance.SpawnEffect("EnemyDefeated", transform.position);
        Destroy(this.gameObject);
    }

    [SerializeField] private LootDisplay lootDisplayPrefab;
    private void SpawnLootDrop()
    {
        foreach (var loot in item.loot)
        {
            if(loot.PassDropChance())
            {
                var lootDisplay = Instantiate(lootDisplayPrefab, transform.position, Quaternion.identity);
                lootDisplay.Set(loot);
                lootDisplay.SetLocalMap(localMap);
                localMap.SpawnLoot(lootDisplay, gridPosition);
            }
        }
    }
    
    
    private Vector2Int gridPosition;
    public Vector2Int GetGridPosition()
    {
        return gridPosition;
    }

    public void SetUsed(bool used)
    {
        
    }

    public void SetGridPosition(Vector2Int value, bool wrapping = false)
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

    [SerializeField] private Destinator destinator;
    public void MoveVisually(Vector3 position)
    {
        destinator.MoveTo(position, false);
    }

    public void Select()
    {
        if (!IsCurrentlySelectable()) return;
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
        return GameManager.Instance.Phase == GamePhase.UsingActiveAbility && 
               GameManager.Instance.InActiveSelectionRange(gridPosition) && 
               CanBeTargeted(GameManager.Instance.AbilityInUse.targetDescription);
    }

    private bool CanBeTargeted(string targettingType)
    {
        if(targettingType == "Opponent") return true;
        if(targettingType == "Any") return true;
        return false;
    }

    public void Activate(SelectableActivatorData data)
    {
        GameManager.Instance.UseActiveCharacterAbility(this);
    }

    private void Start()
    {
        GameManager.Instance.AddSelectable(this, selectionIndicator.type);
        damager = new Damager(item);
        SetHealthBar();
        GameManager.Instance.AddEnemey(this);
    }

    private void SetHealthBar()
    {
        healthBar.Set(new HealthBarHealthAndEffectsData(item.health, item.ActiveEffectsList));
    }


    public void HitByAbility(Ability ability)
    {
        ability.Use(this);
    }
    public Transform GetTransform()
    {
        return transform;
    }

    public void ChangeHealth(int amount, bool ignoreShield = false)
    {
        if (amount < 0)
        {
            amount *= -1;
            ShieldActiveEffect shield = item.ActiveEffectsList.GetEffect<ShieldActiveEffect>();
            if (shield == null || ignoreShield) damager.TakeDamage(amount);
            else if (shield.value > amount)
            {
                shield.value -= amount;
            }
            else
            {
                damager.TakeDamage(amount - shield.value);
                shield.value = 0;
            }
        }
        else damager.Heal(amount);
        SetHealthBar();
    }

    public void ApplyEffect(ActiveEffectType effect)
    {
        if(effect != null) item.ActiveEffectsList.AddEffect(effect);
        SetHealthBar();
    }

    public void MoveToPlace(Vector2Int direction)
    {
        if(IsFrozen()) return;
        localMap.Move(this, direction);
    }
    public void Teleport(Vector2Int pos)
    {
        //SetGridPosition(pos);
        //destinator.MoveTo(new DestinationData(VisualDataHolder.Instance.CoordsToPosition(pos), 0.001f, true));
    }

    public bool IsFrozen()
    {
        FreezeActiveEffect ice = item.ActiveEffectsList.GetEffect<FreezeActiveEffect>();
        return ice != null && ice.value > 0;
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

    public void Active(bool active)
    {
        selectionIndicator.Activated(active);
    }
}
