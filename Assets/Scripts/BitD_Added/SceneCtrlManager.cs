using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum eScene
{
    Start, // 메인화면(타이틀)
    Tutorial, // 튜토리얼
    Game, // 본게임
}

public class SceneCtrlManager : MonoBehaviour
{
    static SceneCtrlManager uniqueInstance;
    public static SceneCtrlManager ins { get { return uniqueInstance; } }



    private void Awake()
    {
        if (uniqueInstance == null) uniqueInstance = this;
        else Destroy(this.gameObject);
        DontDestroyOnLoad(this);
    }

    public void LoadScene(eScene es)
    {
        SceneManager.LoadScene(es.ToString());
    }

    public void ReloadGame()
    {
        SoundPlayer.instance.init();
        SceneManager.LoadScene("Game");
    }
}
