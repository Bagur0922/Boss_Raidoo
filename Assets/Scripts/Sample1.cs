using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sample1 : MonoBehaviour
{
    public MenuObj obj;
    // Start is called before the first frame update
    void Start()
    {
        obj.startMenu(); // 첫 화면 메뉴 실행
        SoundPlayer.instance.startBGM("SeguFight"); // BGM 실행
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
