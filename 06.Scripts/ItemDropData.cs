using UnityEngine;

/// <summary>
/// 敵がドロップするアイテムのドロップ率などを設定するスクリプタブルオブジェクト
/// </summary>
[CreateAssetMenu(fileName = "ItemDropData", menuName = "Scriptable Objects/ItemDropData")]
public class ItemDropData : ScriptableObject
{
    [Header("アイテムドロップ率（0～100％）")]
    [SerializeField, Range(0f, 100f)] float dropRate = 90f;

    [Header("ドロップアイテム")]
    [SerializeField] ItemBase dropItemPrefab;

    [Header("レアアイテムドロップ率（0～100％）")]
    [SerializeField, Range(0f, 100f)] float rareDropRate = 2f;

    [Header("レアドロップアイテム")]
    [SerializeField] ItemBase rareDropItemPrefab;

    //---------------------<取得のみのプロパティ>----------------------

    public float DropRate => dropRate;
    public ItemBase DropItemPrefab => dropItemPrefab;
    public float RareDropRate => rareDropRate;
    public ItemBase RareDropItemPrefab => rareDropItemPrefab;

    //-----------------------------------------------------------------
}
