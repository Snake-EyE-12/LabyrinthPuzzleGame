using System.Collections.Generic;
using Capstone.DataLoad;
using UnityEngine;

public abstract class Unit
{
    protected Map localMap;
     
     
    public string unitName { get; protected set; }
    public int degree { get; protected set; }
    public Health health { get; protected set; }
    public ActiveEffectList ActiveEffectsList { get; protected set; }

    public void Die()
    {
        GameManager.Instance.KillUnit(this);
    }
    public void CheckDeath()
    {
        if (health.isDead) Die();
    }
    
    
    
    
}

