using System.Collections;
using System.Collections.Generic;
using Capstone.DataLoad;
using UnityEngine;

namespace Manipulations
{
    public abstract class Placement
    {
        public abstract List<int> GetAllIndecesToPlace(int max);
    
        public static Placement Load(ManipulationData data)
        {
            switch (data.Placement)
            {
                case "Post":
                    return new PostPlacement();
                case "All":
                    return new AllPlacement();
                case "First":
                    return new FirstPlacement();
                default:
                    return new NullPlacement();
            }
        }
    }
    public class PostPlacement : Placement
    {
        public override List<int> GetAllIndecesToPlace(int max)
        {
            List<int> indeces = new List<int>();
            indeces.Add(max);
            return indeces;
        }
    }
    public class AllPlacement : Placement
    {
        public override List<int> GetAllIndecesToPlace(int max)
        {
            List<int> indeces = new List<int>();
            for (int i = 0; i < max; i++) indeces.Add(i);
            return indeces;
        }
    }
    public class FirstPlacement : Placement
    {
        public override List<int> GetAllIndecesToPlace(int max)
        {
            List<int> indeces = new List<int>();
            indeces.Add(0);
            return indeces;
        }
    }
    public class NullPlacement : Placement
    {
        public override List<int> GetAllIndecesToPlace(int max)
        {
            return new List<int>();
        }
    }
}
