using System.Collections.Generic;
using Capstone.DataLoad;


public class ActiveEffect
{
    private List<ActiveEffectType> effectBar = new List<ActiveEffectType>();
    public ActiveEffect(ActiveEffectData[] effects)
    {
        foreach (var aed in effects)
        {
            AddEffect(aed.Type, aed.Value);
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
                effectBar.Add(new PoisonActiveEffect(value));
                break;
            default:
                break;
        }
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
    protected int value;
    public ActiveEffectType(int value)
    {
        this.value = value;
    }
}

public class PoisonActiveEffect : ActiveEffectType
{
    public PoisonActiveEffect(int value) : base(value)
    {
    }
}
public class BurnActiveEffect : ActiveEffectType
{
    public BurnActiveEffect(int value) : base(value)
    {
    }
}