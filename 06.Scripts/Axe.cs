using UnityEngine;

public class Axe : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [Tooltip("斧の横方向の投擲速度"), SerializeField] float speed_X = 2f;
    [Tooltip("斧の縦方向の投擲速度"), SerializeField] float speed_Y = 8f;
    [Tooltip("斧の回転速度"), SerializeField] float rotSpeed = 720f;
    [Tooltip("斧の生存時間"), SerializeField] float lifeTime = 3f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //投擲する斧の方向の、X軸とY軸の値を決める
        float x = Random.Range(-1f, 1f) * speed_X;    //X軸（横）は(-1.0～1.0)の中でランダムにした値にスピード値を乗算する
        float y = speed_Y;

        //XとYの値を、Vector2型として作成
        Vector2 velocity = new Vector2(x, y);

        //斧の投擲速度として設定
        rb.linearVelocity = velocity;

        //指定秒数後に削除する
        Destroy(gameObject, lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        //斧の回転量を計算
        float rotate = rotSpeed * Time.deltaTime;

        //演出として、斧を常時回転させる
        transform.Rotate(0f, 0f, rotate);
    }
}
