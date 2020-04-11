using System;
using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    private const string NoBGMKey = "none";
    private const string NoChangeBGMKey = "no_change";
    private const float FadeSeconds = 0.5f;

    private AudioSource sfxSource;
    private AudioSource bgmSource;

    private float baseVolume = 1.0f;
    private float bgmVolumeMult = 1.0f;
    private Setting<float> bgmVolumeSetting;
    private Setting<float> sfxVolumeSetting;

    public string CurrentBGMKey { get; private set; }

    public void Awake() {
        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.playOnAwake = false;
        sfxSource.loop = false;

        bgmSource = gameObject.AddComponent<AudioSource>();
        bgmSource.playOnAwake = false;
        bgmSource.loop = true;

        CurrentBGMKey = NoBGMKey;

        gameObject.AddComponent<AudioListener>();
        gameObject.AddComponent<WaveSource>();
    }

    public void Start() {
        sfxVolumeSetting = Global.Instance().Serialization.SystemData.SettingSoundEffectVolume;
        bgmVolumeSetting = Global.Instance().Serialization.SystemData.SettingMusicVolume;
    }

    public void Update() {
        bgmSource.volume = bgmVolumeSetting.Value * baseVolume * bgmVolumeMult;
        sfxSource.volume = sfxVolumeSetting.Value * baseVolume;
    }

    public static void PlayFail() {
        Global.Instance().Audio.PlaySFX("fail");
    }

    public WaveSource GetWaveSource() {
        return GetComponent<WaveSource>();
    }

    public void PlaySFX(Enum enumValue) {
        PlaySFX(enumValue.ToString());
    }
    public void PlaySFX(string key, float muteDuration = 0.0f) {
        AudioClip clip = IndexDatabase.Instance().SFX.GetData(key).clip;
        StartCoroutine(PlaySFXRoutine(sfxSource, clip, muteDuration));
    }

    public void PlayBGM(string key) {
        if (key != CurrentBGMKey && key != NoChangeBGMKey) {
            CurrentBGMKey = key;
            if (key == null || key == NoBGMKey) {
                bgmSource.Stop();
            } else {
                bgmSource.volume = 1.0f;
                AudioClip clip = IndexDatabase.Instance().BGM.GetData(key).track;
                bgmSource.clip = clip;
                bgmSource.Play();
            }
        }
    }

    public AudioClip BGMClip() {
        return bgmSource.clip;
    }

    public IEnumerator FadeOutRoutine(float durationSeconds) {
        CurrentBGMKey = NoBGMKey;
        while (baseVolume > 0.0f) {
            baseVolume -= Time.deltaTime / durationSeconds;
            if (baseVolume < 0.0f) {
                baseVolume = 0.0f;
            }
            yield return null;
        }
        baseVolume = 1.0f;
        PlayBGM(NoBGMKey);
    }

    public IEnumerator CrossfadeRoutine(string tag) {
        if (CurrentBGMKey != null && CurrentBGMKey != NoBGMKey) {
            yield return FadeOutRoutine(FadeSeconds);
        }
        PlayBGM(tag);
    }

    private IEnumerator PlaySFXRoutine(AudioSource source, AudioClip clip, float muteDuration = 0.0f) {
        while (clip.loadState == AudioDataLoadState.Loading) {
            yield return null;
        }
        if (clip.loadState == AudioDataLoadState.Loaded) {
            source.clip = clip;
            if (muteDuration > 0.0f) {
                StartCoroutine(MuteRoutine(muteDuration));
            }
            source.Play();
        }
    }

    private IEnumerator MuteRoutine(float muteDuration) {
        bgmVolumeMult = 0.0f;
        yield return CoUtils.Wait(muteDuration - 0.2f);
        var elapsed = 0.0f;
        while (elapsed < 0.2f) {
            elapsed += Time.deltaTime;
            bgmVolumeMult = elapsed / 0.2f;
            yield return null;
        }
        bgmVolumeMult = 1.0f;

    }
}
