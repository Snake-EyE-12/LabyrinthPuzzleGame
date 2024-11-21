using System;
using System.Collections;
using System.Collections.Generic;
using Capstone.DataLoad;
using Guymon.DesignPatterns;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using EventArgs = Guymon.DesignPatterns.EventArgs;
using EventHandler = Guymon.DesignPatterns.EventHandler;

public class CharacterDisplay : Display<Character>, GridPositionable, Selectable, Targetable
{
    [SerializeField] private Image coloredImage;
    [SerializeField] private Image characterFace;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private SelectionDisplay selectionIndicator;
    [SerializeField] private HealthbarDisplay healthBar;
    [SerializeField] private ExperienceDisplay xpBar;
    private Damager damager;


    public void ApplyDamagePhaseEffects()
    {
        item.ActiveEffectsList.ApplyDamage(this);
        CheckForDeath();
    }
    public void ApplyEndOfTurnPhaseEffects()
    {
        item.ActiveEffectsList.EndOfTurn(this);
        SetHealthBar();
    }
    public override void Render()
    {
        Color solidColor = DataHolder.characterColorEquivalenceTable.GetColor(item.characterType);
        coloredImage.color = new Color(solidColor.r, solidColor.g, solidColor.b, 200.0f/255.0f);
        nameText.text = item.unitName;
        characterFace.sprite = Resources.Load<Sprite>("KeynamedSprites/Faces/Heros/" + item.unitName);
        xpBar.Set(item.XP);
    }
    public void SelectViaClick()
    {
        if(IsCurrentlySelectable()) Activate(null);
    }
    
    private void Awake()
    {
        EventHandler.AddListener("Round/FightOver", OnBattleOver);
        GameManager.Instance.AddCharacter(this);
    }
    

    private void OnBattleOver(EventArgs args)
    {
        EventHandler.RemoveListenerLate("Round/FightOver", OnBattleOver);
        foreach (var a in item.abilityList)
        {
            a.usedThisCombat = false;
            a.growthValue = 0;
        }

        foreach (var card in item.inventory.GetCards())
        {
            card.GetTile().rotation.SetStringRotation("Reset");
            if(card.GetTile().ability != null) card.GetTile().ability.growthValue = 0;
        }
        Vanish();

        item.health.isDead = false;
        SetUsed(false);
        item.ActiveEffectsList.Reset();

    }
    
    public void Vanish()
    {
        GameManager.Instance.RemoveCharacter(this);
        localMap.RemoveGridPositionable(this);
        GameManager.Instance.RemoveSelectable(this, selectionIndicator.type);
        if(this != null && this.gameObject != null) Destroy(this.gameObject);
    }


    private Vector2Int gridPosition;
    

    public Vector2Int GetGridPosition()
    {
        return gridPosition;
    }

    public Transform GetTransform()
    {
        return transform;
    }

    [SerializeField] private Destinator destinator;
    public void SetGridPosition(Vector2Int value, bool wrapping = false)
    {
        // if (wrapping)
        // {
        //     List<DestinationData> positionSet = new();
        //     Vector2Int direction = (value - gridPosition).Normalize();
        //     positionSet.Add(new DestinationData(VisualDataHolder.Instance.CoordsToPosition(value + direction), 0.0001f, true));
        //     positionSet.Add(new DestinationData(VisualDataHolder.Instance.CoordsToPosition(value), 0.5f, true));
        //     destinator.MoveTo(positionSet);
        // }
        // else destinator.MoveTo(VisualDataHolder.Instance.CoordsToPosition(value), true);
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

    public void OnPassOverLoot(List<LootDisplay> loot)
    {
        foreach (var collectable in loot)
        {
            collectable.Collect(this);
        }
    }

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
        return (GameManager.Instance.Phase == GamePhase.UsingActiveAbility && GameManager.Instance.InActiveSelectionRange(gridPosition)) && CanBeTargeted(GameManager.Instance.AbilityInUse.targetDescription) || (!used && GameManager.Instance.GetSelectionType() == SelectableGroupType.Team);
    }
    private bool CanBeTargeted(string targettingType)
    {
        if(targettingType == "Team") return true;
        if(targettingType == "Any") return true;
        return false;
    }

    [SerializeField] private GameObject usedCover;
    private void Update()
    {
        usedCover.SetActive(used);
    }

    public void Activate(SelectableActivatorData data)
    {
        if (data is DirectionalSelectableActivatorData)
        {
            FreezeActiveEffect fea = item.ActiveEffectsList.GetEffect<FreezeActiveEffect>();
            if (fea == null && fea.value <= 0)
            {
                localMap.Move(this, (data as DirectionalSelectableActivatorData).direction);
                AudioManager.Instance.Play("Footsteps");
                CommandHandler.Clear();
            }

            return;
        }
        else
        {
            switch (GameManager.Instance.Phase)
            {
                case GamePhase.UsingActiveAbility :
                {
                    GameManager.Instance.UseActiveCharacterAbility(this);
                    break;
                }
                default:
                {
                    Active(true);
                    Instantiate(characterAbilityDisplayPrefab, GameManager.Instance.GetCanvasParent()).Set(item.abilityList);
                    GameManager.Instance.AbilityUser = this;
                    GameManager.Instance.SetSelectionMode(SelectableGroupType.Ability);
                    AudioManager.Instance.Play("ButtonClick");
                    return;
                }
            }
        }
    }
    [SerializeField] private CharacterAbilityDisplay characterAbilityDisplayPrefab;

    private void Start()
    {
        GameManager.Instance.AddSelectable(this, selectionIndicator.type);
        damager = new Damager(item);
        SetHealthBar();
    }
    private void SetHealthBar()
    {
        healthBar.Set(new HealthBarHealthAndEffectsData(item.health, item.ActiveEffectsList));
    }

    public void HitByAbility(Ability ability)
    {
        ability.Use(this);
    }
    public Character GetCharacter()
    {
        return item;
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
        healthBar.Render();
    }
    
    public Health GetHealthBar()
    {
        return item.health;
    }

    public void ApplyEffect(ActiveEffectType effect)
    {
        if(effect != null) item.ActiveEffectsList.AddEffect(effect);
        SetHealthBar();
    }

    public ActiveEffectList GetEffects()
    {
        return item.ActiveEffectsList;
    }

    public void MoveToPlace(Vector2Int direction)
    {
        
        //Vector2Int newPos = GetGridPosition() + direction;
        //SetGridPosition(newPos);
        //destinator.MoveTo(new DestinationData(VisualDataHolder.Instance.CoordsToPosition(newPos), 0.5f, local));
        
        if(IsFrozen()) return;
        localMap.Move(this, direction);
    }
    private bool IsFrozen()
    {
        FreezeActiveEffect fea = item.ActiveEffectsList.GetEffect<FreezeActiveEffect>();
        return fea != null && fea.value > 0;
    }

    public void Teleport(Vector2Int pos)
    {
        FreezeActiveEffect ice = item.ActiveEffectsList.GetEffect<FreezeActiveEffect>();
        if (ice != null && ice.value > 0) return;
        localMap.TP(this, pos);
        //SetGridPosition(pos);
        //destinator.MoveTo(new DestinationData(VisualDataHolder.Instance.CoordsToPosition(pos), 0.001f, true));
    }
    public void SetUsed(bool used)
    {
        this.used = used;
        Active(false);
    }

    public int GetXPValue()
    {
        return item.XP.value;
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
        item.XP.value += amount;
        xpBar.Render();
    }

    public void Active(bool active)
    {
        selectionIndicator.Activated(active);
    }

    private bool used = false;
    
}

