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
        uniqueInstance = this;
        DontDestroyOnLoad(this);
    }

    public void LoadScene(eScene es)
    {
        SceneManager.LoadScene(es.ToString());
    }
}
