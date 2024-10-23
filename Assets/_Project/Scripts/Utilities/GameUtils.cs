using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameUtils
{
    public static int ModPositive(int val, int size) => (val % size + size) % size;
    
    public static int IndexByWeightedRandom(List<Weighted> list)
    {
        int totalWeight = 0;
        foreach (var item in list)
        {
            totalWeight += item.GetWeight();
        }
        int random = Random.Range(0, totalWeight);
        int currentWeight = 0;
        for (int i = 0; i < list.Count; i++)
        {
            currentWeight += list[i].GetWeight();
            if (currentWeight > random)
            {
                return i;
            }
        }
        return 0;
    }
    public static bool PercentChance(int percent)
    {
        return Random.Range(0, 100) < percent;
    }

    public static bool IsDirectionRow(CardinalDirection direction)
    {
        return direction == CardinalDirection.West ||
               direction == CardinalDirection.East;
    }
    public static bool IsDirectionPositive(CardinalDirection direction)
    {
        return direction == CardinalDirection.North ||
               direction == CardinalDirection.East;
    }
}

public interface Weighted
{
    public int GetWeight();

}