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
    [SerializeField] private Floater burnAfterImage;
    [SerializeField] private GameObject button;
    public override void Render()
    {
        button.SetActive(item is not FightEvent);
        Color color = DataHolder.eventColorEquivalenceTable.GetColor(item.data.Type);
        displayText.text = $"<color=#{ColorUtility.ToHtmlStringRGB(color)}FF>{item.data.Type}</color> ";
        iconImage.sprite = Resources.Load<Sprite>("KeynamedSprites/EventIcons/" + item.data.Type);
        iconImage.color = color;
    }

    public void OnClick()
    {
        if (item.OnClick(this))
        {
            Destroy(gameObject);
        }
    }

    public void Burn(float percent)
    {
        Floater e = Instantiate(burnAfterImage, GameManager.Instance.GetCanvasParent());
        e.Set(percent);
        e.transform.position = transform.position;
        iconImage.color = new Color(iconImage.color.r, iconImage.color.g, iconImage.color.b, percent);
        DataHolder.challengesTaken++;
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
    public abstract bool OnClick(EventAcceptorButtonDisplay button);
    protected virtual bool IsOver() {return true;}
}

public class FightEvent : EventAcceptor
{
    public int GetExtras()
    {
        return additionalEnemyCount;
    }
    private int additionalEnemyCount = 0;
    public void UpTheAnte(int value)
    {
        additionalEnemyCount++;
    }
    public FightEvent(EventData data, RoundMenuDisplay displayBox) : base(data, displayBox)
    {
    }

    public override bool OnClick(EventAcceptorButtonDisplay button)
    {
        EffectHolder.Instance.SpawnEffect("FightBurn", button.transform);
        return IsOver();
    }

    protected override bool IsOver()
    {
        return false;
    }
}
public class ShopEvent : EventAcceptor
{
    public ShopEvent(EventData data, RoundMenuDisplay displayBox) : base(data, displayBox)
    {
    }

    public override bool OnClick(EventAcceptorButtonDisplay button)
    {
        AudioManager.Instance.Play("ButtonClick");
        GameManager.Instance.ShowShop();
        return IsOver();
    }

    protected override bool IsOver()
    {
        return false;
    }
}

public class ChallengeEvent : EventAcceptor
{
    public ChallengeEvent(EventData data, RoundMenuDisplay displayBox) : base(data, displayBox)
    {
    }

    private int clickCount = 0;
    public override bool OnClick(EventAcceptorButtonDisplay button)
    {
        AudioManager.Instance.Play("ButtonClick");
        AudioManager.Instance.Play("Chomp");
        clickCount++;
        button.Burn(1 - (clickCount * 1.0f / DataHolder.currentMode.MaxChallengeAdditions));
        displayBox.UpTheAnte();
        return IsOver();
    }

    protected override bool IsOver()
    {
        return (clickCount >= DataHolder.currentMode.MaxChallengeAdditions);
    }
}