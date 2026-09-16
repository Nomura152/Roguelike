using UnityEngine;

/// <summary>
/// 敵がドロップするアイテムのベースクラス
/// 回復アイテム、経験値のダイヤ、宝箱などはこのクラスを継承して実装する
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public abstract class ItemBase : MonoBehaviour
{

    //-------------------<継承先にのみ情報を取得する用のプロパティ>------------------------
    protected ItemDropManager itemDropManager { get; private set; }

    //---------------------------------------------------------------------------------

    /// <summary>
    /// アイテムの初期化処理
    /// </summary>
    /// <param name="manager"></param>
    public void Initialize(ItemDropManager manager)
    {
        //ItemDropManagerの情報を記憶する
        itemDropManager = manager;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //ヒットした相手のレイヤー名が「Player」かを確認
        if(collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            //ヒット相手から「Player」コンポーネントを取得する
            Player player = collision.gameObject.GetComponent<Player>();

            if(player != null)
            {
                //アイテムの効果を適用する処理を呼び出す
                Apply(player);

                //アイテムの情報を「ItemDropManager」から削除する
                itemDropManager.UnregisterItem(this);

                //アイテムオブジェクトを削除する
                Destroy(gameObject);
            }
        }
    }

    /// <summary>
    /// アイテムの効果を適用する処理
    /// 具体的な処理（回復や経験値獲得など）は継承先で実装する
    /// </summary>
    protected abstract void Apply(Player player);
}
