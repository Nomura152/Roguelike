using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 持続する攻撃判定を扱うクラス
/// エフェクトなどの当たり判定オブジェクトにアタッチして使用する
/// </summary>
[RequireComponent(typeof(Rigidbody2D))] //判定する際はRigidbody2Dが必須
public sealed class RepeatAttackHitbox : MonoBehaviour
{
    [Header("攻撃設定")]
    [Tooltip("ダメージ値"), SerializeField] float damage = 1f;
    [Tooltip("攻撃のヒット間隔"), SerializeField] float hitInterval = 0.1f;

    //攻撃判定にヒットしている相手と、最後にダメージを与えた時間をペアにしたディクショナリ
    Dictionary<IDamageable, float> lastHitTimeDictionary = new Dictionary<IDamageable, float>();

    private void OnEnable()
    {
        //ディクショナリの情報をリセット
        lastHitTimeDictionary.Clear();
    }

    /// <summary>
    /// 外部からの初期化処理
    /// </summary>
    /// <param name="damage">ダメージ値</param>
    /// <param name="hitInterval">ヒット間隔</param>
    public void Initialize(float damage, float hitInterval)
    {
        this.damage = damage;
        this.hitInterval = hitInterval;
    }

    /// <summary>
    /// 外部からダメージを設定するためのメソッド
    /// </summary>
    /// <param name="damage">設定したいダメージ値</param>
    public void SetDamage(float damage)
    {
        this.damage = damage;
    }

    /// <summary>
    /// 外部からヒット間隔を設定するためのメソッド
    /// </summary>
    /// <param name="hitInterval">設定したいヒット間隔</param>
    public void SetHitInterval(float hitInterval)
    {
        this.hitInterval = hitInterval;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        //判定にヒットした相手オブジェクトから「IDamageable」インターフェースを継承したコンポーネントを探す
        IDamageable damageable = other.GetComponent<IDamageable>();

        //「IDamageable」を取得できなかったら処理終了
        if (damageable == null) { return; }

        //攻撃可能かをメソッド（関数）を呼び出して確認し、falseなら処理終了
        if (CanHit(damageable) == false) { return; }

        //相手のインターフェースのメソッド（関数）を呼び出して攻撃通知！
        damageable.TakeDamage(damage);

        // 最後にダメージを与えた時間を上書き（ディクショナリにデータがない場合は新しく追加される）
        lastHitTimeDictionary[damageable] = Time.time;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        //判定から抜けた相手オブジェクトから「IDamageable」インターフェースを実装したコンポーネントを探す
        IDamageable damageable = other.GetComponent<IDamageable>();

        //「IDamageable」を取得できなかったら処理終了
        if (damageable == null) { return; }

        //ディクショナリから情報を削除
        lastHitTimeDictionary.Remove(damageable);
    }


    /// <summary>
    /// ヒットした相手に攻撃可能かを確認する
    /// </summary>
    /// <param name="damageable">攻撃対象</param>
    /// <returns>攻撃可能かをbool型で返す</returns>
    private bool CanHit(IDamageable damageable)
    {
        //ディクショナリにデータがない場合は、まだ攻撃した事がないので攻撃可能
        if (lastHitTimeDictionary.ContainsKey(damageable) == false)
        {
            return true;
        }

        //ディクショナリから最後にダメージを与えた時の時間を取得
        float lastHitTime = lastHitTimeDictionary[damageable];

        //現在の時間が、「最後にダメージを与えた時の時間＋ヒット間隔」を超えていれば攻撃可能
        if (Time.time >= lastHitTime + hitInterval)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}