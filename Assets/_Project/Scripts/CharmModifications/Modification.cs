using System.Collections;
using System.Collections.Generic;
using Capstone.DataLoad;
using UnityEngine;

namespace Manipulations
{
    public abstract class Modification
    {
        public abstract void Modify(Character character, int index);
        public abstract void Strip(Character character, int index);

        public virtual void Set(Character character, int index)
        {
        }

        protected int value;
        protected string name;
        public Modification(ModificationData data)
        {
            value = data.Value;
            name = data.Name;
        }

        public Modification()
        {
        }

        protected bool HasCardWithAbilityAt(Character character, int index)
        {
            if(!HasCardAt(character, index)) return false;
            if(character.inventory.GetCards()[index].GetTile().ability == null) return false;
            return true;
        }
        protected string[] GetKeywords(string keywordList)
        {
            return keywordList.Split(',');
        }
        protected bool HasCardAt(Character character, int index) => character.inventory.GetCards().Count > index;
        protected bool HasAbilityAt(Character character, int index) => character.abilityList.Count > index;
        public static Modification LoadHealthChange(ModificationData data)
        {
            switch (data.Type)
            {
                case "Amount":
                    return new HealthAmountModification(data);
                default:
                    return new NullModification();
            }
        }
        public static Modification LoadXPChange(ModificationData data)
        {
            switch (data.Type)
            {
                case "Value":
                    return new XPValueModification(data);
                case "Amount":
                    return new XPAmountModification(data);
                default:
                    return new NullModification();
            }
        }
        public static Modification LoadCardChange(ModificationData data)
        {
            switch (data.Type)
            {
                case "Keyword":
                    return new CardKeywordModification(data);
                case "Value":
                    return new CardValueModification(data);
                case "Amount":
                    return new CardAmountModification(data);
                case "Placement":
                    return new CardPlacementModification(data);
                case "Wall":
                    return new CardWallModification(data);
                case "Ability":
                    return new CardAbilityModification(data);
                default:
                    return new NullModification();
            }
        }
        public static Modification LoadAbilityChange(ModificationData data)
        {
            switch (data.Type)
            {
                case "Keyword":
                    return new AbilityKeywordModification(data);
                case "Value":
                    return new AbilityValueModification(data);
                case "Ability":
                    return new AbilityAbilityModification(data);
                default:
                    return new NullModification();
            }
        }
    }
    
    public class AbilityKeywordModification : Modification
    {
        public AbilityKeywordModification(ModificationData data) : base(data)
        {
        }
        public override void Modify(Character character, int index)
        {
            if (!HasAbilityAt(character, index)) return;
            foreach (var key in GetKeywords(name))
            {
                character.abilityList[index].AddKeyword(key);
            }
        }
        public override void Strip(Character character, int index)
        {
            if (!HasAbilityAt(character, index)) return;
            foreach (var key in GetKeywords(name))
            {
                character.abilityList[index].RemoveKeyword(key);
            }
        }
        public override void Set(Character character, int index)
        {
            if (!HasAbilityAt(character, index)) return;
            character.abilityList[index].SetKeywords(GetKeywords(name));
        }
    }
    public class AbilityValueModification : Modification
    {
        public AbilityValueModification(ModificationData data) : base(data)
        {
        }
        public override void Modify(Character character, int index)
        {
            if (!HasAbilityAt(character, index)) return;
            character.abilityList[index].AddToValue(value);
        }
        public override void Strip(Character character, int index)
        {
            if (!HasAbilityAt(character, index)) return;
            character.abilityList[index].AddToValue(-value);
        }
        public override void Set(Character character, int index)
        {
            if (!HasAbilityAt(character, index)) return;
            character.abilityList[index].SetValue(value);
        }
    }
    public class AbilityAbilityModification : Modification
    {
        public AbilityAbilityModification(ModificationData data) : base(data)
        {
        }
        public override void Modify(Character character, int index)
        {
            character.AddAbilitySpaceUpTo(index);
            character.abilityList[index] = new Ability(new AbilityData(value, "Any", GetKeywords(name)));
        }
        public override void Strip(Character character, int index)
        {
            if (!HasAbilityAt(character, index)) return;
            character.abilityList.RemoveAt(index);
        }
    }
    
    
    
