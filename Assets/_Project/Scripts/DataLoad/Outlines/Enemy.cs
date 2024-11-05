using System;
using System.Collections.Generic;
using System.Linq;
using Capstone.DataLoad;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : Unit
{
    public int weight { get; private set; }
    public string type { get; private set; }
    public int AttackIQ { get; private set; }
    public int MovementIQ { get; private set; }
    public AttackLayout attackLayout { get; private set; }
    public List<Loot> loot = new List<Loot>();



    public Enemy(EnemyData data)
    {
        unitName = data.Name;
        degree = data.Degree;
        weight = data.Weight;
        health = new Health(data.Health);
        type = data.Type;
        AttackIQ = data.AttackIQ;
        MovementIQ = data.MovementIQ;
        attackLayout = new AttackLayout(data.AttackLayout);
        ActiveEffectsList = new ActiveEffectList(data.ActiveEffects);
        if(data.Loot != null)
        {
            foreach (var lootPiece in data.Loot)
            {
                loot.Add(Loot.Load(lootPiece));
            }
            
        }
    }



    public bool IsInBackLine()
    {
        return type == "Healer" || type == "Ranged" || type == "Backline";
    }
    public static EnemyData Generate(string type, int degree)
    {
        EnemyData cd = new EnemyData();
        cd.Name = "Generated";
        cd.Degree = degree;
        cd.Weight = 1;
        cd.Health = new HealthData[0];
        cd.Type = type;
        cd.AttackIQ = 5;
        cd.MovementIQ = 5;
        
        return cd;
    }

    public static EnemyData Load(string type, int degree)
    {
        List<EnemyData> characterOptions = DataHolder.availableEnemies.FindAllOfType(type);
        for (int i = characterOptions.Count - 1; i > 0; i--)
        {
            if (characterOptions[i].Degree != degree)
            {
                characterOptions.RemoveAt(i);
            }
        }
        return characterOptions[GameUtils.IndexByWeightedRandom(new List<Weighted>(characterOptions))];
    }
    public static EnemyData Load(string name)
    {
        return DataHolder.availableEnemies.FindFirstOfName(name);
    }
}

public class AttackLayout
{
    public AttackLayout(AttackData[] data)
    {
        attacks = new List<Attack>();
        foreach (var d in data)
        {
            attacks.Add(new Attack(d));
        }
    }
    public List<Attack> attacks { get; private set; }

    public Attack Pick(List<Vector2Int> pos, Vector2Int attackPos, int baseIQ)
    {
        
        foreach (var a in attacks)
        {
            a.Prepare(pos, attackPos, baseIQ);
        }
        return attacks[GameUtils.IndexByWeightedRandom(new List<Weighted>(attacks))];
    }
    
}

public class Attack : Weighted
{
    public Attack(AttackData data)
    {
        weight = data.Weight;
        ability = new Ability(data.Ability);
        switch (data.Shape)
        {
            case "Line":
                shape = new LineAttack(data.Power);
                break;
            case "Square":
                shape = new SquareAttack(data.Power);
                break;
            case "Circle":
                shape = new CircleAttack(data.Power);
                break;
            case "Cross":
                shape = new CrossAttack(data.Power);
                break;

        }
    }

    private List<Vector2Int> preparedShapePosition;
    public void Prepare(List<Vector2Int> characterPositions, Vector2Int attackerPosition, int baseIQ)
    {
        shape.Prepare(characterPositions, attackerPosition);
        preparedShapePosition = shape.GetShape();
        UpdateIntelligenceValue(characterPositions, attackerPosition, baseIQ);
    }

    public int GetShapeSize()
    {
        return shape.Size();
    }

    public List<Vector2Int> GetActiveShape(Vector2Int originPoint)
    {
        List<Vector2Int> list = new List<Vector2Int>();
        foreach (var pos in preparedShapePosition)
        {
            list.Add(pos + originPoint);
        }

        return list;
    }

    public int GetAbilityValue()
    {
        return ability.value;
    }

    private int weight;
    private ShapeAttack shape;
    private Ability ability;
    
