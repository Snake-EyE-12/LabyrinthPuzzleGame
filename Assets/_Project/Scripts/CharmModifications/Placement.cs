using System.Collections;
using System.Collections.Generic;
using Capstone.DataLoad;
using UnityEngine;

namespace Manipulations
{
    public abstract class Placement
    {
        public abstract List<int> GetAllIndexesToPlace(int max);

        protected int value;
        protected Formula formula;
        public Placement(PositionData data)
        {
            value = data.Value - 1;
            formula = Formula.Load(data.Operation);
        }

        public Placement()
        {
        }

        public static Placement Load(ManipulationData data)
        {
            switch (data.Position.Type)
            {
                case "Even":
                    return new SequencePlacement(data.Position, new Sequence(new Range(0, int.MaxValue), 0, 1, 2));
                case "Odd":
                    return new SequencePlacement(data.Position, new Sequence(new Range(0, int.MaxValue), 1, 1, 2));
                case "All":
                    return new AllPlacement(data.Position);
                case "Post":
                    return new PostPlacement(data.Position);
                case "Formula":
                    return new FormulaPlacement(data.Position);
                default:
                    return new NullPlacement();
            }
        }
    }

    public class FormulaPlacement : Placement
    {
        public FormulaPlacement(PositionData data) : base(data)
        {
        }

        public override List<int> GetAllIndexesToPlace(int max)
        {
            List<int> indeces = new List<int>();
            for (int i = 0; i < max; i++)
            {
                if (formula.Evaluate(i, value)) indeces.Add(i);
            }
            return indeces;
        }
    }
    public class SequencePlacement : Placement
    {
        protected Sequence sequence;
        public SequencePlacement(PositionData data, Sequence sequence) : base(data)
        {
            this.sequence = sequence;
        }

        public override List<int> GetAllIndexesToPlace(int max)
        {
            List<int> indeces = new List<int>();
            for(int i = 0; i < max; i++)
            {
                if (sequence.alignsAt(i)) indeces.Add(i);
            }
            return indeces;
        }
    }
    public class PostPlacement : Placement
    {
        public PostPlacement(PositionData data) : base(data)
        {
        }

        public override List<int> GetAllIndexesToPlace(int max)
        {
            List<int> indeces = new List<int>();
            indeces.Add(max);
            return indeces;
        }
    }
    public class AllPlacement : Placement
    {
        public AllPlacement(PositionData data) : base(data)
        {
        }

        public override List<int> GetAllIndexesToPlace(int max)
        {
            List<int> indeces = new List<int>();
            for (int i = 0; i < max; i++) indeces.Add(i);
            return indeces;
        }
    }
    public class NullPlacement : Placement
    {
        public override List<int> GetAllIndexesToPlace(int max)
        {
            return new List<int>();
        }
    }
}
