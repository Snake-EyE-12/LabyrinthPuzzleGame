using System.Collections.Generic;
using Capstone.DataLoad;
using UnityEngine;


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
    
    public void Clear<T>(int amount) where T : ActiveEffectType
    {
        T e = GetEffect<T>();
        if (e != null)
        {
            e.value -= amount;
            if (e.value < 0)
            {
                e.value = 0;
            }
        }
    }
    
    public int[] GetDOTArray()
    {
        int[] poisonBurnBleed = new int[3];
        foreach (var aet in effectBar)
        {
            if (aet is PoisonActiveEffect) poisonBurnBleed[0] = aet.value;
            if (aet is BurnActiveEffect) poisonBurnBleed[1] = aet.value;
            if (aet is BleedActiveEffect) poisonBurnBleed[2] = aet.value;
        }
        return poisonBurnBleed;
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

    public void Reset()
    {
        effectBar = new List<ActiveEffectType>();
    }

    public void AddEffect(string type, int value)
    {
        switch (type)
        {
            case "Poison":
                AddEffect(new PoisonActiveEffect(value));
                break;
            case "Burn":
                AddEffect(new BurnActiveEffect(value));
                break;
            case "Freeze":
                AddEffect(new FreezeActiveEffect(value));
                break;
            case "Bleed":
                AddEffect(new BleedActiveEffect(value));
                break;
            case "Shield":
                AddEffect(new ShieldActiveEffect(value));
                break;
            case "Immunity":
                AddEffect(new ImmunityActiveEffect(value));
                break;
            default:
                break;
        }
    }

    public T GetEffect<T>() where T : ActiveEffectType
    {
        foreach (var aet in effectBar)
        {
            if(aet is T) return aet as T;
        }

        return null;
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

    public List<ActiveEffectType> GetActiveEffects()
    {
        return effectBar;
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

    public virtual void AddValue(int value)
    {
        this.value += value;
    }

    public virtual bool IsDOT()
    {
        return false;
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
        u.ChangeHealth(-value, true);
    }
    public override bool IsDOT()
    {
        return true;
    }
}
public class BurnActiveEffect : ActiveEffectType
{
    public BurnActiveEffect(int value) : base(value)
    {
    }
    public override void OnDamagePhase(Targetable u)
    {
        u.ChangeHealth(-value, true);
    }

    public override void OnEndOfTurn(Targetable u)
    {
        value = Mathf.Clamp(--value, 0, value);
    }
    public override bool IsDOT()
    {
        return true;
    }
    
}
public class FreezeActiveEffect : ActiveEffectType
{
    public FreezeActiveEffect(int value) : base(value)
    {
    }
    public override void OnEndOfTurn(Targetable u)
    {
        value = 0;
    }
}
public class BleedActiveEffect : ActiveEffectType
{
    public BleedActiveEffect(int value) : base(value)
    {
    }
    public override void OnDamagePhase(Targetable u)
    {
        u.ChangeHealth(-1, true);
    }
    public override bool IsDOT()
    {
        return true;
    }
    public override void AddValue(int value)
    {
        this.value = Mathf.Clamp(this.value + value, 0, 1);

    }
}

public class ShieldActiveEffect : ActiveEffectType
{
    public ShieldActiveEffect(int value) : base(value)
    {
    }
    public override void OnEndOfTurn(Targetable u)
    {
        value = 0;
    }
}
public class ImmunityActiveEffect : ActiveEffectType
{
    public ImmunityActiveEffect(int value) : base(value)
    {
    }
    public override void OnEndOfTurn(Targetable u)
    {
        value = 0;
    }
}
