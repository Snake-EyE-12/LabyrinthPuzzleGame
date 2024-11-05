using System;
using System.Collections.Generic;
using UnityEngine;

public class CardLayout : MonoBehaviour
{
    private List<CardDisplayPosData> cards = new();
    [SerializeField] private RectTransform init;
    [SerializeField] private RectTransform final;
    public void Add(CardDisplay card, int position)
    {
        card.transform.SetParent(transform);
        cards.Add(new CardDisplayPosData() { card = card, pos = position });
    }

    public void Remove(int index)
    {
        cards[index].card.DiscardFromPlay();
        cards.RemoveAt(index);
        for(int i = index; i < cards.Count; i++)
        {
            cards[i].pos = i;
        }
    }

    public void Clear()
    {
        foreach (var card in cards)
        {
            card.card.RemoveCard();
        }
    }

    public List<CardDisplayPosData> GetHandCardDisplays()
    {
        return cards;
    }

    public void Refresh()
    {
        if(cards.Count <= 0) return;
        for (int i = 0; i < cards.Count; i++)
        {
            cards[i].card.MoveVisually(cards[i].card.transform.position, 1 + i);
            cards[i].card.MoveVisually(Vector3.Lerp(init.position,
                final.position, i * 1.0f / cards.Count), Mathf.Pow(0.9f, 1 + i));
        }
    }
    
}

public class CardDisplayPosData
{
    public CardDisplay card;
    public int pos;
}