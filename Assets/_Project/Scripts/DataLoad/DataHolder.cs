using System.Collections;
using System.Collections.Generic;
using Capstone.DataLoad;
using Guymon.DesignPatterns;
using UnityEngine;

public static class DataHolder
{
    public static Mode currentMode;
    public static CharacterLayoutTable characterLayoutTable;
    public static CharacterColorEquivalenceTable characterColorEquivalenceTable;
    public static CharacterList availableCharacters;
    public static EnemyList availableEnemies;
    public static FightList availableFights;
    public static TileList availableTiles;
    public static List<EventsForRound> eventsForEachRound;
}