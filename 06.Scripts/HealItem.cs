using UnityEngine;

/// <summary>
/// 回復アイテムクラス
/// ItemBaseを継承している
/// </summary>
public class HealItem : ItemBase
{
    [Header("回復量"), SerializeField] float healAmount = 30f;

    /// <summary>
    /// アイテムの効果適用処理
    /// ItemBaseのabstractメソッドをオーバーライドしている
    /// </summary>
    /// <param name="player">アイテム効果を適用するプレイヤー</param>
    protected override void Apply(Player player)
    {
        //Playerの回復メソッドを呼び出す
        player.Heal(healAmount);
    }
}
