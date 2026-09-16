using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーのレベルアップ時のアップグレードを管理するマネージャー
/// </summary>
public class UpgradeManager : MonoBehaviour
{
    [SerializeField] PlayerManager playerManager;
    [SerializeField] GameMainManager gameMainManager;
    [SerializeField] UpgradeSelectPanel upgradeSelectPanel;

    [Header("アップグレードのデータリスト"), SerializeField] UpgradeDataList upgradeDataList;
    [Header("テスト用に獲得するアップグレードデータのリスト番号"), SerializeField] int testApplyUpgradeIndex;


    [Header("武器候補が選ばれる割合（0～1）"), SerializeField, Range(0f, 1f)] float weaponAppearanceRate = 0.5f;
    [Header("アップグレードの候補数（デフォルトは3）"), SerializeField] int optionCount = 3;


    [Header("抽選可能な武器データ"), SerializeField] List<UpgradeData_Weapon> selectableWeaponList = new List<UpgradeData_Weapon> ();
    [Header("抽選可能なバフデータ"), SerializeField] List<UpgradeData_Buff> selectableBuffList = new List<UpgradeData_Buff> ();
    [Header("表示する3つの選択肢のデータ"), SerializeField] List<UpgradeDataBase> options = new List<UpgradeDataBase> ();


    /// <summary>
    /// テスト用のアップグレード適用処理
    /// </summary>
    public void TestApplyUpgrade()
    {
        Debug.Log("アップグレード適用");

        //設定した番号の武器のアップグレードデータをデータリストから取得
        //UpgradeData_Weapon upgradeData = upgradeDataList.WeaponList[testApplyUpgradeIndex];

        //PlayerManagerに武器データを渡して新規獲得orレベルアップ
        //playerManager.AddOrLevelUpWeapon(upgradeData.WeaponData);

        //設定した番号のバフのアップグレードデータをデータリストから取得
        UpgradeData_Buff upgradeData = upgradeDataList.BuffList[testApplyUpgradeIndex];

        //PlayerManagerにバフデータを渡してプレイヤーのステータスを変化させる
        playerManager.ApplyBuff(upgradeData.BuffData);
    }

    /// <summary>
    /// アップグレード開始処理
    /// </summary>
    public void StartUpgrade()
    {
        Debug.Log("アップグレード開始");

        //アップグレード選択状態を変更し、ゲームを一時停止させる
        gameMainManager.StartUpgradeSelect();

        //アップグレードで表示する選択肢を決定
        SelectUpgradeOptions();

        //アップグレード選択パネルを表示
        upgradeSelectPanel.ShowPanel(options, SelectUpgrade);
    }

    /// <summary>
    /// アップグレードで表示する3つの選択肢を抽選
    /// </summary>
    private void SelectUpgradeOptions()
    {
        //現在のリストをリセットする
        options.Clear();

        //武器データの中から抽選可能なデータをすべて取得する
        CreateWeaponCandidates();
        //バフデータの中から抽選可能なデータを全て取得する
        CreateBuffCandidates();

        //3つのデータをランダムに抽選
        for(int i = 0; i < optionCount; i++)
        {
            //武器かバフデータのどちらを選ぶかをランダム値で決定
            bool shouldSelectWeapon = Random.value <= weaponAppearanceRate;

            //武器の抽選可能データが存在しない場合は必ずバフデータを選ぶ
            if(selectableWeaponList.Count <= 0)
            {
                shouldSelectWeapon = false;
            }

            //武器データを選ぶかどうかで処理分岐
            if (shouldSelectWeapon)
            {
                Debug.Log("武器が選ばれました");

                //武器抽選リストの中から番号をランダムで決定
                int randomIndex = Random.Range(0, selectableWeaponList.Count);

                //武器をアップグレード選択肢に登録
                options.Add(selectableWeaponList[randomIndex]);

                //同じ選択肢を選ばないように抽選リストから取り除く
                selectableWeaponList.RemoveAt(randomIndex);
            }
            else
            {
                Debug.Log("バフが選ばれました");

                //バフ抽選リストの中から番号をランダムで決定
                int randomIndex = Random.Range(0, selectableBuffList.Count);

                //バフをアップグレード選択肢に登録
                options.Add(selectableBuffList[randomIndex]);

                //同じ選択肢を選ばないように抽選リストから取り除く
                selectableBuffList.RemoveAt(randomIndex);
            }
        }
    }

