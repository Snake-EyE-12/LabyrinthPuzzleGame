using System.Collections;
using System.Collections.Generic;
using Capstone.DataLoad;
using UnityEngine;


public class Ability
{
    private int value;
    private Target target;
    private List<ValueKey> keys = new List<ValueKey>();

    public void Use()
    {
        
    }
    public Ability(AbilityData data)
    {
        value = data.value;
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
                case "Push":
                    vk = new PushKeyword();
                    break;
                case "Damage":
                    vk = new DamageKeyword();
                    break;
                default:
                    vk = new ValueKey();
                    break;
                
            }
            keys.Add(vk);
        }
    }
}

public class CharacterAction
{
    //public stuff
}

public class ValueKey
{
    public virtual void ModifyAction(CharacterAction action)
    {}
}

public class PushKeyword : ValueKey
{
    
}
public class DamageKeyword : ValueKey
{
    
}

public abstract class Target
{
    
}
public class SelfTarget : Target
{
    
}

public class OpponentTarget : Target
{
    
}
public class TileTarget : Target
{
    
}

public class TeamTarget : Target
{
    
}

public class AnyTarget : Target
{
    
}