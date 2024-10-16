using System;
using System.Collections.Generic;
using Capstone.DataLoad;
using UnityEngine;


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
    public List<HealthType> GetHealth()
    {
        return healthBar;
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

    public void Heal(HealthType health)
    {
        healthBar.Add(health);
        isDead = false;
    }

    public void Damage(int amount)
    {
        int damage = amount;
        for(int i = healthBar.Count - 1; i >= 0; i--)
        {
            LeftOver left = healthBar[i].Remove(damage);
            damage = left.amount;
            if (left.isEmpty)
            {
                healthBar.RemoveAt(i);
            }

            Debug.Log("CURRENT DAMAGE VALUE: " + damage);
            if (damage <= 0)
            {
                return;
            }

        }

        isDead = true;
        Debug.Log("DIED");
    }
    
    private bool isDead = false;
}

[System.Serializable]
public class HealthData
{
    public string Type;
    public int Value;
}
public abstract class HealthType
{
    public int value;
    public HealthType(int value)
    {
        this.value = value;
    }

    public virtual LeftOver Remove(int amount)
    {
        value -= amount;
        Debug.Log("LV: value: " + value);
        Debug.Log("LV: clamp: " + Mathf.Clamp(value, Int32.MinValue, 0));
        Debug.Log("LV: abs: " + Mathf.Abs(Mathf.Clamp(value, Int32.MinValue, 0)));
        return new LeftOver
        {
            amount = Mathf.Abs(Mathf.Clamp(value, Int32.MinValue, 0)),
            //amount = value,
            isEmpty = value <= 0
        };
    }

    public virtual Color GetColor()
    {
        return Color.red;
    }
}

public struct LeftOver
{
    public bool isEmpty;
    public int amount;
}

public class BloodHealth : HealthType
{
    public BloodHealth(int value) : base(value)
    {
    }
}