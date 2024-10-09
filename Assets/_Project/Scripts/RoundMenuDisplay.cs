using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class RoundMenuDisplay : Display<EventsForRound>
{
    [SerializeField] private EventAcceptorButtonDisplay eventAcceptorButtonDisplayPrefab;
    [SerializeField] private Transform buttonParent;
    public override void Render(EventsForRound item)
    {
        foreach (var e in item.events)
        {
            Instantiate(eventAcceptorButtonDisplayPrefab, buttonParent).Set(e);
        }
    }
}
