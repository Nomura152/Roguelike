using TMPro;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    [SerializeField] GameMainManager gameMainManager;
    [SerializeField] TMP_Text timerText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //最初は00:00にする
        timerText.text = "00:00";
    }

    // Update is called once per frame
    void Update()
    {
        //現在のゲーム経過時間をGameMainManagerのプロパティから取得
        float time = gameMainManager.CurrentGameTime;

        //秒数から○○：○○の形式の文字列に変換する
        string timeText = ConvertSecondsToTime(time);

        //テキストメッシュプロに反映
        timerText.text = timeText;
    }

    /// <summary>
    /// 秒数から時間の文字列に変換する
    /// </summary>
    /// <param name="time">現在の時間</param>
    /// <returns></returns>
    private string ConvertSecondsToTime(float time)
    {
        //小数点以下の数値を型変換で切り捨てる
        int totalSeconds = (int)time;

        //---------------<数値を時間の表示に変換>-------------------

        int minutes = totalSeconds / 60;       // 分に変換
        int seconds = totalSeconds % 60;       // 残りを秒数に変換

        return $"{minutes:D2}:{seconds:D2}"; // 〇〇:〇〇 形式にする
    }
}
