using UnityEngine;

public class Shuriken : MonoBehaviour
{
    [Header("コンポーネント群")]
    [SerializeField] Rigidbody2D rb;
    [SerializeField] SingleAttackHitbox hitbox;

    [Header("手裏剣のパラメータ")]
    [Tooltip("手裏剣の回転速度"), SerializeField] float rotSpeed = 1440f;

    /// <summary>
    /// 手裏剣単体の初期化
    /// </summary>
    /// <param name="velocity">速度</param>
    /// <param name="damage">ダメージ値</param>
    /// <param name="lifeTime">手裏剣の生存時間</param>
    public void Initialize(Vector2 velocity, float damage, float lifeTime)
    {
        //Rigidbody2Dに速度を設定。今回はプレイヤーの進行方向に向かわせる
        rb.linearVelocity = velocity;

        //攻撃判定を初期化してダメージ値を渡す
        hitbox.Initialize(damage);

        //指定秒数後に削除する
        Destroy(gameObject, lifeTime);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //手裏剣の回転量を計算
        float rotate = rotSpeed * Time.deltaTime;

        //演出として、手裏剣を常時回転させる
        transform.Rotate(0f, 0f, rotate);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //敵や壁に触れたら手裏剣を削除する
        Destroy(gameObject);
    }
}
