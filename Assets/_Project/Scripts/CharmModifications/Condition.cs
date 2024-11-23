using System.Collections;
using System.Collections.Generic;
using Capstone.DataLoad;
using UnityEngine;

namespace Manipulations
{
    public abstract class Condition
    {
        public abstract bool Evaluate(ConditionData data = null);
    
        public static Condition Load(ManipulationData data)
        {
            switch (data.Condition)
            {
                case "Melee":
                    return new KeywordCondition("Melee");
                case "Empty":
                    return new EmptyCondition();
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
    
    public class EmptyCondition : Condition
    {
        public override bool Evaluate(ConditionData data = null)
        {
            if (data is AbilityConditionData)
            {
                return ((data as AbilityConditionData).ability == null);
            }
            return false;
        }
    }
    
    
    public abstract class ConditionData { }

    public class AbilityConditionData : ConditionData
    {
        public Ability ability;
        public AbilityConditionData(Ability ability) => this.ability = ability;
    }

}
