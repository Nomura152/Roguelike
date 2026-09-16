using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ゲーム実行中の敵を管理するクラス
/// </summary>
public class EnemyManager : MonoBehaviour
{
    [SerializeField] PlayerManager playerManager;
    [SerializeField] ItemDropManager itemDropManager;
    [SerializeField] DeathEffectPool deathEffectPool;

    [Header("テスト用に生成する敵プレハブ"), SerializeField] Enemy testEnemy;
    [Header("敵生成距離"), SerializeField] float generateDistance = 7f;
    [Header("シーン上の敵の上限数"), SerializeField] int maxSpawnLimit = 500;

    [Header("シーン上の敵リスト"), SerializeField] List<Enemy> enemyList = new List<Enemy>();


    /// <summary>
    /// テスト用に敵を生成するメソッド
    /// </summary>
    [ContextMenu("敵生成をテスト")]
    public void TestSpawnEnemy()
    {
        //テスト用の敵を生成する
        SpawnEnemy(testEnemy);
    }

    /// <summary>
    /// 敵の生成処理
    /// </summary>
    /// <param name="enemy">生成する敵</param>
    public void SpawnEnemy(Enemy enemy)
    {
        //登録されている敵の数が上限であれば生成させない
        if(enemyList.Count >= maxSpawnLimit) { return; }

        ////敵の生成座標を計算
        Vector3 generatePos = GetGenerateEnemyPosition();

        ////敵生成
        var generatedEnemy = Instantiate(enemy, generatePos, Quaternion.identity);

        //敵を初期化する
        generatedEnemy.Initialize(playerManager.Player, this, itemDropManager, deathEffectPool);

        //生成した敵情報を追加
        RegisterEnemy(generatedEnemy);
    }

    /// <summary>
    /// 敵の生成座標を取得する
    /// </summary>
    /// <returns>敵の生成座標</returns>
    private Vector3 GetGenerateEnemyPosition()
    {
        //プレイヤーの座標を取得
        Vector3 playerPos = playerManager.Player.transform.position;

        //半径1の円周上でランダムな座標を取得
        Vector3 randomCircle = Random.onUnitCircle;

        //ランダムな座標に距離を乗算
        randomCircle *= generateDistance;

        //「プレイヤーの座標」＋「ランダムな円周上の座標」が敵生成座標となる
        return playerPos + randomCircle;
    }

    /// <summary>
    /// 敵の情報をリストに追加
    /// </summary>
    /// <param name="enemy">追加する敵</param>
    public void RegisterEnemy(Enemy enemy)
    {
        //リストにデータが既に存在していれば追加しない
        if (enemyList.Contains(enemy)) { return; }

        //リストに追加
        enemyList.Add(enemy);
    }

    /// <summary>
    /// 敵の情報をリストから削除
    /// </summary>
    /// <param name="enemy">削除する敵</param>
    public void UnregisterEnemy(Enemy enemy)
    {
        //リストから削除
        enemyList.Remove(enemy);
    }

    /// <summary>
    /// 全ての敵を非表示にして動きを止める
    /// </summary>
    public void HideAllEnemies()
    {
        foreach(Enemy enemy in enemyList)
        {
            enemy.gameObject.SetActive(false);
        }
    }
}
