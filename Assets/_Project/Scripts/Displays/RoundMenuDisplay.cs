using System.Collections;
using System.Collections.Generic;
using Capstone.DataLoad;
using UnityEngine;
using UnityEngine.Serialization;

public class RoundMenuDisplay : Display<EventsForRound>
{
    [SerializeField] private EventAcceptorButtonDisplay eventAcceptorButtonDisplayPrefab;
    [SerializeField] private Transform buttonParent;
    
    
    
    private bool fightExists;
    private bool challengeExists;
    private bool shopExists;
    public override void Render()
    {
        foreach (var e in item.events)
        {
            EventAcceptor ea = CreateEventAcceptor(e);
            if(ea != null) Instantiate(eventAcceptorButtonDisplayPrefab, buttonParent).Set(ea);
        }
    }

    private EventAcceptor CreateEventAcceptor(EventData e)
    {
        switch (e.Type)
        {
            case "Fight":
                if (fightExists) return null;
                fightExists = true;
                return new FightEvent(e, this);
            case "Challenge":
                if (challengeExists) return null;
                challengeExists = true;
                return new ChallengeEvent(e, this);
            case "Shop":
                if (shopExists) return null;
                shopExists = true;
                return new ShopEvent(e, this);
            default:
                return null;
        }
    }

    public void ContinueClick()
    {
        if (buttonParent.childCount > 0) return;
        if (fightExists) GameManager.Instance.BeginBoardFight();
        else GameManager.Instance.ContinueMission();
        Destroy(gameObject);
    }
}
