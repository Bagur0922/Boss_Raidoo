using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum eScene
{
    Start, // ����ȭ��(Ÿ��Ʋ)
    Tutorial, // Ʃ�丮��
    Game, // ������
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

    public void ReloadGame()
    {
        SoundPlayer.instance.init();
        SceneManager.LoadScene("Game");
    }
}
