using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Capstone.DataLoad
{
    [System.Serializable]
    public class Mode
    {
        public string DisplayName;
        public Color DisplayColor;
        public string Description;
        public int GridSize;
        public CharacterSelection CharacterSelection;
        public int Rounds;
        public EventLayout[] EventLayout;
        public int LowHealthPercent;
        public int MaximumIQ;
        public int CardsToPlacePerTurn;
        public int HandSize;
        public int MaxChallengeAdditions;
        public Range ChallengeRange;
        public int PostBattleHealPercent;
        public int ProductsPerShop;
        
        public void Load()
        {
            GamemodeManager.Instance.Load(this);
        }
    }
    
    [System.Serializable]
    public class FightList
    {
        public Fight[] Fights;
        
        public List<Fight> FindAllOfDegree(int roundDegree)
        {
            List<Fight> list = new List<Fight>();
            foreach (var fight in Fights)
            {
                if (fight.Degree.inRange(roundDegree))
                {
                    list.Add(fight);
                }
            }
            return list;
        }
    }
    
    [System.Serializable]
    public class Fight : Weighted
    {
        public Range Degree;
        public int Weight;
        public string Border;
        public string[] Enemies;
        public FloorLayout FloorLayout;
        public int GetWeight()
        {
            return Weight;
        }

        public void AddEnemies(int amount, Range degree)
        {
            for (int a = 0; a < amount; a++)
            {
                extraEnemies.Add(DataHolder.availableEnemies.RandomOfDegreeRange(degree).Name);
            }
        }
        private List<string> extraEnemies = new List<string>();

        public string[] GetEnemies()
        {
            List<string> fightEnemies = new List<string>(Enemies);
            fightEnemies.AddRange(extraEnemies);
            return fightEnemies.ToArray();
        }
    }

    [System.Serializable]
    public class FloorLayout
    {
        public string Fill;
        public FloorTile[] AvailableTiles;
    }
    
    [System.Serializable]
    public class FloorTile : Weighted
    {
        public string Tile;
        public int Value;
        public string Rotation;
        public int GetWeight()
        {
            return Value;
        }
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
            if (!Range.inRange(value)) return false;
            if(value < Begin) return false;
            if (value % (Repeated + 1) > Size) return false;
            return true;
        }
        
    }

    [System.Serializable]
    public class StringColorEquivalenceTable
    {
        public StringColorEquivalence[] Equivalences;

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
    public class StringColorEquivalence
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
    public class TileList
    {
        public CardData[] Tiles;

        public CardData FindCardBySymbol(string symbol)
        {
            foreach (var tile in Tiles)
            {
                if (tile.Symbol.Equals(symbol))
                {
                    return tile;
                }
            }

            return null;
        }
    }

    [System.Serializable]
    public class CardData
    {
        public string Symbol;
        public TileData Tile;
        public AbilityData Ability;
    }

    [System.Serializable]
    public class TileData
    {
        public string Path;
        public string Type;
    }

    [System.Serializable]
    public class CharacterData : Weighted
    {
        public string Name;
        public string Type;
        public int Degree;
        public HealthData[] Health;
        public int Charge;
        public InventoryData Inventory;
        public ActiveEffectData[] ActiveEffects;
        public AbilityData[] Abilities;
        public int GetWeight()
        {
            return 10;
        }
    }
    
    [System.Serializable]
    public class EnemyList
    {
        public EnemyData[] Enemies;
        
        public List<EnemyData> FindAllOfType(string type)
        {
            List<EnemyData> list = new List<EnemyData>();
            foreach (var character in Enemies)
            {
                if (character.Type.Equals(type))
                {
                    list.Add(character);
                }
            }
            return list;
        }

        public EnemyData FindFirstOfName(string name)
        {
            foreach (var character in Enemies)
            {
                if (character.Name.Equals(name))
                {
                    return character;
                }
            }
            return null;
        }

        public EnemyData RandomOfDegreeRange(Range range)
        {
            List<EnemyData> list = new List<EnemyData>();
            foreach (var ed in Enemies)
            {
                if(range.inRange(ed.Degree)) list.Add(ed);
            }
            if(list.Count < 0) return null;
            list.Sort((a, b) => Random.Range(0, 100).CompareTo(Random.Range(0, 100)));
            return list[0];
        }
    }

    [System.Serializable]
    public class EnemyData : Weighted
    {
        public string Name;
        public int Degree;
        public int Weight;
        public HealthData[] Health;
        public string Type;
        public int AttackIQ;
        public int MovementIQ;
        public AttackData[] AttackLayout;
        public ActiveEffectData[] ActiveEffects;
        public LootData[] Loot;
        public int GetWeight()
        {
            return Weight;
        }
    }
    
    [System.Serializable]
    public class LootData
    {
        public string Type;
        public int Value;
    }
    
    
    [System.Serializable]
    public class InventoryData
    {   
        public int ItemSlots;
        public string[] TilePieces;
    }


    [System.Serializable]
    public class AttackData
    {
        public int Weight;
        public string Shape;
        public int Power;
        public AbilityData Ability;
    }

    [System.Serializable]
    public class AbilityData
    {
        public int Value;
        public string Target;
        public string[] Keys;
    }

    [System.Serializable]
    public class ItemList
    {
        public ItemData[] Charms;
        public ItemData RandomOfDegreeRange(Range range)
        {
            List<ItemData> list = new List<ItemData>();
            foreach (var ed in Charms)
            {
                if(range.inRange(ed.Degree)) list.Add(ed);
            }
            if(list.Count <= 0) return null;
            return list[GameUtils.IndexByWeightedRandom(new List<Weighted>(list))];
        }

        public ItemData RandomByDegree(int degree)
        {
            return RandomOfDegreeRange(new Range(degree, degree));
        }
    }

    [System.Serializable]
    public class ItemData : Weighted
    {
        public string Name;
        public string Description;
        public int Degree;
        public Range Price;
        public int Weight;
        public ManipulationData[] Manipulations;


        public int GetWeight()
        {
            return Weight;
        }
    }
    
    [System.Serializable]
    public class ManipulationData
    {
        public string Change;
        public string Condition;
        public string Modification;
        public string With;
    }
    
    
}