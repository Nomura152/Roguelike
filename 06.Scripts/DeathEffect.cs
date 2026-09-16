using UnityEngine;

public class DeathEffect : MonoBehaviour
{
    [Header("エフェクトの生存時間")]
    [SerializeField] float lifeTime = 0.5f;

    DeathEffectPool deathEffectPool;
    float timer = 0f;

    /// <summary>
    /// 死亡エフェクトのオブジェクトプールを管理しているクラスの情報を設定するメソッド
    /// </summary>
    /// <param name="pool"></param>
    public void SetPool(DeathEffectPool pool)
    {
        deathEffectPool = pool;
    }

    /// <summary>
    /// エフェクト再生を開始するメソッド
    /// </summary>
    public void PlayEffect()
    {
        //エフェクトの生存時間を0にリセット
        timer = 0f;

        //SEを鳴らす
        AudioManager.Instance.PlaySE(SoundKey.EnemyDeath);
    }

    // Update is called once per frame
    void Update()
    {
        //生存時間を計測
        timer += Time.deltaTime;

        //生存時間を超えた場合は、自分自身をプールに戻す
        if(timer >= lifeTime)
        {
            deathEffectPool.ReleaseDeathEffect(this);
        }
    }
}
