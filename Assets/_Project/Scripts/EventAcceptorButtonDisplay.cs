using System.Collections;
using System.Collections.Generic;
using Capstone.DataLoad;
using TMPro;
using UnityEngine;

public class EventAcceptorButtonDisplay : Display<EventData>
{
    [SerializeField] private TMP_Text btnText;
    public override void Render(EventData item)
    {
        btnText.text = item.Type;
    }
}
