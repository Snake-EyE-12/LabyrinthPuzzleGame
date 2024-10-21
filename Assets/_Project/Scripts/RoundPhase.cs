using Guymon.DesignPatterns;

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
}


public class OpponentTurnPhase : RoundPhase
{
    public OpponentTurnPhase(TurnManager tm) : base(tm)
    {
    }
}
public class DrawCardsPhase : RoundPhase
{
    public DrawCardsPhase(TurnManager tm) : base(tm)
    {
    }

    public override void StartPhase()
    {
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

    private int amountOfCardsPlaced = 0;

    public override void StartPhase()
    {
        GameManager.Instance.SetSelectionMode(SelectableGroupType.Card);
        amountOfCardsPlaced = 0;
        EventHandler.AddListener("CardPlaced", CardPlaced);
    }

    private void CardPlaced(EventArgs args)
    {
        amountOfCardsPlaced++;
        if (amountOfCardsPlaced >= 2)
        {
            tm.NextPhase();
            return;
        }
        GameManager.Instance.SetSelectionMode(SelectableGroupType.Card);
    }

    public override void EndPhase()
    {
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
        EventHandler.AddListenerLate("Round/EndTurn", EndTurn);
        GameManager.Instance.PrepareTeamTurnStart();
    }

    private void EndTurn(EventArgs args)
    {
        tm.NextPhase();
    }

    public override void EndPhase()
    {
        EventHandler.RemoveListenerLate("Round/EndTurn", EndTurn);
    }
}
public class DamagePhase : RoundPhase
{
    public DamagePhase(TurnManager tm) : base(tm)
    {
    }
}
public class CompletionPhase : RoundPhase
{
    public CompletionPhase(TurnManager tm) : base(tm)
    {
    }

    public override void StartPhase()
    {
        GameManager.Instance.CheckForGameOver();
    }
}