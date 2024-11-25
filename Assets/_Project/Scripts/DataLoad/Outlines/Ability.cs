using System.Collections;
using System.Collections.Generic;
using Capstone.DataLoad;
using Guymon.DesignPatterns;
using UnityEngine;


public class Ability
{
    private int value;
    private Target target;
    public string targetDescription;
    public List<ValueKey> keys = new List<ValueKey>();
    public string owner;
    public bool usedThisCombat;
    public bool isPenetrative;
    public int growthValue = 0;

    public void SetValue(int amount)
    {
        value = amount;
    }
    public int GetValue()
    {
        return value + growthValue;
    }
    public string GetKeywordsString()
    {
        string output = "";
        foreach (var key in keys)
        {
            output += key.GetKeywordName().name + ",";
        }
        return output.Substring(0, output.Length - 1);
    }

    public void AddToValue(int amount)
    {
        value += amount;
    }


    public static Ability Copy(Ability ability)
    {
        Ability output = new Ability();
        output.value = ability.value;
        output.targetDescription = ability.targetDescription;
        output.keys = ability.keys;
        output.owner = ability.owner;
        output.usedThisCombat = false;
        return output;
    }
    public static Sprite GetAbilityIcon(Ability ability)
    {
        string output = "V";
        output += GetAbilityValue(ability.value) + "-";
        output += ability.GetKeywordsString();
        return Resources.Load<Sprite>("KeynamedSprites/TileAbilities/" + output);
    }

    private static string GetAbilityValue(int value)
    {
        if (value == 0) return "" + 0;
        if (value == 1) return "" + 1;
        return "2+";
    }

    public static void OrderAbilityKeywords(List<ValueKey> keywords)
    {
        keywords.Sort((a, b) => a.GetOrder().CompareTo(b.GetOrder()));
    }

    public bool ContainsKeyword(string keyword)
    {
        foreach (var vk in keys)
        {
            if (vk.GetKeywordName().name.Equals(keyword)) return true;
        }

        return false;
    }
    public bool RemoveKeyword(string keyword)
    {
        foreach (var vk in keys)
        {
            if (vk.GetKeywordName().name.Equals(keyword))
            {
                keys.Remove(vk);
                return true;
            }
        }
        return false;
    }
    public bool AddKeyword(string keyword)
    {
        foreach (var vk in keys)
        {
            
            if (vk.GetKeywordName().name.Equals(keyword)) return false;
            
        }
        KeywordName keyName = new KeywordName(){color = DataHolder.keywordColorEquivalenceTable.GetColor(keyword), name = keyword};
        keys.Add(GetNewValueKeyOfType(keyName));
        OrderAbilityKeywords(keys);
        return true;
    }

    public bool SetKeywords(string[] keywords)
    {
        keys.Clear();
        foreach (var keyword in keywords)
        {
            AddKeyword(keyword);
        }
        return true;
        
    }

    
    public void PrepareTarget()
    {
        target.Locate(this);
    }
    public void Use(Targetable target)
    {
        CommandHandler.Clear();
        int startingAbilityValue = value;
        if (!IsImmune(target))
        {
            foreach (var key in keys)
            {
                key.ModifyAction(target, this);
                if (key is SingularKeyword) usedThisCombat = true;
                int valueBeforeAreaUse = value;
                if (key is AreaKeyword) AreaUse(target, startingAbilityValue);
                value = valueBeforeAreaUse;
            }
        }

        if (GameManager.Instance.Phase == GamePhase.UsingActiveAbility)
        {
            GameManager.Instance.EndUseOfAbility();
        }
        value = startingAbilityValue;
        isPenetrative = false;
        target.CheckForDeath();
    }

    private void AreaUse(Targetable target, int value)
    {
        int startingGrowthValue = growthValue;
        List<Targetable> cTargets = new List<Targetable>(GameManager.Instance.GetActiveCharacters());
        List<Targetable> eTargets = new List<Targetable>(GameManager.Instance.GetActiveEnemies());
        cTargets.AddRange(eTargets);
        foreach (var t in cTargets)
        {
            growthValue = startingGrowthValue;
            if (t.GetGridPosition() == target.GetGridPosition() && !IsImmune(t) && t != target)
            {
                int startValue = value;
                foreach (var key in keys)
                {
                    key.ModifyAction(t, this);
                }

                this.value = startValue;
            }
            t.CheckForDeath();
        }
        growthValue = startingGrowthValue;
    }

