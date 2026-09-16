using System;
using UnityEngine;

/// <summary>
/// 武器のレベル毎に適用するパラメータ
/// </summary>
[Serializable]
public class WeaponLevelData
{
    [Header("武器に適用するパラメータ")]
    [Tooltip("基礎ダメージ"), SerializeField] float damage = 1f;
    [Tooltip("攻撃間隔"), SerializeField] float attackInterval = 1f;
    [Tooltip("弾数・攻撃判定の数"), SerializeField] int attackCount = 1;
    [Tooltip("攻撃判定の基礎サイズ倍率"), SerializeField] float scale = 1f;
    [Tooltip("弾速"), SerializeField] float projectileSpeed = 1f;
    [Tooltip("攻撃判定の持続時間"), SerializeField] float duration = 0.5f;
    [Tooltip("持続攻撃のヒット間隔"), SerializeField] float hitInterval = 0.2f;


    //------------------<プロパティ群>---------------------

    public float Damage => damage;
    public float AttackInterval => attackInterval;
    public int AttackCount => attackCount;
    public float Scale => scale;
    public float ProjectileSpeed => projectileSpeed;
    public float Duration => duration;
    public float HitInterval => hitInterval;
}
