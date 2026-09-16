using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ゲームオーバー時の演出を監督するクラス
/// </summary>
public class GameOverDirector : MonoBehaviour
{
    [Header("ゲームオーバーテキスト"), SerializeField] TMP_Text gameOverText;
    [Header("タイトルに戻るボタン"), SerializeField] Button returnTitleButton;

    [Header("ジングル")][SerializeField] AudioClip gameOverJingle;

    //他に演出で使用したいゲームオブジェクトがあれば、ここに追加してください。

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //テキストを一度非表示にする
        gameOverText.gameObject.SetActive(false);

        //ボタンを一度非表示にする
        returnTitleButton.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// ゲームオーバー演出シーケンスの開始
    /// </summary>
    public void StartSequence()
    {
        StartCoroutine(SequenceCoroutine());
    }

    /// <summary>
    /// ゲームオーバーの演出を順番に実行するコルーチン
    /// </summary>
    /// <returns></returns>
    private IEnumerator SequenceCoroutine()
    {
        //BGM停止
        AudioManager.Instance.StopBGM();

        //GameOverジングル再生
        AudioManager.Instance.PlayBGM(gameOverJingle, false);

        //プレイヤーが倒れるまで2秒程待機
        yield return new WaitForSeconds(2f);

        //ゲームオーバー用テキストを表示
        gameOverText.gameObject.SetActive(true);

        //ボタン表示まで3秒程待機
        yield return new WaitForSeconds(3f);

        //ボタン表示
        returnTitleButton.gameObject.SetActive(true);

        //今回は実装しませんが、音などの演出があればここに記述してください。
    }
}
