using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PostBattleCharacterDisplay : Display<Character>
{
    [SerializeField] private Image colorRing;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private Image characterImage;
    [SerializeField] private HealthbarDisplay healthBar;
    [SerializeField] private ExperienceDisplay xpBar;
    [SerializeField] private Transform abilityParent;
    [SerializeField] private PostBattleAbilityDisplay abilityDisplayPrefab;
    public override void Render()
    {
        Color color = DataHolder.characterColorEquivalenceTable.GetColor(item.characterType);
        colorRing.color = color;
        nameText.text = $"<color=#{ColorUtility.ToHtmlStringRGB(color)}FF>{item.unitName}</color> ";
        characterImage.sprite = Resources.Load<Sprite>("KeynamedSprites/Faces/" + item.unitName);
        healthBar.Set(item.health);
        xpBar.Set(item.XP);
        foreach (var a in item.abilityList)
        {
            Instantiate(abilityDisplayPrefab, abilityParent).Set(a);
        }
    }
}
