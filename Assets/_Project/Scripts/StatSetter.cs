using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using EventArgs = Guymon.DesignPatterns.EventArgs;
using EventHandler = Guymon.DesignPatterns.EventHandler;

public class StatSetter : MonoBehaviour
{
    [SerializeField] private TMP_Text deckSize;
    [SerializeField] private TMP_Text itemCount;
    [SerializeField] private TMP_Text challengeCount;
    [SerializeField] private TMP_Text defeatedRound;
    [SerializeField] private TMP_Text coinSpent;
    [SerializeField] private TMP_Text enemiesKilled;

    private void Awake()
    {
        deckSize.text = "" + DataHolder.finalDeckSize;
        itemCount.text = "" + DataHolder.itemsCollected;
        challengeCount.text = "" + DataHolder.challengesTaken;
        defeatedRound.text = "" + DataHolder.defeatedRound;
        coinSpent.text = "" + DataHolder.coinsSpent;
        enemiesKilled.text = "" + DataHolder.enemiesKilled;
    }
}
