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
    public static MapController instance;

    //Map size
    [HideInInspector]
    public int n;

    [HideInInspector]
    public int StartX;

    [HideInInspector]
    public int StartY;

    [HideInInspector]
    public int EndX;

    [HideInInspector]
    public int EndY;

    [HideInInspector]
    public TileType[,] map;

    [HideInInspector]
    public Grid grid;

    [Header("Tile prefabs: ")]
    public GameObject FreeTile;
    public GameObject BlockedTile;

    private void Awake()
    {
        //Using Singleton patern for Map Controller since we know, only one will exist
        instance = this;
        grid = GetComponent<Grid>();

        n = OptionsController.instance.MapSize;
        StartX = OptionsController.instance.XStart;
        StartY = OptionsController.instance.YStart;
        EndX = OptionsController.instance.XEnd;
        EndY = OptionsController.instance.YEnd;

        map = new TileType[n, n];
        GenerateMap();
        RenderMap();

        //Position camera so that the whole map is visible
        PositionCamera();
    }

    private void GenerateMap()
    {
        //Generate map filled with blockers
        List<Vector2Int> blockers = new List<Vector2Int>();
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                map[i, j] = TileType.Blocked;
                blockers.Add(new Vector2Int(i, j));
            }
        }

        //Here we "dig" through blocked, generating a random path that is always guided towards the end. 
        //This effectivelly removes minimum number of blockers to ensure the path exists
        int StartEndDistance = PathfinderHelper.ManhattanDistance(StartX, StartY, EndX, EndY);
        int xOffset = EndX - StartX;
        int yOffset = EndY - StartY;

        int DigX = 0;
        int DigY = 0;

        Random.InitState(System.DateTime.Now.Second);
        map[StartX, StartY] = TileType.Free;
        blockers.Remove(new Vector2Int(StartX, StartY));

        for (int i = 0; i < StartEndDistance; i++)
        {
            if (Random.value < 0.5)
            {
                if (xOffset == 0)
                {
                    if (yOffset > 0)
                    {
                        DigY++;
                        yOffset--;
                    }
                    if (yOffset < 0)
                    {
                        DigY--;
                        yOffset++;
                    }
                }
                else
                {
                    if (xOffset > 0)
                    {
                        DigX++;
                        xOffset--;
                    }
                    if (xOffset < 0)
                    {
                        DigX--;
                        xOffset++;
                    }
                }

            }
            else
            {
                if (yOffset == 0)
                {
                    if (xOffset > 0)
                    {
                        DigX++;
                        xOffset--;
                    }
                    if (xOffset < 0)
                    {
                        DigX--;
                        xOffset++;
                    }
                }
                else
                {
                    if (yOffset > 0)
                    {
                        DigY++;
                        yOffset--;
                    }
                    if (yOffset < 0)
                    {
                        DigY--;
                        yOffset++;
                    }
                }
            }
            map[StartX + DigX, StartY + DigY] = TileType.Free;
            blockers.Remove(new Vector2Int(StartX + DigX, StartY + DigY));
        }


        //Randomly remove blockers so that only specified amount is left
        int numBlockersToRemove = n * n - OptionsController.instance.NumberOfObstacles - StartEndDistance -1;
        for (int i = 0; i < numBlockersToRemove; i++)
        {
            int indexToRemove = Random.Range(0, blockers.Count - 1);
            Vector2Int elementToRemove = blockers[indexToRemove];
            map[elementToRemove.x, elementToRemove.y] = TileType.Free;
            blockers.RemoveAt(indexToRemove);
        }
    }

    //Show the map on screen
    private void RenderMap()
    {
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                GameObject tile;
                if (map[i, j] == TileType.Free)
                {
                    tile = Instantiate(FreeTile, grid.transform);
                }
                else
                {
                    tile = Instantiate(BlockedTile, grid.transform);
                }
                tile.transform.position = CellToWorld(i, j);
            }
        }
    }

    private void PositionCamera()
    {
        if(n % 2 == 1 )
            Camera.main.transform.position = CellToWorld(n / 2, n / 2) + new Vector3(0,0,-10);
        else
            Camera.main.transform.position = CellToWorld(n / 2, n / 2) + new Vector3(-grid.cellSize.x / 2, -grid.cellSize.y / 2, -10);

        Camera.main.orthographicSize = Mathf.Max(n / 2 + n / 4, 5);

    }

    //Wrapper for getting the world location of cell
    public static Vector3 CellToWorld(int x, int y)
    {
        return MapController.instance.grid.GetCellCenterWorld(new Vector3Int(x, y,0));
    }
}
