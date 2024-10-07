using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventArgs = Guymon.DesignPatterns.EventArgs;
using EventHandler = Guymon.DesignPatterns.EventHandler;

public class DeckHandler : MonoBehaviour
{
    [SerializeField] private DeckDisplay deckDisplay;
    private Deck deck = new Deck();

    private void Awake()
    {
        EventHandler.AddListener("Phase/DrawCards", DrawToLimit);
    }

    private void DrawToLimit(EventArgs args)
    {
        deck.Draw(deckDisplay, GetHandLimit() - deck.GetHandSize());
        EventHandler.Invoke("DrawCards/LimitReached", null);
    }

    private int GetHandLimit()
    {
        return 4;
    }
}
