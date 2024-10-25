using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damager
{
    private Unit unit;
    public Damager(Unit c)
    {
        unit = c;
    }
    public void Heal(int amount)
    {
        unit.health.Heal(new BloodHealth(amount));
    }

    public void TakeDamage(int amount)
    {
        unit.health.Damage(amount);
    }
    public void ApplyEffect(ActiveEffect effect)
    {
        
    }

    public float GetHealthPercent()
    {
        return unit.health.GetHealthValue() * 1.0f / unit.health.GetMaxHealthValue();
    }
}