    private bool IsImmune(Targetable t)
    {
        foreach (var aet in t.GetEffects().GetActiveEffects())
        {
            if(aet is ImmunityActiveEffect)
                if (aet.value > 0)
                    return true;
        }

        return false;
    }

    private Ability()
    {
    }

    public Ability(AbilityData data)
    {
        value = data.Value;
        targetDescription = data.Target;
        switch (data.Target)
        {
            case "Self":
                target = new SelfTarget();
                break;
            case "Opponent":
                target = new OpponentTarget();
                break;
            case "Tile":
                target = new TileTarget();
                break;
            case "Team":
                target = new TeamTarget();
                break;
            case "Any":
                target = new AnyTarget();
                break;
        }

        foreach (var keyword in data.Keys)
        {
            KeywordName keyName = new KeywordName(){color = DataHolder.keywordColorEquivalenceTable.GetColor(keyword), name = keyword};
            keys.Add(GetNewValueKeyOfType(keyName));
        }
        OrderAbilityKeywords(keys);
    }

    private ValueKey GetNewValueKeyOfType(KeywordName keyword)
    {
        switch (keyword.name)
            {
                case "Stunted":
                    return new StuntedKeyword(keyword);
                case "Melee":
                    return new MeleeKeyword(keyword);
                case "Knockback":
                    return new KnockbackKeyword(keyword);
                case "Damage":
                    return new DamageKeyword(keyword);
                case "Heal":
                    return new HealKeyword(keyword);
                case "Discard":
                    return new DiscardKeyword(keyword);
                case "Rotate":
                    return new RotateKeyword(keyword);
                case "Train":
                    return new TrainKeyword(keyword);
                case "Bleed":
                    return new BleedKeyword(keyword);
                case "Poison":
                    return new PoisonKeyword(keyword);
                case "Burn":
                    return new BurnKeyword(keyword);
                case "Freeze":
                    return new FreezeKeyword(keyword);
                case "Ranged":
                    return new RangedKeyword(keyword);
                case "Singular":
                    return new SingularKeyword(keyword);
                case "Shield":
                    return new ShieldKeyword(keyword);
                case "Area":
                    return new AreaKeyword(keyword);
                case "Inspire":
                    return new InspireKeyword(keyword);
                case "Intangible":
                    return new IntangibleKeyword(keyword);
                case "Penetrate":
                    return new PenetrateKeyword(keyword);
                case "Teleport":
                    return new TeleportKeyword(keyword);
                case "Provoke":
                    return new ProvokeKeyword(keyword);
                case "Expert":
                    return new ExpertKeyword(keyword);
                case "Effected":
                    return new EffectedKeyword(keyword);
                case "Cleanse":
                    return new CleanseKeyword(keyword);
                case "Wash":
                    return new WashKeyword(keyword);
                case "Wounded":
                    return new WoundedKeyword(keyword);
                case "Build":
                    return new FixKeyword(keyword);
                case "Break":
                    return new BreakKeyword(keyword);
                case "Shift":
                    return new ShiftKeyword(keyword);
                case "Grow":
                    return new GrowKeyword(keyword);
                case "Stun":
                    return new StunKeyword(keyword);
                default:
                    return new ValueKey(keyword);
            }
    }
}









public class KeywordName
{
    public Color color;
    public string name;
    public string ConvertToString()
    { //We are <color=#ff0000ff>colorfully</color> amused - Example
        return $"<color=#{ColorUtility.ToHtmlStringRGB(color)}FF>{name}</color> ";
    }
}

public enum KeywordOrder
{
    None,
    Selection,
    Movement,
    Addition,
    Multiplication,
    Effect,
    Self,
    Board,
    Post,
    Increment
}


public class ValueKey
{
    protected KeywordName keywordName;
    public ValueKey(KeywordName keyName)
    {
        keywordName = keyName;
    }
    public KeywordName GetKeywordName()
    {
        return keywordName;
    }

    public int GetOrder()
    {
        return (int)(GetKeyOrder()) * 10 + GetLayerOrder();
    }
    public virtual KeywordOrder GetKeyOrder()
    {
        return KeywordOrder.None;
    }

    public virtual int GetLayerOrder()
    {
        return 9;
    }
    public virtual void ModifyAction(Targetable t, Ability abilityInUse) {}
    public virtual void ModifyRange(int value) {}
    
}

