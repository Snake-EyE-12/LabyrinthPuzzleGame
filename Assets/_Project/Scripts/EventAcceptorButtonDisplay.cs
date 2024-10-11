using System.Collections;
using System.Collections.Generic;
using Capstone.DataLoad;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EventAcceptorButtonDisplay : Display<EventData>
{
    [SerializeField] private TMP_Text btnText;
    [SerializeField] private Button btn;
    public override void Render(EventData item)
    {
        btnText.text = item.Type;
        btn.onClick.AddListener(OnClick);
    }

    public void OnClick()
    {
        GameManager.Instance.PrepareEvent(item);
        Destroy(gameObject);
    }
}
