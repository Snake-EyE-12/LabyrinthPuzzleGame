using System.Collections;
using System.Collections.Generic;
using Capstone.DataLoad;
using Guymon.DesignPatterns;
using UnityEngine;


public class Ability
{
    public int value;
    private Target target;
    public string targetDescription;
    public List<ValueKey> keys = new List<ValueKey>();
    public string owner;
    public bool usedThisCombat;
    
    public string GetKeywordsString()
    {
        string output = "";
        foreach (var key in keys)
        {
            output += key.GetKeywordName().name + ",";
        }
        return output.Substring(0, output.Length - 1);
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

    
    public void PrepareTarget()
    {
        target.Locate(this);
    }
    public void Use(Targetable target)
    {
        int startingAbilityValue = value;
        foreach (var key in keys)
        {
            key.ModifyAction(target, this);
            if (key is SingularKeyword) usedThisCombat = true;
        }
        if (GameManager.Instance.AbilityUser != null) GameManager.Instance.AbilityUser.BecomeUsed();
        if (GameManager.Instance.Phase == GamePhase.UsingActiveAbility)
        {
            EventHandler.Invoke("Ability/DestroyPanel", null);
            GameManager.Instance.TargetRadius = null;
        }

        value = startingAbilityValue;
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
                case "Charge":
                    return new ChargeKeyword(keyword);
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
                case "Charged":
                    return new ChargedKeyword(keyword);
                case "Effected":
                    return new EffectedKeyword(keyword);
                case "Cleanse":
                    return new CleanseKeyword(keyword);
                case "Wounded":
                    return new WoundedKeyword(keyword);
                case "Fix":
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
    Post
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
        t.GetMap().RotateTile(t.GetGridPosition(), abilityInUse.value);
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
        EventHandler.Invoke("Deck/DiscardFirst", null);
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

    public override void ModifyRange(int value)
    {
        GameManager.Instance.TargetRadius = new TargetRadius() { radius = 1, squareRadius = 1, center = GameManager.Instance.AbilityUser.GetGridPosition()};
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
        t.ChangeHealth(-abilityInUse.value);
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
        t.ChangeHealth(abilityInUse.value);
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
public class ChargeKeyword : ValueKey
{
    public override void ModifyAction(Targetable t, Ability abilityInUse)
    {
        t.GainXP(abilityInUse.value);
    }

    public ChargeKeyword(KeywordName keyName) : base(keyName)
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
        t.ApplyEffect(new BleedActiveEffect(abilityInUse.value));
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
        t.ApplyEffect(new PoisonActiveEffect(abilityInUse.value));
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
        t.ApplyEffect(new BurnActiveEffect(abilityInUse.value));
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
        t.ApplyEffect(new FreezeActiveEffect(abilityInUse.value));
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
public class ShieldKeyword : ValueKey // TODO
{
    public ShieldKeyword(KeywordName keyName) : base(keyName)
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
}
public class ChargedKeyword : ValueKey // TODO
{
    public ChargedKeyword(KeywordName keyName) : base(keyName)
    {
    }
    public override KeywordOrder GetKeyOrder()
    {
        return KeywordOrder.Addition;
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
        foreach (var aet in GameManager.Instance.AbilityUser.GetEffects())
        {
            Debug.Log("Effect Is: " + aet.GetType());
            if (aet.IsDOT()) count++;
        }

        Debug.Log("Amount of DOT was: " + count);
        abilityInUse.value += count;
        Debug.Log("New Value Becuase of Effected: " + abilityInUse.value);
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
}
public class GrowKeyword : ValueKey // TODO
{
    public GrowKeyword(KeywordName keyName) : base(keyName)
    {
    }
    public override KeywordOrder GetKeyOrder()
    {
        return KeywordOrder.Post;
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
            k.ModifyRange(currentAbility.value);
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
        
    }
}

public class TeamTarget : Target
{
    public override void Locate(Ability currentAbility)
    {
        GameManager.Instance.SetSelectionMode(SelectableGroupType.Team);
    }
}

public class AnyTarget : Target
{
    public override void Locate(Ability currentAbility)
    {
        
    }
}
