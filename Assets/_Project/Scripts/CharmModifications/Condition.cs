using System.Collections;
using System.Collections.Generic;
using Capstone.DataLoad;
using UnityEngine;

namespace Manipulations
{
    public abstract class Condition
    {
        public abstract bool Evaluate(Character character, int index);

        protected int number;
        public string name;
        protected Formula formula;
        public Condition(ConditionData data)
        {
            number = data.Value;
            name = data.Name;
            formula = Formula.Load(data.Operation);
        }
        public Condition() { }

        protected bool HasCardWithAbilityAt(Character character, int index)
        {
            if(!HasCardAt(character, index)) return false;
            if(character.inventory.GetCards()[index].GetTile().ability == null) return false;
            return true;
        }
        protected bool HasCardAt(Character character, int index) => character.inventory.GetCards().Count > index;
    
        public static Condition Load(ManipulationData data)
        {
            switch (data.Condition.Type)
            {
                case "MaxHealthAmount":
                    return new MaxHealthAmountCondition(data.Condition);
                case "HealthAmount":
                    return new HealthAmountCondition(data.Condition);
                case "HealthPercent":
                    return new HealthPercentCondition(data.Condition);
                case "XPAmount":
                    return new XPAmountCondition(data.Condition);
                case "XPPercent":
                    return new XPPercentCondition(data.Condition);
                case "WallLocations":
                    return new WallLocationsCondition(data.Condition);
                case "Slider":
                    return new PlaceStyleCondition(data.Condition, true);
                case "Swapper":
                    return new PlaceStyleCondition(data.Condition, false);
                case "CardAmount":
                    return new CardAmountCondition(data.Condition);
                case "CardAbilityValue":
                    return new CardAbilityValueCondition(data.Condition);
                case "CardAbilityKeyword":
                    return new CardAbilityKeywordCondition(data.Condition);
                case "AbilityAmount":
                    return new AbilityAmountCondition(data.Condition);
                case "AbilityValue":
                    return new AbilityValueCondition(data.Condition);
                case "AbilityKeyword":
                    return new AbilityKeywordCondition(data.Condition);
                case "HasCardAbility":
                    return new CardAbilityExistsCondition(data.Condition, true);
                case "MissingCardAbility":
                    return new CardAbilityExistsCondition(data.Condition, false);
                case "HasAbility":
                    return new AbilityExistsCondition(data.Condition, true);
                case "MissingAbility":
                    return new AbilityExistsCondition(data.Condition, false);
                default:
                    return new AlwaysTrueCondition();
            }
        }
    }
    public class MaxHealthAmountCondition : Condition
    {
        public MaxHealthAmountCondition(ConditionData data) : base(data) { }

        public override bool Evaluate(Character character, int index) => formula.Evaluate(character.health.GetMaxHealthValue(), number);
        
    }
    public class HealthAmountCondition : Condition
    {
        public HealthAmountCondition(ConditionData data) : base(data) { }

        public override bool Evaluate(Character character, int index) => formula.Evaluate(character.health.GetHealthValue(), number);
        
    }
    public class HealthPercentCondition : Condition
    {
        public HealthPercentCondition(ConditionData data) : base(data) { }

        public override bool Evaluate(Character character, int index) => formula.Evaluate(character.health.GetIntPercentage(), number);
        
    }
    public class XPAmountCondition : Condition
    {
        public XPAmountCondition(ConditionData data) : base(data) { }

        public override bool Evaluate(Character character, int index) => formula.Evaluate(character.XP.value, number);
        
    }
    public class XPPercentCondition : Condition
    {
        public XPPercentCondition(ConditionData data) : base(data) { }

        public override bool Evaluate(Character character, int index) => formula.Evaluate(character.XP.GetIntPercentage(), number);
        
    }
    public class WallLocationsCondition : Condition
    {
        public WallLocationsCondition(ConditionData data) : base(data) { }

        public override bool Evaluate(Character character, int index)
        {
            if (!HasCardAt(character, index)) return false;
            if (UsingName()) return character.inventory.GetCards()[index].GetTile().path.Equals(name);
            return formula.Evaluate(character.inventory.GetCards()[index].GetTile().path.GetOpenPathCount(), number);
        }

        protected bool UsingName()
        {
            return (name.Contains("N") || name.Contains("S") || name.Contains("E") || name.Contains("W") || name.Contains("_"));
        }
    }
    public class PlaceStyleCondition : BoolCondition
    {
        public PlaceStyleCondition(ConditionData data, bool value) : base(data, value) { }

        public override bool Evaluate(Character character, int index)
        {
            if (!HasCardAt(character, index)) return false;
            return character.inventory.GetCards()[index].GetTile().type == name == boolean;
        }
        
    }
    public class CardAmountCondition : Condition
    {
        public CardAmountCondition(ConditionData data) : base(data)
        {
        }

        public override bool Evaluate(Character character, int index)
        {
            return formula.Evaluate(character.inventory.GetCards().Count, number);
        }
    }
    public class CardAbilityValueCondition : Condition
    {
        public CardAbilityValueCondition(ConditionData data) : base(data)
        {
        }

        public override bool Evaluate(Character character, int index)
        {
            if (!HasCardWithAbilityAt(character, index)) return false;
            return formula.Evaluate(character.inventory.GetCards()[index].GetTile().ability.GetValue(), number);
        }
    }
    public class CardAbilityKeywordCondition : Condition
    {
        public CardAbilityKeywordCondition(ConditionData data) : base(data)
        {
        }

        public override bool Evaluate(Character character, int index)
        {
            if (!HasCardWithAbilityAt(character, index)) return false;
            return character.inventory.GetCards()[index].GetTile().ability.ContainsKeyword(name);
        }
    }
    public class AbilityAmountCondition : Condition
    {
        public AbilityAmountCondition(ConditionData data) : base(data)
        {
        }

        public override bool Evaluate(Character character, int index)
        {
            return formula.Evaluate(character.abilityList.Count, number);
        }
    }
    public class AbilityValueCondition : Condition
    {
        public AbilityValueCondition(ConditionData data) : base(data)
        {
        }

        public override bool Evaluate(Character character, int index)
        {
            if(character.abilityList.Count <= index) return false;
            return formula.Evaluate(character.abilityList[index].GetValue(), number);
        }
    }
    public class AbilityKeywordCondition : Condition
    {
        public AbilityKeywordCondition(ConditionData data) : base(data)
        {
        }

        public override bool Evaluate(Character character, int index)
        {
            if(character.abilityList.Count <= index) return false;
            return character.abilityList[index].ContainsKeyword(name);
        }
    }
    public class CardAbilityExistsCondition : BoolCondition
    {
        public CardAbilityExistsCondition(ConditionData data, bool exists) : base(data, exists)
        {
        }

        public override bool Evaluate(Character character, int index)
        {
            return HasCardWithAbilityAt(character, index) == boolean;
        }
    }
    public class AbilityExistsCondition : BoolCondition
    {
        public AbilityExistsCondition(ConditionData data, bool exists) : base(data, exists)
        {
        }

        public override bool Evaluate(Character character, int index)
        {
            if(character.abilityList.Count <= index) return false;
            return character.abilityList[index] != null == boolean;
        }
    }

    public class AlwaysTrueCondition : Condition
    {
        public override bool Evaluate(Character character, int index)
        {
            return true;
        }
    }
    public abstract class BoolCondition : Condition
    {
        protected bool boolean;

        public BoolCondition(ConditionData data, bool boolean) : base(data)
        {
            this.boolean = boolean;
        }
    }

}
