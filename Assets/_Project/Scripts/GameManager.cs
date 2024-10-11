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

    

    public void SetSelectionMode(SelectableGroupType type)
    {
        selector.ChangeSelectionType(type);
        
    }
}
