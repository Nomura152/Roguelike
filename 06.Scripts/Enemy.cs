using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    [Header("データ")]
    [SerializeField] EnemyData data;

    [Header("ゲーム実行時のステータス")]
    [SerializeField] EnemyStats stats;

    [Header("コンポーネント群")]
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Collider2D col;
    [SerializeField] SpriteRenderer sr;


    //プレイヤーオブジェクトの情報
    Player player;
    //EnemyManagerの情報
    EnemyManager enemyManager;
    //ItemDropManagerの情報
    ItemDropManager itemDropManager;
    //DeathEffectPoolの情報
    DeathEffectPool deathEffectPool;

    //発光演出コルーチン
    Coroutine flashCoroutine;
    float duration = 0.1f;

    //マテリアルのプロパティを効率よく設定するためのクラス
    MaterialPropertyBlock propertyBlock;

    /// <summary>
    /// 初期化処理
    /// </summary>
    /// <param name="p">EnemyManagerから渡されるプレイヤーの情報</param>
    /// <param name="manager">自身を生成させたEnemyManagerの情報</param>
    /// <param name="itemDropManager">EnemyManagerから受け取るItemDropManager</param>
    /// <param name="deathEffectPool">死亡時のエフェクトを管理しているクラス</param>
    public void Initialize(Player p, EnemyManager manager, ItemDropManager itemDropManager, DeathEffectPool deathEffectPool)
    {
        //プレイヤーの情報を記憶する
        player = p;
        //EnemyManagerの情報を記憶する
        enemyManager = manager;
        //ItemDropManagerの情報を記憶する
        this.itemDropManager = itemDropManager;
        //DeathEffectPoolの情報を記憶する
        this.deathEffectPool = deathEffectPool;

        //敵キャラクターの初期パラメータの情報をコピーする
        stats.MaxHp = data.InitialStats.MaxHp;
        stats.Hp = data.InitialStats.Hp;
        stats.Strength = data.InitialStats.Strength;
        stats.Speed = data.InitialStats.Speed;

        propertyBlock = new MaterialPropertyBlock();
    }

    /// <summary>
    /// IDamageableインターフェースのメソッド（ダメージを受け取る）
    /// </summary>
    /// <param name="damage">ダメージ値</param>
    void IDamageable.TakeDamage(float damage)
    {
        //HPが0なら何もしない
        if(stats.Hp <= 0f) { return; }

        //受け取ったダメージ分の値を、EnemyStatsのHPから減らす
        stats.Hp -= damage;

        //被弾SEを鳴らす
        AudioManager.Instance.PlaySE(SoundKey.EnemyHit);

        //被弾演出で発光させる
        if(flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
        }
        flashCoroutine = StartCoroutine(FlashCoroutine());

        //HPが0なら死亡処理を呼び出す
        if(stats.Hp <= 0f)
        {
            Die();
        }
    }

    private IEnumerator FlashCoroutine()
    {
        //発光
        sr.GetPropertyBlock(propertyBlock);
        propertyBlock.SetFloat("_FlashAmount", 1f);
        sr.SetPropertyBlock(propertyBlock);

        //待機
        yield return new WaitForSeconds(duration);

        //発光状態を戻す
        sr.GetPropertyBlock(propertyBlock);
        propertyBlock.SetFloat("_FlashAmount", 0f);
        sr.SetPropertyBlock(propertyBlock);

        flashCoroutine = null;
    }

    // Update is called once per frame
    void Update()
    {
        //プレイヤーオブジェクトが存在していれば、その方向に向かって追跡する
        if (player != null)
        {
            //プレイヤーへの方向を取得
            Vector2 playerPos = player.transform.position;
            Vector2 enemyPos = transform.position;
            Vector2 direction = playerPos - enemyPos;

            //ベクトルを正規化
            direction = direction.normalized;

            //追跡方向に応じてキャラクターを180度反転させる
            if (direction.x > 0f)   //右方向
            {
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            }
            else if (direction.x < 0f)   //左方向
            {
                transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            }

            //Rigidbodyに速度を与える
            rb.linearVelocity = direction * stats.Speed;  //EnemyStatsの移動速度の値を参照
        }
    }

    /// <summary>
    /// Enemyの死亡処理
    /// </summary>
    public void Die()
    {
        //EnemyManagerから自身の情報を削除
        enemyManager.UnregisterEnemy(this);

        //アイテムを自分自身の座標にドロップ
        itemDropManager.DropItem(data.ItemDropData, transform.position);

        //死亡エフェクトをオブジェクトプールから取り出し
        var effect = deathEffectPool.GetEffect();

        //死亡エフェクトの位置を調整
        effect.transform.position = transform.position;

        //エフェクト生存タイマーを再生
        effect.PlayEffect();

        //Enemyオブジェクトを削除
        Destroy(gameObject);
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        //接触相手のレイヤーがプレイヤーか？
        if(collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            //プレイヤーの「IDamageable」インターフェースを探す
            IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();

            //「IDamageable」を取得できなかったら処理終了
            if (damageable == null) { return; }

            //プレイヤーに接触ダメージを与える
            damageable.TakeDamage(stats.Strength);  //与えるダメージはStatsの攻撃力の値
        }
    }
}
