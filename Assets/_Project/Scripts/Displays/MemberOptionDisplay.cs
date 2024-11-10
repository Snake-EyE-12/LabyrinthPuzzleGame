using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MemberOptionDisplay : Display<CharacterColorData>
{
    [SerializeField] private Image colorRing;
    [SerializeField] private Image characterIconImage;
    public override void Render()
    {
        colorRing.color = item.color;
        characterIconImage.sprite = Resources.Load<Sprite>("KeynamedSprites/CharacterIcons/" + item.type);
    }
    
    
    private TeamBuilderHandler tbh;
    public void SetHandler(TeamBuilderHandler handler)
    {
        tbh = handler;
    }

    private bool addedToTeam = false;
    public void OnClick()
    {
        if (addedToTeam) return;
        addedToTeam = true;
        tbh.Pick(item.type, this.transform);
        AudioManager.Instance.Play("ButtonClick");

    }

}