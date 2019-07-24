using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brave : Actor
{
    private IEnumerator DelayMethod(float waitTime, System.Action action)
    {
        yield return new WaitForSeconds(waitTime);
        action();
    }

    public int[,] Route { get; set; } = new int[ControlTile.WIDTH, ControlTile.HEIGHT];
    public override void Birth()
    {
        int[,] currentRoute = GetTileState();
        Route = currentRoute;
        ControlTile.PrintTile(GetRouteState);
        Route = RouteSearch(currentRoute);
        ControlTile.PrintTile(GetRouteState);
        Debug.Log("Birth()");
    }

    public override void Attack()
    {
        throw new System.NotImplementedException();
    }

    public override void Eat()
    {
        throw new System.NotImplementedException();
    }

    public override void Move()
    {
        throw new System.NotImplementedException();
    }

    public override void Death()
    {
        throw new System.NotImplementedException();
    }

    int GetRouteState(int x, int y)
    {
        return Route[x, y];
    }

    /* 現在のマップの通路情報を取得する関数
     * スタート（入り口）は-1、空洞は0、壁が1、ゴール（魔王のいるところ）が2となる
     */
    int[,] GetTileState()
    {
        //返り値用の配列を作成
        int[,] currentTile = new int[ControlTile.WIDTH, ControlTile.HEIGHT];

        // 配列に通路情報を入れていく
        for (int x = 0; x <= currentTile.GetUpperBound(0); x++)
        {
            for (int y = 0; y <= currentTile.GetUpperBound(1); y++)
            {
                currentTile[x, y] = ControlTile.GetTileState(x, y);
            }
        }

        return currentTile;
    }


    /* 経路探索を行う関数
     * アルゴリズムとしては
     * ランダムな方向に移動 -> つきあたりまで移動
     * 行き止まりの場合は、歩いたルートを戻って分岐を探す
     * を繰り返してゴールにたどり着いたら終了
     */ 
    int[,] RouteSearch(int[,] route)
    {
        int[,] resultRoute = route;
        int count = -1;
        int i = 0, j = 0, k = 0;
        bool flag = false;
        Vector2Int current = new Vector2Int(ControlTile.centerX, ControlTile.HEIGHT - 2);
        Vector2Int progDirect = new Vector2Int();

        //ゴールと入り口の指定
        resultRoute[ControlTile.GoalPoint.x, ControlTile.GoalPoint.y] = 2;
        resultRoute[current.x, current.y] = -1;
        count--;
        Debug.Log("current = " + current);

        while (resultRoute[current.x, current.y] != 2)
        {
            i++;
            if (i == 100)
            {
                Debug.Log("1ループ目で無限ループ");
                break;
            }

            progDirect = DecideDirect(resultRoute, current);
            if (progDirect == Vector2Int.zero)
            {
                Debug.Log("はまった");
                break;
            }
            Debug.Log("prog = " + progDirect);

            while(IsNewPassage(current + progDirect, resultRoute))
            {
                j++;
                if (j >= 100)
                {
                    Debug.Log("2ループ目で無限ループ");
                    break;
                }
                
                current += progDirect;
                if(resultRoute[current.x, current.y] == 2)
                {
                    Debug.Log("RouteSearch is end");
                    flag = true;
                    break;
                }
                resultRoute[current.x, current.y] = count--;
            }
            if(flag)
                break;
            
            while (!FindNewPassage(current, resultRoute))
            {
                k++;
                current += GoBack(current, resultRoute);
                Debug.Log("current = " + current);
                if (k >= 100)
                {
                    k = 0;
                    Debug.Log("3ループ目で無限ループ");
                    break;
                }
            }
            
        }
        

        return resultRoute;
    }
    private Vector2Int GoBack(Vector2Int current, int[,] tile)
    {
        Vector2Int surround = new Vector2Int();
        Vector2Int returnVector = new Vector2Int();
        int currentCount = tile[current.x, current.y];
        int surroundCount = currentCount;

        surround = current + Vector2Int.up;
        if(tile[surround.x, surround.y] < 0)
        {
            Debug.Log("tile_up =" + tile[surround.x, surround.y]);
            surroundCount = tile[surround.x, surround.y];
            returnVector = Vector2Int.up;
            if (surroundCount == currentCount + 1)
                return returnVector;
        }

        surround = current + Vector2Int.right;
        if (tile[surround.x, surround.y] < 0)
        {
            Debug.Log("tile_right =" + tile[surround.x, surround.y]);
            if (tile[surround.x, surround.y] > surroundCount)
            {
                surroundCount = tile[surround.x, surround.y];
                returnVector = Vector2Int.right;
            }
            if (surroundCount == currentCount + 1)
                return returnVector;
        }
        surround = current + Vector2Int.down;
        if (tile[surround.x, surround.y] < 0)
        {
            Debug.Log("tile_down =" + tile[surround.x, surround.y]);
            if (tile[surround.x, surround.y] > surroundCount)
            {
                surroundCount = tile[surround.x, surround.y];
                returnVector = Vector2Int.down;
            }
            if (surroundCount == currentCount + 1)
                return returnVector;
        }
        surround = current + Vector2Int.left;
        if (tile[surround.x, surround.y] < 0)
        {

            Debug.Log("tile_left =" + tile[surround.x, surround.y]);
            if (tile[surround.x, surround.y] > surroundCount)
            {
                surroundCount = tile[surround.x, surround.y];
                returnVector = Vector2Int.left;
            }
        }
        return returnVector;
    }

    private static bool IsNewPassage(Vector2Int vector, int[,] tile)
    {
        return IsNewPassage(vector.x, vector.y, tile);
    }

    private static bool IsNewPassage(int x, int y, int[,] tile)
    {
        if (tile[x, y] == 0 || tile[x, y] == 2)
            return true;
        return false;
    }
    private static bool FindNewPassage(Vector2Int current, int[,] tile)
    {
        if (IsNewPassage(current + Vector2Int.up, tile))
            return true;
        if (IsNewPassage(current + Vector2Int.right, tile))
            return true;
        if (IsNewPassage(current + Vector2Int.down, tile))
            return true;
        if (IsNewPassage(current + Vector2Int.left, tile))
            return true;
        return false;
    }

    Vector2Int DecideDirect(int[,] tile, Vector2Int current)
    {
        Vector2Int vector = new Vector2Int(0, 0);
        int count = 0;
        if (IsNewPassage(current + Vector2Int.up, tile))
        {
            vector = Vector2Int.up;
            count++;
        }
        if (IsNewPassage(current + Vector2Int.right, tile))
        {
            if (Random.Range(0, count + 1) == count)
                vector = Vector2Int.right;
            count++;
        }
        if (IsNewPassage(current + Vector2Int.down, tile))
        {
            if (Random.Range(0, count + 1) == count)
                vector = Vector2Int.down;
            count++;
        }
        if (IsNewPassage(current + Vector2Int.left, tile))
        {
            if (Random.Range(0, count + 1) == count)
                vector = Vector2Int.left;
        }
        return vector;
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DelayMethod(10f, Birth));
        //Invoke("Birth", 3f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
