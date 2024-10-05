using System;
using System.Collections;
using System.Collections.Generic;
using Guymon.DesignPatterns;
using UnityEngine;

public class TurnManager : Singleton<TurnManager>
{
    private List<RoundPhase> roundSquence = new List<RoundPhase>();
    private int currentPhase = 0;


    private void Start() //Temporary
    {
        roundSquence.Add(new OpponentTurnPhase());
        roundSquence.Add(new DrawCardsPhase());
        roundSquence.Add(new PlayCardsPhase());
        roundSquence.Add(new TeamTurnPhase());
        roundSquence.Add(new DamagePhase());
        roundSquence.Add(new CompletionPhase());
    }

    private void Update()
    {
        GetCurrentPhase().UpdatePhase();
    }

    [ContextMenu(nameof(NextPhase))]
    public void NextPhase()
    {
        GetCurrentPhase().EndPhase();
        IncrementPhase();
        GetCurrentPhase().StartPhase();
    }
    
    private void IncrementPhase()
    {
        currentPhase++;
        if (currentPhase >= roundSquence.Count)
        {
            currentPhase = 0;
        }
    }

    private RoundPhase GetCurrentPhase()
    {
        return roundSquence[currentPhase];
    }
}