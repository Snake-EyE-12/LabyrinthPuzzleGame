using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitHandler : MonoBehaviour
{
    private Unit unit;
    [SerializeField] private UnitDisplay unitDisplay;
    public MapHandler myMapHandlerGuy;


    private void Start()
    {
        unit = new Unit();
        myMapHandlerGuy.GetMap().SpawnUnit(unit, Vector2Int.zero);
        unit.SetMap(myMapHandlerGuy.GetMap());
        unit.Move(Vector2Int.up);
        unit.Move(Vector2Int.up);
        unitDisplay.Move(unit);
    }
}
