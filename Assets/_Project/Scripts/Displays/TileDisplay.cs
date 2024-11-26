using System;
using System.Collections;
using System.Collections.Generic;
using Guymon.DesignPatterns;
using UnityEngine;
using UnityEngine.UI;
using EventArgs = Guymon.DesignPatterns.EventArgs;
using EventHandler = Guymon.DesignPatterns.EventHandler;
using Random = UnityEngine.Random;

public class TileDisplay : Display<Tile>, GridPositionable, Selectable
{
    [SerializeField] protected List<WallDisplay> wallDisplays = new List<WallDisplay>();
    [SerializeField] private MappableOrganizer mappableOrganizer;
    [SerializeField] private SelectionDisplay selectionIndicator;
    [SerializeField] private SpriteRenderer abilityIcon;
    public override void Render()
    {
        int orientation = item.GetOrientation();
        for (int i = 0, j = 1; i < 4; i++, j *= 2)
        {
            wallDisplays[i].SetVisibility((orientation & j) == 0);
        }

        if (item.ability != null && !item.ability.usedThisCombat)
        {
            abilityIcon.sprite = Ability.GetAbilityIcon(item.ability);
        }
        else abilityIcon.sprite = null;

    }

    public void SelectViaClick()
    {
        if(IsCurrentlySelectable()) Activate(null);
    }
    public Tile GetTile()
    {
        return item;
    }

    public void Break(int amount)
    {
        int breakages = 0;
        
        if (!item.IsOpen(Vector2Int.up) && amount > breakages)
        {
            breakages++;
            item.OpenPath(Vector2Int.up);
            EffectHolder.Instance.SpawnEffect("WallBreak", wallDisplays[0].transform.position);
        }

        if (!item.IsOpen(Vector2Int.right) && amount > breakages)
        {
            breakages++;
            item.OpenPath(Vector2Int.right);
            EffectHolder.Instance.SpawnEffect("WallBreak", wallDisplays[1].transform.position);
        }

        if (!item.IsOpen(Vector2Int.down) && amount > breakages)
        {
            breakages++;
            item.OpenPath(Vector2Int.down);
            EffectHolder.Instance.SpawnEffect("WallBreak", wallDisplays[2].transform.position);
        }

        if (!item.IsOpen(Vector2Int.left) && amount > breakages)
        {
            breakages++;
            item.OpenPath(Vector2Int.left);
            EffectHolder.Instance.SpawnEffect("WallBreak", wallDisplays[3].transform.position);
        }
        Render();
        
    }
    public void Build(int amount)
    {
        int placements = 0;
        if (item.IsOpen(Vector2Int.up) && amount > placements)
        {
            placements++;
            item.ClosePath(Vector2Int.up);
        }

        if (item.IsOpen(Vector2Int.right) && amount > placements)
        {
            placements++;
            item.ClosePath(Vector2Int.right);
        }

        if (item.IsOpen(Vector2Int.down) && amount > placements)
        {
            placements++;
            item.ClosePath(Vector2Int.down);
        }

        if (item.IsOpen(Vector2Int.left) && amount > placements)
        {
            placements++;
            item.ClosePath(Vector2Int.left);
        }
        Render();
    }
    private void Awake()
    {
        EventHandler.AddListener("Round/FightOver", OnBattleOver);
    }

    private void OnBattleOver(EventArgs args)
    {
        EventHandler.RemoveListener("Round/FightOver", OnBattleOver);
        GameManager.Instance.RemoveSelectable(this, selectionIndicator.type);
        Destroy(this.gameObject);
    }


    public void LoseControl(GridPositionable unit)
    {
        mappableOrganizer.Remove(unit);
    }
    public void GainControl(GridPositionable unit)
    {
        unit.SetGridPosition(GetGridPosition());
        mappableOrganizer.Add(unit);
        if (unit is Targetable && item.ability != null && !item.ability.usedThisCombat)
        {
            StartCoroutine(LandOnTile(0.26f, (unit as Targetable), item.ability, this));
        }
        unit.OnPassOverLoot(mappableOrganizer.GetLoot());
    }