    public class CardAbilityModification : Modification
    {
        public CardAbilityModification(ModificationData data) : base(data)
        {
        }
        public override void Modify(Character character, int index)
        {
            if (!HasCardAt(character, index)) return;
            character.inventory.GetCards()[index].GetTile().ability = new Ability(new AbilityData(value, "Any", GetKeywords(name)));
        }
        public override void Strip(Character character, int index)
        {
            if (!HasCardAt(character, index)) return;
            character.inventory.GetCards()[index].GetTile().ability = null;
        }
    }
    public class CardWallModification : Modification
    {
        public CardWallModification(ModificationData data) : base(data)
        {
        }
        public override void Modify(Character character, int index)
        {
            if (!HasCardAt(character, index)) return;
            if (UsingName())
            {
                foreach (Vector2Int direction in GetDirections())
                {
                    character.inventory.GetCards()[index].GetTile().ClosePath(direction);
                }
            }
            character.inventory.GetCards()[index].GetTile().Build(value);
        }
        public override void Strip(Character character, int index)
        {
            if (!HasCardAt(character, index)) return;
            if (UsingName())
            {
                foreach (Vector2Int direction in GetDirections())
                {
                    character.inventory.GetCards()[index].GetTile().OpenPath(direction);
                }
            }
            character.inventory.GetCards()[index].GetTile().Break(value);
        }
        protected bool UsingName()
        {
            return (name.Contains("N") || name.Contains("S") || name.Contains("E") || name.Contains("W") || name.Contains("_"));
        }

        protected List<Vector2Int> GetDirections()
        {
            List<Vector2Int> directions = new List<Vector2Int>();
            if (name.Contains("N")) directions.Add(Vector2Int.up);
            if (name.Contains("S")) directions.Add(Vector2Int.down);
            if (name.Contains("E")) directions.Add(Vector2Int.right);
            if (name.Contains("W")) directions.Add(Vector2Int.left);
            return directions;
        }
    }
    public class CardPlacementModification : Modification
    {
        public CardPlacementModification(ModificationData data) : base(data)
        {
        }
        public override void Modify(Character character, int index)
        {
            if (!HasCardAt(character, index)) return;
            character.inventory.GetCards()[index].GetTile().type = name;
        }
        public override void Strip(Character character, int index)
        {
        }
    }
    public class CardAmountModification : Modification
    {
        public CardAmountModification(ModificationData data) : base(data)
        {
        }
        public override void Modify(Character character, int index)
        {
            character.AddInventoryCardSpaceUpTo(index);
            Card newCard = Card.Load(name);
            Debug.Log("Loading Card to Add" + name);
            newCard.owner = character.characterType;
            character.inventory.GetCards()[index] = newCard;
        }
        public override void Strip(Character character, int index)
        {
            if (!HasCardAt(character, index)) return;
            character.inventory.GetCards().RemoveAt(index);
        }
    }
    public class CardValueModification : Modification
    {
        public CardValueModification(ModificationData data) : base(data)
        {
        }
        public override void Modify(Character character, int index)
        {
            if (!HasCardWithAbilityAt(character, index)) return;
            character.inventory.GetCards()[index].GetTile().ability.AddToValue(value);
        }
        public override void Strip(Character character, int index)
        {
            if (!HasCardWithAbilityAt(character, index)) return;
            character.inventory.GetCards()[index].GetTile().ability.AddToValue(-value);
        }
    }
    public class CardKeywordModification : Modification
    {
        public CardKeywordModification(ModificationData data) : base(data)
        {
        }
        public override void Modify(Character character, int index)
        {
            if (!HasCardWithAbilityAt(character, index)) return;
            character.inventory.GetCards()[index].GetTile().ability.AddKeyword(name);
        }
        public override void Strip(Character character, int index)
        {
            if (!HasCardWithAbilityAt(character, index)) return;
            character.inventory.GetCards()[index].GetTile().ability.RemoveKeyword(name);
        }
    }

    public class XPAmountModification : Modification
    {

        public XPAmountModification(ModificationData data) : base(data)
        {
        }
        
        public override void Modify(Character character, int index)
        {
            character.XP.max += value;
        }
        public override void Strip(Character character, int index)
        {
            character.XP.max = Mathf.Clamp(character.XP.max - value, 1, character.XP.max);
        }
            
    }
    public class XPValueModification : Modification
    {

        public XPValueModification(ModificationData data) : base(data)
        {
        }
        
        public override void Modify(Character character, int index)
        {
            character.XP.value = Mathf.Clamp(character.XP.value + value, 0, character.XP.max);
        }
        public override void Strip(Character character, int index)
        {
            character.XP.value = Mathf.Clamp(character.XP.max - value, 0, character.XP.max);
        }
            
    }
    public class HealthAmountModification : Modification
    {
        public HealthAmountModification(ModificationData data) : base(data)
        {
        }

        public override void Modify(Character character, int index)
        {
            character.health.AddHealthType(name, value);
        }

        public override void Strip(Character character, int index)
        {
            character.health.RemoveHealth(value);
        }
    }
    
    
    public class NullModification : Modification
    {
        public override void Modify(Character character, int index)
        {
            
        }
        public override void Strip(Character character, int index)
        {
            
        }
    }



}
