using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitHandler : MonoBehaviour
{
    
    private Unit unitA;
    private Unit unitB;
    [SerializeField] private UnitDisplay unitDisplayA;
    [SerializeField] private UnitDisplay unitDisplayB;
    public MapHandler myMapHandlerGuy;


    [SerializeField] private List<Unit> units = new List<Unit>();


    private void Start()
    {
        unitA = new Unit();
        myMapHandlerGuy.GetMap().SpawnUnit(unitA, Vector2Int.zero);
        unitA.SetMap(myMapHandlerGuy.GetMap());
        unitA.Move(Vector2Int.up);
        unitA.Move(Vector2Int.up);
        
        unitB = new Unit();
        myMapHandlerGuy.GetMap().SpawnUnit(unitB, Vector2Int.zero);
        unitB.SetMap(myMapHandlerGuy.GetMap());
        unitB.Move(Vector2Int.left);
        unitB.Move(Vector2Int.left);
        
        
        unitDisplayA.Move(unitA);
        unitDisplayB.Move(unitB);
        
        
        
        BuildUnit(myMapHandlerGuy.GetMap(), null);
        BuildUnit(myMapHandlerGuy.GetMap(), null);
        
    }

    public void BuildUnit(Map map, UnitData data)
    {
        //Temporary Self Built Unit
        Unit unit = new Unit();
        unit.SetMap(map);
        unit.Spawn(Vector2Int.zero);
    }
    
}

public class UnitData
{
    
}