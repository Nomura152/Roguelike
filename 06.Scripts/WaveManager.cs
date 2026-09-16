using UnityEngine;

/// <summary>
/// 敵生成のウェーブの管理者
/// </summary>
public class WaveManager : MonoBehaviour
{
    [SerializeField] GameMainManager gameMainManager;
    [SerializeField] EnemyManager enemyManager;

    [Header("ウェーブデータ")]
    [SerializeField] WaveDataList waveDataList;

    [Header("ゲーム中のパラメータ")]
    [Tooltip("現在のウェーブ番号"), SerializeField] int currentWaveIndex = 0;
    [Tooltip("敵生成のタイマー"), SerializeField] float spawnTimer = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //番号リセット
        currentWaveIndex = 0;

        //タイマーリセット
        spawnTimer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        //ゲームプレイ中でなければ、何もさせない
        if (gameMainManager.CurrentGameState != GameMainManager.GameState.Playing) { return; }

        //ウェーブデータ更新
        UpdateWave();

        //---------------------------<↓敵生成処理>------------------------------

        //敵生成のタイマーを更新
        spawnTimer += Time.deltaTime;

        //現在のウェーブデータを取得
        var currentWaveData = waveDataList.WaveDatas[currentWaveIndex];

        //現在の敵生成タイマーが「WaveData」の敵生成インターバル時間を超えていれば生成可能
        if (spawnTimer >= currentWaveData.SpawnInterval)
        {
            //敵生成
            SpawnEnemy();

            //タイマーリセット
            spawnTimer = 0f;
        }
    }

    /// <summary>
    /// ウェーブデータの更新
    /// </summary>
    private void UpdateWave()
    {
        //ウェーブ番号がリストの最後まで到達していれば処理終了
        if(currentWaveIndex >= waveDataList.WaveDatas.Count - 1) { return; }

        //現在のゲーム経過時間を取得
        float gameTime = gameMainManager.CurrentGameTime;

        //次のウェーブ開始時間を取得
        float nextWaveStartTime = waveDataList.WaveDatas[currentWaveIndex + 1].StartTime;   //次のウェーブなので+1

        //次のウェーブ開始時間を超えていればウェーブ変更
        if(gameTime >= nextWaveStartTime)
        {
            //ウェーブ番号更新
            currentWaveIndex++;

            Debug.Log("ウェーブ番号が" + currentWaveIndex + "に移行しました");
        }
    }

    /// <summary>
    /// 敵キャラクターの生成
    /// </summary>
    private void SpawnEnemy()
    {
        //現在のウェーブデータを取得
        var waveData = waveDataList.WaveDatas[currentWaveIndex];

        //ウェーブデータに登録されている一度に生成する敵の数を取得
        int count = waveData.SpawnCount;

        //生成する数ぶんループさせる
        for(int i = 0; i < count; i++)
        {
            //ウェーブデータに登録されている敵リストからランダムで抽出
            int random = Random.Range(0, waveData.EnemyPrefabs.Count);
            Enemy target = waveData.EnemyPrefabs[random];

            //EnemyManagerに敵の生成をお願いする
            enemyManager.SpawnEnemy(target);
        }
    }
}
