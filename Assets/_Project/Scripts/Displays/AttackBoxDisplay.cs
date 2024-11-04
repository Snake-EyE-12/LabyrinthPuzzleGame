using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class AttackBoxDisplay : Display<List<EnemyAttackVisual>>
{
    [SerializeField] private AttackBoxHitPointDisplay[] hitPoints = new AttackBoxHitPointDisplay[20];
    [SerializeField] private GameObject[] corners = new GameObject[3];
    [SerializeField] private List<AttackLineDisplay> activeLines = new();
    [SerializeField] private AttackLineDisplay lineToOwnerPrefab;
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
                GetHitPointAttack(i),
                GetHitPointSeparator(i),
                total
                ));
        }
    }

    private HitPointState GetHitPointState(int index)
    {
        if (index == total) return HitPointState.Empty;
        if (index == total + 1) return HitPointState.End;
        if(index + 1 > total) return HitPointState.Off;
        return HitPointState.Full;
    }

    private Attack GetHitPointAttack(int index)
    {
        int difference = index;
        for(int i = 0; i < attackSegments.Count; i++)
        {
            if (difference < attackSegments[i].GetAbilityValue())
            {
                return attackSegments[i];
            }
            difference -= attackSegments[i].GetAbilityValue();
        }

        return null;
    }

    private bool GetHitPointSeparator(int index)
    {
        int counter = 0;
        foreach (var a in attackSegments)
        {
            counter += a.GetAbilityValue();
            if (counter == index + 1) return true;
        }
        return false;
    }

    private void RenderCorners()
    {
        // for (int i = 0; i < corners.Length; i++)
        // {
        //     corners[i].SetActive(total + 1 / 5 <= i);
        // }
        corners[0].SetActive(total <= 3);
        corners[1].SetActive(total <= 8);
        corners[2].SetActive(total <= 13);
    }
    
    private void CreateLines()
    {
        List<EnemyDisplay> uniqueEnemies = item.Select(x => x.user).Distinct().ToList();
        for(int i = 0; i < uniqueEnemies.Count; i++)
        {
            AttackLineDisplay lineToOwner = Instantiate(lineToOwnerPrefab, transform);
            lineToOwner.Set(new LineRendererData(){follower = uniqueEnemies[i].transform, start = transform.position + new Vector3(0, 0.8f, 0)});
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
    public HitPointData(HitPointState state, Attack attack, bool separator, int total)
    {
        this.state = state;
        this.Attack = attack;
        this.isSeparator = separator;
        this.total = total;
    }
    public HitPointState state;
    public Attack Attack;
    public bool isSeparator;
    public int total;
}

public enum HitPointState
{
    Full,
    End,
    Empty,
    Off
}