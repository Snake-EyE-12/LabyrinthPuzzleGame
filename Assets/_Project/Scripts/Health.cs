using System.Collections.Generic;
using Capstone.DataLoad;


public class Health
{
    private List<HealthType> healthBar = new List<HealthType>();
    public Health(HealthData[] hd)
    {
        foreach (var hp in hd)
        {
            AddHealthType(hp.Type, hp.Value);
        }
    }

    public void AddHealthType(string type, int value)
    {
        switch (type)
        {
            case "Blood":
                healthBar.Add(new BloodHealth(value));
                break;
            default:
                break;
        }
    }
}

[System.Serializable]
public class HealthData
{
    public string Type;
    public int Value;
}
public abstract class HealthType
{
    protected int value;
    public HealthType(int value)
    {
        this.value = value;
    }
}

public class BloodHealth : HealthType
{
    public BloodHealth(int value) : base(value)
    {
    }
}