using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventArgs = Guymon.DesignPatterns.EventArgs;
using EventHandler = Guymon.DesignPatterns.EventHandler;

public class CharacterAbilityDisplay : Display<List<Ability>>
{
    
    [SerializeField] private AbilityDisplay abilityDisplayPrefab;

    private void Awake()
    {
        EventHandler.AddListener("Ability/UsedAbility", OnRemove);
    }

    public override void Render()
    {
        foreach (var a in item)
        {
            Instantiate(abilityDisplayPrefab, transform).Set(a);
        }
    }

    private void OnRemove(EventArgs args)
    {
        Destroy(this.gameObject);
    }

    private void OnDisable()
    {
        EventHandler.RemoveListenerLate("Ability/UsedAbility", OnRemove);
    }
}
