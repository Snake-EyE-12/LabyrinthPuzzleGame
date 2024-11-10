using Guymon.DesignPatterns;
using UnityEngine;


public class SlideCommand : Command
{
    private Map mapOfTiles;
    private Tile onBoardTile;
    private Card playedCard;
    private DeckDisplay deck;
    bool rowOrColumn;
    bool upwardsOrRight;
    int number;

    public SlideCommand(bool row, bool positive, int pos, Map mapOfTiles, Card cardPlayed, DeckDisplay deck)
    {
        this.mapOfTiles = mapOfTiles;
        this.playedCard = cardPlayed;
        this.deck = deck;
        rowOrColumn = row;
        upwardsOrRight = positive;
        number = pos;

    }
    public void Execute()
    {
        if(rowOrColumn) onBoardTile = mapOfTiles.SlideRow(number, upwardsOrRight, Tile.Copy(playedCard.GetTile()));
        else onBoardTile = mapOfTiles.SlideColumn(number, upwardsOrRight, Tile.Copy(playedCard.GetTile()));
    }

    public void Undo()
    {
        deck.GetDeck().UnplayCard(playedCard);
        deck.Render();
        DataHolder.cardsPlacedThisRound--;
        
        if(rowOrColumn) mapOfTiles.SlideRow(number, !upwardsOrRight, Tile.Copy(onBoardTile));
        else mapOfTiles.SlideColumn(number, !upwardsOrRight, Tile.Copy(onBoardTile));
        
        GameManager.Instance.SetSelectionMode(SelectableGroupType.Card);

    }
}

public class SwapCommand : Command
{
    private Map mapOfTiles;
    private Tile onBoardTile;
    private Card playedCard;
    private Vector2Int newTilePosition;
    private DeckDisplay deck;
    public SwapCommand(Map mapOfTiles, Card cardPlayed, Vector2Int newTilePosition, DeckDisplay deck)
    {
        this.mapOfTiles = mapOfTiles;
        this.playedCard = cardPlayed;
        this.newTilePosition = newTilePosition;
        this.deck = deck;
    }

    public void Execute()
    {
        onBoardTile = mapOfTiles.GetTileAtPosition(newTilePosition).GetTile();
        mapOfTiles.Swap(newTilePosition, playedCard.GetTile());
    }

    public void Undo()
    {
        deck.GetDeck().UnplayCard(playedCard);
        deck.Render();
        DataHolder.cardsPlacedThisRound--;
        mapOfTiles.Swap(newTilePosition, onBoardTile);
        
        GameManager.Instance.SetSelectionMode(SelectableGroupType.Card);
    }
}

public class ConvertToTeamPhaseCommand : Command
{
    private TurnManager tm;
    private RoundPhase rp;
    private RoundPhase nextPhase;

    public ConvertToTeamPhaseCommand(TurnManager tm, RoundPhase thisPhase)
    {
        this.tm = tm;
        this.rp = thisPhase;
    }
    public void Execute()
    {
        nextPhase = tm.NextPhase();
    }

    public void Undo()
    {
        if (nextPhase == null) return;
        nextPhase.OnQuickLeave();
        rp.ReturnToThis();
        tm.SetPhaseTo(rp);
        CommandHandler.Undo();
    }
}