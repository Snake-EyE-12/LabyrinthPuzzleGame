using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventArgs = Guymon.DesignPatterns.EventArgs;
using EventHandler = Guymon.DesignPatterns.EventHandler;


public class DeckDisplay : Display<Deck>
{
    [SerializeField] private CardDisplay cardDisplayPrefab;
    [SerializeField] private CardLayout handLayout;
    
    private void Awake()
    {
        EventHandler.AddListener("Phase/DrawCards", DrawToLimit);
        EventHandler.AddListener("CardPlaced", DiscardSpecificCard);
        EventHandler.AddListener("Round/FightOver", OnBattleOver);
        EventHandler.AddListener("Deck/DiscardFirst", DiscardOldest);
    }
    private void OnBattleOver(EventArgs args)
    {
        EventHandler.RemoveListenerLate("Round/FightOver", OnBattleOver);
        Destroy(this.gameObject);
    }
    private void OnDisable()
    {
        EventHandler.RemoveListener("Phase/DrawCards", DrawToLimit);
        EventHandler.RemoveListener("CardPlaced", DiscardSpecificCard);
        handLayout.Clear();
    }

    [SerializeField] private Vector3 spawnPoint;
    public override void Render()
    {
        // int handSize = item.GetHandSize();
        // int maxForLoop = Mathf.Max(handSize, handLayout.GetCount());
        // for (int i = 0; i < maxForLoop; i++)
        // {
        //     if (handLayout.GetCount() - 1 < i)
        //     {
        //         CardDisplay newCard = Instantiate(cardDisplayPrefab, handLayout.transform);
        //         handLayout.Add();
        //     }
        //     handTiles[i].Set(item.GetHandCards()[i]);
        // }
        //DrawToLimit(null);
        for (int i = handLayout.GetHandCardDisplays().Count; i < item.GetHandSize(); i++)
        {
            CardDisplay newCard = Instantiate(cardDisplayPrefab, spawnPoint, Quaternion.identity, handLayout.transform);
            newCard.Set(item.GetHandCards()[i]);
            handLayout.Add(newCard, i);
        }
    }
    private void DrawToLimit(EventArgs args)
    {
        int count = GetHandLimit() - item.GetHandSize();
        item.Draw(count);
        EventHandler.Invoke("DrawCards/LimitReached", null);
        Render();
    }


    private int GetHandLimit()
    {
        return DataHolder.currentMode.HandSize;
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
            DiscardAt(index);
        }
    }
    private void DiscardOldest(EventArgs args)
    {
        if(handLayout.GetHandCardDisplays().Count <= 0) return;
        DiscardAt(0);
    }

    private void DiscardAt(int index)
    {
        handLayout.Remove(index);
        item.Discard(index);
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