public class RotateKeyword : ValueKey
{
    public override void ModifyAction(Targetable t, Ability abilityInUse)
    {
        t.GetMap().RotateTile(t.GetGridPosition(), abilityInUse.GetValue());
        //Transform owner = GameManager.Instance.AbilityUser != null ? GameManager.Instance.AbilityUser.GetTransform() : null;
        EffectHolder.Instance.SpawnEffect(keywordName.name, t.GetTransform());
    }

    public RotateKeyword(KeywordName keyName) : base(keyName)
    {
    }

    public override KeywordOrder GetKeyOrder()
    {
        return KeywordOrder.Board;
    }

}

public class DiscardKeyword : ValueKey
{
    public DiscardKeyword(KeywordName keyName) : base(keyName)
    {
    }
    public override KeywordOrder GetKeyOrder()
    {
        return KeywordOrder.Board;
    }

    public override void ModifyAction(Targetable t, Ability abilityInUse)
    {
        Debug.Log("Phase: " + GameManager.Instance.enemyMovingRightNow);
        if (GameManager.Instance.enemyMovingRightNow) return;
        EventHandler.Invoke("Deck/DiscardFirst", null);
        EffectHolder.Instance.SpawnEffect(keywordName.name, t.GetTransform());
    }
}

public class StuntedKeyword : ValueKey
{
    public StuntedKeyword(KeywordName keyName) : base(keyName)
    {
    }
    public override KeywordOrder GetKeyOrder()
    {
        return KeywordOrder.Selection;
    }
    public override int GetLayerOrder()
    {
        return 1;
    }

    public override void ModifyRange(int value)
    {
        GameManager.Instance.TargetRadius = new TargetRadius() { radius = 1, squareRadius = 1, center = GameManager.Instance.AbilityUser.GetGridPosition()};
    }
}
public class MeleeKeyword : ValueKey
{
    public MeleeKeyword(KeywordName keyName) : base(keyName)
    {
    }
    public override KeywordOrder GetKeyOrder()
    {
        return KeywordOrder.Selection;
    }

    public override int GetLayerOrder()
    {
        return 2;
    }

    public override void ModifyRange(int value)
    {
        GameManager.Instance.TargetRadius = new TargetRadius() { radius = 0, squareRadius = 0, center = GameManager.Instance.AbilityUser.GetGridPosition()};
    }
}
public class KnockbackKeyword : ValueKey
{
    public override void ModifyAction(Targetable t, Ability abilityInUse)
    {
        t.MoveToPlace(t.GetGridPosition() - GameManager.Instance.AbilityUser.GetGridPosition());
    }

    public KnockbackKeyword(KeywordName keyName) : base(keyName)
    {
    }
    public override KeywordOrder GetKeyOrder()
    {
        return KeywordOrder.Movement;
    }
}
public class DamageKeyword : ValueKey
{
    public override void ModifyAction(Targetable t, Ability abilityInUse)
    {
        t.ChangeHealth(-abilityInUse.GetValue(), abilityInUse.isPenetrative);
        if(abilityInUse.GetValue() > 0) EffectHolder.Instance.SpawnEffect(keywordName.name, t.GetTransform());
        
    }

    public DamageKeyword(KeywordName keyName) : base(keyName)
    {
    }
    public override KeywordOrder GetKeyOrder()
    {
        return KeywordOrder.Effect;
    }
}
public class HealKeyword : ValueKey
{
    public override void ModifyAction(Targetable t, Ability abilityInUse)
    {
        t.ChangeHealth(abilityInUse.GetValue());
        EffectHolder.Instance.SpawnEffect(keywordName.name, t.GetTransform());
    }

    public HealKeyword(KeywordName keyName) : base(keyName)
    {
    }
    public override KeywordOrder GetKeyOrder()
    {
        return KeywordOrder.Effect;
    }

    public override int GetLayerOrder()
    {
        return 0;
    }
}
public class TrainKeyword : ValueKey
{
    public override void ModifyAction(Targetable t, Ability abilityInUse)
    {
        t.GainXP(abilityInUse.GetValue());
        EffectHolder.Instance.SpawnEffect(keywordName.name, t.GetTransform());
    }

