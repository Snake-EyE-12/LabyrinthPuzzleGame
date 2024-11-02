using Capstone.DataLoad;
using UnityEngine;

public class Item
{
    public string name;
    public string description;
    public int degree;
    public int price;
    public Item(ItemData data)
    {
        name = data.Name;
        description = data.Description;
        degree = data.Degree;
        price = Random.Range(data.Price.Min, data.Price.Max + 1);
        
    }
    public static ItemData Load(int degree)
    {
        return DataHolder.availableItems.RandomByDegree(degree);
    }
}