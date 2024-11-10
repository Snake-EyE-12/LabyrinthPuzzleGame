using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using Random = UnityEngine.Random;

public class MainMenuTiler : MonoBehaviour
{
    [SerializeField] private int size;
    [SerializeField] private ShowTile tilePrefab;
    private Dictionary<Vector2Int, ShowTile> grid = new();
    private void Awake()
    {
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                var item = Instantiate(tilePrefab, transform);
                Vector2Int pos = new Vector2Int(x, y);
                grid.Add(pos, item);
                item.Set(new ShowTileData(this, pos, Tile.GenerateRandomTile()));
            }
        }

        cam.transform.position = new Vector3(size - 1, size - 1, -10);
    }

    [SerializeField] private Camera cam;





    private float nextInterval = 0;
    [SerializeField, MinMaxSlider(0, 10)] private Vector2 interval;

    private void Update()
    {
        if (nextInterval < Time.time)
        {
            Shift();
        }
    }

    private bool IsOnScreen(Vector3 pos)
    {
        Vector3 point = cam.WorldToViewportPoint(pos);
        return point.x > -0.1 && point.x < 1.1 && point.y > -0.1 && point.y < 1.1;
    }
    private void Shift()
    {
        nextInterval = Time.time + Random.Range(interval.x, interval.y);
        int xPos = Random.Range(0, size);
        int yPos = Random.Range(0, size);
        bool row = Random.Range(0, 2) == 0;
        bool positive = Random.Range(0, 2) == 0;
        ShowTile firstToMove = null;
        int last = size - 1;
        
        bool didSound = false;
        Vector2Int myPos = Vector2Int.zero;
        if (row)
        {
            if (positive)
            {
                Vector2Int direction = Vector2Int.right;
                firstToMove = grid[new Vector2Int(last, yPos)];
                for (int x = last; x >= 0; x--)
                {
                    myPos = new Vector2Int(x, yPos);
                    MoveTile(grid[myPos], myPos + direction);
                    if(IsOnScreen(grid[myPos].transform.position) && !didSound) { AudioManager.Instance.Play("HeavySlide"); didSound = true; }
                }
                MoveTile(firstToMove, new Vector2Int(0, yPos), true);
            }
            else
            {
                Vector2Int direction = Vector2Int.left;
                firstToMove = grid[new Vector2Int(0, yPos)];
                for (int x = 0; x <= last; x++)
                {
                    myPos = new Vector2Int(x, yPos);
                    MoveTile(grid[myPos], myPos + direction);
                    if(IsOnScreen(grid[myPos].transform.position) && !didSound) { AudioManager.Instance.Play("HeavySlide"); didSound = true; }
                }
                MoveTile(firstToMove, new Vector2Int(last, yPos), true);
            }
        }
        else
        {
            if (positive)
            {
                Vector2Int direction = Vector2Int.up;
                firstToMove = grid[new Vector2Int(xPos, last)];
                for (int y = last; y >= 0; y--)
                {
                    myPos = new Vector2Int(xPos, y);
                    MoveTile(grid[myPos], myPos + direction);
                    if(IsOnScreen(grid[myPos].transform.position) && !didSound) { AudioManager.Instance.Play("HeavySlide"); didSound = true; }
                }
                MoveTile(firstToMove, new Vector2Int(xPos, 0), true);
            }
            else
            {
                Vector2Int direction = Vector2Int.down;
                firstToMove = grid[new Vector2Int(xPos, 0)];
                for (int y = 0; y <= last; y++)
                {
                    myPos = new Vector2Int(xPos, y);
                    MoveTile(grid[myPos], myPos + direction);
                    if(IsOnScreen(grid[myPos].transform.position) && !didSound) { AudioManager.Instance.Play("HeavySlide"); didSound = true; }
                }
                MoveTile(firstToMove, new Vector2Int(xPos, last), true);
                
            }
        }
    }

    private void MoveTile(ShowTile tile, Vector2Int newPos, bool instant = false)
    {
        //Vector2Int actualPos = new Vector2Int(GameUtils.ModPositive(newPos.x, size), GameUtils.ModPositive(newPos.y, size));
        if(WithinGrid(newPos)) grid[newPos] = tile;
        tile.Move(newPos, instant);
    }
    private bool WithinGrid(Vector2Int pos)
    {
        return pos.x >= 0 && pos.x < size && pos.y >= 0 && pos.y < size;
    }
}

public class ShowTileData
{
    public ShowTileData(MainMenuTiler tiler, Vector2Int pos, Tile tile)
    {
        this.tiler = tiler;
        this.pos = pos;
        this.tile = tile;
    }
    public MainMenuTiler tiler;
    public Vector2Int pos;
    public Tile tile;

}

