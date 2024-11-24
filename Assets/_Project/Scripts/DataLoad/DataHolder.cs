using System.Collections;
using System.Collections.Generic;
using Capstone.DataLoad;
using Guymon.DesignPatterns;
using UnityEngine;

public static class DataHolder
{
    public static Mode currentMode;
    public static CharacterLayoutTable characterLayoutTable;
    public static StringColorEquivalenceTable characterColorEquivalenceTable;
    public static StringColorEquivalenceTable keywordColorEquivalenceTable;
    public static StringColorEquivalenceTable eventColorEquivalenceTable;
    public static StringColorEquivalenceTable attackColorEquivalenceTable;
    public static CharacterList availableCharacters;
    public static EnemyList availableEnemies;
    public static FightList availableFights;
    public static TileList availableTiles;
    public static ItemList availableItems;
    public static List<EventsForRound> eventsForEachRound;




    public static int finalDeckSize;
    public static int itemsCollected;
    public static int challengesTaken;
    public static int defeatedRound;
    public static int coinsSpent;
    public static int enemiesKilled;

    public static int cardsPlacedThisRound;


    public static void Clear()
    {
        finalDeckSize = 0;
        itemsCollected = 0;
        challengesTaken = 0;
        defeatedRound = 0;
        coinsSpent = 0;
        enemiesKilled = 0;
        cardsPlacedThisRound = 0;
    }
}