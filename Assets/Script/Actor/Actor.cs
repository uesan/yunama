using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Actor : MonoBehaviour
{
    /**
     * "kind" show what type of Actor.
     */
    public string ActorName { get; set; } = "Actor";
    public Kind ActorKind { get; set; } = Kind.unknown;
    public Kind[] Enemy { get; set; } = null;
    public int AttackDamage { get; set; } = 0;//攻撃力
    public int Health { get; set; } = 0;     //体力 誤ツルハシ防止のため誕生から0.2秒ぐらいはHPが減らないようにしたい
    public int Nourish { get; set; } = 0;    //養分
    public int Magish { get; set; } = 0;     //魔分
    public float Speed { get; set; } = 5f;   //移動スピード

    // プレハブからインスタンスを生成し、体力や魔分・養分の初期値を決める
    public abstract void Birth();

    // 攻撃の判定を行う
    public abstract void Attack();

    // 捕食した際の回復などを行う
    public abstract void Eat();

    // アクターの移動を行う
    public abstract void Move();

    // 死亡時の養分・魔分飛散を行う
    public abstract void Death();

    // toStringのオーバーライド
    public override string ToString()
    {
        return this.ActorName;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
