using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HughGeneric;
public class AudioManager : Singleton<AudioManager>
{
    private AudioSource audioSource;

    //BGM은 InGame한개랑 배열로 안만들고 그냥 사용
    [Header("In Game BGM")]
    [SerializeField] private AudioClip bgmClip;
    private AudioSource bgmSource;

    //EnemySFX는 Enemy가 Pooling중으로, Manager에서 따로 관리한다.
    [Header("Enemy SFX")]
    [SerializeField] private AudioClip hitSFXClip;
    private AudioSource enemySFXSource;

    /// <summary>
    /// 추후 테마별로 따라 Sound 구별 가능하게
    /// </summary>
    public enum BGMType
    {
        InGame = 0,
    }

    /// <summary>
    /// 추후 Enemy 종류에따라 Sound 구별 가능하게
    /// </summary>
    public enum EnemySFXType
    {
        Cat = 0,
    }

    protected override void OnAwake()
    {
        InitAudioManager();
    }

    private void InitAudioManager()
    {
        //BGM 관리할 AudioSource 만들기
        GameObject bgmObj = new GameObject("BGM Player");
        bgmObj.transform.parent = this.transform;
        bgmSource = bgmObj.AddComponent<AudioSource>();
        bgmSource.clip = bgmClip;
        bgmSource.playOnAwake = false;
        bgmSource.bypassListenerEffects = true;
        bgmSource.volume = 0.2f;
        bgmSource.mute = true;
        bgmSource.loop = false;

        //BGM 관리할 AudioSource 만들기
        GameObject enemySFXObj = new GameObject("EnemySFX Player");
        enemySFXObj.transform.parent = this.transform;
        enemySFXSource = enemySFXObj.AddComponent<AudioSource>();
        enemySFXSource.clip = hitSFXClip;
        enemySFXSource.playOnAwake = false;
        enemySFXSource.bypassListenerEffects = true;
        enemySFXSource.volume = 0.3f;
        enemySFXSource.mute = true;
        enemySFXSource.loop = false;
    }

    public void BGMPlay()
    {
        bgmSource.volume = 0.1f;
        bgmSource.mute = false;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    public void BGMStop()
    {
        bgmSource.volume = 0.1f;
        bgmSource.mute = true;
        bgmSource.loop = false;
        bgmSource.Stop();
    }

    public void EnemySFXPlay()
    {
        enemySFXSource.volume = 0.8f;
        enemySFXSource.mute = false;
        enemySFXSource.loop = false;
        enemySFXSource.Play();
    }
}
