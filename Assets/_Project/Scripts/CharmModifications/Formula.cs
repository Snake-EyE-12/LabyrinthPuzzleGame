using System.Collections;
using System.Collections.Generic;
using Capstone.DataLoad;
using UnityEngine;

namespace Manipulations
{
    public abstract class Formula
    {
        public static Formula Load(string type)
        {
            switch (type)
            {
                case "Greater":
                    return new GreaterFormula();
                case "Lesser":
                    return new LesserFormula();
                case "Equal":
                    return new EqualFormula();
                default:
                    return new NullFormula();
            }
        }
        public abstract bool Evaluate(int x, int y);
    }
    public class GreaterFormula : Formula
    {
        public override bool Evaluate(int x, int y)
        {
            return x > y;
        }
    }
    public class LesserFormula : Formula
    {
        public override bool Evaluate(int x, int y)
        {
            return x < y;
        }
    }
    public class EqualFormula : Formula
    {
        public override bool Evaluate(int x, int y)
        {
            return x == y;
        }
    }
    public class NullFormula : Formula
    {
        public override bool Evaluate(int x, int y)
        {
            return false;
        }
    }
}
