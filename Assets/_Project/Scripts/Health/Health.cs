using System;
using System.Collections.Generic;
using Capstone.DataLoad;
using UnityEngine;


public class Health
{
    
    private List<HealthType> healthBar = new List<HealthType>();
    private int maxHealth;
    public Health(HealthData[] hd)
    {
        foreach (var hp in hd)
        {
            AddHealthType(hp.Type, hp.Value);
            maxHealth += hp.Value;
        }
    }
    public List<HealthType> GetHealthBarSegments()
    {
        return healthBar;
    }

    public int GetHealthValue()
    {
        int sum = 0;
        foreach (var hp in healthBar)
        {
            sum += hp.value;
        }
        return sum;
    }

    public int GetMaxHealthValue()
    {
        return maxHealth;
    }

    private int GetMissingHealthValue()
    {
        return maxHealth - GetHealthValue();
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
        health.value = Mathf.Min(health.value, GetMissingHealthValue());
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

            if (healthBar.Count == 0)
            {
                isDead = true;
            }
            if (damage <= 0)
            {
                return;
            }

        }

        isDead = true;
    }
    
    public bool isDead = false;
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
        return new LeftOver
        {
            amount = Mathf.Abs(Mathf.Clamp(value, Int32.MinValue, 0)),
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