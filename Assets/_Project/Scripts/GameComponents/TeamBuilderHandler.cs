using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Capstone.DataLoad;
using UnityEngine;
using Random = UnityEngine.Random;

public class TeamBuilderHandler : MonoBehaviour
{
    [SerializeField] private TeamOptionDisplay teamOptionDisplayPrefab;
    [SerializeField] private MemberOptionDisplay memberOptionDisplayPrefab;
    [SerializeField] private Transform currentTeamParent;
    [SerializeField] private Transform optionsParent;
    
    private TeamBuilder teamBuilder;
    private void Awake()
    {
        switch (DataHolder.currentMode.CharacterSelection.Type)
        {
            case "Draft":
                teamBuilder = new DraftTeam(DataHolder.currentMode.CharacterSelection, this);
                break;
            case "Choose":
                teamBuilder = new ChooseTeam(DataHolder.currentMode.CharacterSelection, this);
                break;
            case "Preset":
                teamBuilder = new PresetTeam(DataHolder.currentMode.CharacterSelection, this);
                break;
            default:
                break;
        }

        teamBuilder.Load();
    }

    private string[] team;
    private List<string> teamCreationList = new List<string>();
    public void SetTeam(string[] team)
    {
        this.team = team;
        Finish();
    }

    public void Pick(string character, Transform selectedCharacterObject)
    {
        selectedCharacterObject.SetParent(currentTeamParent);
        foreach (Transform child in optionsParent)
        {
            Destroy(child.gameObject);
        }
        teamCreationList.Add(character);
        if (teamCreationList.Count >= DataHolder.currentMode.CharacterSelection.Total)
        {
            team = teamCreationList.ToArray();
            Finish();
            return;
        }

        LoadNextOption();
    }

    public void PickFull(string[] team)
    {
        SetTeam(team);
    }

    public void DisplayTeamOptions(List<CharacterLayout> options)
    {
        foreach (var team in options)
        {
            TeamOptionDisplay teamOptionDisplay = Instantiate(teamOptionDisplayPrefab, optionsParent);
            teamOptionDisplay.Set(team);
            teamOptionDisplay.SetHandler(this);
        }
    }

    public void DisplayDraftCharactersOptions(List<CharacterColorData> options)
    {
        foreach (var member in options)
        {
            MemberOptionDisplay memberOptionDisplay = Instantiate(memberOptionDisplayPrefab, optionsParent);
            memberOptionDisplay.Set(member);
            memberOptionDisplay.SetHandler(this);
        }
    }

    public void LoadNextOption()
    {
        teamBuilder.Next();
    }

    public void Finish()
    {
        GameManager.Instance.SetTeam(new List<string>(team));
        Destroy(gameObject);
    }
}


public class CharacterColorData
{
    public CharacterColorData(Color color, string type)
    {
        this.color = color;
        this.type = type;
    }
    public Color color;
    public string type;
}

public abstract class TeamBuilder
{
    protected CharacterSelection selection;
    protected TeamBuilderHandler handler;
    public TeamBuilder(CharacterSelection characterSelection, TeamBuilderHandler handler)
    {
        selection = characterSelection;
        this.handler = handler;
    }

    public abstract void Next();
    public abstract void Load();
}

public class DraftTeam : TeamBuilder
{
    public DraftTeam(CharacterSelection characterSelection, TeamBuilderHandler handler) : base(characterSelection, handler)
    {
    }

    public override void Next()
    {
        Load();
    }

    public override void Load()
    {
        List<CharacterColorData> ccdList = new();
        for (int i = 0; i < selection.Choices; i++)
        {
            ccdList.Add(GetRandomMember());
        }
        handler.DisplayDraftCharactersOptions(ccdList);
    }

    private CharacterColorData GetRandomMember()
    {
        CharacterColorEquivalence cce = DataHolder.characterColorEquivalenceTable.Equivalences[Random.Range(0, 6)];
        return new CharacterColorData(cce.Color, cce.Type);
    }
}
public class ChooseTeam : TeamBuilder
{
    public ChooseTeam(CharacterSelection characterSelection, TeamBuilderHandler handler) : base(characterSelection, handler)
    {
    }

    public override void Next()
    {
    }

    public override void Load()
    {
        List<CharacterLayout> possibleTeams = new List<CharacterLayout>();
        for (int i = 0; i < selection.Choices; i++)
        {
            possibleTeams.Add(GetRandomTeam());
        }
        handler.DisplayTeamOptions(possibleTeams);
    }

    private CharacterLayout GetRandomTeam()
    {
        return DataHolder.characterLayoutTable.Layouts[GameUtils.IndexByWeightedRandom(new List<Weighted>(DataHolder.characterLayoutTable.Layouts))];
    }
}
public class PresetTeam : TeamBuilder
{
    public PresetTeam(CharacterSelection characterSelection, TeamBuilderHandler handler) : base(characterSelection, handler)
    {
    }

    public override void Next()
    {
        
    }

    public override void Load()
    {
        handler.SetTeam((DataHolder.characterLayoutTable.GetLayout(selection.Default).ToArray()));
    }
}