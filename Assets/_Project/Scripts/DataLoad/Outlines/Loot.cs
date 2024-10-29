using System.Collections;
using System.Collections.Generic;
using Capstone.DataLoad;
using UnityEngine;

public class Item : Loot
{
    private ItemData data;
    public override void Collect(CharacterDisplay collector)
    {
        
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

    public Item(int value) : base(value)
    {
        data = new ItemData(); //Insert correct value & load data
    }
}

public class ItemData
{
    
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

    public XP(int value) : base(value)
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

    public Coin(int value) : base(value)
    {
    }
}

public abstract class Loot : Collectable
{
    protected int val;
    public Loot(int value) => this.val = value;
    public abstract void Collect(CharacterDisplay collector);
    public abstract string GetImageName();
    public abstract string GetDisplayValue();

    public static Loot Load(LootData data)
    {
        switch (data.Type)
        {
            case "XP":
                return new XP(data.Value);
            case "Coin":
                return new Coin(data.Value);
            default:
                return new Item(data.Value);
        }
    }
}