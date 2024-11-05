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
        inventory = new Inventory(data.Inventory, characterType);
        ActiveEffectsList = new ActiveEffectList(data.ActiveEffects);
        abilityList = new List<Ability>();

        foreach (var a in data.Abilities)
        {
            Ability newAbility = new Ability(a);
            newAbility.owner = characterType;
            abilityList.Add(newAbility);
        }
    }

    public void AddInventoryCardSpaceUpTo(int index)
    {
        inventory.Expand(index);
    }
    public void EquipCharm(Item charm)
    {
        charm.ApplyTo(this);
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
    public Inventory(InventoryData data, string owner)
    {
        foreach (var tp in data.TilePieces)
        {
            Card c = Card.Load(tp);
            c.owner = owner;
            tilePieces.Add(c);
        }
    }

    public void Expand(int index)
    {

        for (int i = tilePieces.Count - 1; i < index; i++)
        {
            tilePieces.Add(null);
        }
    }

    public List<Card> GetCards()
    {
        return tilePieces;
    }
}
