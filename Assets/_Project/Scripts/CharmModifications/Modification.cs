using System.Collections;
using System.Collections.Generic;
using Capstone.DataLoad;
using UnityEngine;

namespace Manipulations
{
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
                Card c = Card.Load(withData);
                c.owner = cardData.cardList[0].owner;
                SetCardAt(cardData.cardList, cardData.index, c);
                return;
            }
            if (data is HealthModificationData)
            {
                int value = int.Parse(withData);
                HealthModificationData healthData = data as HealthModificationData;
                healthData.characterHealthBar.AddHealthType("Blood", value);
                return;
            }
            if (data is CardAbilityModificationData)
            {
                CardAbilityModificationData cardAbilityData = data as CardAbilityModificationData;
                int value = int.Parse(withData.Substring(1, 1));
                Debug.Log("Value: " + value + " Words: " + GetKeywords(withData)[0]);
                cardAbilityData.cards[cardAbilityData.index].GetTile().ability = new Ability(new AbilityData(value, "Any", GetKeywords(withData)));
                return;
            }
        }
        private string[] GetKeywords(string withData)
        {
            int indexOfDash = withData.IndexOf('-');
            withData = withData.Substring(indexOfDash + 1);
            return withData.Split(',');
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




    public abstract class ModificationData { }

    public class CardModificationData : ModificationData
    {
        public List<Card> cardList;
        public int index;
        public CardModificationData(List<Card> cards, int i) => (cardList, index) = (cards, i);
    }

    public class HealthModificationData : ModificationData
    {
        public Health characterHealthBar;
        public HealthModificationData(Character character) => characterHealthBar = character.health;
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
    public class CardAbilityModificationData : ModificationData
    {
        public List<Card> cards;
        public int index;
        public CardAbilityModificationData(List<Card> cList, int i) => (cards, index) = (cList, i);
    }

}
