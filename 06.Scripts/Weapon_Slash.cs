using UnityEngine;

/// <summary>
/// 一定時間ごとに斬撃を出す武器
/// </summary>
public sealed class Weapon_Slash : WeaponBase
{
    [Header("斬撃の攻撃判定")]
    [SerializeField] SingleAttackHitbox slashHitboxPrefab;

    [Header("斬撃の出現位置")]
    [SerializeField] Transform spawnPoint;

    float attackTimer;

    protected override void OnInitialized()
    {
        attackTimer = 0f;
    }

    protected override void OnLevelUp()
    {
        //今回は特に特別な処理は無いので空でOK
        //レベルアップ時にエフェクトやSEを出したいならここに書く
    }

    private void Update()
    {
        //時間を加算
        attackTimer += Time.deltaTime;

        //斬撃エフェクト生成のインターバル時間を「武器レベルデータのインターバル時間」×「プレイヤーの発射間隔倍率」で計算する
        float interval = CurrentLevelData.AttackInterval * PlayerManager.Stats.FireIntervalMultiplier;

        //経過時間が武器のレベルデータで設定された攻撃間隔を超えれば攻撃
        if (attackTimer >= interval)
        {
            //タイマーリセット
            attackTimer = 0f;

            //攻撃
            Attack();
        }
    }

    /// <summary>
    /// 攻撃処理
    /// </summary>
    private void Attack()
    {
        //生成位置をスポーンポイントとする
        Vector3 spawnPosition = spawnPoint.position;

        //斬撃エフェクト生成
        SingleAttackHitbox hitbox = Instantiate(slashHitboxPrefab, spawnPosition, transform.rotation);

        //斬撃のダメージ値は、「武器のレベルデータの基礎ダメージ」×「プレイヤーの攻撃力」とする
        float damage = CurrentLevelData.Damage * PlayerManager.Stats.Strength;

        //斬撃エフェクトを初期化して、ダメージ値を渡す
        hitbox.Initialize(damage);

        //斬撃エフェクトの大きさは、「武器のレベルデータの大きさ倍率」×「プレイヤーの弾サイズ倍率」とする
        hitbox.transform.localScale = Vector3.one * CurrentLevelData.Scale * PlayerManager.Stats.ProjectileScaleMultiplier;

        //斬撃エフェクトの持続時間は「武器のレベルデータの持続時間」とする
        Destroy(hitbox.gameObject, CurrentLevelData.Duration);
    }
}