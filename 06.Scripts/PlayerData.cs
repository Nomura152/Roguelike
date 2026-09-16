using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/PlayerData")]
public class PlayerData : ScriptableObject
{
    [Header("ゲーム開始時の初期パラメータ")] public PlayerStats InitialStats;

    [Header("初期武器")] public WeaponData InitialWeapon;

    [Header("経験値テーブル")] public AnimationCurve ExpTableCurve;
    [Header("最大レベル")] public int MaxLevel = 30;
}
