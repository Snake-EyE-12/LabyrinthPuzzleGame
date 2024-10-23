using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileOnCardDisplay : Display<Tile>
{
    [SerializeField] private List<WallDisplay> wallDisplays = new List<WallDisplay>();
    [SerializeField] private Image abilityIcon;

    public override void Render()
    {
        int orientation = item.GetOrientation();
        for (int i = 0, j = 1; i < 4; i++, j *= 2)
        {
            wallDisplays[i].SetVisibility((orientation & j) == 0);
        }

        if (item.ability != null)
        {
            abilityIcon.sprite = Ability.GetAbilityIcon(item.ability);
            abilityIcon.gameObject.SetActive(true);
        }
        else
        {
            abilityIcon.gameObject.SetActive(false);
        }
    }
}
