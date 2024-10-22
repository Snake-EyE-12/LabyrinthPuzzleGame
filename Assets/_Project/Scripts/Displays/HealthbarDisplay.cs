using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthbarDisplay : Display<Health>
{
    [SerializeField] private Heart heartPrefab;
    private List<Heart> hearts = new List<Heart>();

    public override void Render()
    {
        int heartIndex = 0;
        foreach (var hpSection in item.GetHealth())
        {
            for (int i = 0; i < hpSection.value; i++, heartIndex++)
            {
                if (heartIndex >= hearts.Count)
                {
                    hearts.Add(Instantiate(heartPrefab, transform));
                }
                hearts[heartIndex].SetColor(hpSection.GetColor());
            }
        }
        for(int i = hearts.Count - 1; i >= heartIndex; i--)
        {
            Destroy(hearts[i].gameObject);
        }

        if(hearts.Count > 0) hearts.RemoveRange(heartIndex, (hearts.Count - heartIndex));
    }
}
