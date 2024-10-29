
using TMPro;
using UnityEngine;

public class PostBattleAbilityDisplay : Display<Ability>
{
    [SerializeField] private TMP_Text textbox;
    public override void Render()
    {
        var descriptionBuilder = item.value + " ";
        foreach (var vk in item.keys)
        {
            descriptionBuilder += vk.GetKeywordName().ConvertToString() + ", ";
        }
        textbox.text = descriptionBuilder.Substring(0, descriptionBuilder.Length - 2);
    }
}
