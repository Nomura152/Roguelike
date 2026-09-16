using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーの武器を管理するクラス
/// </summary>
public class WeaponController : MonoBehaviour
{
    [Header("武器設定")]
    [Tooltip("所持可能な武器の最大数"), SerializeField] int maxWeaponCount = 3;

    [Header("武器リスト")]
    [Tooltip("所持している武器のデータ"), SerializeField] List<WeaponData> dataList = new List<WeaponData>();
    [Tooltip("シーン上に展開している武器"), SerializeField] List<WeaponBase> weaponList = new List<WeaponBase>();

    //プレイヤーマネージャー
    PlayerManager playerManager;

    //---------------------<取得専用プロパティ>----------------------

    /// <summary>
    /// 読み取り専用の所持武器データリスト
    /// </summary>
    public IReadOnlyList<WeaponData> DataList => dataList;

    /// <summary>
    /// 所持可能な武器の最大数
    /// </summary>
    public int MaxWeaponCount => maxWeaponCount;

    //-----------------------------------------------------------------

    /// <summary>
    /// 初期化処理
    /// </summary>
    /// <param name="manager">プレイヤーマネージャー</param>
    /// <param name="initialWeapon">初期の武器データ</param>
    public void Initialize(PlayerManager manager, WeaponData initialWeapon)
    {
        //プレイヤーマネージャーの情報を記憶
        playerManager = manager;

        //初期武器を追加
        AddWeapon(initialWeapon);
    }

    /// <summary>
    /// 武器が追加またはレベルアップできるかをチェックする
    /// </summary>
    /// <param name="weaponData">確認したい武器データ</param>
    /// <returns>武器の追加かレベルアップが出来ればtrue。出来ないならfalse。</returns>
    public bool CanAddOrLevelUp(WeaponData weaponData)
    {
        //所持している武器を全て確認
        foreach (var weapon in weaponList)
        {
            //所持している武器が持つデータと一致しているか確認
            if (weapon.WeaponData == weaponData)
            {
                //既に持っている武器で、最大レベルであればレベルアップ出来ないのでfalse。
                if (weapon.Level >= weaponData.MaxLevel)
                {
                    return false;
                }
                else
                {
                    //最大レベルではないならtrue
                    return true;
                }
            }
        }

        //未所持の武器で、武器枠が空いていれば追加できる
        if(weaponList.Count < maxWeaponCount)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// 武器の追加またはレベルアップ
    /// </summary>
    /// <param name="weaponData">追加する武器データ</param>
    public void AddOrLevelUpWeapon(WeaponData weaponData)
    {
        //所持している武器を全て確認
        foreach(var weapon in  weaponList)
        {
            //所持している武器が持つデータと一致していれば、レベルアップして処理終了
            if (weapon.WeaponData == weaponData)
            {
                weapon.LevelUp();
                return;
            }
        }

        //未所持で、武器枠が埋まっているなら追加できない
        if (weaponList.Count == maxWeaponCount)
        {
            return;
        }
        else
        {
            //武器枠が空いているなら武器追加処理
            AddWeapon(weaponData);
        }
    }

    /// <summary>
    /// 武器の追加
    /// </summary>
    /// <param name="weaponData">追加する武器データ</param>
    public void AddWeapon(WeaponData weaponData)
    {
        //スクリプトが付いているGameObjectを親として武器オブジェクトを生成
        WeaponBase weapon = Instantiate(weaponData.WeaponPrefab, transform);

        //武器の初期化メソッドを呼ぶ
        weapon.Initialize(playerManager, weaponData);

        //武器データと生成武器をリストに追加
        dataList.Add(weaponData);
        weaponList.Add(weapon);
    }

    /// <summary>
    /// 全ての武器を停止する
    /// </summary>
    public void StopAllWeapons()
    {
        foreach (WeaponBase weapon in weaponList)
        {
            weapon.gameObject.SetActive(false);
        }
    }
}