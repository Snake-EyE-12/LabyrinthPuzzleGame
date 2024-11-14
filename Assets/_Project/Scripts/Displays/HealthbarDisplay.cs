using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthbarDisplay : Display<HealthBarHealthAndEffectsData>
{
    [SerializeField] private Heart heartPrefab;
    [SerializeField] private GameObject iceBox;
    private List<Heart> hearts = new List<Heart>();
    [SerializeField] private GameObject shieldObject;
    [SerializeField] private GameObject immunityObject;
    [SerializeField] private TMP_Text shieldAmountTextbox;

    public override void Render()
    {
        iceBox.SetActive(item.frozen);
        shieldObject.SetActive(item.shield > 0);
        immunityObject.SetActive(item.immune);
        shieldAmountTextbox.text = item.shield.ToString();
        int heartIndex = 0;
        foreach (var hpSection in item.health.GetHealthBarSegments())
        {
            for (int i = 0; i < hpSection.value; i++, heartIndex++)
            {
                if (heartIndex >= hearts.Count)
                {
                    hearts.Add(Instantiate(heartPrefab, transform));
                }
                hearts[heartIndex].Fill(GetHeartData(hpSection.GetColor(), heartIndex, item.health.GetHealthValue()));
            }
        }

        for (int i = heartIndex; i < item.health.GetMaxHealthValue(); i++, heartIndex++)
        {
            if (heartIndex >= hearts.Count)
            {
                hearts.Add(Instantiate(heartPrefab, transform));
            }
            hearts[heartIndex].Empty();
        }
        for(int i = hearts.Count - 1; i >= heartIndex; i--)
        {
            Destroy(hearts[i].gameObject);
        }

        if(hearts.Count > 0) hearts.RemoveRange(heartIndex, (hearts.Count - heartIndex));
    }

    private HeartState GetHeartData(Color okColor, int positionInBar, int total)
    {
        return new HeartState((total - item.poisonCount <= positionInBar), (total - item.bleedCount <= positionInBar),
            (total - item.burnCount <= positionInBar), okColor);
    }
}

public class HealthBarHealthAndEffectsData
{
    public HealthBarHealthAndEffectsData(Health h, ActiveEffectList effects)
    {
        health = h;
        int[] dots = effects.GetDOTArray();
        poisonCount = dots[0];
        burnCount = dots[1];
        bleedCount = dots[2];
        FreezeActiveEffect fae = effects.GetEffect<FreezeActiveEffect>();
        frozen = fae != null && fae.value > 0;
        ShieldActiveEffect sae = effects.GetEffect<ShieldActiveEffect>();
        shield = sae != null ? sae.value : 0;
        ImmunityActiveEffect iae = effects.GetEffect<ImmunityActiveEffect>();
        immune = iae != null && iae.value > 0;
    }
    public Health health;
    public int poisonCount;
    public int burnCount;
    public int bleedCount;
    public bool frozen;
    public bool immune;
    public int shield;
}