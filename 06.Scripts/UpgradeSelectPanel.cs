using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// アップグレード選択時に表示するパネルUIを管理するクラス
/// </summary>
public class UpgradeSelectPanel : MonoBehaviour
{
    [Header("パネルオブジェクト本体"), SerializeField] GameObject panelOb;
    [Header("選択肢ボタンのリスト"), SerializeField] List<UpgradeSelectButton> buttonList;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //ゲーム開始時にパネルを非アクティブにする
        panelOb.SetActive(false);
    }

    /// <summary>
    /// パネルの表示
    /// </summary>
    /// <param name="selectedUpgradeDataList">抽選されたアップグレードのデータ（3つ）</param>
    /// <param name="onSelected">アップグレードの選択肢ボタンを押したときのコールバック</param>
    public void ShowPanel(List<UpgradeDataBase> selectedUpgradeDataList, Action<UpgradeDataBase> onSelected)
    {
        //パネルをアクティブにする
        panelOb.SetActive(true);

        //SE再生
        AudioManager.Instance.PlaySE(SoundKey.LevelUp);

        //それぞれのボタンにアップグレードのデータを渡す
        for(int i = 0; i < buttonList.Count; i++)
        {
            buttonList[i].SetData(selectedUpgradeDataList[i], onSelected);
        }
    }

    /// <summary>
    /// パネルの非表示
    /// </summary>
    public void HidePanel()
    {
        //パネルを非アクティブにする
        panelOb.SetActive(false);
    }
}
