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
        if (unit.health.isDead) unit.Die();
    }
    public void ApplyEffect(ActiveEffect effect)
    {
        
    }
}
