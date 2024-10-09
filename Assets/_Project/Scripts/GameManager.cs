using System;
using System.Collections;
using System.Collections.Generic;
using Capstone.DataLoad;
using Guymon.DesignPatterns;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private RoundMenuDisplay roundMenuDisplayPrefab;
    public int currentRound = 0;
    //private GameState gameState = GameState.CharacterSelection;

    // private void Update()
    // {
    //     switch (gameState)
    //     {
    //         case GameState.CharacterSelection:
    //             break;
    //         case GameState.Event:
    //             break;
    //         case GameState.Over:
    //             break;
    //         default:
    //             break;
    //     }
    // }

    private List<CharacterData> team = new List<CharacterData>();
    public void SetTeam(List<string> teamLayout)
    {
        for (int i = 0; i < teamLayout.Count; i++)
        {
            team.Add(GameComponentDealer.GetCharacterData(teamLayout[i], DataHolder.currentMode.CharacterSelection.InitialDegree));
        }
        ContinueMission();
        


    }

    [SerializeField] private Transform eventMenuParent;
    public void ContinueMission()
    {
        currentRound++;
        Instantiate(roundMenuDisplayPrefab, eventMenuParent).Set(DataHolder.EventsForEachRound[currentRound]);
    }
}

public enum GameState
{
    CharacterSelection,
    Event,
    Over
}