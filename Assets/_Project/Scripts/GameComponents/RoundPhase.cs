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
        GameManager.Instance.enemyMovingRightNow = true;
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
        Debug.Log("UNDOING SYSTEM: STARTING DRAW PHASE");
        EventHandler.AddListener("DrawCards/LimitReached", OnLimitReached);
        EventHandler.Invoke("Phase/DrawCards", null);
    }

    public void OnLimitReached(EventArgs args)
    {
        Debug.Log("UNDOING SYSTEM: CARD LIMIT REACHED");
        tm.NextPhase();
    }

    public override void EndPhase()
    {
        Debug.Log("UNDOING SYSTEM: ENDING DRAW PHASE");
        EventHandler.RemoveListener("DrawCards/LimitReached", OnLimitReached);
    }
}
public class PlayCardsPhase : RoundPhase
{
    public PlayCardsPhase(TurnManager tm) : base(tm)
    {
    }


    public override void StartPhase()
    {
        Debug.Log("UNDOING SYSTEM: STARTING PLAYING PHASE");
        CommandHandler.Clear();
        GameManager.Instance.SetSelectionMode(SelectableGroupType.Card);
        DataHolder.cardsPlacedThisRound = 0;
        EventHandler.AddListener("CardPlaced", CardPlaced);
    }

    private void CardPlaced(EventArgs args)
    {
        Debug.Log("UNDOING SYSTEM: CARD PLACED");
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
        Debug.Log("UNDOING SYSTEM: RETURN TO PLAYING CARDS PHASE");
        GameManager.Instance.SetSelectionMode(SelectableGroupType.Card);
        EventHandler.AddListener("CardPlaced", CardPlaced);
    }

    public override void EndPhase()
    {
        Debug.Log("UNDOING SYSTEM: ENDING PLAYING PHASE");
        GameManager.Instance.HideSliderDisplay();
        EventHandler.RemoveListener("CardPlaced", CardPlaced);
    }
}
public class TeamTurnPhase : RoundPhase
{
    public TeamTurnPhase(TurnManager tm) : base(tm)
    {
    }

    public override void StartPhase()
    {
        Debug.Log("UNDOING SYSTEM: STARTING TEAM PHASE");
        GameManager.Instance.enemyMovingRightNow = false;
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
        Debug.Log("UNDOING SYSTEM: QUICK LEAVE");
        EventHandler.RemoveListener("Round/EndTurn", TryEndTurn);
    }

    private void TryEndTurn(EventArgs args)
    {
        EventHandler.RemoveListener("Round/EndTurn", TryEndTurn);
        EndTurn(null);
    }

    private void EndTurn(EventArgs args)
    {
        Debug.Log("UNDOING SYSTEM: ENDING TEAM PHASE");
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
        EffectTeam();
        AlterTeamEffects();
        EffectEnemies();
        AlterEnemyEffects();
        HaveEnemiesAttack();
        CheckEnemyDeaths();
        CheckTeamDeaths();
        tm.NextPhase();
    }

    private void HaveEnemiesAttack()
    {
        AttackIndicator.Instance.ExecuteAttacks();
    }

    private void CheckTeamDeaths()
    {
        List<CharacterDisplay> characterList = GameManager.Instance.GetActiveCharacters();
        for (int i = characterList.Count - 1; i >= 0; i--)
        {
            characterList[i].CheckForDeath();
        }
    }

    private void CheckEnemyDeaths()
    {
        List<EnemyDisplay> activeEnemies = GameManager.Instance.GetActiveEnemies();
        for (int i = activeEnemies.Count - 1; i >= 0; i--)
        {
            activeEnemies[i].CheckForDeath();
        }
    }

    private void EffectEnemies()
    {
        List<EnemyDisplay> activeEnemies = GameManager.Instance.GetActiveEnemies();
        for (int i = activeEnemies.Count - 1; i >= 0; i--)
        {
            activeEnemies[i].ApplyDamagePhaseEffects();
        }
    }

    private void EffectTeam()
    {
        List<CharacterDisplay> characterList = GameManager.Instance.GetActiveCharacters();
        for (int i = characterList.Count - 1; i >= 0; i--)
        {
            characterList[i].ApplyDamagePhaseEffects();
        }
    }
    private void AlterEnemyEffects()
    {
        List<EnemyDisplay> activeEnemies = GameManager.Instance.GetActiveEnemies();
        for (int i = activeEnemies.Count - 1; i >= 0; i--)
        {
            activeEnemies[i].ApplyEndOfTurnEffectChanges();
        }
    }

    private void AlterTeamEffects()
    {
        List<CharacterDisplay> characterList = GameManager.Instance.GetActiveCharacters();
        for (int i = characterList.Count - 1; i >= 0; i--)
        {
            characterList[i].ApplyEndOfTurnEffectChanges();
        }
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