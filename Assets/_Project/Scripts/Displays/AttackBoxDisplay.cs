using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class AttackBoxDisplay : Display<List<EnemyAttackVisual>>
{
    [SerializeField] private AttackBoxHitPointDisplay[] hitPoints = new AttackBoxHitPointDisplay[20];
    [SerializeField] private GameObject[] corners = new GameObject[3];
    [SerializeField] private List<LineRenderer> activeLines = new();
    [SerializeField] private LineRenderer lineToOwnerPrefab;
    public override void Render()
    {
        total = 0;
        attackSegments.Clear();
        foreach (var attack in item)
        {
            attackSegments.Add(attack.attack);
            total += attack.attack.GetAbilityValue();;
        }
        CreateLines();
        RenderHitPoints();
        RenderCorners();
    }

    private int total = 0;
    private List<Attack> attackSegments = new();

    private void RenderHitPoints()
    {
        for (int i = 0; i < hitPoints.Length; i++)
        {
            hitPoints[i].Set(new HitPointData(
                GetHitPointState(i),
                GetHitPointColor(),
                GetHitPointSeparator(),
                total
                ));
        }
    }

    private HitPointState GetHitPointState(int index)
    {
        if (index == total) return HitPointState.Empty;
        if (index == total + 1) return HitPointState.End;
        return HitPointState.Full;
    }

    private Color GetHitPointColor()
    {
        return Color.white; //TODO:
    }

    private bool GetHitPointSeparator()
    {
        return false; //TODO:
    }

    private void RenderCorners()
    {
        for (int i = 0; i < corners.Length; i++)
        {
            corners[i].SetActive(total + 1 / 5 <= i);
        }
    }
    
    private void CreateLines()
    {
        List<EnemyDisplay> uniqueEnemies = item.Select(x => x.user).Distinct().ToList();
        for(int i = 0; i < uniqueEnemies.Count; i++)
        {
            LineRenderer lineToOwner = Instantiate(lineToOwnerPrefab, transform);
            Vector3[] positions = new[] { uniqueEnemies[i].transform.position, transform.position };
            lineToOwner.SetPositions(positions);
            activeLines.Add(lineToOwner);
        }
    }

    public void ShowLine()
    {
        foreach (var lr in activeLines)
        {
            lr.gameObject.SetActive(true);
        }
    }

    public void HideLine()
    {
        foreach (var lr in activeLines)
        {
            lr.gameObject.SetActive(false);
        }
    }
}

public class HitPointData
{
    public HitPointData(HitPointState state, Color color, bool separator, int total)
    {
        this.state = state;
        this.color = color;
        this.isSeparator = separator;
        this.total = total;
    }
    public HitPointState state;
    public Color color;
    public bool isSeparator;
    public int total;
}

public enum HitPointState
{
    Full,
    End,
    Empty
}