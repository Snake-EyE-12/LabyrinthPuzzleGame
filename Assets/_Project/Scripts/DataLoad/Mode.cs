using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;

namespace Capstone.DataLoad
{
    [System.Serializable]
    public class Mode
    {
        public string DisplayName;
        public Color displayColor;
        public string Description;
        public int GridSize;
        public CharacterSelection CharacterSelection;
        public int Rounds;
        public EventLayout[] EventLayout;
        public Fight[] Fights;
        
        public void Load()
        {
            GamemodeManager.Instance.Load(this);
        }
    }
    
    [System.Serializable]
    public class Fight
    {
        public Range Degree;
        public int Weight;
        public string Border;
        public string[] Enemies;
        public FloorLayout FloorLayout;
    }

    [System.Serializable]
    public class FloorLayout
    {
        public string Fill;
        public FloorTile[] AvailableTiles;
    }
    
    [System.Serializable]
    public class FloorTile
    {
        public string Tile;
        public int Value;
    }

    [System.Serializable]
    public class CharacterSelection
    {
        public string Type; // Draft - Choose - Preset
        public int Choices;
        public string Default; // Preset
        public int Total;
        public int InitialDegree;
    }

    [System.Serializable]
    public class EventLayout
    {
        public int IncludeChance;
        public InGameEvent[] Events;
    }

    [System.Serializable]
    public class InGameEvent
    {
        public EventData Event;
        public Sequence Sequence;
    }
    
    [System.Serializable]
    public class EventData
    {
        public string Type;
        public int Weight;
    }

    [System.Serializable]
    public class Sequence
    {
        public Range Range;
        public int Begin;
        public int Size;
        public int Repeated;
    }

    [System.Serializable]
    public class CharacterColorEquivalenceTable
    {
        public CharacterColorEquivalence[] Equivalences;

        public Color GetColor(string type)
        {
            foreach (var equivalence in Equivalences)
            {
                if (equivalence.Type.Equals(type))
                {
                    return equivalence.Color;
                }
            }
            return Color.magenta;
        }
    }
    
    [System.Serializable]
    public class CharacterColorEquivalence
    {
        public string Type;
        public Color Color;

    }

    [System.Serializable]
    public class CharacterLayoutTable
    {
        public CharacterLayout[] Layouts;

        public List<string> GetLayout(string name)
        {
            foreach (var layout in Layouts)
            {
                if (layout.Name.Equals(name))
                {
                    return new List<string>(layout.Characters);
                }
            }

            return new List<string>();
        }
    }
    
    [System.Serializable]
    public class CharacterLayout : Weighted
    {
        public string Name;
        public string[] Characters;
        public int Weight;

        public int GetWeight()
        {
            return Weight;
        }
        
    }
}