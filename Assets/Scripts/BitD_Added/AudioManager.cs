using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("기본 할당")]
    [SerializeField] AudioSource bgmPlayer;
    [SerializeField] AudioSource sePlayer;

    public enum eBGM
    {
        tmp,
    }
    [SerializeField] AudioClip[] BGM;
    public enum eSE
    {
        tmp,
    }
    [SerializeField] AudioClip[] SE;

    static AudioManager uniqueInstance;
    public static AudioManager ins { get { return uniqueInstance; } }
    public const float interval = 0.125f;

    [Header("기본 볼륨 설정"), Space(10f)]
    [SerializeField] public float masterVolume = 1;
    [SerializeField] public float bgmVolume = 1;
    [SerializeField] public float seVolume = 1;

    private void Awake()
    {
        uniqueInstance = this;
        DontDestroyOnLoad(this);
    }
    private void Start()
    {
        bgmPlayer.volume = bgmVolume * masterVolume;
        sePlayer.volume = seVolume * masterVolume;
    }
    public void PlayBGM(eBGM e)
    {
        bgmPlayer.clip = BGM[(int)e];
        bgmPlayer.Play();
    }
    public void PlaySE(eSE e)
    {
        sePlayer.PlayOneShot(SE[(int)e]);
    }

    public void VolumeSet(eOptionType e, int value)
    {
        switch (e)
        {
            case eOptionType.Master:
                MasterVolumeSet(value);
                break;
            case eOptionType.BGM:
                BGMVolumeSet(value);
                break;
            case eOptionType.SE:
                SEVolumeSet(value);
                break;
        }
    }
    public void MasterVolumeSet(int value)
    {
        masterVolume = value * interval;
        bgmPlayer.volume = bgmVolume * masterVolume;
        sePlayer.volume = seVolume * masterVolume;
    }
    public void BGMVolumeSet(int value)
    {
        bgmVolume = value * interval;
        bgmPlayer.volume = bgmVolume * masterVolume;
    }
    public void SEVolumeSet(int value)
    {
        seVolume = value * interval;
        sePlayer.volume = seVolume * masterVolume;
    }
}
