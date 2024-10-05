using Guymon.DesignPatterns;

public abstract class RoundPhase
{
    public virtual void StartPhase() { SkipPhase(); }
    public virtual void UpdatePhase() { }
    public virtual void EndPhase() { }
    protected void SkipPhase() { TurnManager.Instance.NextPhase();}
}


public class OpponentTurnPhase : RoundPhase
{
    
}
public class DrawCardsPhase : RoundPhase
{
    public override void StartPhase()
    {
        EventHandler.AddListener("DrawCards/LimitReached", OnLimitReached);
        EventHandler.Invoke("Phase/DrawCards", null);
    }

    public void OnLimitReached(EventArgs args)
    {
        TurnManager.Instance.NextPhase();
    }

    public override void EndPhase()
    {
        EventHandler.RemoveListenerLate("DrawCards/LimitReached", OnLimitReached);
    }
}
public class PlayCardsPhase : RoundPhase
{
    public override void StartPhase()
    {
        
    }
}
public class TeamTurnPhase : RoundPhase
{
    
}
public class DamagePhase : RoundPhase
{
    
}
public class CompletionPhase : RoundPhase
{
    
}