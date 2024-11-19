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
    protected Condition condition;
    protected Placement placer;
    protected Modification mod;
    public Manipulation(ManipulationData data)
    {
        condition = Condition.Load(data);
        placer = Placement.Load(data);
        mod = Modification.Load(data);
    }

    protected List<int> indeces = new List<int>();
    protected void PrepareList(int limit)
    {
        indeces = placer.GetAllIndecesToPlace(limit);
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
            case "AbilityKeyword":
                return new AbilityKeywordManipulation(data);
            case "Ability":
                return new AbilityManipulation(data);
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
    private string newCard;
    
    public CardManipulation(ManipulationData data) : base(data)
    {
        newCard = data.With;
    }

    public override void Apply(Character character)
    {
        int cardCount = character.inventory.GetCards().Count;
        PrepareList(cardCount);
        foreach (var index in indeces)
        {
            if (index >= cardCount) character.AddInventoryCardSpaceUpTo(index);
            if (condition.Evaluate()) mod.Modify(newCard, new CardModificationData(character.inventory.GetCards(), index));
        }
    }
    
}

public class AbilityKeywordManipulation : Manipulation
{
    private string oldKeyword;
    private string newKeyword;
    public AbilityKeywordManipulation(ManipulationData data) : base(data)
    {
        oldKeyword = data.Condition;
        newKeyword = data.With;
    }

    public override void Apply(Character character)
    {
        int abilityCount = character.abilityList.Count;
        PrepareList(abilityCount);
        foreach (var index in indeces)
        {
            if (index >= abilityCount) character.AddAbilitySpaceUpTo(index);
            if (condition.Evaluate(new AbilityConditionData(character.abilityList[index]))) mod.Modify(newKeyword, new AbilityKeywordModificationData(oldKeyword, character.abilityList, index));
        }
    }
}

public class AbilityManipulation : Manipulation
{
    private string number;
    public AbilityManipulation(ManipulationData data) : base(data)
    {
        number = data.With;
    }

    public override void Apply(Character character)
    {
        int abilityCount = character.abilityList.Count;
        PrepareList(abilityCount);
        foreach (var index in indeces)
        {
            if (index >= abilityCount) character.AddAbilitySpaceUpTo(index);
            if (condition.Evaluate(new AbilityConditionData(character.abilityList[index]))) mod.Modify(number, new AbilityValueModificationData(character.abilityList, index));
        }
    }
}

public abstract class Placement
{
    public abstract List<int> GetAllIndecesToPlace(int max);

    public static Placement Load(ManipulationData data)
    {
        switch (data.Placement)
        {
            case "Post":
                return new PostPlacement();
            case "All":
                return new AllPlacement();
            case "First":
                return new FirstPlacement();
            default:
                return new NullPlacement();
        }
    }
}
public class PostPlacement : Placement
{
    public override List<int> GetAllIndecesToPlace(int max)
    {
        List<int> indeces = new List<int>();
        indeces.Add(max);
        return indeces;
    }
}
public class AllPlacement : Placement
{
    public override List<int> GetAllIndecesToPlace(int max)
    {
        List<int> indeces = new List<int>();
        for (int i = 0; i < max; i++) indeces.Add(i);
        return indeces;
    }
}
public class FirstPlacement : Placement
{
    public override List<int> GetAllIndecesToPlace(int max)
    {
        List<int> indeces = new List<int>();
        indeces.Add(0);
        return indeces;
    }
}
public class NullPlacement : Placement
{
    public override List<int> GetAllIndecesToPlace(int max)
    {
        return new List<int>();
    }
}

public abstract class ConditionData { }

public class AbilityConditionData : ConditionData
{
    public Ability ability;
    public AbilityConditionData(Ability ability) => this.ability = ability;
}
public abstract class Condition
{
    public abstract bool Evaluate(ConditionData data = null);

    public static Condition Load(ManipulationData data)
    {
        switch (data.Condition)
        {
            case "Melee":
                return new KeywordCondition("Melee");
            default:
                return new AlwaysTrueCondition();
        }
    }
}
public class AlwaysTrueCondition : Condition
{
    public override bool Evaluate(ConditionData data = null)
    {
        return true;
    }
}
public class KeywordCondition : Condition
{
    private string keyword;
    public KeywordCondition(string keyword)
    {
        this.keyword = keyword;
    }
    public override bool Evaluate(ConditionData data = null)
    {
        if (data is AbilityConditionData)
        {
            return ((data as AbilityConditionData).ability.ContainsKeyword(keyword));
        }
        return false;
    }
}

public abstract class ModificationData { }

public class CardModificationData : ModificationData
{
    public List<Card> cardList;
    public int index;
    public CardModificationData(List<Card> cards, int i) => (cardList, index) = (cards, i);
}
public class AbilityKeywordModificationData : ModificationData
{
    public string oldKey;
    public List<Ability> abilityList;
    public int index;
    public AbilityKeywordModificationData(string k, List<Ability> aList, int i) => (oldKey, abilityList, index) = (k, aList, i);
}
public class AbilityValueModificationData : ModificationData
{
    public List<Ability> abilityList;
    public int index;
    public AbilityValueModificationData(List<Ability> aList, int i) => (abilityList, index) = (aList, i);
}
public abstract class Modification
{
    public abstract void Modify(string withData, ModificationData data = null);

    public void SetCardAt(List<Card> cards, int index, Card newCard)
    {
        cards[index] = newCard;
    }

    public static Modification Load(ManipulationData data)
    {
        switch (data.Modification)
        {
            case "Add":
                return new AddModification();
            case "Replace":
                return new ReplaceModification();
            case "ValueAddition":
                return new ValueModification();
            default:
                return new NullModification();
        }
    }
}
public class AddModification : Modification
{
    public override void Modify(string withData, ModificationData data = null)
    {
        if (data is CardModificationData)
        {
            CardModificationData cardData = data as CardModificationData;
            SetCardAt(cardData.cardList, cardData.index, Card.Load(withData));
        }
    }
}

public class ValueModification : Modification
{
    public override void Modify(string withData, ModificationData data = null)
    {
        int value = int.Parse(withData);
        if (data is AbilityValueModificationData)
        {
            AbilityValueModificationData abilityData = data as AbilityValueModificationData;
            abilityData.abilityList[abilityData.index].AddToValue(value);
        }
    }
}
public class ReplaceModification : Modification
{
    public override void Modify(string withData, ModificationData data = null)
    {
        if (data is AbilityKeywordModificationData)
        {
            AbilityKeywordModificationData abilityData = data as AbilityKeywordModificationData;
            //abilityData.abilityList[abilityData.index].keys.Remove(abilityData.oldKey);
            abilityData.abilityList[abilityData.index].RemoveKeyword(abilityData.oldKey);
            abilityData.abilityList[abilityData.index].AddKeyword(withData);
        }
    }
}
public class NullModification : Modification
{
    public override void Modify(string withData, ModificationData data = null)
    {
        
    }
}