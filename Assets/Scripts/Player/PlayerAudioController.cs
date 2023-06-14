using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioController : MonoBehaviour
{
    [SerializeField] private AudioSource[] audioSources;

    private void Start()
    {
        InitAudioSetting();
    }

    private void InitAudioSetting()
    {
        for (int i = 0; i < audioSources.Length; i++)
        {
            audioSources[i].playOnAwake = false;
            audioSources[i].loop = false;
            audioSources[i].volume = 1.0f;
            audioSources[i].bypassListenerEffects = true;
            audioSources[i].mute = true;
        }
    }

    /// <summary>
    /// Attack시 사운드는, Animation의 Event 함수로 등록되어, 동작에 맞춰 소리가 난다.
    /// </summary>
    public void PlayAttackSFX()
    {
        audioSources[0].volume = 0.5f;
        audioSources[0].mute = false;
        audioSources[0].Play();
    }

    public void PlayDefenseSFX()
    {
        audioSources[1].volume = 0.8f;
        audioSources[1].mute = false;
        audioSources[1].Play();
    }
}
