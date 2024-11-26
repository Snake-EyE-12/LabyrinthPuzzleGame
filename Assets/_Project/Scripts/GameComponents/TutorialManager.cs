using System;
using System.Collections.Generic;
using Guymon.DesignPatterns;
using UnityEngine;

public class TutorialManager : Singleton<TutorialManager>
{
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    [SerializeField] private List<TutorialPiece> pieces = new();

    public void Teach(string id)
    {
        foreach (var tut in pieces)
        {
            if (tut.GetName().Equals(id))
            {
                if (tut.HasBeenSeen()) return;
                tut.Show();
            }
        }
    }

    public void Reset()
    {
        foreach (var tut in pieces)
        {
            tut.Reset();
        }
        
    }
}
