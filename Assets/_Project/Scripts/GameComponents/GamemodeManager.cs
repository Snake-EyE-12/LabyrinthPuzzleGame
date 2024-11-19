using System.Collections;
using System.Collections.Generic;
using Capstone.DataLoad;
using Guymon.DesignPatterns;
using UnityEngine;

public class GamemodeManager : Singleton<GamemodeManager>
{
    [SerializeField] private GamemodeDisplay modeDisplayPrefab;
    [SerializeField] private Transform modeDisplayParent;
    private DataHandler handler;
    public void PrepareGamemodes(List<Mode> modes, DataHandler handler)
    {
        this.handler = handler;
        foreach (var mode in modes)
        {
            if(mode.Active) CreateGamemode(mode);
        }
    }

    private void CreateGamemode(Mode mode)
    {
        GamemodeDisplay modeDisplay = Instantiate(modeDisplayPrefab, modeDisplayParent);
        modeDisplay.Set(mode);
    }
    
    public void Load(Mode mode)
    {
        handler.ReadDataFromMode(mode);
        EventHandler.ClearListeners();
        SceneChanger.LoadScene("Game");
    }
}