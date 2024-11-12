using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthbarDisplay : Display<HealthBarHealthAndEffectsData>
{
    [SerializeField] private Heart heartPrefab;
    private List<Heart> hearts = new List<Heart>();

    public override void Render()
    {
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
    public HealthBarHealthAndEffectsData(Health h, int p, int burn, int bl)
    {
        health = h;
        poisonCount = p;
        burnCount = burn;
        bleedCount = bl;
    }
    public Health health;
    public int poisonCount;
    public int burnCount;
    public int bleedCount;
}