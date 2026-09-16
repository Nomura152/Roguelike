using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ゲームクリア時の演出を監督するクラス
/// </summary>
public class GameClearDirector : MonoBehaviour
{
    [Header("ゲームクリアテキスト"), SerializeField] TMP_Text clearText;
    [Header("タイトルに戻るボタン"), SerializeField] Button returnTitleButton;

    [Header("ジングル")][SerializeField] AudioClip gameClearJingle;

    //他に演出で使用したいゲームオブジェクトがあれば、ここに追加してください。

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //テキストを一度非表示にする
        clearText.gameObject.SetActive(false);

        //ボタンを一度非表示にする
        returnTitleButton.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// ゲームクリア演出シーケンスの開始
    /// </summary>
    public void StartSequence()
    {
        StartCoroutine(SequenceCoroutine());
    }

    /// <summary>
    /// ゲームクリアの演出を順番に実行するコルーチン
    /// </summary>
    /// <returns></returns>
    private IEnumerator SequenceCoroutine()
    {
        //BGM停止
        AudioManager.Instance.StopBGM();

        //GameClearジングル再生
        AudioManager.Instance.PlayBGM(gameClearJingle, false);

        //クリア用テキストを表示
        clearText.gameObject.SetActive(true);

        //今回は実装しませんが、音などの演出があればここに記述してください。

        //3秒待機
        yield return new WaitForSeconds(3f);

        //ボタン表示
        returnTitleButton.gameObject.SetActive(true);
    }
}
