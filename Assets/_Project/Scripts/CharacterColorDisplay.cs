using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterColorDisplay : Display<CharacterColorData>
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private Image image;
    public override void Render(CharacterColorData item)
    {
        text.text = item.type;
        image.color = item.color;
    }
}