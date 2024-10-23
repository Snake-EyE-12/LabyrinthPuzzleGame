using TMPro;
using UnityEngine;

public class AttackBoxDisplay : Display<string>
{
    [SerializeField] private TMP_Text text;
    public override void Render()
    {
        text.text = item;
    }
}