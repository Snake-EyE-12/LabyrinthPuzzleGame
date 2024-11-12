using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventArgs = Guymon.DesignPatterns.EventArgs;
using EventHandler = Guymon.DesignPatterns.EventHandler;

public class CharacterAbilityDisplay : Display<List<Ability>>
{
    
    [SerializeField] private AbilityDisplay abilityDisplayPrefab;
    [SerializeField] private Transform parent;

    private void Awake()
    {
        EventHandler.AddListener("Ability/DestroyPanel", OnRemove);
    }

    public override void Render()
    {
        foreach (var a in item)
        {
            Instantiate(abilityDisplayPrefab, parent).Set(a);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1))
        {
            GameManager.Instance.Phase = GamePhase.None;
            GameManager.Instance.Clean();
            GameManager.Instance.SetSelectionMode(SelectableGroupType.Team);
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
