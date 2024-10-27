using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterColorDisplay : Display<CharacterColorData>
{
    [SerializeField] private Image colorRing;
    [SerializeField] private Image characterIconImage;
    public override void Render()
    {
        colorRing.color = item.color;
        characterIconImage.sprite = Resources.Load<Sprite>("KeynamedSprites/CharacterIcons/" + item.type);
    }
}