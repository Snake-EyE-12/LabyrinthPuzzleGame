using System.Collections;
using System.Collections.Generic;
using Capstone.DataLoad;
using UnityEngine;

public class PickupItem : Loot
{
    public override void Collect(CharacterDisplay collector)
    {
        GameManager.Instance.GainCharm(GetValue());
    }

    public override string GetImageName()
    {
        return "UnknownItem";
    }

    public override string GetDisplayValue()
    {
        return "";
    }

    private int GetValue()
    {
        return val == -1 ? GameManager.Instance.currentRound : val;
    }

    public PickupItem(LootData data) : base(data)
    {
    }
}


public class XP : Loot
{
    public override void Collect(CharacterDisplay collector)
    {
        collector.GainXP(val);
    }
    public override string GetImageName()
    {
        return "XP";
    }

    public override string GetDisplayValue()
    {
        return "" + val;
    }

    public XP(LootData data) : base(data)
    {
    }
}
public class Coin : Loot
{
    public override void Collect(CharacterDisplay collector)
    {
        GameManager.Instance.CoinCount += val;
    }
    public override string GetImageName()
    {
        return "Coin-" + ConvertValueToSize();
    }

    public override string GetDisplayValue()
    {
        return "" + val;
    }

    private string ConvertValueToSize()
    {
        return val > 2 ? "Large" : "Small";
    }

    public Coin(LootData data) : base(data)
    {
    }
}

public abstract class Loot : Collectable
{
    protected int val;
    protected int dropChance;
    public Loot(LootData data)
    {
        this.val = data.Value;
        this.dropChance = data.DropChance;
    }

    public abstract void Collect(CharacterDisplay collector);
    public abstract string GetImageName();
    public abstract string GetDisplayValue();
    public bool PassDropChance() => Random.Range(0, 100) < dropChance;

    public static Loot Load(LootData data)
    {
        switch (data.Type)
        {
            case "XP":
                return new XP(data);
            case "Coin":
                return new Coin(data);
            default:
                return new PickupItem(data);
        }
    }
}