using System;
using System.Collections;
using System.Collections.Generic;
using Capstone.DataLoad;
using NaughtyAttributes;
using UnityEngine;
using Range = Capstone.DataLoad.Range;

public class DataHandler : MonoBehaviour
{
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.I)) ReadData();
    }

    [Button]
    private void ReadData()
    {
        DataHolder.characterColorEquivalenceTable = GameDataReader.ConvertToJsonObject<CharacterColorEquivalenceTable>("LoadData/CharacterEquivalence");
        DataHolder.characterLayoutTable = GameDataReader.ConvertToJsonObject<CharacterLayoutTable>("LoadData/CharacterLayouts");

        DataHolder.availableCharacters = GameDataReader.ConvertToJsonObject<CharacterList>("LoadData/Characters");
        DataHolder.availableEnemies = GameDataReader.ConvertToJsonObject<EnemyList>("LoadData/Enemies");
        DataHolder.availableFights = GameDataReader.ConvertToJsonObject<FightList>("LoadData/Fights");
        DataHolder.availableTiles = GameDataReader.ConvertToJsonObject<TileList>("LoadData/Tiles");



        
        
        GamemodeManager.Instance.PrepareGamemodes(ReadGamemodes());
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
