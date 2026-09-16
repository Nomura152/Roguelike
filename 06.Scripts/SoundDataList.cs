using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "SoundDataList", menuName = "Scriptable Objects/SoundDataList")]
public class SoundDataList : ScriptableObject
{
    [Header("音源キーと対応する音源をまとめたリスト")]
    [SerializeField] List<SoundPair> soundDatas;

    public IReadOnlyList<SoundPair> SoundPairDatas => soundDatas;

    //[SerializeField] Dictionary<SoundKey, AudioClip> soundDictionary;   //Unityの6000.6よりSerializeFieldに対応しています。
    
    //public IReadOnlyDictionary<SoundKey, AudioClip> SoundDictionary => soundDictionary;
}


[Serializable]
public class SoundPair
{
    [SerializeField] SoundKey key;
    [SerializeField] SoundData value;

    public SoundKey Key => key;
    public SoundData Value => value;
}