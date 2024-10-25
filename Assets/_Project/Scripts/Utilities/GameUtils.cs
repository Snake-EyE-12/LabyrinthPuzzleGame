using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public static class GameUtils
{
    public static int ModPositive(int val, int size) => (val % size + size) % size;
    
    public static int IndexByWeightedRandom(List<Weighted> list)
    {
        int totalWeight = 0;

        // Calculate total weight, skipping items with weight <= 0
        foreach (var item in list)
        {
            totalWeight += item.GetWeight();
        }

        // If total weight is 0, return an invalid index or handle it accordingly
        if (totalWeight <= 0)
        {
            return Random.Range(0, list.Count);
        }

        int random = Random.Range(0, totalWeight);
        int currentWeight = 0;

        // Select an index based on the weighted random selection
        for (int i = 0; i < list.Count; i++)
        {
            int weight = list[i].GetWeight();
            if (weight <= 0)
            {
                continue; // Skip items with weight <= 0
            }

            currentWeight += weight;
            if (currentWeight > random)
            {
                return i;
            }
        }

        // If for some reason no valid index is found, return a fallback
        return Random.Range(0, list.Count); // Shouldn't happen if totalWeight > 0
    }
    public static Vector2Int DirectionToVector(CardinalDirection direction)
    {
        switch (direction)
        {
            case CardinalDirection.North:
                return Vector2Int.up;
            case CardinalDirection.East:
                return Vector2Int.right;
            case CardinalDirection.South:
                return Vector2Int.down;
            case CardinalDirection.West:
                return Vector2Int.left;
            default:
                return Vector2Int.zero;
        }
    }
    public static bool PercentChance(int percent)
    {
        return PercentChance(percent, 100);
    }
    public static bool PercentChance(int value, int max)
    {
        return Random.Range(0, max) < value;
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

    public static Vector2Int Normalize(this Vector2Int vector)
    {
        Vector2Int absVec = new Vector2Int(Math.Abs(vector.x), Math.Abs(vector.y));
        if (absVec.x == absVec.y)
        {
            // Set X xor Y Value To 0
            if (Random.Range(0, 2) == 0) vector = new Vector2Int(0, vector.y);
            else vector = new Vector2Int(vector.x, 0);
        }
        int max = Math.Max(absVec.x, absVec.y);
        if (max == 0) return Vector2Int.zero;
        return new Vector2Int(vector.x / max, vector.y / max);
    }

    public static Vector2Int GetRandomDirection()
    {
        switch (Random.Range(0, 4))
        {
            case 0:
                return Vector2Int.up;
            case 1:
                return Vector2Int.right;
            case 2:
                return Vector2Int.down;
            default:
                return Vector2Int.left;
        
        }
    }
}

public interface Weighted
{
    public int GetWeight();

}