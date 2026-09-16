using UnityEngine;

/// <summary>
/// 一定時間ごとに斧を投擲する武器
/// </summary>
public class Weapon_Axe : WeaponBase
{
    [Header("投擲する斧プレハブ")]
    [SerializeField] SingleAttackHitbox axePrefab;

    float attackTimer = 0f;


    protected override void OnInitialized()
    {
        //タイマー初期化
        attackTimer = 0f;
    }

    protected override void OnLevelUp()
    {
        //今回は特に特別な処理は無いので空でOK
    }

    void Update()
    {
        //時間を加算
        attackTimer += Time.deltaTime;

        //斧生成のインターバル時間を「武器レベルデータのインターバル時間」×「プレイヤーの発射間隔倍率」で計算する
        float interval = CurrentLevelData.AttackInterval * PlayerManager.Stats.FireIntervalMultiplier;

        //経過時間が武器のレベルデータで設定された攻撃間隔を超えれば斧生成
        if (attackTimer >= interval)
        {
            //タイマーリセット
            attackTimer = 0f;

            //斧を「武器レベルデータの弾数」ぶん、一度に生成する
            for(int i = 0; i < CurrentLevelData.AttackCount; i++)
            {
                ThrowAxe();
            }
        }
    }


    /// <summary>
    /// 斧の投擲処理
    /// </summary>
    private void ThrowAxe()
    {
        //生成位置はプレイヤーの座標とする
        Vector3 pos = PlayerManager.Player.transform.position;

        //斧生成
        SingleAttackHitbox axe = Instantiate(axePrefab, pos, Quaternion.identity);

        //斧のダメージ値は、「武器のレベルデータの基礎ダメージ」×「プレイヤーの攻撃力」とする
        float damage = CurrentLevelData.Damage * PlayerManager.Stats.Strength;

        //斧の攻撃判定を初期化して、ダメージ値を渡す
        axe.Initialize(damage);

        //斧の大きさは、「武器のレベルデータの大きさ倍率」×「プレイヤーの弾サイズ倍率」とする
        axe.transform.localScale = Vector3.one * CurrentLevelData.Scale * PlayerManager.Stats.ProjectileScaleMultiplier;
    }
}
