using UnityEngine;

/// <summary>
/// 経験値アイテムクラス
/// ItemBaseを継承している
/// </summary>
public class ExpItem : ItemBase
{
    [Header("経験値量"), SerializeField] int expAmount = 1;

    /// <summary>
    /// アイテムの効果適用処理
    /// ItemBaseのabstractメソッドをオーバーライドしている
    /// </summary>
    /// <param name="player">アイテム効果を適用するプレイヤー</param>
    protected override void Apply(Player player)
    {
        //Playerの経験値獲得メソッドを呼び出す
        player.AddExp(expAmount);
    }
}
