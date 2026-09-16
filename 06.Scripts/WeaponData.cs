using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 武器のデータをまとめるスクリプタブルオブジェクト
/// </summary>
[CreateAssetMenu(fileName = "WeaponData", menuName = "Scriptable Objects/WeaponData")]
public class WeaponData : ScriptableObject
{
    [Header("表示情報")]
    [SerializeField] private Sprite icon;

    [Header("武器Prefab")]
    [SerializeField] private WeaponBase weaponPrefab;

    [Header("レベルごとの性能")]
    [SerializeField] private List<WeaponLevelData> levels;


    //---------------------<取得のみのプロパティ>----------------------

    public Sprite Icon => icon;
    public WeaponBase WeaponPrefab => weaponPrefab;

    /// <summary>
    /// 武器の最大レベル
    /// </summary>
    public int MaxLevel => levels.Count;

    //-----------------------------------------------------------------


    /// <summary>
    /// 指定したレベルのレベルデータを取得する
    /// </summary>
    /// <param name="level">取得したいレベル</param>
    /// <returns></returns>
    public WeaponLevelData GetLevelData(int level)
    {
        //実際のレベルから1引いた要素番号のレベルデータを取得
        WeaponLevelData levelData = levels[level - 1];

        return levelData;
    }
}
