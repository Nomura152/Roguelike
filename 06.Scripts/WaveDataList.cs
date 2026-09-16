using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ゲーム内のステージで使用するウェーブデータをリストでまとめたスクリプタブルオブジェクト
/// </summary>
[CreateAssetMenu(fileName = "WaveDataList", menuName = "Scriptable Objects/WaveDataList")]
public class WaveDataList : ScriptableObject
{
    [Header("ウェーブ一覧（順番にウェーブが実行されます）")]
    [SerializeField] List<WaveData> waveDatas = new List<WaveData>();

    //-------------------<外部から情報を取得する用のプロパティ>------------------------

    public IReadOnlyList<WaveData> WaveDatas => waveDatas;
}
