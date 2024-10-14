using System;
using System.Collections;
using System.Collections.Generic;
using Guymon.DesignPatterns;
using NaughtyAttributes;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    private List<RoundPhase> roundSquence = new List<RoundPhase>();
    private int currentPhase = -1;


    private void Start() //Temporary
    {
        LoadSequence(null);
    }

    public void LoadSequence(string[] sequence)
    {
        roundSquence.Add(new OpponentTurnPhase(this));
        roundSquence.Add(new DrawCardsPhase(this));
        roundSquence.Add(new PlayCardsPhase(this));
        roundSquence.Add(new TeamTurnPhase(this));
        roundSquence.Add(new DamagePhase(this));
        roundSquence.Add(new CompletionPhase(this));
    }

    private void Update()
    {
        if (currentPhase < 0) return;
        GetCurrentPhase().UpdatePhase();
    }

    [Button(nameof(NextPhase))]
    public void NextPhase()
    {
        GetCurrentPhase()?.EndPhase();
        IncrementPhase();
        GetCurrentPhase().StartPhase();
    }

    public void Reset()
    {
        GetCurrentPhase()?.EndPhase();
        currentPhase = -1;
    }
    
    private void IncrementPhase()
    {
        currentPhase = (currentPhase + 1) % roundSquence.Count;
    }

    private RoundPhase GetCurrentPhase()
    {
        if (currentPhase < 0) return null;
        return roundSquence[currentPhase];
    }
}