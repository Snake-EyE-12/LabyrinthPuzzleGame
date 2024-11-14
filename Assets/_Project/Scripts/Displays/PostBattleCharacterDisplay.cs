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
    [SerializeField] private Transform cardParent;
    [SerializeField] private TileOnCardDisplay cardDisplayPrefab;
    [SerializeField] private GameObject upgradeButton;
    public override void Render()
    {
        RemoveExtras();
        Color color = DataHolder.characterColorEquivalenceTable.GetColor(item.characterType);
        colorRing.color = color;
        nameText.text = $"<color=#{ColorUtility.ToHtmlStringRGB(color)}FF>{item.unitName}</color> ";
        characterImage.sprite = Resources.Load<Sprite>("KeynamedSprites/Faces/Heros/" + item.unitName);
        healthBar.Set(new HealthBarHealthAndEffectsData(item.health, item.ActiveEffectsList));
        xpBar.Set(item.XP);
        foreach (var a in item.abilityList)
        {
            PostBattleAbilityDisplay pbad = Instantiate(abilityDisplayPrefab, abilityParent);
            pbad.Set(a);
            abilities.Add(pbad);
        }

        foreach (var card in item.inventory.GetCards())
        {
            TileOnCardDisplay tocd = Instantiate(cardDisplayPrefab, cardParent);
            tocd.Set(card.GetTile());
            cards.Add(tocd);
        }
        upgradeButton.SetActive(item.XP.value >= item.XP.max);
    }

    private List<TileOnCardDisplay> cards = new();
    private List<PostBattleAbilityDisplay> abilities = new();

    public Character GetCharacter()
    {
        return item;
    }
    private void RemoveExtras()
    {
        foreach (var card in cards)
        {
            Destroy(card.gameObject);
        }
        cards.Clear();
        foreach (var ability in abilities)
        {
            Destroy(ability.gameObject);
        }
        abilities.Clear();
    }
    public void Upgrade()
    {
        Set(GameManager.Instance.UpgradeCharacter(item));
    }
}
