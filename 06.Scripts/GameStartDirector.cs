using System.Collections;
using TMPro;
using UnityEngine;

/// <summary>
/// ゲームスタート時の演出を監督するクラス
/// </summary>
public class GameStartDirector : MonoBehaviour
{
    [Header("ゲーム開始テキスト"), SerializeField] TMP_Text startText;
    [Header("BGM")][SerializeField] AudioClip bgm;

    //他に演出で使用したいゲームオブジェクトがあれば、ここに追加してください。

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        //テキストを一度非表示にする
        startText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    /// <summary>
    /// ゲームスタート演出シーケンスの開始
    /// </summary>
    public void StartSequence()
    {
        StartCoroutine(SequenceCoroutine());
    }

    /// <summary>
    /// ゲーム開始の演出を順番に実行するコルーチン
    /// </summary>
    /// <returns></returns>
    private IEnumerator SequenceCoroutine()
    {
        //スタート用テキストを表示
        startText.gameObject.SetActive(true);

        //今回は実装しませんが、音などの演出があればここに記述してください。

        //2秒待機
        yield return new WaitForSeconds(2f);

        //テキストを非表示にする
        startText.gameObject.SetActive(false);

        //BGM再生
        AudioManager.Instance.PlayBGM(bgm, true);
    }
}
