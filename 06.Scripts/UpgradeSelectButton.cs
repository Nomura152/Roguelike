using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// アップグレード時の選択ボタンを操作するクラス
/// </summary>
public class UpgradeSelectButton : MonoBehaviour
{
    [Header("ボタンに表示しているテキストや画像")]
    [SerializeField] TMP_Text nameText;
    [SerializeField] TMP_Text descriptionText;
    [SerializeField] Image iconImage;

    //使用しているアップグレードデータ
    UpgradeDataBase upgradeData;

    //ボタンを押したときに実行するコールバック
    Action<UpgradeDataBase> onSelectedUpgrade;

    /// <summary>
    /// ボタンで使用する情報を取得して反映する
    /// </summary>
    /// <param name="data">対象のアップグレードデータ</param>
    /// <param name="onSelected">自身の選択肢ボタンを押したときのコールバック</param>
    public void SetData(UpgradeDataBase data, Action<UpgradeDataBase> onSelected)
    {
        //適用するデータを保存する
        upgradeData = data;

        //コールバックを代入して置き換える（登録ではないので注意！）
        onSelectedUpgrade = onSelected;

        // 武器でもバフでも共通情報を表示できる
        nameText.text = upgradeData.DisplayName;
        descriptionText.text = upgradeData.Description;
        iconImage.sprite = upgradeData.Icon;
    }

    /// <summary>
    /// ボタンを押したときに呼び出すイベントメソッド
    /// Buttonコンポーネントにインスペクターから登録する
    /// </summary>
    public void OnClick()
    {
        Debug.Log("アップグレードボタンクリック！");

        //アップグレード選択時のSE再生
        AudioManager.Instance.PlaySE(SoundKey.SelectUpgrade);

        //登録されているコールバックを実行
        //引数としてUpgradeDataBaseが必要なので渡す
        onSelectedUpgrade.Invoke(upgradeData);
    }
}
