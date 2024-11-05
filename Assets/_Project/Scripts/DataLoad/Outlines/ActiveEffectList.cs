using System.Collections.Generic;
using Capstone.DataLoad;


public class ActiveEffectList
{
    private List<ActiveEffectType> effectBar = new List<ActiveEffectType>();
    public ActiveEffectList(ActiveEffectData[] effects)
    {
        foreach (var aed in effects)
        {
            AddEffect(aed.Type, aed.Value);
        }
    }

    public void ApplyDamage(Targetable t)
    {
        foreach (var aet in effectBar)
        {
            aet.OnDamagePhase(t);
        }
    }
    public void EndOfTurn(Targetable t)
    {
        foreach (var aet in effectBar)
        {
            aet.OnEndOfTurn(t);
        }
    }

    public void AddEffect(string type, int value)
    {
        switch (type)
        {
            case "Poison":
                effectBar.Add(new PoisonActiveEffect(value));
                break;
            case "Burn":
                effectBar.Add(new BurnActiveEffect(value));
                break;
            case "Freeze":
                effectBar.Add(new FreezeActiveEffect(value));
                break;
            case "Bleed":
                effectBar.Add(new BleedActiveEffect(value));
                break;
            default:
                break;
        }
    }

    public void AddEffect(ActiveEffectType effect)
    {
        int indexOfEffect = GetIndexOfActiveEffectType(effect);
        if (indexOfEffect == -1)
        {
            effectBar.Add(effect);
            return;
        }
        effectBar[indexOfEffect].AddValue(effect.value);
    }
    private int GetIndexOfActiveEffectType(ActiveEffectType type)
    {
        for (int i = 0; i < effectBar.Count; i++)
        {
            if (effectBar[i].GetType() == type.GetType())
            {
                return i;
            }
        }
        return -1;
    }
}

[System.Serializable]
public class ActiveEffectData
{
    public string Type;
    public int Value;
}
public abstract class ActiveEffectType
{
    public int value;
    public ActiveEffectType(int value)
    {
        this.value = value;
    }

    public void AddValue(int value)
    {
        this.value += value;
    }

    public virtual void OnEffectGained(Targetable u)
    {
        
    }
    public virtual void OnDamagePhase(Targetable u)
    {
        
    }
    public virtual void OnEndOfTurn(Targetable u)
    {
        
    }
}

public class PoisonActiveEffect : ActiveEffectType
{
    public PoisonActiveEffect(int value) : base(value)
    {
    }
    public override void OnDamagePhase(Targetable u)
    {
        u.ChangeHealth(-value);
    }
}
public class BurnActiveEffect : ActiveEffectType
{
    public BurnActiveEffect(int value) : base(value)
    {
    }
    public override void OnDamagePhase(Targetable u)
    {
        u.ChangeHealth(-value);
    }

    public override void OnEndOfTurn(Targetable u)
    {
        value--;
    }
    
}
public class FreezeActiveEffect : ActiveEffectType
{
    public FreezeActiveEffect(int value) : base(value)
    {
    }
}
public class BleedActiveEffect : ActiveEffectType
{
    public BleedActiveEffect(int value) : base(value)
    {
    }
    public override void OnDamagePhase(Targetable u)
    {
        u.ChangeHealth(-1);
    }
}