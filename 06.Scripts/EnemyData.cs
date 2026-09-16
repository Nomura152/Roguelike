using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/EnemyData")]
public class EnemyData : ScriptableObject
{
    [Header("ゲーム開始時の初期パラメータ")] public EnemyStats InitialStats;

    [Header("アイテムドロップデータ")] public ItemDropData ItemDropData;
}
