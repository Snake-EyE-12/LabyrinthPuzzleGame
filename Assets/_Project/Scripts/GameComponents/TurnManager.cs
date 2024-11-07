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


    private void Awake() //Temporary
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
        if (transitioning && timeToTransition < Time.time)
        {
            //Enable Selector
            GameManager.Instance.SetSelectionEnabled(true);
            transitioning = false;
            GetCurrentPhase().StartPhase();
        }
        
        GetCurrentPhase()?.UpdatePhase();
    }

    private float timeToTransition;
    private bool transitioning;

    [Button(nameof(NextPhase))]
    public RoundPhase NextPhase()
    {
        if (timeToTransition > Time.time) return null;
        //Disable Selector
        GameManager.Instance.SetSelectionEnabled(false);
        RoundPhase phase = GetCurrentPhase();
        if (phase != null)
        { 
            timeToTransition = Time.time + phase.GetTransitionTime();
            transitioning = true;
            phase.EndPhase();
            IncrementPhase();
            return GetCurrentPhase();
        }
        if (currentPhase == -1)
        {
            IncrementPhase();
            GetCurrentPhase().StartPhase();
            return GetCurrentPhase();
        }

        return null;
    }

    public void SetPhaseTo(RoundPhase phase)
    {
        currentPhase = roundSquence.IndexOf(phase);
    }

    public void Reset()
    {
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