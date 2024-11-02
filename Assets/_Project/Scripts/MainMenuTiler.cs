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
    [SerializeField] private int camSize;
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
        cam.orthographicSize = camSize;
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

    private void Shift()
    {
        nextInterval = Time.time + Random.Range(interval.x, interval.y);
        int xPos = Random.Range(0, size);
        int yPos = Random.Range(0, size);
        bool row = Random.Range(0, 2) == 0;
        bool positive = Random.Range(0, 2) == 0;
        ShowTile firstToMove = null;

        Vector2Int myPos = Vector2Int.zero;
        if (row)
        {
            if (positive)
            {
                Vector2Int direction = Vector2Int.right;
                for (int x = size - 1; x <= 0; x--)
                {
                    myPos = new Vector2Int(x, yPos);
                    if (x == size - 1) firstToMove = grid[myPos];
                    MoveTile(grid[myPos], myPos + direction);
                }
                MoveTile(firstToMove, new Vector2Int(size - 1, yPos), true);
            }
            else
            {
                Vector2Int direction = Vector2Int.left;
                for (int x = 0; x < size; x++)
                {
                    myPos = new Vector2Int(x, yPos);
                    if (x == 0) firstToMove = grid[myPos];
                    MoveTile(grid[myPos], myPos + direction);
                }
                MoveTile(firstToMove, new Vector2Int(size - 1, yPos), true);
            }
        }
        else
        {
            if (positive)
            {
                Vector2Int direction = Vector2Int.up;
                for (int y = size - 1; y <= 0; y--)
                {
                    myPos = new Vector2Int(xPos, y);
                    if (y == size - 1) firstToMove = grid[myPos];
                    MoveTile(grid[myPos], myPos + direction);
                }
                MoveTile(firstToMove, new Vector2Int(xPos, size - 1), true);
            }
            else
            {
                Vector2Int direction = Vector2Int.down;
                for (int y = 0; y < size; y++)
                {
                    myPos = new Vector2Int(xPos, y);
                    if (y == 0) firstToMove = grid[myPos];
                    MoveTile(grid[myPos], myPos + direction);
                }
                MoveTile(firstToMove, new Vector2Int(xPos, size - 1), true);
                
            }
        }
    }

    private void MoveTile(ShowTile tile, Vector2Int newPos, bool instant = false)
    {
        grid[newPos] = tile;
        tile.Move(newPos, instant);
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

