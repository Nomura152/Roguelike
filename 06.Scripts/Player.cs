using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour, IDamageable
{
    //自身のプレイヤーマネージャー情報
    PlayerManager playerManager;


    [Header("コンポーネント群")]
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Collider2D col;
    [SerializeField] SpriteRenderer sr;
    [SerializeField] PlayerInput input;
    [SerializeField] Animator anim;
    [SerializeField] WeaponController weaponController;

    //発光演出コルーチン
    Coroutine flashCoroutine;
    float duration = 0.1f;

    //マテリアルのプロパティを効率よく設定するためのクラス
    MaterialPropertyBlock propertyBlock;

    //--------------------<取得専用プロパティ>------------------------

    public WeaponController WeaponController => weaponController;

    /// <summary>
    /// 最後に移動した方向（初期値は右向き）
    /// </summary>
    public Vector2 LastMoveDirection { get; private set; } = Vector2.right;

    /// <summary>
    /// プレイヤーが動けるかどうか
    /// </summary>
    public bool CanMove { get; private set; }


    //----------------------------------------------------------------


    /// <summary>
    /// プレイヤーの初期化（プレイヤーをPlayerManagerが生成した時に呼び出す）
    /// </summary>
    /// <param name="manager">呼び出したPlayerManagerの情報</param>
    /// <param name="initialWeapon">プレイヤーの初期装備の武器データ</param>
    public void Initialize(PlayerManager manager, WeaponData initialWeapon)
    {
        //PlayerManagerの情報を記憶
        playerManager = manager;

        //武器システムの初期化（初期装備の武器データを渡す）
        weaponController.Initialize(playerManager, initialWeapon);

        //プレイヤーの移動可能フラグをtrueにする
        CanMove = true;

        propertyBlock = new MaterialPropertyBlock();
    }

    // Update is called once per frame
    void Update()
    {
        //プレイヤーが動けないなら操作させない
        if(!CanMove) { return; }

        //死亡後は操作させない
        if(playerManager.Stats.Hp <= 0f) { return; }

        //移動入力値を取得
        Vector2 moveInput = input.currentActionMap["Move"].ReadValue<Vector2>();

        //移動ボタンが入力されているかを確認
        if (input.currentActionMap["Move"].IsPressed())
        {
            //移動アニメーション
            anim.SetBool("Run", true);

            //入力方向に応じてプレイヤーを180度反転させる
            if (moveInput.x > 0f)   //右方向
            {
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            }
            else if (moveInput.x < 0f)   //左方向
            {
                transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            }

            //移動方向を最後の移動先として記憶（正規化してベクトルの長さを1にする）
            LastMoveDirection = moveInput.normalized;
        }
        else
        {
            //停止アニメーション
            anim.SetBool("Run", false);
        }

        //Rigidbodyに速度を与える
        rb.linearVelocity = moveInput * playerManager.Stats.Speed;  //PlayerManagerからPlayerStatsの移動速度の値を参照
    }

    /// <summary>
    /// 経験値獲得
    /// </summary>
    /// <param name="exp">獲得経験値</param>
    public void AddExp(int exp)
    {
        //PlayerManagerの経験値獲得処理メソッドを呼び出す
        playerManager.AddExp(exp);

        //SE再生
        AudioManager.Instance.PlaySE(SoundKey.AddExp);
    }

    /// <summary>
    /// 回復処理
    /// </summary>
    /// <param name="heal">回復量</param>
    public void Heal(float heal)
    {
        //PlayerManagerの回復処理メソッドを呼び出す
        playerManager.Heal(heal);

        //SE再生
        AudioManager.Instance.PlaySE(SoundKey.Heal);
    }

    /// <summary>
    /// IDamageableインターフェースのメソッド（ダメージを受け取る）
    /// </summary>
    /// <param name="damage">ダメージ値</param>
    void IDamageable.TakeDamage(float damage)
    {
        //PlayerManagerのダメージ処理メソッドを呼び出す
        playerManager.TakeDamage(damage);

        //被弾演出で発光させる
        if (flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
        }
        else
        {
            //SE再生
            AudioManager.Instance.PlaySE(SoundKey.PlayerHit);
        }
        
        flashCoroutine = StartCoroutine(FlashCoroutine());
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

    /// <summary>
    /// プレイヤーの死亡処理
    /// </summary>
    public void Die()
    {
        //プレイヤーの死亡アニメーション
        anim.SetTrigger("Death");

        //プレイヤーの判定を無効化する
        col.enabled = false;

        //プレイヤーをその場で停止させる
        rb.linearVelocity = Vector2.zero;

        //起動中の武器を全て停止させる
        weaponController.StopAllWeapons();

        //プレイヤーを移動させないようにする
        CanMove = false;
    }

    /// <summary>
    /// プレイヤーを停止させる
    /// </summary>
    public void Stop()
    {
        //移動アニメーションを停止
        anim.SetBool("Run", false);

        //プレイヤーをその場で停止させる
        rb.linearVelocity = Vector2.zero;

        //起動中の武器を全て停止させる
        weaponController.StopAllWeapons();

        //プレイヤーを移動させないようにする
        CanMove = false;
    }
}
