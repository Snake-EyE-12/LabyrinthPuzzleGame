using System.Collections;
using System.Collections.Generic;
using Guymon.DesignPatterns;
using UnityEngine;

public class Deck
{
    private Collection<Card> drawPile = new ListCollection<Card>();
    private Collection<Card> hand = new ListCollection<Card>();
    private Collection<Card> discardPile = new ListCollection<Card>();


    public Deck()
    {
        drawPile.Add(new Card());
        drawPile.Add(new Card());
        drawPile.Add(new Card());
        drawPile.Add(new Card());
        drawPile.Add(new Card());
        drawPile.Add(new Card());
        drawPile.Add(new Card());
    }

    public void Draw(DeckDisplay display, int amount)
    {
        if(drawPile.Count() < amount) Shuffle();
        if(drawPile.Count() < amount) amount = drawPile.Count();
        for (int i = 0; i < amount; i++)
        {
            Debug.Log("Drawing 1 Card");
            Card drawnCard = drawPile.Get();
            hand.Add(drawnCard);
            drawPile.Remove();
            display.PullCard(drawnCard);
        }
    }

    public void Discard(int index)
    {
        discardPile.Add(hand.GetAt(index));
        hand.RemoveAt(index);
        
    }

    public void Shuffle()
    {
        discardPile.Shuffle();
        drawPile.AddAll(discardPile);
        discardPile.Clear();
    }

    public int GetHandSize()
    {
        return hand.Count();
    }

    public List<Card> GetHandCards()
    {
        List<Card> cards = new List<Card>();
        for (int i = 0; i < hand.Count(); i++)
        {
            cards.Add(hand.GetAt(i));
        }
        return cards;
    }
}


public class Card
{
    private Tile tile;

    public Tile GetTile()
    {
        return Tile.GenerateRandomTile();
    }
}