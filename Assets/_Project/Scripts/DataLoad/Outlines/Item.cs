using System.Collections.Generic;
using Capstone.DataLoad;
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

public abstract class Manipulation
{
    public Manipulation(ManipulationData data)
    {
    }

    public abstract void Apply(Character character);

    public static Manipulation Load(ManipulationData data)
    {
        switch (data.Change)
        {
            case "Health":
                return new HealthManipulation(data);
            case "XP":
                return new XPManipulation(data);
            case "Card":
                return new CardManipulation(data);
        }

        return null;
    }
}

public class HealthManipulation : Manipulation
{
    public HealthManipulation(ManipulationData data) : base(data)
    {
        
    }

    public override void Apply(Character character)
    {
        throw new System.NotImplementedException();
    }
}
public class XPManipulation : Manipulation
{
    public XPManipulation(ManipulationData data) : base(data)
    {
        
    }

    public override void Apply(Character character)
    {
        throw new System.NotImplementedException();
    }
}
public class CardManipulation : Manipulation
{
    private Card modifiedCard;
    private Placement placer;
    private Condition condition;
    public CardManipulation(ManipulationData data) : base(data)
    {
        modifiedCard = Card.Load(data.With);
        switch (data.Modification)
        {
            case "Append":
                placer = new AppendPlacement();
                break;
            case "Replace":
                placer = new ReplacePlacement();
                break;
            case "Delete":
                placer = new DeletePlacement();
                break;
        }

        switch (data.Condition)
        {
            case "None":
                condition = new NoCondition();
                break;
        }
    }

    public override void Apply(Character character)
    {
        int cardCount = character.inventory.GetCards().Count;
        List<int> indeces = placer.GetAllIndecesToPlace(cardCount);
        foreach (var index in indeces)
        {
            if (index >= cardCount) character.AddInventoryCardSpaceUpTo(index);
            if (condition.Evaluate()) SetCardAt(character, index);
        }
    }

    private void SetCardAt(Character character, int index)
    {
        modifiedCard.owner = character.characterType;
        character.inventory.GetCards()[index] = modifiedCard;
    }
}


public abstract class Placement
{
    public abstract List<int> GetAllIndecesToPlace(int count);
}
public class AppendPlacement : Placement
{
    public override List<int> GetAllIndecesToPlace(int count)
    {
        List<int> indeces = new List<int>();
        indeces.Add(count);
        return indeces;
    }
}
public class ReplacePlacement : Placement
{
    public override List<int> GetAllIndecesToPlace(int count)
    {
        throw new System.NotImplementedException();
    }
}

public class DeletePlacement : Placement
{
    public override List<int> GetAllIndecesToPlace(int count)
    {
        throw new System.NotImplementedException();
    }
}

public abstract class ConditionData { }
public abstract class Condition
{
    public abstract bool Evaluate(ConditionData data = null);
}
public class NoCondition : Condition
{
    public override bool Evaluate(ConditionData data = null)
    {
        return true;
    }
}