    public int GetWeight()
    {
        return currentIntelligenceWeightBonus;
    }
    private int currentIntelligenceWeightBonus = 0;
    public void UpdateIntelligenceValue(List<Vector2Int> characterPositions, Vector2Int attackerPosition, int baseIQ)
    {
        int hits = 0;
        foreach (var shapePos in GetActiveShape(attackerPosition))
        {
            foreach (var cPos in characterPositions)
            {
                if (shapePos == cPos)
                {
                    hits++;
                }
            }
        }

        hits *= ability.value;
        int intel = hits * hits;

        currentIntelligenceWeightBonus = (int)(Mathf.Lerp(weight, intel, baseIQ * 1.0f / DataHolder.currentMode.MaximumIQ) * 100);
    }
    public void Use(EnemyDisplay user)
    {
        GameManager.Instance.AbilityUser = user;
        foreach (var tilePosition in GetActiveShape(user.GetGridPosition()))
        {
            foreach (var character in GameManager.Instance.GetActiveCharacters())
            {
                if(character.GetGridPosition() == tilePosition)
                {
                    ability.Use(character);
                }
            }
            // foreach enemy - self hitting
            // foreach item - destruction
        }
        GameManager.Instance.AbilityInUse = null;
        GameManager.Instance.AbilityUser = null;
    }
}

public abstract class ShapeAttack
{
    protected int power;
    public ShapeAttack(int power)
    {
        this.power = power;
    }

    public abstract int Size();
    public abstract List<Vector2Int> GetShape();

    public virtual void Prepare(List<Vector2Int> characterPositions, Vector2Int attackerPosition)
    {
    }

}

public class LineAttack : ShapeAttack
{
    public LineAttack(int power) : base(power)
    {
        
    }

    public override int Size()
    {
        return power;
    }

    public override List<Vector2Int> GetShape()
    {
        List<Vector2Int> list = new List<Vector2Int>();
        Vector2Int direction = RandomDirection;
        for (int i = 0; i < power; i++)
        {
            list.Add((direction * (i + 1)));
        }
        
        return list;
    }

    public override void Prepare(List<Vector2Int> characterPositions, Vector2Int attackerPosition)
    {
        RandomDirection = BestLineDirection(characterPositions, attackerPosition);
    }
    private static CardinalDirection[] directions = {
        CardinalDirection.North,
        CardinalDirection.East,
        CardinalDirection.South,
        CardinalDirection.West};
    private Vector2Int BestLineDirection(List<Vector2Int> charactersPositions, Vector2Int attackerPosition)
    {
        int[] matches = new int[4];
        foreach (var pos in charactersPositions)
        {
            if(pos.x != attackerPosition.x && pos.y != attackerPosition.y) continue;

            if(pos.x == attackerPosition.x)
            {
                if(pos.y < attackerPosition.y)
                {
                    matches[2]++;
                }
                else
                {
                    matches[0]++;
                }
            }
            else
            {
                if(pos.x < attackerPosition.x)
                {
                    matches[3]++;
                }
                else
                {
                    matches[1]++;
                }
            }
        }
        int maxValue = matches.Max();
        if(maxValue == 0) return Vector2Int.zero;
        List<int> indexesOfMaximums = new List<int>();
        for (int i = 0; i < matches.Length; i++)
        {
            if (matches[i] == maxValue)
            {
                indexesOfMaximums.Add(i);
            }
        }
        return GameUtils.DirectionToVector(directions[indexesOfMaximums[Random.Range(0, indexesOfMaximums.Count)]]);
    }
    

    private Vector2Int rd = Vector2Int.zero;
    private Vector2Int RandomDirection {
        get
        {
            if (rd == Vector2Int.zero) return GameUtils.GetRandomDirection();
            return rd;
        }
        set
        {
            rd = value;
        }
    }
}

public class SquareAttack : ShapeAttack
{
    public SquareAttack(int power) : base(power)
    {
        
    }
    public override int Size()
    {
        return power * power;
    }

    public override List<Vector2Int> GetShape()
    {
        List<Vector2Int> list = new List<Vector2Int>();
        
        
        
        return list;
    }
}

public class CircleAttack : ShapeAttack
{
    public CircleAttack(int power) : base(power)
    {
        
    }
    public override int Size()
    {
        return ((power * power * 3) / 4) + 1;
    }
    public override List<Vector2Int> GetShape()
    {
        List<Vector2Int> list = new List<Vector2Int>();
        
        
        
        return list;
    }
}
public class CrossAttack : ShapeAttack
{
    public CrossAttack(int power) : base(power)
    {
        
    }
    public override int Size()
    {
        return power * 4;
    }
    public override List<Vector2Int> GetShape()
    {
        List<Vector2Int> list = new List<Vector2Int>();

        for (int i = 1; i <= power; i++)
        {
            list.Add((Vector2Int.up * i));
            list.Add((Vector2Int.down * i));
            list.Add((Vector2Int.left * i));
            list.Add((Vector2Int.right * i));
        }
        
        return list;
    }
}