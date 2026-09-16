using System;
using UnityEngine;
using UnityEngine.Events;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] GameMainManager gameMainManager;
    [SerializeField] UpgradeManager upgradeManager;

    [Header("プレイヤーのプレハブ"), SerializeField] Player playerPrefab;
    [Header("プレイヤーのデータ"), SerializeField] PlayerData data;

    [Header("プレイヤーの現在のステータス"), SerializeField] PlayerStats stats;

    [Header("プレイヤーのHPが変化したときのイベント"), SerializeField] UnityEvent<float, float> onHpChanged;
    [Header("プレイヤーの獲得経験値が変化した時のイベント"), SerializeField] UnityEvent<int, int> onExpChanged;
    [Header("プレイヤーが所持する武器が変化した時のイベント"), SerializeField] UnityEvent<WeaponController> onWeaponChanged;

    //-------------------<外部から情報を取得する用のプロパティ>------------------------

    /// <summary>
    /// シーン上のプレイヤー
    /// </summary>
    public Player Player { get; private set; }

    /// <summary>
    /// プレイヤーの現在のステータス
    /// </summary>
    public PlayerStats Stats => stats;


    void Awake()
    {
        //初期化処理を呼び出す
        Initialize();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 初期化処理
    /// </summary>
    public void Initialize()
    {
        //プレイヤーの初期パラメータの情報をコピーする
        stats.MaxHp = data.InitialStats.MaxHp;
        stats.Hp = data.InitialStats.Hp;
        stats.Strength = data.InitialStats.Strength;
        stats.Speed = data.InitialStats.Speed;
        stats.ProjectileSpeedMultiplier = data.InitialStats.ProjectileSpeedMultiplier;
        stats.FireIntervalMultiplier = data.InitialStats.FireIntervalMultiplier;
        stats.ProjectileScaleMultiplier = data.InitialStats.ProjectileScaleMultiplier;
        stats.Exp = data.InitialStats.Exp;
        stats.Level = data.InitialStats.Level;

        //プレイヤープレハブを原点に生成
        Player = Instantiate(playerPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity);

        //プレイヤー初期化（引数に自分自身の情報と、プレイヤーの初期装備の武器データを渡す）
        Player.Initialize(this, data.InitialWeapon);

        //武器が変わったのでイベントを発生させる
        onWeaponChanged.Invoke(Player.WeaponController);
    }

    /// <summary>
    /// 経験値獲得
    /// </summary>
    /// <param name="exp">獲得経験値</param>
    public void AddExp(int exp)
    {
        //PlayerStatsのExpを上昇させる
        stats.Exp += exp;

        //次のレベルまでに必要な経験値をAnimationCurveから取得
        int nextLevel = stats.Level + 1;
        float requiredExp = data.ExpTableCurve.Evaluate(nextLevel);

        //必要経験値をfloat型(小数点込み)からint型(整数)に型変換
        int requiredExp_Int = (int)requiredExp;

        //獲得経験値が変化したのでイベントを発生させる
        onExpChanged.Invoke(stats.Exp, requiredExp_Int);

        //現在の獲得経験値が必要経験値を上回ればレベルアップ
        if (stats.Exp >= requiredExp_Int)
        {
            LevelUp(requiredExp_Int);
        }
    }

    /// <summary>
    /// プレイヤーのレベルアップ処理
    /// </summary>
    /// <param name="requiredExp">レベルアップに必要だった経験値</param>
    private void LevelUp(int requiredExp)
    {
        //最大レベルに達していればレベルアップさせない
        if(stats.Level >= data.MaxLevel) { return; }

        //PlayerStatsのLevelを1上昇させる
        stats.Level++;

        //アップグレード開始
        upgradeManager.StartUpgrade();

        //獲得経験値を必要だった経験値分引く
        stats.Exp -= requiredExp;

        //次のレベルまでに必要な経験値をAnimationCurveから取得
        int nextLevel = stats.Level + 1;
        float nextRequiredExp = data.ExpTableCurve.Evaluate(nextLevel);

        //必要経験値をfloat型(小数点込み)からint型(整数)に型変換
        int nextRequiredExp_Int = (int)nextRequiredExp;

        //獲得経験値が変化したのでイベントを発生させる
        onExpChanged.Invoke(stats.Exp, nextRequiredExp_Int);
    }

    /// <summary>
    /// 武器を新規獲得orレベルアップする
    /// </summary>
    /// <param name="addWeapon">追加する武器</param>
    public void AddOrLevelUpWeapon(WeaponData addWeapon)
    {
        //武器を新規獲得またはレベルアップができるかをチェック
        bool ok = Player.WeaponController.CanAddOrLevelUp(addWeapon);

        if (ok)
        {
            //武器の獲得・レベルアップ処理
            Player.WeaponController.AddOrLevelUpWeapon(addWeapon);

            //武器が変わったのでイベントを発生させる
            onWeaponChanged.Invoke(Player.WeaponController);
        }
    }


    /// <summary>
    /// プレイヤーにバフを適用する
    /// </summary>
    /// <param name="buff">適用するバフのデータ</param>
    public void ApplyBuff(BuffData buff)
    {
        //適用するバフの種類に応じてパラメータを変化させる
        switch (buff.BuffType)
        {
            case BuffType.MaxHpUp:
                stats.MaxHp += buff.ApplyValue;

                //プレイヤーの最大HPが変化したので、イベントを発生させる
                onHpChanged.Invoke(stats.Hp, stats.MaxHp);

                break;
            case BuffType.StrengthUp:
                stats.Strength += buff.ApplyValue;
                break;
            case BuffType.MoveSpeedUp:
                stats.Speed += buff.ApplyValue;
                break;
            case BuffType.ProjectileSpeedUp:
                stats.ProjectileSpeedMultiplier += buff.ApplyValue;
                break;
            case BuffType.FireIntervalReduce:
                //時間短縮が強化になるので調整する際は、マイナスの値にする必要がある点に注意
                stats.FireIntervalMultiplier += buff.ApplyValue;
                stats.FireIntervalMultiplier = Mathf.Max(stats.FireIntervalMultiplier, 0.1f);
                break;
            case BuffType.ProjectileScaleUp:
                stats.ProjectileScaleMultiplier += buff.ApplyValue;
                break;
        }
    }

    /// <summary>
    /// プレイヤーへの回復処理
    /// </summary>
    /// <param name="heal">回復量</param>
    public void Heal(float heal)
    {
        //HPが0なら処理終了
        if (stats.Hp <= 0f) { return; }

        //PlayerStatsのHPを上昇させる
        stats.Hp += heal;

        //HP変化のイベントに登録されているメソッドを呼び出す
        onHpChanged.Invoke(stats.Hp, stats.MaxHp);
    }

    /// <summary>
    /// プレイヤーへのダメージ処理
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(float damage)
    {
        //HPが0なら処理終了
        if(stats.Hp <= 0f) { return; }

        //PlayerStatsのHPを減少させる
        stats.Hp -= damage;

        //HP変化のイベントに登録されているメソッドを呼び出す
        onHpChanged.Invoke(stats.Hp, stats.MaxHp);

        //HPが0なら死亡処理を呼ぶ
        if(stats.Hp <= 0f)
        {
            Die();
        }
    }

    /// <summary>
    /// プレイヤーの死亡処理
    /// </summary>
    public void Die()
    {
        //シーン上のプレイヤーの死亡処理を呼ぶ
        Player.Die();

        //GameMainManagerのゲームオーバー処理を呼び出す
        gameMainManager.GameOver();
    }

    /// <summary>
    /// ゲームクリア時にプレイヤーや武器を停止させる
    /// </summary>
    public void Stop()
    {
        //プレイヤーの停止処理を呼び出す
        Player.Stop();
    }
}
