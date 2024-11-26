using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileOnCardDisplay : Display<Tile>
{
    [SerializeField] private List<WallDisplay> wallDisplays = new List<WallDisplay>();
    [SerializeField] private Image abilityIcon;
    [SerializeField] private GameObject fire;
    [SerializeField] private bool canShowRotateTutorial;

    public override void Render()
    {
        int orientation = item.GetOrientation();
        for (int i = 0, j = 1; i < 4; i++, j *= 2)
        {
            wallDisplays[i].SetVisibility((orientation & j) == 0);
        }

        if (item.ability != null && !item.ability.usedThisCombat)
        {
            abilityIcon.sprite = Ability.GetAbilityIcon(item.ability);
            abilityIcon.gameObject.SetActive(true);
        }
        else
        {
            abilityIcon.gameObject.SetActive(false);
        }
        fire.SetActive(item.type != "Slide");
    }

    public void TryShowRotateTutorial()
    {
        if (!canShowRotateTutorial) return;
        int orientation = item.GetOrientation();
        bool lastVisible = false;
        for (int i = 0, j = 1; i < 4; i++, j *= 2)
        {
            bool visible = (orientation & j) == 0;
            if(i != 0 && visible != lastVisible) TutorialManager.Instance.Teach("CardRotation");
            lastVisible = visible;
        }
    }
}
