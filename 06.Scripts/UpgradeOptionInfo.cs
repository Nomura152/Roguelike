using UnityEngine;

public class UpgradeOptionInfo
{
    /// <summary>
    /// アップグレードの種類
    /// </summary>
    public enum UpgradeType
    {
        AddWeapon,      //新規武器追加
        LevelUpWeapon,  //武器レベルアップ
        Buff,           //バフ獲得
    }
}
