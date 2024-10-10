using System.Collections.Generic;
using Capstone.DataLoad;

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
        
    }
    public List<Attack> attacks { get; private set; }
    
}

public class Attack
{
    public Attack(AttackData data)
    {
        
    }
}