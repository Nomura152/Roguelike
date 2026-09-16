using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeData_Weapon", menuName = "Scriptable Objects/UpgradeData_Weapon")]
public class UpgradeData_Weapon : UpgradeDataBase
{
    [Header("武器データ")]
    [SerializeField] WeaponData weaponData;

    //---------------------<取得のみのプロパティ>----------------------

    public WeaponData WeaponData => weaponData;
}
