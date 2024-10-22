using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventArgs = Guymon.DesignPatterns.EventArgs;
using EventHandler = Guymon.DesignPatterns.EventHandler;


public class DeckDisplay : Display<Deck>
{
    private List<CardDisplay> handTiles = new List<CardDisplay>();
    [SerializeField] private Transform handParent;
    [SerializeField] private CardDisplay cardDisplayPrefab;

    public override void Render()
    {
        int handSize = item.GetHandSize();
        int maxForLoop = Mathf.Max(handSize, handTiles.Count);
        for (int i = 0; i < maxForLoop; i++)
        {
            if (handTiles.Count - 1 < i) handTiles.Add(Instantiate(cardDisplayPrefab, handParent));
            handTiles[i].Set(item.GetHandCards()[i]);
        }
    }
    private void Awake()
    {
        EventHandler.AddListener("Phase/DrawCards", DrawToLimit);
        EventHandler.AddListener("CardPlaced", DiscardSpecificCard);
        EventHandler.AddListener("Round/FightOver", OnBattleOver);
    }
    private void OnBattleOver(EventArgs args)
    {
        EventHandler.RemoveListenerLate("Round/FightOver", OnBattleOver);
        Destroy(this.gameObject);
    }
    private void DrawToLimit(EventArgs args)
    {
        int count = GetHandLimit() - item.GetHandSize();
        item.Draw(count);
        EventHandler.Invoke("DrawCards/LimitReached", null);
        Render();
    }

    private void OnDisable()
    {
        EventHandler.RemoveListener("Phase/DrawCards", DrawToLimit);
        EventHandler.RemoveListener("CardPlaced", DiscardSpecificCard);
        foreach (var cd in handTiles)
        {
            cd.RemoveFromPlay();
        }
    }

    private int GetHandLimit()
    {
        return 4;
    }

    private void DiscardSpecificCard(EventArgs args)
    {
        if (args is CardEventArgs)
        {
            Card cardToDiscard = (args as CardEventArgs).card;
            int index = -1;
            for (int i = 0; i < item.GetHandCards().Count; i++)
            {
                if (item.GetHandCards()[i].Equals(cardToDiscard))
                {
                    index = i;
                    break;
                }
            }
            if (index == -1) return;
            item.Discard(index);
            handTiles[index].RemoveFromPlay();
            handTiles.RemoveAt(index);
        }
    }

    

    
}

public class CardEventArgs : EventArgs
{
    public Card card;

    public CardEventArgs(Card c)
    {
        card = c;
    }
}