    private IEnumerator LandOnTile(float delay, Targetable t, Ability ability, TileDisplay tile)
    {
        if (!t.GetHealthBar().isDead)
        {
            yield return new WaitForSeconds(delay);

            item.ability.Use(t);
            t.CheckForDeath();
            if (item.ability.usedThisCombat) Render();
        }
    }

    private Vector2Int gridPosition;
    public Vector2Int GetGridPosition()
    {
        return gridPosition;
    }


    [SerializeField] private Destinator destinator;
    public void SetGridPosition(Vector2Int value, bool wrapping = false)
    {
        if (wrapping)
        {
            List<DestinationData> positionSet = new();
            Vector2Int direction = (value - gridPosition).Normalize();
            positionSet.Add(new DestinationData(VisualDataHolder.Instance.CoordsToPosition(value + direction), 0.0001f, false));
            positionSet.Add(new DestinationData(VisualDataHolder.Instance.CoordsToPosition(value), 0.5f, false));
            destinator.MoveTo(positionSet);
        }
        else destinator.MoveTo(VisualDataHolder.Instance.CoordsToPosition(value), false);
        gridPosition = value;
        mappableOrganizer?.ResetPositions(gridPosition);
    }

    

    public OnTileLocation GetTileLocation()
    {
        return OnTileLocation.None;
    }

    public Transform GetSelfTransform()
    {
        return gameObject.transform;
    }

    public void SetLocalMap(Map map)
    {
        localMap = map;
    }

    public void OnPassOverLoot(List<LootDisplay> loot)
    {
        
    }

    public void MoveVisually(Vector3 position)
    {
        
    }


    private Map localMap;
    public void SetOntoMap(Map map, Vector2Int position)
    {
        //SetGridPosition(position, false);
        gridPosition = position;
        transform.position = VisualDataHolder.Instance.CoordsToPosition(position);
        map.SpawnTile(this, position);
        SetLocalMap(map);

    }

    public bool IsOpen(Vector2Int direction)
    {
        return item.IsOpen(direction);
    }
    private void Start()
    {
        GameManager.Instance.AddSelectable(this, selectionIndicator.type);
    }

    public void Select()
    {
        if (!IsCurrentlySelectable()) return;
        selectionIndicator.StartSelection();
    }

    public void Deselect()
    {
        selectionIndicator.EndSelection();
    }

    public int GetOrderValue()
    {
        return gridPosition.y * DataHolder.currentMode.GridSize - gridPosition.x;
    }

    public bool IsCurrentlySelectable()
    {
        return GameManager.Instance.GetSelectionType() == SelectableGroupType.Tile;;
    }

    public void Activate(SelectableActivatorData data)
    {
        if(GameManager.Instance.cardToPlace.GetCard().GetTile().type == "Slide")
        {
            bool onRow = GameUtils.IsDirectionRow(GameManager.Instance.DirectionToSlide);
            bool posDirection = GameUtils.IsDirectionPositive(GameManager.Instance.DirectionToSlide);
            //localMap.Slide(onRow, posDirection, ((onRow) ? gridPosition.y : gridPosition.x));
            CommandHandler.Execute(new SlideCommand(onRow, posDirection, ((onRow) ? gridPosition.y : gridPosition.x), localMap, GameManager.Instance.cardToPlace.GetCard(), GameManager.Instance.activeDeck));
        }
        else
        {
            CommandHandler.Execute(new SwapCommand(localMap, GameManager.Instance.cardToPlace.GetCard(), gridPosition, GameManager.Instance.activeDeck));
            //localMap.Swap(gridPosition, GameManager.Instance.cardToPlace.GetCard().GetTile());
        }
        EventHandler.Invoke("CardPlaced", new CardEventArgs(GameManager.Instance.cardToPlace.GetCard()));
        GameManager.Instance.cardToPlace = null;
    }
}

