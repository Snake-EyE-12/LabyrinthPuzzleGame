using System;
using System.Collections;
using System.Collections.Generic;
using Capstone.DataLoad;
using UnityEngine;
using EventArgs = Guymon.DesignPatterns.EventArgs;
using EventHandler = Guymon.DesignPatterns.EventHandler;

public class MapDisplay : Display<Map>
{
    public override void Render()
    {
        BuildMap();
        BuildDeck();
    }

    [SerializeField] private TileDisplay tilePrefab;
    [SerializeField] private CharacterDisplay characterPrefab;
    [SerializeField] private EnemyDisplay enemyPrefab;
    [SerializeField] private DeckDisplay deckDisplayPrefab;

    private void BuildDeck()
    {
        List<Card> cardsForDeck = new List<Card>();
        foreach (var c in GameManager.Instance.GetCurrentTeam())
        {
            cardsForDeck.AddRange(c.inventory.GetCards());
        }

        //Debug.Log("CardCount: " + cardsForDeck.Count);

        Instantiate(deckDisplayPrefab, GameManager.Instance.GetCanvasParent()).Set(new Deck(cardsForDeck));
    }

    private void Awake()
    {
        EventHandler.AddListener("Round/FightOver", OnBattleOver);
    }

    private void OnBattleOver(EventArgs args)
    {
        EventHandler.RemoveListenerLate("Round/FightOver", OnBattleOver);
        Destroy(this.gameObject);
    }

    private void BuildMap()
    {
        //Map
        int size = DataHolder.currentMode.GridSize;
        GridFiller filler = GridFiller.GetTypeOf(item.GetFight().FloorLayout == null ? "Blank" : item.GetFight().FloorLayout.Fill);
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                BuildNewTile(new Vector2Int(i, j), filler);
            }
        }
        //Team Units
        foreach (var teamMember in GameManager.Instance.GetCurrentTeam())
        {
            CharacterDisplay c = Instantiate(characterPrefab, transform);
            c.Set(teamMember);
            c.SetLocalMap(item);
            TileDisplay t = item.GetRandomBorderTile();
            c.SetGridPosition(t.GetGridPosition());
            t.GainControl(c);
        }
        //Villains
        foreach (var enemy in item.GetStartingEnemyNames())
        {
            EnemyDisplay c = Instantiate(enemyPrefab, transform);
            c.Set(new Enemy(Enemy.Load(enemy)));
            c.SetLocalMap(item);
            TileDisplay t = item.GetRandomCenterTile();
            c.SetGridPosition(t.GetGridPosition());
            t.GainControl(c);
        }
    }

    private void BuildNewTile(Vector2Int coords, GridFiller fill)
    {
        TileDisplay newTile = Instantiate(tilePrefab, transform);
        newTile.Set(fill.GetFillAt(DataHolder.currentMode.GridSize, coords, item.GetFight().FloorLayout.AvailableTiles));
        newTile.SetOntoMap(item, coords);
    }

    

    
}