using System.Collections;
using System.Collections.Generic;
using Capstone.DataLoad;
using UnityEngine;

namespace Manipulations
{
    public abstract class Manipulation
    {
        protected Condition condition;
        protected Placement placer;
        protected Modification mod;
        protected string withData;
        public Manipulation(ManipulationData data)
        {
            condition = Condition.Load(data);
            placer = Placement.Load(data);
            mod = Modification.Load(data);
            withData = data.With;
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
                case "Tile":
                    return new TileManipulation(data);
                case "AbilityKeyword":
                    return new AbilityKeywordManipulation(data);
                case "Ability":
                    return new AbilityManipulation(data);
            }
            return null;
        }
    }
    
    /////////////////////////////////////////////////////////

    public class HealthManipulation : Manipulation
    {
        public HealthManipulation(ManipulationData data) : base(data)
        {
            
        }

        public override void Apply(Character character)
        {
            if(condition.Evaluate()) mod.Modify(withData, new HealthModificationData(character));
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
        
        public CardManipulation(ManipulationData data) : base(data)
        {
        }

        public override void Apply(Character character)
        {
            int cardCount = character.inventory.GetCards().Count;
            PrepareList(cardCount);
            foreach (var index in indeces)
            {
                if (index >= cardCount) character.AddInventoryCardSpaceUpTo(index);
                if (condition.Evaluate()) mod.Modify(withData, new CardModificationData(character.inventory.GetCards(), index));
            }
        }
    }
    public class TileManipulation : Manipulation
    {
        
        public TileManipulation(ManipulationData data) : base(data)
        {
        }

        public override void Apply(Character character)
        {
            Debug.Log("Applied");
            int cardCount = character.inventory.GetCards().Count;
            PrepareList(cardCount);
            foreach (var index in indeces)
            {
                Debug.Log("Index: " + index);
                if (index >= cardCount) character.AddInventoryCardSpaceUpTo(index);
                if (condition.Evaluate(new AbilityConditionData(character.inventory.GetCards()[index].GetTile().ability)))
                {
                    Debug.Log("Modified");
                    mod.Modify(withData, new CardAbilityModificationData(character.inventory.GetCards(), index));
                }
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
}
