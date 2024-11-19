using System.Collections.Generic;
using Guymon.Components;
using Guymon.DesignPatterns;
using UnityEngine;

public class AttackIndicator : Singleton<AttackIndicator>
{
    private List<AttackBoxDisplay> visuals = new();
    private List<EnemyAttackVisual> activeAttacks = new List<EnemyAttackVisual>();
    [SerializeField] private AttackBoxDisplay attackBoxDisplayPrefab; 
    public void AddAttack(EnemyAttackVisual attack)
    {
        activeAttacks.Add(attack);
    }

    public void RemoveAttack(EnemyAttackVisual attack)
    {
        activeAttacks.Remove(attack);
        Recalculate();
    }

    public void Recalculate()
    {
        grid.Clear();
        VisualizeAttacks();
    }

    public void ClearAttacks()
    {
        activeAttacks.Clear();
        grid.Clear();
        ClearVisuals();
    }

    private void ClearVisuals()
    {
        foreach(var v in visuals)
        {
            Destroy(v.gameObject);
        }
        visuals.Clear();
    }

    public void VisualizeAttacks()
    {
        ClearVisuals();
        foreach (var eav in activeAttacks)
        {
            AddAttackToGrid(eav);
        }

        foreach (var key in grid.Keys)
        {
            SpawnBox(grid[key], key);
        }
        
    }

    private Dictionary<Vector2Int, List<EnemyAttackVisual>> grid = new();
    private void AddAttackToGrid(EnemyAttackVisual visualData)
    {
        foreach (var position in visualData.attack.GetActiveShape(visualData.user.GetGridPosition()))
        {
            if(grid.ContainsKey(position)) grid[position].Add(visualData);
            else grid[position] = new List<EnemyAttackVisual>(){visualData};
        }
    }

    private void SpawnBox(List<EnemyAttackVisual> eavList, Vector2Int pos)
    {
        AttackBoxDisplay abd = Instantiate(attackBoxDisplayPrefab, transform);
        abd.transform.position = VisualDataHolder.Instance.CoordsToPosition(pos);
        abd.Set(eavList);
        visuals.Add(abd);
    }

    public void UseSpecificEnemyAttack(EnemyDisplay earlyAttacker)
    {
        foreach (var a in activeAttacks)
        {
            if (a.user == earlyAttacker)
            {
                a.attack.Use(a.user);
                activeAttacks.Remove(a);
                Recalculate();
                return;
            }
        }
    }

    public void ExecuteAttacks()
    {
        foreach (var a in activeAttacks)
        {
            a.attack.Use(a.user);
            cameraShaker.Shake(shakePerEnemy);
        }
        ClearAttacks();
    }

    [SerializeField] private float shakePerEnemy;
    [SerializeField] private Shake2D cameraShaker;
}

public struct EnemyAttackVisual
{
    public Attack attack;
    public EnemyDisplay user;

}