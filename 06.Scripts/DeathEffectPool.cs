using UnityEngine;
using UnityEngine.Pool;


/// <summary>
/// Unityのオブジェクトプールを使用した敵の死亡エフェクトプールクラス
/// </summary>
public class DeathEffectPool : MonoBehaviour
{
    [Header("敵の死亡エフェクト")]
    [SerializeField] DeathEffect effectPrefab;

    //Unity公式のオブジェクトプール
    ObjectPool<DeathEffect> effectPool;

    //最優先で初期化するためAwakeを使用
    void Awake()
    {
        //死亡エフェクト専用のプールクラスのインスタンスを作成
        //特定のタイミングで呼び出すメソッドを4種類登録出来る
        effectPool = new ObjectPool<DeathEffect>(
            createFunc: Pool_Create,
            actionOnGet: Pool_OnGet,
            actionOnRelease: Pool_OnRelease,
            actionOnDestroy: Pool_OnDestroy
            );
    }

    /// <summary>
    /// ★Unityのオブジェクトプールに登録する専用メソッド★
    /// プールにオブジェクトが無い時に呼び出される
    /// </summary>
    private DeathEffect Pool_Create()
    {
        //エフェクトを生成
        var effect = Instantiate(effectPrefab);

        //死亡エフェクト側に自分自身の情報を渡す
        effect.SetPool(this);

        return effect;
    }

    /// <summary>
    /// ★Unityのオブジェクトプールに登録する専用メソッド★
    /// プールからオブジェクトが取り出された時に呼び出される
    /// </summary>
    /// <param name="effect">取り出されたエフェクトオブジェクト</param>
    private void Pool_OnGet(DeathEffect effect)
    {
        //エフェクトオブジェクトをアクティブ化して表示する
        effect.gameObject.SetActive(true);
    }

    /// <summary>
    /// ★Unityのオブジェクトプールに登録する専用メソッド★
    /// プールにオブジェクトが戻された時に呼び出される
    /// </summary>
    /// <param name="effect">戻されたエフェクトオブジェクト</param>
    private void Pool_OnRelease(DeathEffect effect)
    {
        //エフェクトオブジェクトを非アクティブ化して非表示にする
        effect.gameObject.SetActive(false);
    }

    /// <summary>
    /// ★Unityのオブジェクトプールに登録する専用メソッド★
    /// プールに入りきらないオブジェクトが出てきたときに呼び出される
    /// </summary>
    /// <param name="effect">戻されたエフェクトオブジェクト</param>
    private void Pool_OnDestroy(DeathEffect effect)
    {
        //プールに入りきらないオブジェクトは削除する
        Destroy(effect.gameObject);
    }


    //--------------------------<以下は死亡エフェクトを使いたいクラスが呼び出すメソッド>---------------------------------

    /// <summary>
    /// 死亡エフェクトを取得するメソッド
    /// オブジェクトプールから死亡エフェクトを効率よく使いまわす
    /// </summary>
    /// <returns></returns>
    public DeathEffect GetEffect()
    {
        //プールから再利用可能なエフェクトオブジェクトを取得して返す
        return effectPool.Get();
    }

    /// <summary>
    /// 使い終わった死亡エフェクトをオブジェクトプールに戻すメソッド
    /// </summary>
    /// <param name="effect"></param>
    public void ReleaseDeathEffect(DeathEffect effect)
    {
        //プールにエフェクトを戻すことで再利用可能にする
        effectPool.Release(effect);
    }
}
