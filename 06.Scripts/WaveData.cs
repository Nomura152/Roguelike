using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 一つのウェーブデータ
/// </summary>
[Serializable]
public class WaveData
{
    [Header("ウェーブ開始時間")]
    [SerializeField] float startTime = 60f;

    [Header("ウェーブで出現する敵リスト")]
    [SerializeField] List<Enemy> enemyPrefabs;

    [Header("敵生成の間隔")]
    [SerializeField] float spawnInterval = 2f;

    [Header("一度に生成する敵の数")]
    [SerializeField] int spawnCount = 1;


    //---------------------<外部から読み取り可能なプロパティ>----------------------

    public IReadOnlyList<Enemy> EnemyPrefabs => enemyPrefabs;

    public float StartTime => startTime;

    public float SpawnInterval => spawnInterval;

    public int SpawnCount => spawnCount;
}
