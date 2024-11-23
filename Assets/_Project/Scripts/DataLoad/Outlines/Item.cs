using System.Collections.Generic;
using System.Text.RegularExpressions;
using Capstone.DataLoad;
using Manipulations;
using UnityEngine;

public class Item
{
    public string name;
    public string description;
    public int degree;
    public int price;
    private List<Manipulation> manipulations = new List<Manipulation>();

    public void ApplyTo(Character character)
    {
        foreach (var manipulation in manipulations)
        {
            manipulation.Apply(character);
        }
    }
    public Item(ItemData data)
    {
        name = data.Name;
        description = data.Description;
        degree = data.Degree;
        price = Random.Range(data.Price.Min, data.Price.Max + 1);
        foreach (var manipulation in data.Manipulations)
        {
            manipulations.Add(Manipulation.Load(manipulation));
        }
        
    }
    public static ItemData Load(int degree)
    {
        return DataHolder.availableItems.RandomByDegree(degree);
    }
}




