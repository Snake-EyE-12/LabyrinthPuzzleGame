using System.Collections.Generic;
using Capstone.DataLoad;
using UnityEngine;

public class Character : Unit
{
    public string characterType { get; private set; }
    public XPBar XP { get; private set; }
    public Inventory inventory { get; private set; }
    public List<Ability> abilityList = new List<Ability>();



    public Character(CharacterData data)
    {
        unitName = data.Name;
        characterType = data.Type;
        degree = data.Degree;
        health = new Health(data.Health);
        XP = new XPBar(data.Charge);
        inventory = new Inventory(data.Inventory);
        activeEffects = new ActiveEffect(data.ActiveEffects);
        abilityList = new List<Ability>();

        foreach (var a in data.Abilities)
        {
            abilityList.Add(new Ability(a));
        }
    }

    


    

    public static CharacterData Generate(string type, int degree)
    {
        CharacterData cd = new CharacterData();
        cd.Name = "Generated";
        cd.Type = type;
        cd.Degree = degree;
        cd.Health = new HealthData[0];
        cd.Charge = 5;
        cd.Inventory = new InventoryData();
        cd.Abilities = new AbilityData[1];
        
        return cd;
    }

    public static CharacterData Load(string type, int degree)
    {
        List<CharacterData> characterOptions = DataHolder.availableCharacters.FindAllOfType(type);
        for (int i = characterOptions.Count - 1; i > 0; i--)
        {
            if (characterOptions[i].Degree != degree)
            {
                characterOptions.RemoveAt(i);
            }
        }
        return characterOptions[GameUtils.IndexByWeightedRandom(new List<Weighted>(characterOptions))];
    }
}

public class Inventory
{
    private List<Card> tilePieces = new List<Card>();
    public Inventory(InventoryData data)
    {
        foreach (var tp in data.TilePieces)
        {
            tilePieces.Add(Card.Load(tp));
        }
    }

    public List<Card> GetCards()
    {
        return tilePieces;
    }
}
