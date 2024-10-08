using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MemberOptionDisplay : Display<CharacterColorData>
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private Image image;
    public override void Render(CharacterColorData item)
    {
        
    }
}