using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

/**************************
    BGM용으로 만든 Class
    버거인사이드에서 BGM마다 Intro, Loop 로 나누어져 있어서 만듦
    SFX에는 쓸모 없지만 따로 만들기 귀찮아서 냅뒀습니다.
**************************/
[System.Serializable]
public class Bgm{
    public string name;
    public List<AudioClip> audios;
}

/**************************
    사운드 관련 볼륨 조절
    Audios/SFX, Audios/BGM 에 있는 파일들을 자동으로 리스트에 등록
    BGM 같이 Start 와 Loop 가 따로 있는 것은 따로 등록해야 합니다.
    
    자주 사용하는 함수
    startBGM : 이전 BGM이 꺼지고 BGM 출력, Loop 반복
    playbgmOneShot : 이전 BGM이 꺼지고 BGM 출력, Bgm.audios의 가장 앞 BGM만 실행
    startSFX : 아무 사운드와 관련 없이 Bgm.audios의 가장 앞 BGM만 실행
    startSFX override : 이름으로 접근하는 것이 아닌 clip으로 접근
**************************/
public class SoundPlayer : MonoBehaviour
{
    public AudioMixer masterMixer;
    public List<Bgm> bgms;
    public List<Bgm> sfxs;
    public AudioSource bgmSource;
    public AudioSource sfxSource;
    [Range(-40f, 0f)]
    public float BGMvolume = 0f;
    public int bgmVInt = 8;
    [Range(-40f, 0f)]
    public float SFXvolume = 0f;
    public int sfxVInt = 8;
    [Range(-40f, 0f)]
    public float MasterVolume = 0f;
    public int masterVInt = 8;
    private Bgm nowPlaying;
    private int trackNum;
    private bool isPlayBgmOneShot = false;
    private static SoundPlayer _instance;

    public static SoundPlayer instance
    {
        get
        {
           return _instance;
        }
    }
    // Start is called before the first frame update
    void Awake()
    {
        if(_instance == null){
            _instance = this;

            foreach (var item in bgms)
            {
                item.name = item.name.ToLower();   
            }

            foreach (var item in sfxs)
            {
                item.name = item.name.ToLower();
            }

            var bgmAudios = new List<AudioClip>(Resources.LoadAll<AudioClip>("Audios/BGM"));        
            foreach (var item in bgmAudios)
            {
                var bgm = new Bgm();
                bgm.name = item.name.ToLower();
                bgm.audios = new List<AudioClip>();
                bgm.audios.Add(item);
                bgms.Add(bgm);
            }

            var sfxAudios = new List<AudioClip>(Resources.LoadAll<AudioClip>("Audios/SFX"));        
            foreach (var item in sfxAudios)
            {
                var sfx = new Bgm();
                sfx.name = item.name.ToLower();
                sfx.audios = new List<AudioClip>();
                sfx.audios.Add(item);
                sfxs.Add(sfx);
            }

            print(PlayerPrefs.GetFloat("MasterVolume", 0f));

            DontDestroyOnLoad(gameObject);
            init();
        }
        else{
            Destroy(gameObject);
            return;
        }
    }
    void Start(){
        setMasterVolume(PlayerPrefs.GetFloat("MasterVolume", 0f));
        setBGMVolume(PlayerPrefs.GetFloat("BGMVolume", 0f));
        setSFXVolume(PlayerPrefs.GetFloat("SFXVolume", 0f));
    }

    public void setVolume(eOptionType e, int volume)
    {
        float value = (volume * 5f) - 40f;
        switch (e)
        {
            case eOptionType.Master:
                setMasterVolume(value);
                break;
            case eOptionType.BGM:
                setBGMVolume(value);
                break;
            case eOptionType.SFX:
                setSFXVolume(value);
                break;
        }
    }
    public void setMasterVolume(float volume){
        if(volume == -40f) masterMixer.SetFloat("Master", -80f);
        else masterMixer.SetFloat("Master", volume);

        MasterVolume = volume;
        masterVInt = Mathf.RoundToInt((volume + 40f) / 5f);
        PlayerPrefs.SetFloat("MasterVolume", volume);
    }
    public void setBGMVolume(float volume){
        if(volume == -40f) masterMixer.SetFloat("BGM", -80f);
        else masterMixer.SetFloat("BGM", volume);
        
        BGMvolume = volume;
        bgmVInt = Mathf.RoundToInt((volume + 40f) / 5f);
        PlayerPrefs.SetFloat("BGMVolume", volume);
    }
    public void setSFXVolume(float volume){
        if(volume == -40f) masterMixer.SetFloat("SFX", -80f);
        else masterMixer.SetFloat("SFX", volume);
        
        SFXvolume = volume;
        sfxVInt = Mathf.RoundToInt((volume + 40f) / 5f);
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }

    void init(){
        bgmSource.Stop();
        nowPlaying = null;
        trackNum = 0;
    }

    void Update(){
        if(nowPlaying != null && isPlayBgmOneShot == false){
            if(!bgmSource.isPlaying){
                bgmSource.PlayOneShot(nowPlaying.audios[trackNum]);
                if(trackNum < nowPlaying.audios.Count - 1){
                    trackNum++;
                }
            }
        }
    }

    public void startBGM(string name){
        name = name.ToLower();
        if(nowPlaying != null && nowPlaying.name.Equals(name)){
            return;
        }
        
        isPlayBgmOneShot = false;

        foreach (var bgm in bgms)
        {
            if(bgm.name.Equals(name)){
                init();
                nowPlaying = bgm;
                break;
            }
        }
    }

    public void playBgmOneShot(string name){
        name = name.ToLower();
        if(nowPlaying != null && nowPlaying.name.Equals(name)){
            return;
        }

        isPlayBgmOneShot = true;

        foreach (var bgm in bgms)
        {
            if(bgm.name.Equals(name)){
                init();
                nowPlaying = bgm;
                bgmSource.PlayOneShot(nowPlaying.audios[trackNum]);
                break;
            }
        }
    }



    public void startSFX(string name){
        foreach (var sfx in sfxs)
        {
            if(sfx.name.Equals(name)){
                sfxSource.PlayOneShot(sfx.audios[0]);
                break;
            }
        }
    }

    public void startSFX(AudioClip ac){
        sfxSource.PlayOneShot(ac);
    }
}
