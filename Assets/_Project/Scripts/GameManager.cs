using System;
using System.Collections;
using System.Collections.Generic;
using Guymon.DesignPatterns;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private GameState gameState = GameState.CharacterSelection;

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

    public void SetTeam(List<string> team)
    {
        //set team
        //begin game - load first event options
    }
}

public enum GameState
{
    CharacterSelection,
    Event,
    Over
}