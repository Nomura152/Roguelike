using System.Collections.Generic;
using UnityEngine;

public class Weapon_SpikedIronBall : WeaponBase
{
    [Header("攻撃判定の鉄球プレハブ")]
    [SerializeField] RepeatAttackHitbox spikedIronBallPrefab;

    [Header("シーン上に展開されている鉄球")]
    [SerializeField] List<RepeatAttackHitbox> generatedBalls = new List<RepeatAttackHitbox>();

    [Header("プレイヤーを中心とした弾との距離")]
    [SerializeField] float length = 1f;

    protected override void OnInitialized()
    {
        //初期レベルの弾の数を確認
        int generateCount = CurrentLevelData.AttackCount;

        //鉄球を生成
        GenerateSpikedIronBall(generateCount);
    }

    protected override void OnLevelUp()
    {
        //レベルデータのカウント数と、現在の鉄球の数を減算する
        int generateCount = CurrentLevelData.AttackCount - generatedBalls.Count;

        //鉄球を生成
        GenerateSpikedIronBall(generateCount);
    }

    void Update()
    {
        //鉄球が0なら処理終了
        if(generatedBalls.Count <= 0) { return; }

        //座標、回転、スケール、攻撃判定のパラメータを更新
        UpdateWeaponTransform();
        UpdateBallStatus();
        UpdateBallPositions();
    }

    /// <summary>
    /// ボールの座標以外のトランスフォーム情報を更新
    /// </summary>
    private void UpdateWeaponTransform()
    {
        //親の回転やプレイヤーの左右反転の影響を受けず、鉄球の回転値をワールド基準に保つ
        transform.rotation = Quaternion.identity;

        //弾のサイズは武器システム本体の親オブジェクトを拡大する
        //（サイズは（1,1,1）×「武器レベルデータのサイズ」×「プレイヤーの弾のサイズ倍率」とする）
        transform.localScale = Vector3.one * CurrentLevelData.Scale * PlayerManager.Stats.ProjectileScaleMultiplier;
    }

    /// <summary>
    /// ボールの攻撃判定パラメータを更新
    /// </summary>
    private void UpdateBallStatus()
    {
        //鉄球のダメージ値は、「武器のレベルデータの基礎ダメージ」×「プレイヤーの攻撃力」とする
        float damage = CurrentLevelData.Damage * PlayerManager.Stats.Strength;

        //鉄球の攻撃判定インターバルは「武器のレベルデータのヒット間隔」の値をそのまま使用する
        float interval = CurrentLevelData.HitInterval;

        //全ての鉄球のパラメータをfor文で更新
        for (int i = 0; i < generatedBalls.Count; i++)
        {
            var ball = generatedBalls[i];

            ball.SetDamage(damage);
            ball.SetHitInterval(interval);
        }
    }

    /// <summary>
    /// ボールの回転位置を更新
    /// </summary>
    private void UpdateBallPositions()
    {
        //時間を更新（時間は「武器レベルデータの弾速」×「プレイヤーの弾速倍率」とする）
        float time = Time.time * CurrentLevelData.ProjectileSpeed * PlayerManager.Stats.ProjectileSpeedMultiplier;

        //鉄球の配置を360度の周囲に等間隔で配置する為、鉄球ごとの一つ一つの角度を計算する
        float angleStep = 360f / generatedBalls.Count;

        //全ての鉄球の位置をfor文で更新
        for (int i = 0; i < generatedBalls.Count; i++)
        {
            var ball = generatedBalls[i];

            //弾の回転角度を取得
            float angle = time + (angleStep * i);   //鉄球ごとの配置する角度
            Quaternion rot = Quaternion.Euler(0f, 0f, angle);

            //鉄球の位置（座標（0,1,0）×「プレイヤーと鉄球の距離」）
            Vector3 pos = Vector3.up * length;

            //鉄球の最終座標を回転角度を混ぜた位置を武器のローカル座標にする
            ball.transform.localPosition = rot * pos;
        }
    }

    /// <summary>
    /// 鉄球を生成する
    /// </summary>
    /// <param name="count">生成する鉄球の数</param>
    private void GenerateSpikedIronBall(int count)
    {
        //生成する数が0以下なら処理しない
        if (count <= 0) { return; }

        //増加する弾の数分ループ
        for (int i = 0; i < count; i++)
        {
            //鉄球を子オブジェクトとして生成
            var ball = Instantiate(spikedIronBallPrefab, transform);

            //生成した鉄球をリストに追加
            generatedBalls.Add(ball);

            //鉄球のダメージ値は、「武器のレベルデータの基礎ダメージ」×「プレイヤーの攻撃力」とする
            float damage = CurrentLevelData.Damage * PlayerManager.Stats.Strength;

            //鉄球の攻撃判定インターバルは「武器のレベルデータのヒット間隔」の値をそのまま使用する
            float interval = CurrentLevelData.HitInterval;

            //鉄球の攻撃に使用する値を初期化
            ball.Initialize(damage, interval);
        }
    }
}
