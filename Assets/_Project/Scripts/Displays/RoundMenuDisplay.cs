using System;
using System.Collections;
using System.Collections.Generic;
using Capstone.DataLoad;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using EventArgs = Guymon.DesignPatterns.EventArgs;
using EventHandler = Guymon.DesignPatterns.EventHandler;

public class RoundMenuDisplay : Display<EventsForRound>
{
    [SerializeField] private EventAcceptorButtonDisplay eventAcceptorButtonDisplayPrefab;
    [SerializeField] private LayoutGroup buttonParent;
    [SerializeField] private Button arrowButton;
    [SerializeField] private Transform teamParent;
    [SerializeField] private PostBattleCharacterDisplay postBattleCharacterDisplayPrefab;
    [SerializeField] private TMP_Text coins;
    [SerializeField] private InventoryCharmDisplay inventoryCharmDisplayPrefab;
    [SerializeField] private Transform inventoryCharmParent;
    [SerializeField] private LayoutDisable layoutDisabler;
    [SerializeField] private TMP_Text roundNumberTextbox;
    
    public override void Render()
    {
        foreach (var e in item.events)
        {
            EventAcceptor ea = CreateEventAcceptor(e);
            if(ea != null) Instantiate(eventAcceptorButtonDisplayPrefab, buttonParent.transform).Set(ea);
        }
        roundNumberTextbox.text = GameManager.Instance.currentRound + "/" + DataHolder.currentMode.Rounds;
        layoutDisabler.AllowDisable();

        //buttonParent.enabled = false;

        foreach (var c in GameManager.Instance.GetCurrentTeam())
        {
            Instantiate(postBattleCharacterDisplayPrefab, teamParent).Set(c);
        }

        coins.text = GameManager.Instance.CoinCount + "";
        UpdateCharms();
        ActivateButton();
    }

    public void UpdateCharms()
    {
        foreach (var c in charms)
        {
            if(c != null) Destroy(c.gameObject);
        }
        foreach (var iCharm in GameManager.Instance.GetAllInventoryCharms())
        {
            CreateCharm(iCharm);
        }
    }
    private List<InventoryCharmDisplay> charms = new List<InventoryCharmDisplay>();
    private void CreateCharm(Item charm)
    {
        InventoryCharmDisplay icd = Instantiate(inventoryCharmDisplayPrefab, inventoryCharmParent);
        icd.Set(charm);
        charms.Add(icd);
    }
    
    
    private FightEvent fightExists;
    private bool challengeExists;
    private bool shopExists;

    private EventAcceptor CreateEventAcceptor(EventData e)
    {
        switch (e.Type)
        {
            case "Fight":
                if (fightExists != null) return null;
                fightExists = new FightEvent(e, this);
                arrowButton.gameObject.SetActive(false);
                return fightExists;
            case "Challenge":
                if (challengeExists) return null;
                challengeExists = true;
                return new ChallengeEvent(e, this);
            case "Shop":
                if (shopExists) return null;
                shopExists = true;
                return new ShopEvent(e, this);
            default:
                return null;
        }
    }

    public void ContinueClick()
    {
        AudioManager.Instance.Play("ButtonClick");
        if (fightExists == null)
        {
            
            GameManager.Instance.ContinueMission();
        }
        else
        {
            AudioManager.Instance.Play("ButtonClick");
            GameManager.Instance.LoadFight(fightExists.GetExtras());
            GameManager.Instance.BeginBoardFight();
        }
        Destroy(gameObject);
    }

    public void ActivateButton()
    {
        arrowButton.gameObject.SetActive(true);
    }
    private void SetCoinValue(EventArgs args)
    {
        coins.text = (args as IntEventArgs).value + "";
    }

    public void UpTheAnte()
    {
        if(fightExists == null) return;
        fightExists.UpTheAnte(1); // Number (enemies to add to fight)
    }

    private void OnEnable()
    {
        EventHandler.AddListener("Coins/Change", SetCoinValue);
    }

    private void OnDisable()
    {
        EventHandler.RemoveListener("Coins/Change", SetCoinValue);
    }
}

public class IntEventArgs : EventArgs
{
    public int value;
}