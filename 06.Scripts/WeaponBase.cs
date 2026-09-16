using UnityEngine;

/// <summary>
/// 全ての武器のベースとなるクラス
/// 武器を作成する際は、このクラスを継承する
/// </summary>
public abstract class WeaponBase : MonoBehaviour
{
    [Header("武器の共通情報")]
    [Tooltip("武器のレベル"), SerializeField, Min(1)] int level = 1;

    //---------------------<取得専用プロパティ>-------------------------

    /// <summary>
    /// 武器のレベル
    /// </summary>
    public int Level => level;

    /// <summary>
    /// プレイヤーマネージャー
    /// </summary>
    public PlayerManager PlayerManager { get; private set; }

    /// <summary>
    /// 武器のデータ
    /// </summary>
    public WeaponData WeaponData { get; private set; }

    /// <summary>
    /// 現在の武器のレベルデータ
    /// </summary>
    public WeaponLevelData CurrentLevelData { get; private set; }


    //---------------------<ベースの共通メソッド>---------------------

    /// <summary>
    /// 武器の初期化
    /// </summary>
    /// <param name="manager">プレイヤーマネージャー</param>
    /// <param name="data">武器データ</param>
    public void Initialize(PlayerManager manager, WeaponData data)
    {
        //初期化時にデータを受け取る
        PlayerManager = manager;
        WeaponData = data;

        //武器レベルデータを取得する
        CurrentLevelData = WeaponData.GetLevelData(Level);

        //武器ごとに個別で初期化処理を行う
        OnInitialized();
    }

    /// <summary>
    /// レベルアップ処理
    /// </summary>
    public void LevelUp()
    {
        //レベルを1上げる
        level++;

        //対象のレベルの武器レベルデータを取得
        CurrentLevelData = WeaponData.GetLevelData(level);

        //武器ごとに個別でレベルアップ処理を行う
        OnLevelUp();
    }

    //---------------------<継承先専用のメソッド>---------------------

    /// <summary>
    /// それぞれの武器ごとに初期化を行う
    /// </summary>
    protected abstract void OnInitialized();

    /// <summary>
    /// それぞれの武器ごとにレベルアップ時の処理を行う
    /// </summary>
    protected abstract void OnLevelUp();
}
