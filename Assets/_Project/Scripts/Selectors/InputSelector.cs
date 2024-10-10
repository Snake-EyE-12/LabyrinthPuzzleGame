using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputSelector : MonoBehaviour
{
    private List<SelectorGroup> groups = new List<SelectorGroup>();
    
    private SelectorGroup active;

    private void Awake()
    {
        groups.Add(new SelectorGroup(SelectableGroupType.Card));
        groups.Add(new SelectorGroup(SelectableGroupType.Tile));
        groups.Add(new SelectorGroup(SelectableGroupType.Team));
        groups.Add(new SelectorGroup(SelectableGroupType.Enemy));
    }

    public void ChangeSelectionType(SelectableGroupType newType)
    {
        foreach (var group in groups)
        {
            if (group.Type.Equals(newType))
            {
                active = group;
                return;
            }
        }

        active = null;
    }

    private void ChangeSelectionGroup(SelectorGroup newGroup)
    {
        active.Cancel();
        active = newGroup;
    }
    
    public void AddSelectable(Selectable selectable, SelectableGroupType groupType)
    {
        groups[((int)(groupType)) - 1].Add(selectable);
    }

    public void SelectFirst()
    {
        if (active == null) return;
        active.Shift();
        
    }
}

public enum SelectableGroupType
{
    None,
    Card,
    Tile,
    Team,
    Enemy
    
}

public class SelectorGroup
{
    private int currentSelectedIndex = -1;
    private List<Selectable> selectables = new List<Selectable>();

    public SelectableGroupType Type { get; private set; }
    public SelectorGroup(SelectableGroupType selectType)
    {
        Type = selectType;
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
        currentSelectedIndex = (currentSelectedIndex + 1) % selectables.Count;
        selectables[currentSelectedIndex].Select();
    }

    private void AttemptStartSelection()
    {
        if(selectables.Count == 0) return;
        currentSelectedIndex = 0;
        selectables[currentSelectedIndex].Select();
    }

    private void OrderSelectables()
    {
        selectables.Sort((a, b) => a.GetOrderValue().CompareTo(b.GetOrderValue()));
    }
}

public interface Selectable
{
    public void Select();
    public void Deselect();
    public int GetOrderValue();
    public bool IsCurrentlySelectable();
}