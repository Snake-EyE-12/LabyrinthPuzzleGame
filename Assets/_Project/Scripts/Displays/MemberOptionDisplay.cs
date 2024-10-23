using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MemberOptionDisplay : Display<CharacterColorData>
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private Image image;
    public override void Render()
    {
        text.text = item.type;
        image.color = item.color;
    }
    
    
    private TeamBuilderHandler tbh;
    public void SetHandler(TeamBuilderHandler handler)
    {
        tbh = handler;
    }
    public void OnClick()
    {
        tbh.Pick(item.type, this.transform);
    }

}