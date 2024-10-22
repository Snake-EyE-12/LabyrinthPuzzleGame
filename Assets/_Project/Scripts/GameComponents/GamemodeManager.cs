using System.Collections;
using System.Collections.Generic;
using Capstone.DataLoad;
using Guymon.DesignPatterns;
using UnityEngine;

public class GamemodeManager : Singleton<GamemodeManager>
{
    [SerializeField] private GamemodeDisplay modeDisplayPrefab;
    [SerializeField] private Transform modeDisplayParent;
    public void PrepareGamemodes(List<Mode> modes)
    {
        foreach (var mode in modes)
        {
            CreateGamemode(mode);
        }
    }

    private void CreateGamemode(Mode mode)
    {
        GamemodeDisplay modeDisplay = Instantiate(modeDisplayPrefab, modeDisplayParent);
        modeDisplay.Set(mode);
    }
    
    public void Load(Mode mode)
    {
        DataHolder.currentMode = mode;
        SceneChanger.LoadScene("Game");
    }
}