using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeDataList", menuName = "Scriptable Objects/UpgradeDataList")]
public class UpgradeDataList : ScriptableObject
{
    [Header("レベルアップで獲得可能な武器のアップグレードデータ集")]
    [SerializeField] List<UpgradeData_Weapon> weaponList = new List<UpgradeData_Weapon>();

    [Header("レベルアップで獲得可能なバフのアップグレードデータ集")]
    [SerializeField] List<UpgradeData_Buff> buffList = new List<UpgradeData_Buff>();


    //---------------------<取得のみのプロパティ>----------------------

    public IReadOnlyList<UpgradeData_Weapon> WeaponList => weaponList;

    public IReadOnlyList<UpgradeData_Buff> BuffList => buffList;

    //-----------------------------------------------------------------
}
