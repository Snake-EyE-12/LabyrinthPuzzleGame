using System;
using System.Collections;
using System.Collections.Generic;
using Capstone.DataLoad;
using NaughtyAttributes;
using UnityEngine;
using Range = Capstone.DataLoad.Range;

public class DataHandler : MonoBehaviour
{
    private void Awake()
    {
        ReadData();
    }

    [Button]
    private void ReadData()
    {
        GamemodeManager.Instance.PrepareGamemodes(ReadGamemodes(), this);
    }

    public void ReadDataFromMode(Mode mode)
    {
        DataHolder.currentMode = mode;
        
        DataHolder.characterColorEquivalenceTable = GameDataReader.ConvertToJsonObject<StringColorEquivalenceTable>("LoadData/Gamemodes/" + mode.DisplayName + "/CharacterEquivalence");
        DataHolder.keywordColorEquivalenceTable = GameDataReader.ConvertToJsonObject<StringColorEquivalenceTable>("LoadData/Gamemodes/" + mode.DisplayName + "/KeywordColorEquivalence");
        DataHolder.eventColorEquivalenceTable = GameDataReader.ConvertToJsonObject<StringColorEquivalenceTable>("LoadData/Gamemodes/" + mode.DisplayName + "/EventColorEquivalence");
        DataHolder.characterLayoutTable = GameDataReader.ConvertToJsonObject<CharacterLayoutTable>("LoadData/Gamemodes/" + mode.DisplayName + "/CharacterLayouts");

        DataHolder.availableCharacters = GameDataReader.ConvertToJsonObject<CharacterList>("LoadData/Gamemodes/" + mode.DisplayName + "/Characters");
        DataHolder.availableEnemies = GameDataReader.ConvertToJsonObject<EnemyList>("LoadData/Gamemodes/" + mode.DisplayName + "/Enemies");
        DataHolder.availableFights = GameDataReader.ConvertToJsonObject<FightList>("LoadData/Gamemodes/" + mode.DisplayName + "/Fights");
        DataHolder.availableTiles = GameDataReader.ConvertToJsonObject<TileList>("LoadData/Gamemodes/" + mode.DisplayName + "/Tiles");
        DataHolder.availableItems = GameDataReader.ConvertToJsonObject<ItemList>("LoadData/Gamemodes/" + mode.DisplayName + "/Charms");
    }

    private List<Mode> ReadGamemodes()
    {
        List<Mode> modes = new List<Mode>();
        modes.Clear();
        List<string> gamemodes = GameDataReader.GetAllGamemodeNames();

        foreach (string gamemode in gamemodes)
        {
            Mode mode = GameDataReader.ConvertToJsonObject<Mode>("LoadData/Gamemodes/" + gamemode + "/Mode");
            mode.DisplayName = gamemode;
            modes.Add(mode);
        }

        return modes;
    }
    
    

}
