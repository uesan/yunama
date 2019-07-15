using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Kind {slime, bug, lizard, elemental, succubus, dragon, daemon, unknown, brave}
enum Direct {up, right, down, left}

public class Monster : MonoBehaviour
{
    public const int width = 41, height = 17, directionNum = 4;
    /* xが幅の半分(20.5)から+1ずつ、yが-高さ(17?)から+1ずつ増えていくので、
     * xはtransformに20.5を足した値を、yはtransformに0.5を足してyの絶対値を用いる
     */
    public int[,] tile = new int[width, height];
    public const int tileOffsetX = width / 2 + 1;
    public const int tileOffsetY = height;

    string monsterName = "Monster";
    /**
     * "kind" show what type of monster.
     */
    Kind kind = Kind.unknown;
    Kind[] feed;
    Kind[] enemy;
    int attackDamage = 0;//攻撃力
    int health = 0;     //体力
    int nourish = 0;    //養分
    int magish = 0;     //魔分
    int losehealth = 0; //1秒ごとに失う体力
    int lifespan = 0;   //寿命
    float speed = 5f;   //移動スピード
    int x, y;
    
    Vector3 target = new Vector3(0, 0, 0);//移動目標
    Vector3 targetDelta = new Vector3(0, 1f, 0);

    void InitTileArrey()
    {
        for (int i = 0; i < width; i++)
        {
            tile[i, height - 1] = 1;
            tile[i, 0] = 1;
        }
        for (int i = 0; i < height; i++)
        {
            tile[width - 1, i] = 1;
            tile[0, i] = 1;
        }
    }
    void Birth()
    {
        //プレハブからインスタンスを生成し、体力や魔分・養分の初期値を決める
    }

    void Attack()
    {
        //攻撃の判定を行う
    }

    void Eat()
    {
        //捕食した際の回復などを行う
    }

    void Move()
    {
        //モンスターの移動を行う
        if (this.transform.position == target)
        {
            int tmpx = x, tmpy = y;
            if (targetDelta.x > 0) tmpx++;
            else if (targetDelta.x < 0) tmpx--;
            if (targetDelta.y > 0) tmpy++;
            else if (targetDelta.y < 0) tmpy--;
            /*
            Debug.Log(x);
            Debug.Log(y);
            Debug.Log(tmpx);
            Debug.Log(tmpy);
            */

            if (/*進行方向に壁があるか*/tile[tmpx, tmpy] == 1)
            {
                int noWallDirection = 0;
                int[] aroundTile = { tile[x, y + 1], tile[x + 1, y], tile[x, y - 1], tile[x - 1, y] };
                for (int i = 0; i < directionNum; i++)
                    if(aroundTile[i] == 0)
                        noWallDirection++;

                //壁がない方向のうち１つをランダムに決定
                int moveDirect = Random.Range(0, noWallDirection);
                Direct direct = Direct.up;
                Debug.Log(moveDirect);

                for (int i = 0,count = 0; i < directionNum; i++)
                {
                    if (aroundTile[i] == 0)
                    {
                        if (count != moveDirect)
                        {
                            count++;
                            Debug.Log("count = " + count);
                            continue;
                        }
                    }
                    else continue;
                    direct = (Direct)i;
                    Debug.Log("i =" + i);
                    break;
                }

                Debug.Log(direct);

                switch (direct)
                {
                    case Direct.up:
                        targetDelta = new Vector3(0, 1f, 0);
                        break;
                    case Direct.right:
                        targetDelta = new Vector3(1f, 0, 0);
                        break;
                    case Direct.down:
                        targetDelta = new Vector3(0, -1f, 0);
                        break;
                    case Direct.left:
                        targetDelta = new Vector3(-1f, 0, 0);
                        break;
                    default:
                        break;
                }

                Debug.Log(tile[17, 16]);
            }
            target += targetDelta;
            x = (int)(target.x + tileOffsetX);
            y = (int)(target.y + tileOffsetY);

        }
        this.transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
    }

    void Death()
    {
        //死亡時の養分・魔分飛散を行う
    }


    // Start is called before the first frame update
    void Start()
    {
        InitTileArrey();
        x = (int)(transform.position.x + tileOffsetX);
        y = (int)(transform.position.y + tileOffsetY);
        target = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }
}