    public TrainKeyword(KeywordName keyName) : base(keyName)
    {
    }
    public override KeywordOrder GetKeyOrder()
    {
        return KeywordOrder.Effect;
    }
}
public class BleedKeyword : ValueKey
{
    public BleedKeyword(KeywordName keyName) : base(keyName)
    {
    }
    public override KeywordOrder GetKeyOrder()
    {
        return KeywordOrder.Effect;
    }

    public override void ModifyAction(Targetable t, Ability abilityInUse)
    {
        t.ApplyEffect(new BleedActiveEffect(abilityInUse.GetValue()));
    }
}

public class PoisonKeyword : ValueKey
{
    public PoisonKeyword(KeywordName keyName) : base(keyName)
    {
    }

    public override KeywordOrder GetKeyOrder()
    {
        return KeywordOrder.Effect;
    }

    public override void ModifyAction(Targetable t, Ability abilityInUse)
    {
        t.ApplyEffect(new PoisonActiveEffect(abilityInUse.GetValue()));
        EffectHolder.Instance.SpawnEffect(keywordName.name, t.GetTransform());
    }

}
public class BurnKeyword : ValueKey
{
    public BurnKeyword(KeywordName keyName) : base(keyName)
    {
    }

    public override KeywordOrder GetKeyOrder()
    {
        return KeywordOrder.Effect;
    }

    public override void ModifyAction(Targetable t, Ability abilityInUse)
    {
        t.ApplyEffect(new BurnActiveEffect(abilityInUse.GetValue()));
        EffectHolder.Instance.SpawnEffect(keywordName.name, t.GetTransform());
    }
}

public class FreezeKeyword : ValueKey
{
    public FreezeKeyword(KeywordName keyName) : base(keyName)
    {
    }

    public override KeywordOrder GetKeyOrder()
    {
        return KeywordOrder.Effect;
    }

    public override void ModifyAction(Targetable t, Ability abilityInUse)
    {
        t.ApplyEffect(new FreezeActiveEffect(abilityInUse.GetValue()));
        EffectHolder.Instance.SpawnEffect(keywordName.name, t.GetTransform());
    }
}
public class RangedKeyword : ValueKey // TODO
{
    public RangedKeyword(KeywordName keyName) : base(keyName)
    {
    }
    public override KeywordOrder GetKeyOrder()
    {
        return KeywordOrder.Self;
    }

}

public class SingularKeyword : ValueKey // TODO
{
    public SingularKeyword(KeywordName keyName) : base(keyName)
    {
    }

    public override KeywordOrder GetKeyOrder()
    {
        return KeywordOrder.Post;
    }

}
public class ShieldKeyword : ValueKey
{
    public ShieldKeyword(KeywordName keyName) : base(keyName)
    {
    }
    public override KeywordOrder GetKeyOrder()
    {
        return KeywordOrder.Effect;
    }
    public override void ModifyAction(Targetable t, Ability abilityInUse)
    {
        t.ApplyEffect(new ShieldActiveEffect(abilityInUse.GetValue()));
        EffectHolder.Instance.SpawnEffect(keywordName.name, t.GetTransform());
    }

    public override int GetLayerOrder()
    {
        return 0;
    }
}
public class AreaKeyword : ValueKey // TODO
{
    public AreaKeyword(KeywordName keyName) : base(keyName)
    {
    }
    public override KeywordOrder GetKeyOrder()
    {
        return KeywordOrder.Selection;
    }
}

public class InspireKeyword : ValueKey // TODO
{
    public InspireKeyword(KeywordName keyName) : base(keyName)
    {
    }
    public override KeywordOrder GetKeyOrder()
    {
        return KeywordOrder.Self;
    }

    public override void ModifyAction(Targetable t, Ability abilityInUse)
    {
        t.SetUsed(false);
        EffectHolder.Instance.SpawnEffect(keywordName.name, t.GetTransform());
    }
}
public class IntangibleKeyword : ValueKey // TODO
{
    public IntangibleKeyword(KeywordName keyName) : base(keyName)
    {
    }
    public override KeywordOrder GetKeyOrder()
    {
        return KeywordOrder.Effect;
    }
    public override void ModifyAction(Targetable t, Ability abilityInUse)
    {
        t.ApplyEffect(new ImmunityActiveEffect(abilityInUse.GetValue()));
        EffectHolder.Instance.SpawnEffect(keywordName.name, t.GetTransform());
    }
}
public class PenetrateKeyword : ValueKey // TODO
{
    public PenetrateKeyword(KeywordName keyName) : base(keyName)
    {
    }
    public override KeywordOrder GetKeyOrder()
    {
        return KeywordOrder.Selection;
    }

