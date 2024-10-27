using System.Collections;
using System.Collections.Generic;
using Capstone.DataLoad;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EventAcceptorButtonDisplay : Display<EventAcceptor>
{
    [SerializeField] private TMP_Text displayText;
    [SerializeField] private Image iconImage;
    public override void Render()
    {
        Color color = DataHolder.eventColorEquivalenceTable.GetColor(item.data.Type);
        displayText.text = $"<color=#{ColorUtility.ToHtmlStringRGB(color)}FF>{item.data.Type}</color> ";
        iconImage.sprite = Resources.Load<Sprite>("KeynamedSprites/EventIcons/" + item.data.Type);
        iconImage.color = color;
    }

    public void OnClick()
    {
        if (item.OnClick())
        {
            Destroy(gameObject);
        }
    }
}








public abstract class EventAcceptor
{
    public EventData data;
    protected RoundMenuDisplay displayBox;
    public EventAcceptor(EventData data, RoundMenuDisplay displayBox)
    {
        this.data = data;
        this.displayBox = displayBox;
    }
    public abstract bool OnClick();
    protected virtual bool IsOver() {return true;}
}

public class FightEvent : EventAcceptor
{
    public FightEvent(EventData data, RoundMenuDisplay displayBox) : base(data, displayBox)
    {
    }

    public override bool OnClick()
    {
        GameManager.Instance.LoadFight();
        return IsOver();
    }
}
public class ShopEvent : EventAcceptor
{
    public ShopEvent(EventData data, RoundMenuDisplay displayBox) : base(data, displayBox)
    {
    }

    public override bool OnClick()
    {
        throw new System.NotImplementedException();
    }
}

public class ChallengeEvent : EventAcceptor
{
    private int challengeCap = 4;
    public ChallengeEvent(EventData data, RoundMenuDisplay displayBox) : base(data, displayBox)
    {
    }

    private int clickCount = 0;
    public override bool OnClick()
    {
        clickCount++;
        return IsOver();
    }

    protected override bool IsOver()
    {
        return (clickCount >= challengeCap);
    }
}