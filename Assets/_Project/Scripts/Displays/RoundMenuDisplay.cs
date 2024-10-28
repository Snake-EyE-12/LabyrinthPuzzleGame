using System.Collections;
using System.Collections.Generic;
using Capstone.DataLoad;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class RoundMenuDisplay : Display<EventsForRound>
{
    [SerializeField] private EventAcceptorButtonDisplay eventAcceptorButtonDisplayPrefab;
    [SerializeField] private Transform buttonParent;
    [SerializeField] private Button arrowButton;
    
    
    
    private FightEvent fightExists;
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
                if (fightExists != null) return null;
                fightExists = new FightEvent(e, this);
                arrowButton.gameObject.SetActive(false);
                return fightExists;
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
        if(fightExists == null) GameManager.Instance.ContinueMission();
        Destroy(gameObject);
    }

    public void ActivateButton()
    {
        arrowButton.gameObject.SetActive(true);
    }
}
