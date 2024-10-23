using System.Collections.Generic;
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
        VisualizeAttacks();
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
        foreach (var a in activeAttacks)
        {
            AddAttackToGrid(a.attack, a.attack.GetActiveShape(a.user.GetGridPosition()));
        }

        foreach (var key in grid.Keys)
        {
            SpawnBox(grid[key], key);
        }
        
    }

    private Dictionary<Vector2Int, List<Attack>> grid = new Dictionary<Vector2Int, List<Attack>>();
    private void AddAttackToGrid(Attack a, List<Vector2Int> pos)
    {
        foreach (var position in pos)
        {
            if(grid.ContainsKey(position)) grid[position].Add(a);
            else grid[position] = new List<Attack>(){a};
        }
    }

    private void SpawnBox(List<Attack> a, Vector2Int pos)
    {
        AttackBoxDisplay abd = Instantiate(attackBoxDisplayPrefab, transform);
        abd.transform.position = VisualDataHolder.Instance.CoordsToPosition(pos);
        abd.Set(GetBoxString(a));
        visuals.Add(abd);
    }

    private string GetBoxString(List<Attack> a)
    {
        string output = "";
        foreach (var attack in a)
        {
            output += attack.GetDescription() + "\n";
        }
        return output;
    }

    public void ExecuteAttacks()
    {
        foreach (var a in activeAttacks)
        {
            a.attack.Use(a.user);
        }
        ClearAttacks();
    }
}

public struct EnemyAttackVisual
{
    public Attack attack;
    public EnemyDisplay user;

}