using System;
using System.Collections;
using System.Collections.Generic;
using Capstone.DataLoad;
using UnityEngine;
using EventArgs = Guymon.DesignPatterns.EventArgs;

public class MapDisplay : Display<Map>
{
    public override void Render(Map item)
    {
        BuildMap();
    }

    [SerializeField] private TileDisplay tilePrefab;
    [SerializeField] private CharacterDisplay characterPrefab;


    private void BuildMap()
    {
        //Map
        int size = DataHolder.currentMode.GridSize;
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                BuildNewTile(new Vector2Int(i, j));
            }
        }
        //Units
        foreach (var teamMember in GameManager.Instance.GetCurrentTeam())
        {
            CharacterDisplay c = Instantiate(characterPrefab, transform);
            c.Set(teamMember);
            TileDisplay t = item.GetRandomBorderTile();
            c.SetGridPosition(t.GetGridPosition());
            t.GainControl(c);
        }
    }

    private void BuildNewTile(Vector2Int coords)
    {
        TileDisplay newTile = Instantiate(tilePrefab, transform);
        newTile.Set(GetTile());
        newTile.SetOntoMap(item, coords);
    }

    private Tile GetTile()
    {
        // Should Use Tile Filler
        return Tile.GenerateRandomTile();
    }
    

    
}