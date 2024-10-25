using System;
using System.Collections;
using System.Collections.Generic;
using Capstone.DataLoad;
using Guymon.DesignPatterns;
using UnityEngine;
using UnityEngine.Serialization;
using EventArgs = Guymon.DesignPatterns.EventArgs;
using EventHandler = Guymon.DesignPatterns.EventHandler;
using Random = UnityEngine.Random;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private RoundMenuDisplay roundMenuDisplayPrefab;
    [HideInInspector] public int currentRound = 0;
    [SerializeField] private EventBuilder eventBuilder;
    [SerializeField] private Transform canvasTransform;
    [FormerlySerializedAs("slideDisplay")] [SerializeField] private Director slide;

    public void DisplayDirection(Vector2Int gridPos, bool sliding)
    {
        slide.Display(DirectionToSlide, DataHolder.currentMode.GridSize, gridPos, sliding);
    }

    public void HideSliderDisplay()
    {
        slide.Hide(null);
    }


    public void MoveEnemies()
    {
        Vector2Int center = GetCenter();
        Vector2Int averageTeamPos = GetAverageTeamPos();
        Vector2Int averageEnemyPos = GetAverageEnemyPos();
        foreach (var enemy in activeEnemies)
        {
            enemy.Move(enemy.FindSmartDirectionToMove(center, GetClosestUnit(enemy.GetGridPosition(), new List<GridPositionable>(activeTeam)), GetClosestUnit(enemy.GetGridPosition(), new List<GridPositionable>(activeEnemies)), averageTeamPos, averageEnemyPos));
        }
    }

    private Vector2Int GetClosestUnit(Vector2Int pos, List<GridPositionable> units)
    {
        if(units.Count <= 0) return pos;
        units.Sort((a, b) => (a.GetGridPosition() - pos).sqrMagnitude.CompareTo((b.GetGridPosition() - pos).sqrMagnitude));
        return units[0].GetGridPosition();
    }
    
    private Vector2Int GetCenter()
    {
        int size = DataHolder.currentMode.GridSize;
        return new Vector2Int(size / 2 + BonusSizeIncrease(size), size / 2 + BonusSizeIncrease(size));
    }

    private Vector2Int GetAverageTeamPos()
    {
        Vector2Int pos = Vector2Int.zero;
        foreach (var character in activeTeam)
        {
            pos += character.GetGridPosition();
        }
        pos /= activeTeam.Count;
        return pos;
    }
    private Vector2Int GetAverageEnemyPos()
    {
        Vector2Int pos = Vector2Int.zero;
        foreach (var enemy in activeEnemies)
        {
            pos += enemy.GetGridPosition();
        }
        pos /= activeTeam.Count;
        return pos;
    }

    private int BonusSizeIncrease(int size)
    {
        if(size == 1 || size % 2 == 1) return 0;
        return Random.Range(0, 2) - 1;
    }
    
    
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
        Clean();
    }
    public void UseActiveCharacterAbility(CharacterDisplay target)
    {
        AbilityInUse.Use(target);
        Clean();
    }

    public void Clean()
    {
        AbilityInUse = null;
        AbilityUser = null;
        selector.FullCancel();
        SetSelectionMode(SelectableGroupType.Team);
        Phase = GamePhase.None;
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
                    if (activeEnemies.Count <= 0) WinFight();
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
                    if(activeTeam.Count <= 0) Lose();
                    return;
                }
            }
            
        }
    }

    private void Lose()
    {
        Debug.Log("You Lose");
        FightOver();
    }

    private void WinFight()
    {
        Debug.Log("You Won Fight: " + currentRound);
        FightOver();
        ContinueMission();
    }

    private void FightOver()
    {
        EventHandler.Invoke("Round/FightOver", null);
        //EventHandler.Invoke("Ability/UsedAbility", null);
        AttackIndicator.Instance.ClearAttacks();
        selector.Empty();
        Phase = GamePhase.None;
        ;
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
        if (currentRound > DataHolder.currentMode.Rounds)
        {
            CompleteGame();
            return;
        }
        Instantiate(roundMenuDisplayPrefab, eventMenuParent).Set(DataHolder.eventsForEachRound[currentRound - 1]);
    }

    private void CompleteGame()
    {
        SceneChanger.LoadScene("Win");
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

    private CardinalDirection directionToSlide = CardinalDirection.North;
    public CardinalDirection DirectionToSlide
    {
        get
        {
            return directionToSlide;
        }
        set
        {
            directionToSlide = value;
            slide.ChangeRotation(value);
        }
    }

    public Ability AbilityInUse { get; set; }
    public Targetable AbilityUser { get; set; }
    public GamePhase Phase { get; set; }
    public int CoinCount { get; set; }
    public TargetRadius TargetRadius { get; set; }

    public bool InActiveSelectionRange(Vector2Int pos)
    {
        if (TargetRadius == null) return true;
        return TargetRadius.InCircleRange(pos);
    }
}

public enum GamePhase
{
    None,
    UsingActiveAbility
}

public interface Targetable
{
    public void HitByAbility(Ability ability);
    public void ChangeHealth(int amount);
    public void ApplyEffect(ActiveEffect effect);
    public void Move(Vector2Int direction);
    public Vector2Int GetGridPosition();

    public void BecomeUsed();
    public void CheckForDeath();
    public Map GetMap();
    public void GainXP(int amount);
}