    public override void ModifyAction(Targetable t, Ability abilityInUse)
    {
        abilityInUse.isPenetrative = true;
    }
}
public class TeleportKeyword : ValueKey // TODO
{
    public TeleportKeyword(KeywordName keyName) : base(keyName)
    {
    }
    public override KeywordOrder GetKeyOrder()
    {
        return KeywordOrder.Movement;
    }

    public override void ModifyAction(Targetable t, Ability abilityInUse)
    {
        GameManager.Instance.AbilityUser.Teleport(t.GetGridPosition());
        EffectHolder.Instance.SpawnEffect(keywordName.name, t.GetTransform());
    }
}
public class ProvokeKeyword : ValueKey // TODO
{
    public ProvokeKeyword(KeywordName keyName) : base(keyName)
    {
    }
    public override KeywordOrder GetKeyOrder()
    {
        return KeywordOrder.Post;
    }

    public override void ModifyAction(Targetable t, Ability abilityInUse)
    {
        if (t is EnemyDisplay) (t as EnemyDisplay).UseAttackNow();
        EffectHolder.Instance.SpawnEffect(keywordName.name, t.GetTransform());
    }
}
public class ExpertKeyword : ValueKey // TODO
{
    public ExpertKeyword(KeywordName keyName) : base(keyName)
    {
    }
    public override KeywordOrder GetKeyOrder()
    {
        return KeywordOrder.Addition;
    }

    public override void ModifyAction(Targetable t, Ability abilityInUse)
    {
        abilityInUse.AddToValue(GameManager.Instance.AbilityUser.GetXPValue());
    }
}
public class EffectedKeyword : ValueKey // TODO
{
    public EffectedKeyword(KeywordName keyName) : base(keyName)
    {
    }
    public override KeywordOrder GetKeyOrder()
    {
        return KeywordOrder.Addition;
    }

    public override void ModifyAction(Targetable t, Ability abilityInUse)
    {
        int count = 0;
        foreach (var aet in GameManager.Instance.AbilityUser.GetEffects().GetActiveEffects())
        {
            if (aet.IsDOT() && aet.value > 0) count++; // Using amount of DOT effects vs total value
        }

        abilityInUse.AddToValue(count);
    }
}
public class CleanseKeyword : ValueKey // TODO
{
    public CleanseKeyword(KeywordName keyName) : base(keyName)
    {
    }
    public override KeywordOrder GetKeyOrder()
    {
        return KeywordOrder.Effect;
    }

    public override void ModifyAction(Targetable t, Ability abilityInUse)
    {
        t.GetEffects().Clear<PoisonActiveEffect>(abilityInUse.GetValue());
        t.ApplyEffect(null);
        EffectHolder.Instance.SpawnEffect(keywordName.name, t.GetTransform());
    }
}
public class WashKeyword : ValueKey // TODO
{
    public WashKeyword(KeywordName keyName) : base(keyName)
    {
        
    }
    public override KeywordOrder GetKeyOrder()
    {
        return KeywordOrder.Effect;
    }

    public override void ModifyAction(Targetable t, Ability abilityInUse)
    {
        t.GetEffects().Clear<BurnActiveEffect>(abilityInUse.GetValue());
        t.ApplyEffect(null);
        EffectHolder.Instance.SpawnEffect(keywordName.name, t.GetTransform());
    }
}
public class WoundedKeyword : ValueKey // TODO
{
    public WoundedKeyword(KeywordName keyName) : base(keyName)
    {
    }
    public override KeywordOrder GetKeyOrder()
    {
        return KeywordOrder.Addition;
    }

    public override void ModifyAction(Targetable t, Ability abilityInUse)
    {
        Health h = t.GetHealthBar();
        int missingHealth = h.GetMaxHealthValue() - h.GetHealthValue();
        abilityInUse.AddToValue(missingHealth);
    }
}
public class FixKeyword : ValueKey // TODO
{
    public FixKeyword(KeywordName keyName) : base(keyName)
    {
    }
    public override KeywordOrder GetKeyOrder()
    {
        return KeywordOrder.Board;
    }
    public override void ModifyAction(Targetable t, Ability abilityInUse)
    {
        t.GetMap().GetTileAtPosition(t.GetGridPosition()).Build(abilityInUse.GetValue());
        EffectHolder.Instance.SpawnEffect("Hammer", t.GetTransform());
        
    }
}
public class BreakKeyword : ValueKey // TODO
{
    public BreakKeyword(KeywordName keyName) : base(keyName)
    {
    }
    public override KeywordOrder GetKeyOrder()
    {
        return KeywordOrder.Board;
    }

