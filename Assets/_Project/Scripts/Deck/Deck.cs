using System.Collections;
using System.Collections.Generic;
using Capstone.DataLoad;
using Guymon.DesignPatterns;
using UnityEngine;

public class Deck
{
    private Collection<Card> drawPile = new ListCollection<Card>();
    private Collection<Card> hand = new ListCollection<Card>();
    private Collection<Card> discardPile = new ListCollection<Card>();


    public Deck(List<Card> cards)
    {
        foreach (var card in cards)
        {
            drawPile.Add(card);
        }
        drawPile.Shuffle();
    }

    public void UnplayCard(Card c)
    {
        hand.Add(c);
        discardPile.RemoveLast();
    }

    public void Draw(int amount)
    {
        if(drawPile.Count() < amount) Shuffle();
        if(drawPile.Count() < amount) amount = drawPile.Count();
        for (int i = 0; i < amount; i++)
        {
            Card drawnCard = drawPile.Get();
            hand.Add(drawnCard);
            drawPile.RemoveFirst();
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
    public string owner;

    public Card(Tile tile)
    {
        this.tile = tile;
    }
    public static Card Generate()
    {
        return new Card(Tile.GenerateRandomTile());
    }

    public static Card Load(string symbol)
    {
        CardData cardData = DataHolder.availableTiles.FindCardBySymbol(symbol);
        Card card = null;
        try
        {
            card = new Card(new Tile(cardData.Tile, cardData.Ability));
        }
        catch (System.Exception e)
        {
            Debug.Log("Failed to find card of symbol: " + symbol);
            card = Card.Generate();
        }
        return card;
    }

    public Tile GetTile()
    {
        return tile;
    }
}