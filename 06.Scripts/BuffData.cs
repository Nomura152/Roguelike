using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuffData", menuName = "Scriptable Objects/BuffData")]
public class BuffData : ScriptableObject
{
    [Header("バフを適用するステータス")]
    [SerializeField] BuffType buffType;

    [Header("反映する値")]
    [SerializeField] float applyValue = 10;


    //---------------------<取得のみのプロパティ>----------------------

    public BuffType BuffType => buffType;

    public float ApplyValue => applyValue;
}
