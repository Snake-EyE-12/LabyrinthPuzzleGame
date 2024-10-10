using System.Collections.Generic;
using Capstone.DataLoad;

public class Character : Unit
{
    public string characterType { get; private set; }
    public int charge { get; private set; }
    public Inventory inventory { get; private set; }



    public Character(CharacterData data)
    {
        unitName = data.Name;
        characterType = data.Type;
        degree = data.Degree;
        health = new Health(data.Health);
        charge = data.Charge;
        inventory = data.Inventory;
        activeEffects = new ActiveEffect(data.ActiveEffects);
    }




    public static CharacterData Generate(string type, int degree)
    {
        CharacterData cd = new CharacterData();
        cd.Name = "Generated";
        cd.Type = type;
        cd.Degree = degree;
        cd.Health = new HealthData[0];
        cd.Charge = 5;
        cd.Inventory = new Inventory();
        
        return cd;
    }

    public static CharacterData Load(string type, int degree)
    {
        List<CharacterData> characterOptions = DataHolder.availableCharacters.FindAllOfType(type);
        for (int i = characterOptions.Count - 1; i > 0; i--)
        {
            if (characterOptions[i].Degree != degree)
            {
                characterOptions.RemoveAt(i);
            }
        }
        return characterOptions[GameUtils.IndexByWeightedRandom(new List<Weighted>(characterOptions))];
    }
}