    /// <summary>
    /// 武器の所持状況やレベルを確認して、アップグレードとして抽選可能な武器リストを抽出する
    /// </summary>
    private void CreateWeaponCandidates()
    {
        //リストをリセット
        selectableWeaponList.Clear();

        //すべての武器データを確認して取得できる武器データを抽出する
        foreach (UpgradeData_Weapon upgradeWeaponData in upgradeDataList.WeaponList)
        {
            //PlayerManagerからWeaponControllerの情報を取得する
            var weaponController = playerManager.Player.WeaponController;

            //対象の武器が追加orレベルアップが出来るかを確認する
            bool ok = weaponController.CanAddOrLevelUp(upgradeWeaponData.WeaponData);

            //追加orレベルアップが可能な場合は抽選リストに追加する
            if (ok)
            {
                selectableWeaponList.Add(upgradeWeaponData);
            }
        }
    }

    /// <summary>
    /// バフの抽選候補リストを作成する
    /// </summary>
    private void CreateBuffCandidates()
    {
        //リストをリセット
        selectableBuffList.Clear();

        //すべてのバフデータが抽選可能なので、そのままリストに追加する
        foreach (UpgradeData_Buff upgradeBuffData in upgradeDataList.BuffList)
        {
            selectableBuffList.Add(upgradeBuffData);
        }
    }

    /// <summary>
    /// アップグレードを選択したときの処理
    /// </summary>
    /// <param name="selectedData">選択されたアップグレードデータ</param>
    private void SelectUpgrade(UpgradeDataBase selectedData)
    {
        Debug.Log(selectedData.name + "が選択されました！");

        //取得したアップグレードデータの内容を適用
        ApplyUpgrade(selectedData);

        //アップグレードを終了する
        EndUpgrade();
    }


    /// <summary>
    /// アップグレード適用処理
    /// </summary>
    /// <param name="upgradeData">対象のアップグレードデータ</param>
    private void ApplyUpgrade(UpgradeDataBase upgradeData)
    {
        switch (upgradeData)
        {
            case UpgradeData_Buff buffData:
                //バフの適用
                playerManager.ApplyBuff(buffData.BuffData);
                break;

            case UpgradeData_Weapon weaponData:
                //武器の新規獲得またはレベルアップ
                playerManager.AddOrLevelUpWeapon(weaponData.WeaponData);
                break;
        }
    }

    /// <summary>
    /// アップグレード終了処理
    /// </summary>
    public void EndUpgrade()
    {
        Debug.Log("アップグレード終了");

        //アップグレード選択画面UIを非表示にする
        upgradeSelectPanel.HidePanel();

        //ゲーム状態を元に戻す
        gameMainManager.EndUpgradeSelect();
    }

    //private void CheckAddOrLevelUpWeapons()
    //{
    //    //リストを一度リセットする
    //    selectableWeapons.Clear();

    //    //全ての武器データを取得
    //    var weaponDatas = upgradeDataList.WeaponDatas;

    //    //全ての武器データをforeach文で検証
    //    foreach(var weaponData in weaponDatas)
    //    {
    //        //対象の武器がレベルアップまたは獲得可能かをチェックする
    //        bool ok = playerManager.Player.WeaponController.CanAddOrLevelUp(weaponData);

    //        //レベルアップまたは獲得可能ならリストに追加
    //        if (ok)
    //        {
    //            //selectableWeapons.Add(weaponData);
    //        }
    //    }
    //}
}
