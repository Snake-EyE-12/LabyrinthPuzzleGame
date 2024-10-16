using System;
using System.Collections;
using System.Collections.Generic;
using Capstone.DataLoad;
using Guymon.DesignPatterns;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private RoundMenuDisplay roundMenuDisplayPrefab;
    [HideInInspector] public int currentRound = 0;
    [SerializeField] private EventBuilder eventBuilder;
    [SerializeField] private Transform canvasTransform;
    
    private List<Character> team = new List<Character>();
    public void SetTeam(List<string> teamLayout)
    {
        for (int i = 0; i < teamLayout.Count; i++)
        {
            team.Add(new Character(GameComponentDealer.GetCharacterData(teamLayout[i],
                DataHolder.currentMode.CharacterSelection.InitialDegree)));
            eventBuilder.SetUpEventOptions();
        }
        ContinueMission();
        


    }

    public Transform GetCanvasParent()
    {
        return canvasTransform;
    }

    [SerializeField] private Transform eventMenuParent;
    public void ContinueMission()
    {
        currentRound++;
        Instantiate(roundMenuDisplayPrefab, eventMenuParent).Set(DataHolder.eventsForEachRound[currentRound]);
    }

    [SerializeField] private TurnManager turnManager;
    public void BeginBoardFight()
    {
        turnManager.NextPhase();
    }

    public void PrepareEvent(EventData ed)
    {
        eventBuilder.PrepareEvent(ed);
    }

    public List<Character> GetCurrentTeam()
    {
        return team;
    }

    [SerializeField] private InputSelector selector;
    public void AddSelectable(Selectable selectable, SelectableGroupType groupType)
    {
        selector.AddSelectable(selectable, groupType);
    }
    public void RemoveSelectable(Selectable selectable, SelectableGroupType groupType)
    {
        selector.RemoveSelectable(selectable, groupType);
    }
    

    

    public void SetSelectionMode(SelectableGroupType type)
    {
        selector.ChangeSelectionType(type);
        
    }
    
    
    
    

    public CardDisplay cardToPlace { get; set; }

    public void SetCardToPlace(CardDisplay cd)
    {
        cardToPlace = cd;
        SetSelectionMode(SelectableGroupType.Tile);
    }

    public void RotateSelectedTile(RotationDirection d, int i)
    {
        cardToPlace.RotateTile(d, i);
    }

    public CardinalDirection DirectionToSlide { get; set; }

    public Ability AbilityInUse { get; set; }
    public Targetable AbilityUser { get; set; }

    
    
}

public interface Targetable
{
    public void HitByAbility(Ability ability);
    public void ChangeHealth(int amount);
    public void ApplyEffect(ActiveEffect effect);
    public void Move(Vector2Int direction);
    public Vector2Int GetGridPosition();
}
