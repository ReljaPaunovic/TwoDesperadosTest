using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType
{
    Free,
    Blocked
}

public class MapController : MonoBehaviour
{
    public TileType[][] map;
    private Grid grid;

    private void Awake()
    {
        grid = GetComponent<Grid>();
        GenerateMap();
        RenderMap();
    }

    private void GenerateMap()
    {
        int n = OptionsController.instance.MapSize;

        for(int i = 0; i < n; i++)
        {
            for(int j = 0; j < n; j++)
            {
                map[i][j] = TileType.Free;
            }
        }

    }

    private void RenderMap()
    {
        int n = OptionsController.instance.MapSize;

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                grid.CellToWorld(new Vector3Int)
            }
        }
    }

    private Vector3 CellToWorld(int x, int y)
    {
        return grid.CellToWorld(new Vector3Int(x, y,0));
    }
}
