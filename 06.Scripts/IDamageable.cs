using UnityEngine;

/// <summary>
/// ダメージを受ける事が出来るようになるインターフェース
/// </summary>
public interface IDamageable
{
    /// <summary>
    /// ダメージを受けるメソッド
    /// </summary>
    /// <param name="damage">ダメージ量</param>
    public void TakeDamage(float damage);
}
