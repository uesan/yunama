using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Kind {slime, bug, lizard, elemental, succubus, dragon, daemon, unknown, brave}

public class Monster : MonoBehaviour
{
    string monsterName = "Monster";
    /**
     * "kind" show what type of monster.
     */
    Kind kind = Kind.unknown;
    Kind[] feed;
    Kind[] enemy;
    int attackDamage = 0;//攻撃力
    int health = 0;     //体力 誤ツルハシ防止のため誕生から0.2秒ぐらいはHPが減らないようにしたい
    int nourish = 0;    //養分
    int magish = 0;     //魔分
    int losehealth = 0; //1秒ごとに失う体力
    int lifespan = 0;   //寿命
    float speed = 5f;   //移動スピード
    int x, y;
    
    Vector3 target = new Vector3(0, 0, 0);//移動目標
    Vector3 targetDelta = new Vector3(0, 1f, 0);

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

            if (/*進行方向に壁があるか*/ControlTile.GetTileState(tmpx, tmpy) == 1)
            {
                int noWallDirection = 0;
                int[] aroundTile = { ControlTile.GetTileState(x, y + 1), ControlTile.GetTileState(x + 1, y), ControlTile.GetTileState(x, y - 1), ControlTile.GetTileState(x - 1, y) };
                for (int i = 0; i < ControlTile.directionNum; i++)
                    // まわりの通路の数を数える
                    if(aroundTile[i] == 0)
                        noWallDirection++;

                //壁がない方向のうち１つをランダムに決定
                int moveDirect = Random.Range(0, noWallDirection);
                Direct direct = Direct.up;
                Debug.Log(moveDirect);

                for (int i = 0,count = 0; i < ControlTile.directionNum; i++)
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

                //壁でない移動方向に向かうように目標を変更
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
                
            }
            target += targetDelta;
            x = (int)(target.x + ControlTile.tileOffsetX);
            y = (int)(target.y + ControlTile.tileOffsetY);

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
        x = (int)(transform.position.x + ControlTile.tileOffsetX);
        y = (int)(transform.position.y + ControlTile.tileOffsetY);
        target = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }
}
