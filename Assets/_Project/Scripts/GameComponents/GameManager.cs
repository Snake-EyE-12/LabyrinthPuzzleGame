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
    [SerializeField] private Director slide;
    [SerializeField] private ColorShift camColorShifter;
    [SerializeField] private Color lightColorEventPage;
    [SerializeField] private Color darkColorEventPage;
    [SerializeField] private Color lightColorFight;
    [SerializeField] private Color darkColorFight;



    public DeckDisplay activeDeck;

    private RoundMenuDisplay activeRoundMenuDisplay;
    private List<Item> inventoryCharms = new();

    public void RemoveCharm(Item charm)
    {
        inventoryCharms.Remove(charm);
    }
    public void GainCharm(Item charm)
    {
        DataHolder.itemsCollected++;
        inventoryCharms.Add(charm);
        if (activeRoundMenuDisplay != null) activeRoundMenuDisplay.UpdateCharms();
    }
    public void GainCharm(int degree)
    {
        if (degree == -1) degree = currentRound;
        GainCharm(new Item(Item.Load(degree)));
    }

    public List<Item> GetAllInventoryCharms()
    {
        return inventoryCharms;
    }




    public Character UpgradeCharacter(Character c)
    {
        for (int i = 0; i < team.Count; i++)
        {
            if (team[i] == c)
            {
                team[i] = new Character(GameComponentDealer.GetCharacterData(c.characterType,
                    c.degree + 1));
                
                return team[i];
            }
        }

        return null;
    }
    
    public void DisplayDirection(Vector2Int gridPos, bool sliding)
    {
        slide.Display(DirectionToSlide, DataHolder.currentMode.GridSize, gridPos, sliding);
    }

    [SerializeField] private ShopDisplay shopDisplayPrefab;
    private ShopDisplay activeShop;
    public void ShowShop()
    {
        if (activeShop != null) activeShop.Open();
        else
        {
            activeShop = Instantiate(shopDisplayPrefab, canvasTransform);
            activeShop.Set(GetRoundShopData());
        }
    }

    private ShopData GetRoundShopData()
    {
        return new ShopData(currentRound);
    }

    public void HideSliderDisplay()
    {
        slide.Hide(null);
    }

    
    [SerializeField] private DeckDisplay deckDisplayPrefab;

    private void BuildDeck()
    {
        List<Card> cardsForDeck = new List<Card>();
        foreach (var c in GetCurrentTeam())
        {
            cardsForDeck.AddRange(c.inventory.GetCards());
        }

        DeckDisplay deck = Instantiate(deckDisplayPrefab, canvasTransform);
        deck.Set(new Deck(cardsForDeck));
        deck.transform.SetSiblingIndex(1);
        activeDeck = deck;
        DataHolder.finalDeckSize = cardsForDeck.Count;
    }

    public void MoveEnemies()
    {
        Vector2Int center = GetCenter();
        Vector2Int averageTeamPos = GetAverageTeamPos();
        Vector2Int averageEnemyPos = GetAverageEnemyPos();
        foreach (var enemy in activeEnemies)
        {
            enemy.MoveToPlace(enemy.FindSmartDirectionToMove(center, GetClosestUnit(enemy.GetGridPosition(), new List<GridPositionable>(activeTeam)), GetClosestUnit(enemy.GetGridPosition(), new List<GridPositionable>(activeEnemies)), averageTeamPos, averageEnemyPos));
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
        SetSelectionMode(SelectableGroupType.Team);
        Clean();
    }
    public void UseActiveCharacterAbility(CharacterDisplay target)
    {
        AbilityInUse.Use(target);
        SetSelectionMode(SelectableGroupType.Team);
        Clean();
    }

    public void Clean()
    {
        AbilityInUse = null;
        AbilityUser = null;
        selector.FullCancel();
        Phase = GamePhase.None;
    }

    public void SetSelectionEnabled(bool enabled)
    {
        selector.SelectionEnabled = enabled;
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
        if (activeEnemies.Count <= 0)
        {
            WinFight();
            return;
        }
        if (activeTeam.Count <= 0)
        {
            Lose();
            return;
        }
        turnManager.NextPhase();
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
                    DataHolder.enemiesKilled++;
                    //if (activeEnemies.Count <= 0) WinFight();
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
                    //if(activeTeam.Count <= 0) Lose(); // Immediate Lose
                    return;
                }
            }
            
        }
    }

    private void Lose()
    {
        DataHolder.defeatedRound = currentRound;
        EventHandler.Invoke("OnGameLost", null);
        ResetGameManagerForNextGame();
        //Debug.Log("You Lose");
        //FightOver();
    }

    private void ResetGameManagerForNextGame()
    {
        Destroy(this.gameObject);
        EventHandler.ClearListeners();
        CommandHandler.Clear();
        Clean();
    }

    private void WinFight()
    {
        Debug.Log("You Won Fight: " + currentRound);
        HealCharacters();
        FightOver();
        ContinueMission();
    }

    private void HealCharacters()
    {
        foreach (var member in team)
        {
            member.health.Reset(DataHolder.currentMode.PostBattleHealPercent);
        }
    }

    private void FightOver()
    {
        EventHandler.Invoke("Round/FightOver", null);
        EventHandler.Invoke("Ability/DestroyPanel", null);
        AttackIndicator.Instance.ClearAttacks();
        selector.Empty();
        Phase = GamePhase.None;
    }
    
    
    private List<EnemyDisplay> activeEnemies = new List<EnemyDisplay>();
    public List<EnemyDisplay> GetActiveEnemies()
    {
        return activeEnemies;
    }
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

    public void ContinueMission()
    {
        currentRound++;
        if (currentRound > DataHolder.currentMode.Rounds)
        {
            CompleteGame();
            return;
        }
        activeRoundMenuDisplay = Instantiate(roundMenuDisplayPrefab, canvasTransform);
        activeRoundMenuDisplay.Set(DataHolder.eventsForEachRound[currentRound - 1]);
        camColorShifter.SetColorSet(lightColorEventPage, darkColorEventPage);
    }

    private void CompleteGame()
    {
        SceneChanger.LoadScene("Win");
    }

    [SerializeField] private TurnManager turnManager;
    public void BeginBoardFight()
    {
        activeRoundMenuDisplay = null;
        BuildDeck();
        turnManager.Reset();
        turnManager.NextPhase();
        camColorShifter.SetColorSet(lightColorFight, darkColorFight);
    }

    public void LoadFight(int additions)
    {
        eventBuilder.PrepareFight(additions);
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
        cd.SetAsActiveSelection((cd != null));
        if (cd == null) return;
        cardToPlace = cd;
        SetSelectionMode(SelectableGroupType.Tile);
    }

    public SelectableGroupType GetSelectionType()
    {
        return selector.GetActiveType();
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
    private Targetable abilityUser;
    public Targetable AbilityUser
    {
        get { return abilityUser; }
        set
        {
            if(abilityUser != null) abilityUser.Active(false);
            abilityUser = value;
        }
    }
    public GamePhase Phase { get; set; }
    private int money = 100;
    public int CoinCount
    {
        get { return money;}
        set
        {
            money = value;
            EventHandler.Invoke("Coins/Change", new IntEventArgs() { value = value });
        }
    }
    public TargetRadius TargetRadius { get; set; }
    public Map ActiveMap { get; set; }

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
    public void ApplyEffect(ActiveEffectType effect);
    public List<ActiveEffectType> GetEffects();
    public void MoveToPlace(Vector2Int direction);
    public Vector2Int GetGridPosition();

    public void BecomeUsed();
    public void CheckForDeath();
    public Map GetMap();
    public void GainXP(int amount);
    public void Active(bool active);
}
