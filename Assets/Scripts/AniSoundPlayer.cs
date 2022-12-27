using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AniSoundPlayer : MonoBehaviour
{
    public void playSFX(Object obj){ // object 로 사운드 실행
        var a = obj as AudioClip;        
        SoundPlayer.instance.startSFX(a);
    }

    public void playSFXwithString(string name){ // 이름으로 사운드 실행
        SoundPlayer.instance.startSFX(name);
    }
    public void playBGMwithString(string name)
    { // 이름으로 사운드 실행
        SoundPlayer.instance.startBGM(name);
    }
}
