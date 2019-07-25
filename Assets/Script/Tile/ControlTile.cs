using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

enum Direct { up, right, down, left }
struct TileInfo
{
    public int state;
    public int nourish;
    public int magish;
    public GameObject[] monsters;
}

public class ControlTile : MonoBehaviour
{
    public const int WIDTH = 41, HEIGHT = 17, DIRECTIONNUM = 4;
    /* xが幅の半分(20.5)から+1ずつ、yが-高さ(17?)から+1ずつ増えていくので、
     * xはtransformに20.5を足した値を、yはtransformに0.5を足してyの絶対値を用いる
     */
    //private static int[,] tile = new int[width, height];
    private static TileInfo[,] tile = new TileInfo[WIDTH, HEIGHT];
    public const int tileOffsetX = WIDTH / 2 + 1;
    public const int tileOffsetY = HEIGHT;
    public const int centerX = WIDTH / 2;
    const int firstDigHeight = 3;
    const int firstDigWidth = 2;
    //public Tilemap rockTilemap;
    public TileBase nourish0;

    public static Vector2Int GoalPoint { get; set; } = new Vector2Int(0, 0);

    private static Tilemap rockTilemap;

    //tile[][] の一番外側に壁を現す値を入れる
    public static void InitTileArrey()
    {
        /*
        for (int i = 0; i < ControlTile.width; i++)
        {
            tile[i, ControlTile.height - 1] = 1;
            tile[i, 0] = 1;
        }
        for (int i = 0; i < ControlTile.height; i++)
        {
            tile[ControlTile.width - 1, i] = 1;
            tile[0, i] = 1;
        }
        */

        for (int x = 0; x <= tile.GetUpperBound(0); x++)
        {
            for (int y = 0; y <= tile.GetUpperBound(1); y++)
            {
                
                SetTileState(x, y, 1);
                
            }
        }
        //最初の穴を開けておく
        for (int y = tile.GetUpperBound(1) - 1; y >= tile.GetUpperBound(1) - firstDigHeight; y--)
        {
            SetTileState(centerX, y, 0);
            if (y == tile.GetUpperBound(1) - firstDigHeight)
            {
                SetTileState(centerX + 1, y, 0);
                GoalPoint = new Vector2Int(centerX + 1, y);
            }
        }
    }

    private static void RenderMap(TileInfo[,] map, Tilemap tilemap, TileBase tile)
    {
        //マップをクリアする（重複しないようにする）
        tilemap.ClearAllTiles();
        //マップの幅の分、周回する
        for (int x = map.GetLowerBound(0) + 1; x < map.GetUpperBound(0); x++)
        {
            //マップの高さの分、周回する
            for (int y = map.GetLowerBound(1) + 1; y < map.GetUpperBound(1); y++)
            {
                // 1 = タイルあり、0 = タイルなし
                if (map[x, y].state == 1)
                {
                    tilemap.SetTile(new Vector3Int(x - tileOffsetX, y - tileOffsetY, 0), tile);
                }
            }
        }
    }

    public static int GetTileState(int width, int height)
    {
        return tile[width, height].state;
    }

    public static bool SetTileState(int width, int height, int setNum)
    {
        if (width >= ControlTile.WIDTH || width < 0 || height >= ControlTile.HEIGHT || height < 0)
        {
            Debug.Log("Worng wid or high");
            return false;
        }
        tile[width, height].state = setNum;
        return true;
    }

    public static bool IsWall(Vector2Int vector)
    {
        return IsWall(vector.x, vector.y);
    }

    public static bool IsWall(int x, int y)
    {
        if (tile.GetLowerBound(0) < x && x < tile.GetUpperBound(0) && tile.GetLowerBound(1) < y && y < tile.GetUpperBound(1))
            return false;
        return true;
    }

    public static bool Dig(int x, int y)
    {
        if (IsSurroundWall(x, y))
            return false;
        SetTileState(x, y, 0);
        //controltile.rockTilemap.SetTile(new Vector3Int(x - tileOffsetX, y - tileOffsetY, 0), null);
        DeleteMapTile(x, y, ControlTile.rockTilemap);
        return true;
    }
    
    public static bool IsSurroundWall(int x, int y)
    {
        if (GetTileState(x, y + 1) == 1 && GetTileState(x + 1, y) == 1 && GetTileState(x, y - 1) == 1 && GetTileState(x - 1, y) == 1)
            return true;
        return false;
    }

    private static void DeleteMapTile(int x,int y, Tilemap tilemap)
    {
        tilemap.SetTile(new Vector3Int(x - tileOffsetX, y - tileOffsetY, 0), null);
    }

    public delegate int GetSomeState(int x, int y);

    public static void PrintTile(GetSomeState getSome)
    {
        string[] log = new string[HEIGHT];
        for (int y = 0; y <= tile.GetUpperBound(1); y++)
            log[y] = "y =" + y.ToString("000") + " ";

        for (int y = tile.GetUpperBound(1); y >= tile.GetLowerBound(1); y--)
        {

            for (int x = 0; x <= tile.GetUpperBound(0); x++)
            {

                log[tile.GetUpperBound(1) - y] += getSome(x,y).ToString();

            }
        }
        for (int i = 0; i < HEIGHT; i++)
        {
            Debug.Log(log[i]);
        }
        
    }

    // Start is called before the first frame update
    void Start()
    {
        rockTilemap = GameObject.Find("Rock Tilemap").GetComponent(typeof(Tilemap)) as Tilemap;
        InitTileArrey();
        //PrintTile(GetTileState);
        RenderMap(tile, rockTilemap, nourish0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
