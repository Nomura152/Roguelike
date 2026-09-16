using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI.Table;

public class AudioManager : MonoBehaviour
{
    [Header("BGM用のAudioScource")]
    [Tooltip("BGM用のAudioSource"), SerializeField] AudioSource bgmSource;

    [Header("SE用のAudioSource")]
    [Tooltip("優先度最高のAudioSource"), SerializeField] AudioSource seSource_Top;
    [Tooltip("優先度高のAudioSource"), SerializeField] AudioSource seSource_High;
    [Tooltip("優先度中のAudioSource"), SerializeField] AudioSource seSource_Medium;
    [Tooltip("優先度低のAudioSource"), SerializeField] AudioSource seSource_Low;
    [Tooltip("優先度最低のAudioSource"), SerializeField] AudioSource seSource_Bottom;

    [Header("ゲーム全体で共通使用する音源")]
    [SerializeField] SoundDataList commonSoundData;

    /// <summary>
    /// 誰でも呼び出せるようにシングルトンにする
    /// </summary>
    public static AudioManager Instance { get; private set; }

    //共通音源をまとめたディクショナリ
    Dictionary<SoundKey, SoundData> commonSoundDictionary = new Dictionary<SoundKey, SoundData>();

    //再生可能したSEデータと再生した時の時間を記録したディクショナリ
    Dictionary<SoundData, float> lastPlayTimesDictionary = new Dictionary<SoundData, float>();


    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            Initialize();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 初期化
    /// </summary>
    private void Initialize()
    {
        //共通音源データをディクショナリに変換
        commonSoundDictionary.Clear();
        foreach(var pair in commonSoundData.SoundPairDatas)
        {
            commonSoundDictionary.Add(pair.Key, pair.Value);
        }

        //BGM用のAudioSourceの優先度を設定
        bgmSource.priority = 0;

        //SE用のAudioSourceの優先度を設定（低いほど優先される）
        seSource_Top.priority = 16;
        seSource_High.priority = 64;
        seSource_Medium.priority = 128;
        seSource_Low.priority = 192;
        seSource_Bottom.priority = 240;
    }

    /// <summary>
    /// BGMを再生
    /// </summary>
    public void PlayBGM(AudioClip bgm, bool loop)
    {
        //BGM音源が存在するか確認
        if(bgm == null) { return; }

        //BGM再生
        bgmSource.clip = bgm;
        bgmSource.loop = loop;
        bgmSource.Play();
    }

    /// <summary>
    /// BGMを停止
    /// </summary>
    public void StopBGM()
    {
        bgmSource.Stop();
    }

    /// <summary>
    /// SEをキーを元に探して鳴らす
    /// </summary>
    /// <param name="key">音源に対応するキー</param>
    public void PlaySE(SoundKey key)
    {
        //共通音源リストから該当するキーの音源データを探す
        if (commonSoundDictionary.TryGetValue(key, out SoundData data))
        {
            //取得した音源データをそのまま鳴らす
            PlaySE(data);
        }
        else
        {
            Debug.LogWarning($"{key}に該当する音源データが存在しませんでした。");
        }
    }

    /// <summary>
    /// SEを再生
    /// </summary>
    public void PlaySE(SoundData data)
    {
        //SE音源が存在するか確認
        if (data.Clip == null) { return; }

        //現在の時刻を取得
        float currentTime = Time.unscaledTime;

        // クールダウン判定
        if (lastPlayTimesDictionary.TryGetValue(data, out float lastTime))
        {
            //指定したクールタイムをまだ経過していない場合は処理終了
            if (currentTime - lastTime < data.Cooldown) { return; }
        }

        //最後に再生したSEを登録or再生時刻を更新
        lastPlayTimesDictionary[data] = currentTime;

        //対応する優先度のAudioScourceを使用して再生
        switch (data.Priority)
        {
            case SoundPriority.Top:
                seSource_Top.PlayOneShot(data.Clip, data.Volume);
                break;
            case SoundPriority.High:
                seSource_High.PlayOneShot(data.Clip, data.Volume);
                break;
            case SoundPriority.Medium:
                seSource_Medium.PlayOneShot(data.Clip, data.Volume);
                break;
            case SoundPriority.Low:
                seSource_Low.PlayOneShot(data.Clip, data.Volume);
                break;
            case SoundPriority.Bottom:
                seSource_Bottom.PlayOneShot(data.Clip, data.Volume);
                break;
        }
    }
}
