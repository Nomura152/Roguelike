using UnityEngine;

/// <summary>
/// レベルアップで獲得できるボーナスのベースクラス
/// 武器のレベルアップとバフのレベルアップは、このクラスを継承して実装する
/// </summary>
[CreateAssetMenu(fileName = "UpgradeDataBase", menuName = "Scriptable Objects/UpgradeDataBase")]
public abstract class UpgradeDataBase : ScriptableObject
{
    //----------<武器とバフの両方のレベルアップで使用するデータ>--------

    [Header("表示名"), SerializeField] string displayName;
    [Header("説明文"), SerializeField, TextArea(3, 10)] string description;
    [Header("アイコン画像"), SerializeField] Sprite icon;

    //---------------------<取得のみのプロパティ>----------------------

    public string DisplayName => displayName;

    public string Description => description;

    public Sprite Icon => icon;
}