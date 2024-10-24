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
        EventHandler.AddListener("Ability/DestroyPanel", OnRemove);
    }

    public override void Render()
    {
        foreach (var a in item)
        {
            Instantiate(abilityDisplayPrefab, transform).Set(a);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.Instance.Phase = GamePhase.None;
            GameManager.Instance.Clean();
            ImmediateRemove();
        }
    }

    private void OnRemove(EventArgs args)
    {
        Destroy(this.gameObject);
        EventHandler.RemoveListenerLate("Ability/DestroyPanel", OnRemove);
    }

    private void ImmediateRemove()
    {
        Destroy(this.gameObject);
        EventHandler.RemoveListener("Ability/DestroyPanel", OnRemove);
    }
}
