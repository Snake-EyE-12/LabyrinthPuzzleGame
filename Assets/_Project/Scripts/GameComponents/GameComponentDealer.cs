using System.Collections;
using System.Collections.Generic;
using Capstone.DataLoad;
using Guymon.DesignPatterns;
using UnityEngine;

public static class GameComponentDealer
{
    public static CharacterData GetCharacterData(string type, int degree)
    {
        return (UseGeneratedVersion()) ? Character.Generate(type, degree) : Character.Load(type, degree);
    }

    private static bool UseGeneratedVersion()
    {
        return false;
    }
}
