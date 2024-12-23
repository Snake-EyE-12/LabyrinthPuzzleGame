using System.Collections;
using System.Collections.Generic;
using Capstone.DataLoad;
using UnityEngine;

namespace Manipulations
{
    public abstract class Manipulation
    {
        protected Modification mod;
        protected Condition condition;
        protected Placement looker;
        protected Method method;
        public Manipulation(ManipulationData data)
        {
            condition = Condition.Load(data);
            looker = Placement.Load(data);
            method = Method.Load(data);
        }

        protected abstract int GetMaxAmount(Character character);
        public virtual void Apply(Character character)
        {
            foreach (var index in looker.GetAllIndexesToPlace(GetMaxAmount(character)))
            {
                if (condition.Evaluate(character, index))
                {
                    if (method.MustAdd())
                    {
                        mod.Modify(character, index);
                    }
                    if (method.MustRemove())
                    {
                        mod.Strip(character, index);
                    }
                }
            }
        }
        public static Manipulation Load(ManipulationData data)
        {
            switch (data.Asset)
            {
                case "Health":
                    return new HealthManipulation(data);
                case "XP":
                    return new XPManipulation(data);
                case "Cards":
                    return new CardManipulation(data);
                case "Abilities":
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
            mod = Modification.LoadHealthChange(data.Modification);
        }

        protected override int GetMaxAmount(Character character)
        {
            return character.health.GetMaxHealthValue();
        }
    }
    public class XPManipulation : Manipulation
    {
        public XPManipulation(ManipulationData data) : base(data)
        {
            mod = Modification.LoadXPChange(data.Modification);
        }

        protected override int GetMaxAmount(Character character)
        {
            return character.XP.max;
        }
    }
    public class CardManipulation : Manipulation
    {
        
        public CardManipulation(ManipulationData data) : base(data)
        {
            mod = Modification.LoadCardChange(data.Modification);
        }

        protected override int GetMaxAmount(Character character)
        {
            return character.inventory.GetCards().Count;
        }
    }
    public class AbilityManipulation : Manipulation
    {
        private string number;
        public AbilityManipulation(ManipulationData data) : base(data)
        {
            mod = Modification.LoadAbilityChange(data.Modification);
        }

        protected override int GetMaxAmount(Character character)
        {
            return character.abilityList.Count;
        }
    }
}
