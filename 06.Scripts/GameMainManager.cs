using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// ゲームの進行を管理する代表者
/// </summary>
public class GameMainManager : MonoBehaviour
{
    [SerializeField] PlayerManager playerManager;
    [SerializeField] EnemyManager enemyManager;

    //ゲーム状態
    public enum GameState
    {
        Ready,      // 開始前
        Playing,   // ゲーム中
        UpgradeSelect,   //アップグレード選択中
        GameOver,  // ゲームオーバー
        GameClear  // ゲームクリア
    }

    [Header("目標時間"), SerializeField] float targetTime = 600f;

    [Header("ゲーム中のパラメータ")]
    [Tooltip("現在のゲーム状態"), SerializeField] GameState currentGameState = GameState.Ready;
    [Tooltip("現在のゲーム経過時間"), SerializeField] float currentGameTime = 0f;

    [Header("ゲームイベント")]
    [Tooltip("ゲームスタート時のイベント"), SerializeField] UnityEvent startGameEvent;
    [Tooltip("ゲームオーバー時のイベント"), SerializeField] UnityEvent gameOverEvent;
    [Tooltip("ゲームクリア時のイベント"), SerializeField] UnityEvent gameClearEvent;

    //-------------------<外部から情報を取得する用のプロパティ>------------------------

    public GameState CurrentGameState => currentGameState;
    public float CurrentGameTime => currentGameTime;
    public float TargetTime => targetTime;

    //---------------------------------------------------------------------------------


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //ゲーム開始処理を呼び出す
        StartGame();
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentGameState)
        {
            case GameState.Ready:
                break;
            case GameState.Playing:

                //ゲーム進行中のみゲーム経過時間を計測
                currentGameTime += Time.deltaTime;

                //目標時間に到達していればゲームクリア
                if(currentGameTime >= targetTime)
                {
                    GameClear();
                }

                break;
            case GameState.UpgradeSelect:
                break;
            case GameState.GameOver:
                break;
            case GameState.GameClear:
                break;
        }
    }

    /// <summary>
    /// ゲームスタート
    /// </summary>
    private void StartGame()
    {
        //ゲーム状態を変更する
        currentGameState = GameState.Playing;

        //ゲームスタートイベントを通知
        startGameEvent.Invoke();
    }

    /// <summary>
    /// アップグレード選択開始
    /// </summary>
    public void StartUpgradeSelect()
    {
        //ゲーム中ではない（ゲームクリア状態など）ならアップグレード選択中にさせない
        if (currentGameState != GameState.Playing)
        {
            return;
        }

        //ゲーム状態を変更する
        currentGameState = GameState.UpgradeSelect;

        //タイムスケールを0にしてゲームを停止させる
        Time.timeScale = 0f;
    }

    /// <summary>
    /// アップグレード選択終了
    /// </summary>
    public void EndUpgradeSelect()
    {
        //アップグレード選択中ではないなら処理終了
        if (currentGameState != GameState.UpgradeSelect)
        {
            return;
        }

        //ゲーム状態を変更する
        currentGameState = GameState.Playing;

        //タイムスケールを1にして停止していたゲームを再開させる
        Time.timeScale = 1f;
    }

    /// <summary>
    /// ゲームオーバー
    /// </summary>
    public void GameOver()
    {
        //ゲーム中ではない（ゲームクリア状態など）ならゲームオーバーにさせない
        if(currentGameState != GameState.Playing) { return; }

        //ゲーム状態を変更する
        currentGameState = GameState.GameOver;

        //ゲームオーバーイベントを通知
        gameOverEvent.Invoke();
    }

    /// <summary>
    /// ゲームクリア
    /// </summary>
    public void GameClear()
    {
        //ゲーム中ではない（ゲームオーバー状態など）ならゲームクリアにさせない
        if (currentGameState != GameState.Playing) { return; }

        //ゲーム状態を変更する
        currentGameState = GameState.GameClear;

        //プレイヤーを停止させる
        playerManager.Stop();

        //敵の動きを止める
        enemyManager.HideAllEnemies();

        //ゲームクリアイベントを通知
        gameClearEvent.Invoke();
    }
}
