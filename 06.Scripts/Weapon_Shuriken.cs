using UnityEngine;

/// <summary>
/// 一定時間ごとにプレイヤーの進行方向に向かって手裏剣を投げる武器
/// </summary>
public class Weapon_Shuriken : WeaponBase
{
    [Header("手裏剣のプレハブ")]
    [SerializeField] Shuriken shurikenPrefab;

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

        //手裏剣生成のインターバル時間を「武器レベルデータのインターバル時間」×「プレイヤーの発射間隔倍率」で計算する
        float interval = CurrentLevelData.AttackInterval * PlayerManager.Stats.FireIntervalMultiplier;

        //経過時間が武器のレベルデータで設定された攻撃間隔を超えれば手裏剣生成
        if (attackTimer >= interval)
        {
            //タイマーリセット
            attackTimer = 0f;

            //手裏剣生成
            ThrowShuriken();

        }
    }

    /// <summary>
    /// プレイヤーの進行方向に向かって手裏剣を生成
    /// </summary>
    public void ThrowShuriken()
    {
        //生成位置はプレイヤーの座標とする
        Vector3 pos = PlayerManager.Player.transform.position;

        //手裏剣生成
        Shuriken shuriken = Instantiate(shurikenPrefab, pos, Quaternion.identity);

        //手裏剣が飛ぶ方向は、プレイヤーの最後の移動方向とする
        Vector2 direction = PlayerManager.Player.LastMoveDirection;

        //手裏剣の速度は、「武器のレベルデータの弾速」×「プレイヤーの弾速倍率」とする
        float speed = CurrentLevelData.ProjectileSpeed * PlayerManager.Stats.ProjectileSpeedMultiplier;

        //手裏剣のダメージ値は、「武器のレベルデータの基礎ダメージ」×「プレイヤーの攻撃力」とする
        float damage = CurrentLevelData.Damage * PlayerManager.Stats.Strength;

        //手裏剣の生存時間は、「武器のレベルデータの攻撃判定の持続時間」とする
        float lifeTime = CurrentLevelData.Duration;

        //「Shuriken」クラスを初期化して、速度（direction × speed）、ダメージ値、生存時間のパラメータを渡す
        shuriken.Initialize(direction * speed, damage, lifeTime);

        //手裏剣の大きさは、「武器のレベルデータの大きさ倍率」×「プレイヤーの弾サイズ倍率」とする
        shuriken.transform.localScale = Vector3.one * CurrentLevelData.Scale * PlayerManager.Stats.ProjectileScaleMultiplier;
    }
}
