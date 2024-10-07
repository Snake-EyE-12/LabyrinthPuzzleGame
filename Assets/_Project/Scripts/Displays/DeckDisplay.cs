using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventArgs = Guymon.DesignPatterns.EventArgs;

public class DeckDisplay : Display<Deck>
{
    private List<TileDisplay> handTiles = new List<TileDisplay>();
    [SerializeField] private float spacing = 2;
    [SerializeField] private Transform handParent;
    public override void Render(Deck deck)
    {
        // int handSize = itemToDisplay.GetHandSize();
        // int maxForLoop = Mathf.Max(handSize, handTiles.Count);
        // for (int i = 0; i < maxForLoop; i++)
        // {
        //     if (handTiles.Count < i) handTiles.Add(NewTileDisplay());
        //     handTiles[i].Set(itemToDisplay.GetHandCards()[i].GetTile());
        // }
    }

    public void PullCard(Card card)
    {
        TileDisplay newCard = NewTileDisplay();
        newCard.Render(card.GetTile());
        handTiles.Add(newCard);
        RepositionCards();
    }

    private void RepositionCards()
    {
        for (int i = 0; i < handTiles.Count; i++)
        {
            handTiles[i].transform.position = handParent.position + new Vector3(i * spacing, 0, 0);
            
        }
    }

    private TileDisplay NewTileDisplay()
    {
        TileDisplay obj = ObjectFactory.Instance.GetTileDisplay();
        obj.transform.SetParent(handParent);
        return obj;
    }
}
