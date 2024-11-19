using System.Collections.Generic;
using Guymon.DesignPatterns;
using UnityEngine;

public abstract class RoundPhase
{
    protected TurnManager tm;

    public RoundPhase(TurnManager tm)
    {
        this.tm = tm;
    }

    public virtual void StartPhase()
    {
        SkipPhase();
    }
    public virtual void UpdatePhase() { }
    public virtual void EndPhase() { }
    protected void SkipPhase() { tm.NextPhase();}
    public virtual float GetTransitionTime() { return 0.0f; }
    public virtual void ReturnToThis() { }
    public virtual void OnQuickLeave() { }
}


public class OpponentTurnPhase : RoundPhase
{
    public OpponentTurnPhase(TurnManager tm) : base(tm)
    {
    }

    public override void StartPhase()
    {
        AttackIndicator.Instance.ClearAttacks();
        GameManager.Instance.MoveEnemies();
        GameManager.Instance.PickEnemyAttacks();
        AttackIndicator.Instance.VisualizeAttacks();
        GameManager.Instance.SetCharactersUsable();
    }
    
}
public class DrawCardsPhase : RoundPhase
{
    public DrawCardsPhase(TurnManager tm) : base(tm)
    {
    }

    public override void StartPhase()
    {
        EventHandler.Invoke("Ability/DestroyPanel", null);
        EventHandler.AddListener("DrawCards/LimitReached", OnLimitReached);
        EventHandler.Invoke("Phase/DrawCards", null);
    }

    public void OnLimitReached(EventArgs args)
    {
        tm.NextPhase();
    }

    public override void EndPhase()
    {
        EventHandler.RemoveListenerLate("DrawCards/LimitReached", OnLimitReached);
    }
}
public class PlayCardsPhase : RoundPhase
{
    public PlayCardsPhase(TurnManager tm) : base(tm)
    {
    }


    public override void StartPhase()
    {
        CommandHandler.Clear();
        GameManager.Instance.SetSelectionMode(SelectableGroupType.Card);
        DataHolder.cardsPlacedThisRound = 0;
        EventHandler.AddListener("CardPlaced", CardPlaced);
    }

    private void CardPlaced(EventArgs args)
    {
        DataHolder.cardsPlacedThisRound++;
        if (DataHolder.cardsPlacedThisRound >= DataHolder.currentMode.CardsToPlacePerTurn)
        {
            CommandHandler.Execute(new ConvertToTeamPhaseCommand(tm, this));
            return;
        }
        GameManager.Instance.SetSelectionMode(SelectableGroupType.Card);
    }

    public override void ReturnToThis()
    {
        GameManager.Instance.SetSelectionMode(SelectableGroupType.Card);
        EventHandler.AddListener("CardPlaced", CardPlaced);
    }

    public override void EndPhase()
    {
        GameManager.Instance.HideSliderDisplay();
        EventHandler.RemoveListenerLate("CardPlaced", CardPlaced);
    }
}
public class TeamTurnPhase : RoundPhase
{
    public TeamTurnPhase(TurnManager tm) : base(tm)
    {
    }

    public override void StartPhase()
    {
        GameManager.Instance.SetSelectionMode(SelectableGroupType.Team);
        EventHandler.AddListener("Round/EndTurn", TryEndTurn);
    }

    public override void UpdatePhase()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            EventHandler.RemoveListener("Round/EndTurn", TryEndTurn);
            EndTurn(null);
        }
    }

    public override void OnQuickLeave()
    {
        EventHandler.RemoveListener("Round/EndTurn", TryEndTurn);
    }

    private void TryEndTurn(EventArgs args)
    {
        EventHandler.RemoveListenerLate("Round/EndTurn", TryEndTurn);
        EndTurn(null);
    }

    private void EndTurn(EventArgs args)
    {
        AudioManager.Instance.Play("ButtonClick");
        AudioManager.Instance.Play("Whoosh");
        tm.NextPhase();
    }
}
public class DamagePhase : RoundPhase
{
    public DamagePhase(TurnManager tm) : base(tm)
    {
        
    }

    public override void StartPhase()
    {
        // Enemy Damage
        AttackIndicator.Instance.ExecuteAttacks();
        List<CharacterDisplay> characterList = GameManager.Instance.GetActiveCharacters();
        for (int i = characterList.Count - 1; i >= 0; i--)
        {
            characterList[i].CheckForDeath();
        }
        // Enemy Effects
        List<EnemyDisplay> activeEnemies = GameManager.Instance.GetActiveEnemies();
        for (int i = activeEnemies.Count - 1; i >= 0; i--)
        {
            activeEnemies[i].ApplyDamagePhaseEffects();
        }
        // Team Effects
        characterList = GameManager.Instance.GetActiveCharacters();
        for (int i = characterList.Count - 1; i >= 0; i--)
        {
            characterList[i].ApplyDamagePhaseEffects();
        }
        // Enemy Die From Effects
        for (int i = activeEnemies.Count - 1; i >= 0; i--)
        {
            activeEnemies[i].CheckForDeath();
        }
        // Team Die From Effects
        for (int i = characterList.Count - 1; i >= 0; i--)
        {
            characterList[i].CheckForDeath();
        }
        // Enemy Effects Altering
        activeEnemies = GameManager.Instance.GetActiveEnemies();
        for (int i = activeEnemies.Count - 1; i >= 0; i--)
        {
            activeEnemies[i].ApplyEndOfTurnPhaseEffects();
        }
        // Team Effects Altering
        characterList = GameManager.Instance.GetActiveCharacters();
        for (int i = characterList.Count - 1; i >= 0; i--)
        {
            characterList[i].ApplyEndOfTurnPhaseEffects();
        }
        
        tm.NextPhase();
    }
}
public class CompletionPhase : RoundPhase
{
    public CompletionPhase(TurnManager tm) : base(tm)
    {
    }

    public override void StartPhase()
    {
        GameManager.Instance.CheckGameOver();
    }
}