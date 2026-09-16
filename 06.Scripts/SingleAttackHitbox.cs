using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 単発の攻撃判定を扱うクラス
/// 弾や斬撃などの当たり判定オブジェクトにアタッチして使用する
/// </summary>
[RequireComponent(typeof(Rigidbody2D))] //判定する際はRigidbody2Dが必須
public class SingleAttackHitbox : MonoBehaviour
{
    [Header("攻撃設定")]
    [Tooltip("ダメージ値"), SerializeField] float damage = 1f;

    //攻撃判定にヒットした相手の情報を記録するリスト
    List<IDamageable> hitList = new List<IDamageable>();

    void OnEnable()
    {
        //ヒット情報のリストをリセット
        hitList.Clear();
    }

    /// <summary>
    /// 外部からの初期化処理
    /// </summary>
    /// <param name="damage">ダメージ値</param>
    public void Initialize(float damage)
    {
        this.damage = damage;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //ヒットした相手オブジェクトから「IDamageable」インターフェースを継承したコンポーネントを探す
        IDamageable damageable = other.GetComponent<IDamageable>();

        //「IDamageable」を取得できなかったら処理終了
        if (damageable == null) { return; }

        //既にヒット済リストに情報があれば処理終了
        if (hitList.Contains(damageable) == true) { return; }

        //相手のインターフェースのメソッド（関数）を呼び出して攻撃通知！
        damageable.TakeDamage(damage);

        //ヒット済リストに情報を追加
        hitList.Add(damageable);
    }
}