using System.Collections.Generic;
using Capstone.DataLoad;
using UnityEngine;

public class Enemy : Unit
{
    public int weight { get; private set; }
    public string type { get; private set; }
    public int IQ { get; private set; }
    public AttackLayout attackLayout { get; private set; }



    public Enemy(EnemyData data)
    {
        unitName = data.Name;
        degree = data.Degree;
        weight = data.Weight;
        health = new Health(data.Health);
        type = data.Type;
        IQ = data.IQ;
        attackLayout = new AttackLayout(data.AttackLayout);
        activeEffects = new ActiveEffect(data.ActiveEffects);
    }




    public static EnemyData Generate(string type, int degree)
    {
        EnemyData cd = new EnemyData();
        cd.Name = "Generated";
        cd.Degree = degree;
        cd.Weight = 1;
        cd.Health = new HealthData[0];
        cd.Type = type;
        cd.IQ = 5;
        
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

    public Attack Pick()
    {
        foreach (var a in attacks)
        {
            a.UpdateIntelligenceValue();
        }
        Attack choosen = attacks[GameUtils.IndexByWeightedRandom(new List<Weighted>(attacks))];
        choosen.Prepare();
        return choosen;
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

    private List<Vector2Int> presetShape;
    public void Prepare()
    {
        presetShape = PresetShape();
    }

    public List<Vector2Int> GetActiveShape(Vector2Int originPoint)
    {
        List<Vector2Int> list = new List<Vector2Int>();
        foreach (var shape in presetShape)
        {
            list.Add(shape + originPoint);
        }

        return list;
    }

    public string GetDescription()
    {
        string output = "";
        output += ability.value;
        return output;
    }

    private int weight;
    private ShapeAttack shape;
    private Ability ability;
    
    public int GetWeight()
    {
        return weight + currentIntelligence;
    }
    private int currentIntelligence = 0;
    public void UpdateIntelligenceValue()
    {
        currentIntelligence = 0; // update for smartness
    }
    public List<Vector2Int> PresetShape()
    {
        return shape.GetShape();
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

    public abstract List<Vector2Int> GetShape();

    protected Vector2Int GetRandomDirection()
    {
        switch (Random.Range(0, 4))
        {
            case 0:
                return Vector2Int.up;
            case 1:
                return Vector2Int.right;
            case 2:
                return Vector2Int.down;
            default:
                return Vector2Int.left;
            
        }
    }
}

public class LineAttack : ShapeAttack
{
    public LineAttack(int power) : base(power)
    {
        
    }

    public override List<Vector2Int> GetShape()
    {
        List<Vector2Int> list = new List<Vector2Int>();
        Vector2Int direction = GetRandomDirection();
        for (int i = 0; i < power; i++)
        {
            list.Add((direction * (i + 1)));
        }
        
        return list;
    }
}

public class SquareAttack : ShapeAttack
{
    public SquareAttack(int power) : base(power)
    {
        
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