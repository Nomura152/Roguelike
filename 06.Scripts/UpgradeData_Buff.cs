using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeData_Buff", menuName = "Scriptable Objects/UpgradeData_Buff")]
public class UpgradeData_Buff : UpgradeDataBase
{
    [Header("バフデータ")]
    [SerializeField] BuffData buffData;

    //---------------------<取得のみのプロパティ>----------------------

    public BuffData BuffData => buffData;
}
