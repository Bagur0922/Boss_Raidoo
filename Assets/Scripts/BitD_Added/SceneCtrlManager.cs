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

    [SerializeField] float loadingTime = 1f;
    [SerializeField] float loadingWaitTime = 3f;
    [SerializeField] Animator loadingAnim;
    [System.NonSerialized] public int deadCnt = 0;
    [System.NonSerialized] public float savedTimer = 0f;

    private void Awake()
    {
        if (uniqueInstance == null) uniqueInstance = this;
        else Destroy(this.gameObject);
        DontDestroyOnLoad(this);
    }

    public void LoadScene(eScene es, bool needLoading = false)
    {
        if (es == eScene.Game && needLoading) StartCoroutine(LoadSceneWithLoading());
        else SceneManager.LoadScene(es.ToString());
    }
    IEnumerator LoadSceneWithLoading()
    {
        deadCnt = 0;
        savedTimer = 0f;
        loadingAnim.speed = 1 / loadingTime;
        loadingAnim.SetTrigger("ImageOn");
        yield return new WaitForSeconds(loadingTime + loadingWaitTime);
        loadingAnim.speed = 1 / loadingTime;
        loadingAnim.SetTrigger("ImageOff");
        SceneManager.LoadScene("Game");
    }

    public void ReloadGame()
    {
        SoundPlayer.instance.init();
        LoadScene(eScene.Game);
    }
    public void SaveTime(float timer)
    {
        savedTimer += timer;
    }
}
