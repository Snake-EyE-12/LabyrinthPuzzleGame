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
        public EventData[] Events;
        public Sequence Sequence;
    }
    
    [System.Serializable]
    public class EventData : Weighted
    {
        public string Type;
        public int Weight;
        public int GetWeight()
        {
            return Weight;
        }
    }

    [System.Serializable]
    public class Sequence
    {
        public Range Range;
        public int Begin;
        public int Size;
        public int Repeated;

        
        public bool alignsAt(int value)
        {
            if (!isInRange(value)) return false;
            if(value < Begin) return false;
            if (value % (Repeated + 1) > Size) return false;
            return true;
        }

        private bool isInRange(int value)
        {
            return value >= Range.Min && value <= Range.Max;
        }
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

    [System.Serializable]
    public class CharacterList
    {
        public CharacterData[] Characters;
        
        public List<CharacterData> FindAllOfType(string type)
        {
            List<CharacterData> list = new List<CharacterData>();
            foreach (var character in Characters)
            {
                if (character.Type.Equals(type))
                {
                    list.Add(character);
                }
            }
            return list;
        }
    }

    [System.Serializable]
    public class CharacterData : Weighted
    {
        public string Name;
        public string Type;
        public int Degree;
        public Health[] Health;
        public int Charge;
        public Inventory Inventory;
        public ActiveEffect[] ActiveEffects;
        public Modifier[] Modifiers;
        public int GetWeight()
        {
            return 10;
        }
    }
    
    [System.Serializable]
    public class Health
    {
        public string Type;
        public int Value;
    }

    [System.Serializable]
    public class ActiveEffect
    {
    
    }

    [System.Serializable]
    public class Modifier
    {
    
    }
}