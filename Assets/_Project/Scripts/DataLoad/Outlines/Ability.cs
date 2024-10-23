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
    
    public string GetKeywordsString()
    {
        string output = "";
        foreach (var key in keys)
        {
            output += key.GetKeywordName().name + ",";
        }
        return output.Substring(0, output.Length - 1);
    }
    
    
    public static Sprite GetAbilityIcon(Ability ability)
    {
        string output = "V";
        output += ability.value + "-" + ability.targetDescription + "-";
        output += ability.GetKeywordsString();
        Sprite sprite = Resources.Load<Sprite>("KeynamedSprites/" + output);
        return sprite;
    }

    public static void OrderAbilityKeywords(List<ValueKey> keywords)
    {
        keywords.Sort((a, b) => b.Order().CompareTo(a.Order()));
    }

    
    public void PrepareTarget()
    {
        target.Locate();
    }
    public void Use(Targetable target)
    {
        foreach (var key in keys)
        {
            key.ModifyAction(target, value);
        }
        if (GameManager.Instance.AbilityUser != null) GameManager.Instance.AbilityUser.BecomeUsed();
        if(GameManager.Instance.Phase == GamePhase.UsingActiveAbility) EventHandler.Invoke("Ability/DestroyPanel", null);
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
            ValueKey vk;
            switch (keyword)
            {
                case "Stunted":
                    vk = new StuntedKeyword(new KeywordName(){color = Color.red, name = keyword});
                    break;
                case "Knockback":
                    vk = new KnockbackKeyword(new KeywordName(){color = Color.green, name = keyword});
                    break;
                case "Damage":
                    vk = new DamageKeyword(new KeywordName(){color = Color.blue, name = keyword});
                    break;
                case "Heal":
                    vk = new HealKeyword(new KeywordName(){color = Color.yellow, name = keyword});
                    break;
                case "Discard":
                    vk = new DiscardKeyword(new KeywordName(){color = Color.magenta, name = keyword});
                    break;
                case "Rotate":
                    vk = new RotateKeyword(new KeywordName(){color = Color.cyan, name = keyword});
                    break;
                default:
                    vk = new ValueKey(new KeywordName(){color = Color.black, name = null});
                    break;
                
            }
            keys.Add(vk);
        }
        Ability.OrderAbilityKeywords(keys);
    }
}









public struct KeywordName
{
    public Color color;
    public string name;
}

public enum KeywordOrder
{
    None,
    Selection,
    Movement,
    Effect
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

    public virtual int Order()
    {
        return 0;
    }
    public virtual void ModifyAction(Targetable t, int value)
    {}
}

public class RotateKeyword : ValueKey
{
    public override void ModifyAction(Targetable t, int value)
    {
        t.GetMap().RotateTile(t.GetGridPosition(), value);
    }

    public RotateKeyword(KeywordName keyName) : base(keyName)
    {
    }

    public override int Order()
    {
        return (int)KeywordOrder.Movement;
    }
}

public class DiscardKeyword : ValueKey
{
    // Discard first card in deck
    public DiscardKeyword(KeywordName keyName) : base(keyName)
    {
    }
    public override int Order()
    {
        return (int)KeywordOrder.None;
    }

    public override void ModifyAction(Targetable t, int value)
    {
        EventHandler.Invoke("Deck/DiscardFirst", null);
    }
}

public class StuntedKeyword : ValueKey
{
    public StuntedKeyword(KeywordName keyName) : base(keyName)
    {
    }
    public override int Order()
    {
        return (int)KeywordOrder.Selection;
    }
}
public class KnockbackKeyword : ValueKey
{
    public override void ModifyAction(Targetable t, int value)
    {
        t.Move(t.GetGridPosition() - GameManager.Instance.AbilityUser.GetGridPosition());
    }

    public KnockbackKeyword(KeywordName keyName) : base(keyName)
    {
    }
    public override int Order()
    {
        return (int)KeywordOrder.Movement;
    }
}
public class DamageKeyword : ValueKey
{
    public override void ModifyAction(Targetable t, int value)
    {
        t.ChangeHealth(-value);
    }

    public DamageKeyword(KeywordName keyName) : base(keyName)
    {
    }
    public override int Order()
    {
        return (int)KeywordOrder.Effect;
    }
}
public class HealKeyword : ValueKey
{
    public override void ModifyAction(Targetable t, int value)
    {
        t.ChangeHealth(value);
    }

    public HealKeyword(KeywordName keyName) : base(keyName)
    {
    }
    public override int Order()
    {
        return (int)KeywordOrder.Effect;
    }
}









public abstract class Target
{
    public abstract void Locate();
}
public class SelfTarget : Target
{
    public override void Locate()
    {
        GameManager.Instance.AbilityUser.HitByAbility(GameManager.Instance.AbilityInUse);
        GameManager.Instance.SetSelectionMode(SelectableGroupType.Team);
        GameManager.Instance.Phase = GamePhase.None;
    }
}

public class OpponentTarget : Target
{
    public override void Locate()
    {
        GameManager.Instance.SetSelectionMode(SelectableGroupType.Enemy);
    }
}
public class TileTarget : Target
{
    public override void Locate()
    {
        
    }
}

public class TeamTarget : Target
{
    public override void Locate()
    {
        GameManager.Instance.SetSelectionMode(SelectableGroupType.Team);
    }
}

public class AnyTarget : Target
{
    public override void Locate()
    {
        
    }
}