    public override void ModifyAction(Targetable t, Ability abilityInUse)
    {
        t.GetMap().GetTileAtPosition(t.GetGridPosition()).Break(abilityInUse.GetValue());
        EffectHolder.Instance.SpawnEffect("Hammer", t.GetTransform());
        AudioManager.Instance.Play("Break");
    }
}
public class GrowKeyword : ValueKey // TODO
{
    public GrowKeyword(KeywordName keyName) : base(keyName)
    {
    }
    public override KeywordOrder GetKeyOrder()
    {
        return KeywordOrder.Increment;
    }

    public override void ModifyAction(Targetable t, Ability abilityInUse)
    {
        abilityInUse.growthValue++;
    }
}
public class ShiftKeyword : ValueKey // TODO
{
    public ShiftKeyword(KeywordName keyName) : base(keyName)
    {
    }
    public override KeywordOrder GetKeyOrder()
    {
        return KeywordOrder.Board;
    }

    public override void ModifyAction(Targetable t, Ability abilityInUse)
    {
        bool row = abilityInUse.GetValue() % 2 == 1;
        bool positive = abilityInUse.GetValue() % 4 >= 1;
        int number = row ? t.GetGridPosition().y : t.GetGridPosition().x;
        t.GetMap().Slide(row, positive, number, FindFallOffTile(row, positive, number, t.GetMap()));
    }

    private Tile FindFallOffTile(bool row, bool positive, int number, Map map)
    {
        if (row)
        {
            return map.GetTileAtPosition(new Vector2Int(positive ? map.GetSize() - 1 : 0, number)).GetTile();
        }
        else
        {
            return map.GetTileAtPosition(new Vector2Int(number, positive ? map.GetSize() - 1 : 0)).GetTile();
        }
    }
}
public class StunKeyword : ValueKey // TODO
{
    public StunKeyword(KeywordName keyName) : base(keyName)
    {
    }
    public override KeywordOrder GetKeyOrder()
    {
        return KeywordOrder.Effect;
    }
}








public class TargetRadius
{
    public int radius;
    public int squareRadius;
    public Vector2Int center;

    public bool InCircleRange(Vector2Int pos)
    {
        return (pos - center).sqrMagnitude <= radius * radius;
    }
    public bool InSquareRange(Vector2Int pos)
    {
        Vector2Int difference = new Vector2Int(Mathf.Abs((pos - center).x), Mathf.Abs((pos - center).y));
        return difference.x <= radius && difference.y <= radius;
    }
}

public abstract class Target
{
    public abstract void Locate(Ability currentAbility);

    protected void TestRange(Ability currentAbility)
    {
        foreach (var k in currentAbility.keys)
        {
            k.ModifyRange(currentAbility.GetValue());
        }
    }
}

public class SelfTarget : Target
{
    public override void Locate(Ability currentAbility)
    {
        GameManager.Instance.AbilityUser.HitByAbility(GameManager.Instance.AbilityInUse);
        GameManager.Instance.SetSelectionMode(SelectableGroupType.Team);
        GameManager.Instance.Phase = GamePhase.None;
    }
}

public class OpponentTarget : Target
{
    public override void Locate(Ability currentAbility)
    {
        TestRange(currentAbility);
        GameManager.Instance.SetSelectionMode(SelectableGroupType.Enemy);
    }
}
public class TileTarget : Target
{
    public override void Locate(Ability currentAbility)
    {
        TestRange(currentAbility);
    }
}

public class TeamTarget : Target
{
    public override void Locate(Ability currentAbility)
    {
        TestRange(currentAbility);
        GameManager.Instance.SetSelectionMode(SelectableGroupType.Team);
    }
}

public class AnyTarget : Target
{
    public override void Locate(Ability currentAbility)
    {
        TestRange(currentAbility);
        GameManager.Instance.SetSelectionMode(SelectableGroupType.None);
    }
    
    
}
