using UnityEngine;

/// <summary>
/// プレイヤーの周囲にバリアを展開する武器
/// </summary>
public class Weapon_Barrier : WeaponBase
{
    [Header("バリアのプレハブ")]
    [SerializeField] RepeatAttackHitbox barrierPrefab;

    //シーン上のバリアオブジェクト
    RepeatAttackHitbox barrierAttack;

    protected override void OnInitialized()
    {
        //最初にバリアを子オブジェクトとして生成
        barrierAttack = Instantiate(barrierPrefab, transform.position, Quaternion.identity, transform);

        //バリアのダメージ値は、「武器のレベルデータの基礎ダメージ」×「プレイヤーの攻撃力」とする
        float damage = CurrentLevelData.Damage * PlayerManager.Stats.Strength;

        //バリアの攻撃判定インターバルは「武器のレベルデータのヒット間隔」の値をそのまま使用する
        float interval = CurrentLevelData.HitInterval;

        //攻撃判定を初期化
        barrierAttack.Initialize(damage, interval);

        //バリアを更新
        UpdateBarrier();
    }

    protected override void OnLevelUp()
    {
        //今回は特に特別な処理は無いので空でOK
    }

    void Update()
    {
        //バリアを更新
        UpdateBarrier();
    }

    /// <summary>
    /// バリアの状態を更新
    /// </summary>
    private void UpdateBarrier()
    {
        //バリアのダメージ値は、「武器のレベルデータの基礎ダメージ」×「プレイヤーの攻撃力」とする
        float damage = CurrentLevelData.Damage * PlayerManager.Stats.Strength;

        //バリアの攻撃判定インターバルは「武器のレベルデータのヒット間隔」の値をそのまま使用する
        float interval = CurrentLevelData.HitInterval;

        //値を設定
        barrierAttack.SetDamage(damage);
        barrierAttack.SetHitInterval(interval);

        //親の回転やプレイヤーの左右反転の影響を受けず、鉄球の回転値をワールド基準に保つ
        barrierAttack.transform.rotation = Quaternion.identity;

        //バリアのサイズを拡大する
        //（サイズは（1,1,1）×「武器レベルデータのサイズ」×「プレイヤーの弾のサイズ倍率」とする）
        barrierAttack.transform.localScale = Vector3.one * CurrentLevelData.Scale * PlayerManager.Stats.ProjectileScaleMultiplier;
    }
}
