using System;
using System.Collections.Generic;
using Capstone.DataLoad;
using UnityEngine;


public class Health
{
    
    private List<HealthType> healthBar = new List<HealthType>();
    private int maxHealth;
    private HealthData[] resetData;
    public Health(HealthData[] hd)
    {
        resetData = hd;
        Reset(100, false);
    }

    public int GetIntPercentage()
    {
        return GetHealthValue() * 100 / GetMaxHealthValue();
    }
    public void Reset(int percent, bool hadDied)
    {
        //if(GetHealthValue() * 100.0f / maxHealth >= percent) return;
        maxHealth = 0;
        healthBar = new List<HealthType>();
        foreach (var hp in resetData)
        {
            AddHealthType(hp.Type, hp.Value);
        }

        if (percent != 100 && hadDied)
        {
            Damage(Mathf.FloorToInt(maxHealth * (percent / 100.0f)));
            TutorialManager.Instance.Teach("DamagedCharacters");
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
        maxHealth += value;
        switch (type)
        {
            case "Blood":
                healthBar.Add(new BloodHealth(value));
                break;
            default:
                break;
        }
    }

    public void RemoveHealth(int amount)
    {
        Damage(amount - GetMissingHealthValue());
        maxHealth = Mathf.Clamp(maxHealth - amount, 1, maxHealth);
    }

    public void Heal(HealthType health)
    {
        health.value = Mathf.Min(health.value, GetMissingHealthValue());
        healthBar.Add(health);
        isDead = false;
    }

    public void Damage(int amount)
    {
        if(amount <= 0) return;
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