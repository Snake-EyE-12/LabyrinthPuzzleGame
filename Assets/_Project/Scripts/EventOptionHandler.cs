using System;
using System.Collections;
using System.Collections.Generic;
using Capstone.DataLoad;
using UnityEngine;
using Random = UnityEngine.Random;

public class EventOptionHandler : MonoBehaviour
{
    private void Awake()
    {
        SetUpEventOptions();
    }

    private void SetUpEventOptions()
    {
        EventLayout[] allPossibleGameEvents = DataHolder.currentMode.EventLayout;
        DataHolder.EventsForEachRound = BuildEvents(GatherEvents(allPossibleGameEvents));

    }

    private List<ChosenEvent> GatherEvents(EventLayout[] layoutArray)
    {
        List<ChosenEvent> guaranteedEvents = new List<ChosenEvent>();
        foreach (var eLayout in layoutArray)
        {
            if (GameUtils.PercentChance(eLayout.IncludeChance))
            {
                guaranteedEvents.Add(PickEvent(eLayout));
            }
        }

        return guaranteedEvents;
    }

    private ChosenEvent PickEvent(EventLayout eLayout)
    {
        return new ChosenEvent(eLayout.Events[GameUtils.IndexByWeightedRandom(new List<Weighted>(eLayout.Events))], eLayout);
    }
    
    private List<EventsForRound> BuildEvents(List<ChosenEvent> chosenEvents)
    {
        List<EventsForRound> rEvents = new List<EventsForRound>();
        for (int i = 0; i < DataHolder.currentMode.Rounds; i++)
        {
            EventsForRound eventsForIRound = new EventsForRound();
            foreach (var cEvent in chosenEvents)
            {
                if(cEvent.eLayout.Sequence.alignsAt(i + 1)) eventsForIRound.events.Add(cEvent.chosenEvent);
            }
            rEvents.Add(eventsForIRound);
        }

        return rEvents;
    }
    
}


public class ChosenEvent
{
    public ChosenEvent(EventData chosenEvent, EventLayout eLayout)
    {
        this.chosenEvent = chosenEvent;
        this.eLayout = eLayout;
    }
    public EventData chosenEvent;
    public EventLayout eLayout;
}
public class EventsForRound
{
    public List<EventData> events = new List<EventData>();
}
