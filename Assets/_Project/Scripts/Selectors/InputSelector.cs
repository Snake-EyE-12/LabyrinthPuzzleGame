using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventArgs = Guymon.DesignPatterns.EventArgs;
using EventHandler = Guymon.DesignPatterns.EventHandler;

public class InputSelector : MonoBehaviour
{
    private List<SelectorGroup> groups = new List<SelectorGroup>();
    
    private SelectorGroup active;

    private void Awake()
    {
        groups.Add(new SelectorGroup(SelectableGroupType.Card, new CardGameInput(this)));
        groups.Add(new SelectorGroup(SelectableGroupType.Tile, new TileGameInput(this)));
        groups.Add(new SelectorGroup(SelectableGroupType.Team, new CharacterMovementGameInput(this)));
        groups.Add(new SelectorGroup(SelectableGroupType.Enemy, new EnemyGameInput(this)));
        groups.Add(new SelectorGroup(SelectableGroupType.Ability, new AbilityGameInput(this)));
        EventHandler.AddListener("CardPlaced", Reorder);
    }

    public bool SelectionEnabled { get; set; }
    

    public void FullCancel()
    {
        ChangeSelectionGroup(null);
    }

    public void Empty()
    {
        foreach (var group in groups)
        {
            group.Clear();
        }
    }

    private void Update()
    {
        if (active == null || !SelectionEnabled) return;
        if (Input.GetKeyDown(KeyCode.Tab)) //Forced Keycode
        {
            active.Shift();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            active.Cancel();
        }
        
        active.UpdateInput();
    }

    public void ChangeSelectionType(SelectableGroupType newType)
    {
        if (!SelectionEnabled) return;
        foreach (var group in groups)
        {
            if (group.Type.Equals(newType))
            {
                ChangeSelectionGroup(group);
                return;
            }
        }

        active = null;
    }

    private void ChangeSelectionGroup(SelectorGroup newGroup)
    {
        active?.Cancel();
        active = newGroup;
    }

    private void Reorder(EventArgs args)
    {
        active?.OrderSelectables();
    }
    
    public void AddSelectable(Selectable selectable, SelectableGroupType groupType)
    {
        groups[((int)(groupType)) - 1].Add(selectable);
    }
    public void RemoveSelectable(Selectable selectable, SelectableGroupType groupType)
    {
        groups[((int)(groupType)) - 1].Remove(selectable);
    }

    
    public Selectable GetSelected()
    {
        if (active == null) return null;
        return active.ChooseSelected();
    }

    public SelectableGroupType GetActiveType()
    {
        if (active == null) return SelectableGroupType.None;
        return active.Type;
    }

    public void Activate(SelectableActivatorData data)
    {
        if (active == null) return;
        active.Activate(data);
    }
}

public enum SelectableGroupType
{
    None,
    Card,
    Tile,
    Team,
    Enemy,
    Ability
    
}

public class SelectorGroup
{
    private int currentSelectedIndex = -1;
    private List<Selectable> selectables = new List<Selectable>();

    public SelectableGroupType Type { get; private set; }
    public SelectorGroup(SelectableGroupType selectType, GameInput inputType)
    {
        Type = selectType;
        input = inputType;
    }

    private GameInput input;

    public void UpdateInput()
    {
        if(input != null) input.Update();
    }

    public void Activate(SelectableActivatorData data)
    {
        if(currentSelectedIndex == -1 || !selectables[currentSelectedIndex].IsCurrentlySelectable()) return;
        selectables[currentSelectedIndex].Activate(data);
    }
    public void Cancel()
    {
        foreach (var selectable in selectables)
        {
            selectable.Deselect();
        }
        currentSelectedIndex = -1;
    }

    public void Clear()
    {
        selectables.Clear();
        currentSelectedIndex = -1;
    }
    public void Add(Selectable selectable)
    {
        selectables.Add(selectable);
        OrderSelectables();
        currentSelectedIndex = -1;
    }
    public void Remove(Selectable selectable)
    {
        selectables.Remove(selectable);
        OrderSelectables();
        currentSelectedIndex = -1;
    }

    public Selectable ChooseSelected()
    {
        
        if (currentSelectedIndex == -1 || !selectables[currentSelectedIndex].IsCurrentlySelectable())
        {
            return null;
        }
        return selectables[currentSelectedIndex];
    }

    public void Shift()
    {
        if (currentSelectedIndex == -1)
        {
            AttemptStartSelection();
            return;
        }

        selectables[currentSelectedIndex].Deselect();
        currentSelectedIndex = FindNextSelectable();
        if(currentSelectedIndex == -1) return;
        selectables[currentSelectedIndex].Select();
    }

    private int FindNextSelectable()
    {
        int traverseCount = 0;
        do
        {
            traverseCount++;
            currentSelectedIndex = (currentSelectedIndex + 1) % selectables.Count;
        } while (!selectables[currentSelectedIndex].IsCurrentlySelectable() && traverseCount <= selectables.Count);

        if (traverseCount > selectables.Count) return -1;
        return currentSelectedIndex;
    }
    private void AttemptStartSelection()
    {
        if(selectables.Count == 0) return;
        for (int i = 0; i < selectables.Count; i++)
        {
            if (selectables[i].IsCurrentlySelectable())
            {
                currentSelectedIndex = i;
                selectables[currentSelectedIndex].Select();
                return;
            }
        }
    }

    public void OrderSelectables()
    {
        selectables.Sort((a, b) => b.GetOrderValue().CompareTo(a.GetOrderValue()));
    }
}

public interface Selectable
{
    public void Select();
    public void Deselect();
    public int GetOrderValue();
    public bool IsCurrentlySelectable();
    public void Activate(SelectableActivatorData data);
}
public class SelectableActivatorData
{
    
}
public class DirectionalSelectableActivatorData : SelectableActivatorData
{
    public DirectionalSelectableActivatorData(Vector2Int value)
    {
        direction = value;
    }
    public Vector2Int direction;
}

public class ConfirmSelectableActivatorData : SelectableActivatorData
{
    
}