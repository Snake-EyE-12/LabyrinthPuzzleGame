using System;
using System.Collections;
using System.Collections.Generic;
using Capstone.DataLoad;
using Guymon.DesignPatterns;
using UnityEngine;
using EventArgs = Guymon.DesignPatterns.EventArgs;
using EventHandler = Guymon.DesignPatterns.EventHandler;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private RoundMenuDisplay roundMenuDisplayPrefab;
    [HideInInspector] public int currentRound = 0;
    [SerializeField] private EventBuilder eventBuilder;
    [SerializeField] private Transform canvasTransform;
    
    private List<Character> team = new List<Character>();
    public void SetTeam(List<string> teamLayout)
    {
        for (int i = 0; i < teamLayout.Count; i++)
        {
            team.Add(new Character(GameComponentDealer.GetCharacterData(teamLayout[i],
                DataHolder.currentMode.CharacterSelection.InitialDegree)));
            eventBuilder.SetUpEventOptions();
        }
        ContinueMission();
        


    }

    public void UseActiveCharacterAbility(EnemyDisplay target)
    {
        AbilityInUse.Use(target);
        AbilityInUse = null;
        AbilityUser = null;
        selector.FullCancel();
        SetSelectionMode(SelectableGroupType.Team);
    }

    public List<CharacterDisplay> GetActiveCharacters()
    {
        return activeTeam;
    }

    public void PickEnemyAttacks()
    {
        foreach (var enemy in activeEnemies)
        {
            enemy.ChooseAttack();
        }
        turnManager.NextPhase();
    }
    public void UseEnemyAttacks()
    {
        foreach (var enemy in activeEnemies)
        {
            enemy.UseAttack();
        }
        turnManager.NextPhase();
    }
    

    public void CheckGameOver()
    {
        if (activeTeam.Count <= 0)
        {
            //lose
        }
        else
        {
            turnManager.NextPhase();
        }
    }

    // public IEnumerator DelayMethodCall(float time, Action action)
    // {
    //     yield return new WaitForSeconds(time);
    //     action();
    // }

    public void PrepareTeamTurnStart()
    {
        foreach (var member in activeTeam)
        {
            member.BecomeAvailable();
        }
    }

    public void KillUnit(Unit u)
    {
        if (u is Enemy)
        {
            Enemy deadEnemy = u as Enemy;
            foreach (var ed in activeEnemies)
            {
                if (ed.GetEnemy().Equals(deadEnemy))
                {
                    ed.Vanish();
                    CheckFightOver();
                    return;
                }
            }
        }

        if (u is Character)
        {
            Character deadCharacter = u as Character;
            foreach (var cd in activeTeam)
            {
                if (cd.GetCharacter().Equals(deadCharacter))
                {
                    cd.Vanish();
                    Lose();
                    return;
                }
            }
            
        }
    }

    private void Lose()
    {
        Debug.Log("You Lose");
    }

    private void CheckFightOver()
    {
        if (activeEnemies.Count <= 0)
        {
            EventHandler.Invoke("Round/FightOver", null);
            EventHandler.Invoke("Ability/UsedAbility", null);
            ContinueMission();
        }
        
    }
    
    
    private List<EnemyDisplay> activeEnemies = new List<EnemyDisplay>();
    public void AddEnemey(EnemyDisplay e)
    {
        activeEnemies.Add(e);
    }
    public void RemoveEnemy(EnemyDisplay e)
    {
        activeEnemies.Remove(e);
    }
    private List<CharacterDisplay> activeTeam = new List<CharacterDisplay>();
    public void AddCharacter(CharacterDisplay c)
    {
        activeTeam.Add(c);
    }
    public void RemoveCharacter(CharacterDisplay c)
    {
        activeTeam.Remove(c);
    }

    public Transform GetCanvasParent()
    {
        return canvasTransform;
    }

    [SerializeField] private Transform eventMenuParent;
    public void ContinueMission()
    {
        currentRound++;
        Instantiate(roundMenuDisplayPrefab, eventMenuParent).Set(DataHolder.eventsForEachRound[currentRound]);
        
    }

    [SerializeField] private TurnManager turnManager;
    public void BeginBoardFight()
    {
        turnManager.Reset();
        turnManager.NextPhase();
    }

    public void PrepareEvent(EventData ed)
    {
        eventBuilder.PrepareEvent(ed);
    }

    public List<Character> GetCurrentTeam()
    {
        return team;
    }

    [SerializeField] private InputSelector selector;
    public void AddSelectable(Selectable selectable, SelectableGroupType groupType)
    {
        selector.AddSelectable(selectable, groupType);
    }
    public void RemoveSelectable(Selectable selectable, SelectableGroupType groupType)
    {
        selector.RemoveSelectable(selectable, groupType);
    }
    

    

    public void SetSelectionMode(SelectableGroupType type)
    {
        selector.ChangeSelectionType(type);
        
    }
    
    
    
    

    public CardDisplay cardToPlace { get; set; }

    public void SetCardToPlace(CardDisplay cd)
    {
        cardToPlace = cd;
        SetSelectionMode(SelectableGroupType.Tile);
    }

    public void RotateSelectedTile(RotationDirection d, int i)
    {
        cardToPlace.RotateTile(d, i);
    }

    public CardinalDirection DirectionToSlide { get; set; }

    public Ability AbilityInUse { get; set; }
    public Targetable AbilityUser { get; set; }
}

public interface Targetable
{
    public void HitByAbility(Ability ability);
    public void ChangeHealth(int amount);
    public void ApplyEffect(ActiveEffect effect);
    public void Move(Vector2Int direction);
    public Vector2Int GetGridPosition();

    public void BecomeUsed();
}
