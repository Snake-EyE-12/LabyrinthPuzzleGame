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
    private AbilityData data;

    public string GetKeywordsString()
    {
        string output = "";
        foreach (var key in data.Keys)
        {
            output += key + ", ";
        }
        return output;
    }


    public void PrepareTarget()
    {
        Debug.Log("Preparing Target: " + targetDescription);
        target.Locate();
    }
    public void Use(Targetable target)
    {
        foreach (var key in keys)
        {
            key.ModifyAction(target, value);
        }
        if (GameManager.Instance.AbilityUser != null) GameManager.Instance.AbilityUser.BecomeUsed();
        EventHandler.Invoke("Ability/UsedAbility", null);
    }
    public Ability(AbilityData data)
    {
        value = data.Value;
        this.data = data;
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
                    vk = new StuntedKeyword();
                    break;
                case "Knockback":
                    vk = new KnockbackKeyword();
                    break;
                case "Damage":
                    vk = new DamageKeyword();
                    break;
                case "Heal":
                    vk = new HealKeyword();
                    break;
                default:
                    vk = new ValueKey();
                    break;
                
            }
            keys.Add(vk);
        }
    }
}



public class ValueKey
{
    public virtual void ModifyAction(Targetable t, int value)
    {}
}

public class StuntedKeyword : ValueKey
{
    
}
public class KnockbackKeyword : ValueKey
{
    public override void ModifyAction(Targetable t, int value)
    {
        t.Move(t.GetGridPosition() - GameManager.Instance.AbilityUser.GetGridPosition());
    }
}
public class DamageKeyword : ValueKey
{
    public override void ModifyAction(Targetable t, int value)
    {
        t.ChangeHealth(-value);
    }
}
public class HealKeyword : ValueKey
{
    public override void ModifyAction(Targetable t, int value)
    {
        t.ChangeHealth(value);
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
    }
}

public class OpponentTarget : Target
{
    public override void Locate()
    {
        GameManager.Instance.SetSelectionMode(SelectableGroupType.Enemy);
        Debug.Log("Enemy Selection Started");
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
