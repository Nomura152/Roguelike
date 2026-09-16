using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// シーン上のドロップしたアイテムやアイテムのドロップを管理するクラス
/// </summary>
public class ItemDropManager : MonoBehaviour
{
    [Header("テスト用に使用するアイテムドロップデータ"), SerializeField] ItemDropData testData;
    [Header("シーン上のアイテムの上限数"), SerializeField] int maxDropLimit = 200;

    [Header("シーン上のドロップアイテムリスト"), SerializeField] List<ItemBase> dropItemList = new List<ItemBase>();

    /// <summary>
    /// テスト用にアイテムをドロップするメソッド
    /// </summary>
    [ContextMenu("アイテムドロップをテスト")]
    public void TestDropItem()
    {
        //テスト用にアイテムのドロップ位置を適当に決定
        Vector3 testDropPos = new Vector3(5f, 5f, 0f);

        //アイテムドロップ
        DropItem(testData, testDropPos);
    }

    /// <summary>
    /// アイテムのドロップ処理
    /// </summary>
    /// <param name="itemDropData">アイテムドロップのデータ</param>
    /// <param name="dropPos">アイテムがドロップする位置</param>
    public void DropItem(ItemDropData itemDropData, Vector3 dropPos)
    {
        //アイテムドロップ抽選（0～100％）
        float random_Drop = Random.Range(0f, 100f);

        //ドロップ率の値以下であればアイテム確定
        if (random_Drop <= itemDropData.DropRate)
        {
            //ドロップするアイテムを取得するメソッドを呼ぶ
            var dropItem = GetDropItem(itemDropData);


            //アイテムドロップ
            ItemBase droppedItem = Instantiate(dropItem, dropPos, Quaternion.identity);

            //生成したアイテムを初期化
            droppedItem.Initialize(this);

            //アイテムの情報をリストに追加
            RegisterItem(droppedItem);

            //シーン上に生成可能なアイテムの上限数を超えていれば、古いアイテムから削除する
            if(dropItemList.Count > maxDropLimit)
            {
                //一番古い（0番目）アイテムの情報を取得
                ItemBase oldestItem = dropItemList[0];

                //古いアイテムの情報をリストから削除
                UnregisterItem(oldestItem);

                //アイテムオブジェクトを削除
                Destroy(oldestItem.gameObject);
            }
        }
    }

    /// <summary>
    /// ドロップするアイテムを抽選で取得
    /// </summary>
    /// <param name="itemDropData">アイテムドロップのデータ</param>
    /// <returns>ドロップするアイテム</returns>
    private ItemBase GetDropItem(ItemDropData itemDropData)
    {
        //レアアイテムドロップ抽選（0～100％）
        float random_Rare = Random.Range(0f, 100f);

        //レアドロップ率の値以下であればレアアイテム確定
        if (random_Rare <= itemDropData.RareDropRate)
        {
            return itemDropData.RareDropItemPrefab;
        }
        else
        {
            return itemDropData.DropItemPrefab;
        }
    }

    /// <summary>
    /// アイテムの情報をリストに追加
    /// </summary>
    /// <param name="item">追加するアイテム</param>
    public void RegisterItem(ItemBase item)
    {
        //リストにデータが既に存在していれば追加しない
        if (dropItemList.Contains(item)) { return; }

        //リストに追加
        dropItemList.Add(item);
    }

    /// <summary>
    /// アイテムの情報をリストから削除
    /// </summary>
    /// <param name="item">削除するアイテム</param>
    public void UnregisterItem(ItemBase item)
    {
        //リストから削除
        dropItemList.Remove(item);
    }
}
