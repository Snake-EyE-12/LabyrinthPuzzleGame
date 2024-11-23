using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceDisplay : Display<XPBar>
{
    [SerializeField] private ExperienceNode frontPrefab;
    [SerializeField] private ExperienceNode midPrefab;
    [SerializeField] private ExperienceNode endPrefab;
    [SerializeField] private ExperienceNode soloPrefab;
    private List<ExperienceNode> nodes = new List<ExperienceNode>();
    public override void Render()
    {
        Clear();
        if (item.max <= 0) return;
        if (item.max == 1)
        {
            ExperienceNode node = Instantiate(soloPrefab, transform);
            nodes.Add(node);
        }
        if (item.max >= 2)
        {
            ExperienceNode node = Instantiate(frontPrefab, transform);
            nodes.Add(node);
        }
        if (item.max >= 3)
        {
            for(int i = 0; i < item.max - 2; i++)
            {
                ExperienceNode node = Instantiate(midPrefab, transform);
                nodes.Add(node);
            }
        }
        if (item.max >= 2)
        {
            ExperienceNode node = Instantiate(endPrefab, transform);
            nodes.Add(node);
        }

        SetVisibility();
    }
    private void SetVisibility()
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            nodes[i].IsDisplayed(i < item.value);
        }
    }

    
    private void Clear()
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            Destroy(nodes[i].gameObject);
        }
        nodes.Clear();
    }
}

public class XPBar
{
    public XPBar(int val)
    {
        value = 0;
        max = val;
    }
    public int value;
    public int max;
    public int GetIntPercentage()
    {
        return (value * 100) / max;
    }
}