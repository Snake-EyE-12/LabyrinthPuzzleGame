
using System;
using TMPro;
using UnityEngine;

public class CardCounterDisplay : Display<CardCounterDisplayData>
{
    [SerializeField] private TMP_Text valueTextbox;


    public override void Render()
    {
        valueTextbox.text = item.Value + " / " + item.Max;
    }

    private void Start()
    {
        item = new CardCounterDisplayData(){Value = DataHolder.cardsPlacedThisRound, Max = DataHolder.currentMode.CardsToPlacePerTurn};
    }

    private void Update()
    {
        item.Value = DataHolder.cardsPlacedThisRound;
        item.Max = DataHolder.currentMode.CardsToPlacePerTurn;
        Render();
    }
}

public class CardCounterDisplayData
{
    public int Value;
    public int Max;
}