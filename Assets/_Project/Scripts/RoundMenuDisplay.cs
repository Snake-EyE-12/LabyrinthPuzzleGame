using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class RoundMenuDisplay : Display<EventsForRound>
{
    [SerializeField] private EventAcceptorButtonDisplay eventAcceptorButtonDisplayPrefab;
    [SerializeField] private Transform buttonParent;
    public override void Render()
    {
        foreach (var e in item.events)
        {
            if(e.Type.Equals("Fight")) fightExists = true;
            Instantiate(eventAcceptorButtonDisplayPrefab, buttonParent).Set(e);
        }
    }

    private bool fightExists;
    public void ContinueClick()
    {
        if (buttonParent.childCount > 0) return;
        if (fightExists) GameManager.Instance.BeginBoardFight();
        else GameManager.Instance.ContinueMission();
        Destroy(gameObject);
    }
}
