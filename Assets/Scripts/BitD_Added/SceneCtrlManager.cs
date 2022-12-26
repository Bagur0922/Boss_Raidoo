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

    private void Awake()
    {
        if (uniqueInstance == null) uniqueInstance = this;
        else Destroy(this.gameObject);
        DontDestroyOnLoad(this);
    }

    public void LoadScene(eScene es)
    {
        if (es == eScene.Game) StartCoroutine(LoadSceneWithLoading());
        else SceneManager.LoadScene(es.ToString());
    }
    IEnumerator LoadSceneWithLoading()
    {
        loadingAnim.speed = 1 / loadingTime;
        loadingAnim.SetTrigger("ImageOn");
        yield return new WaitForSeconds(loadingTime);
        loadingAnim.speed = 1 / loadingWaitTime;
        loadingAnim.SetTrigger("ImageOff");
        yield return new WaitForSeconds(loadingWaitTime + 0.2f);
        SceneManager.LoadScene("Game");
    }

    public void ReloadGame()
    {
        SoundPlayer.instance.init();
        LoadScene(eScene.Game);
    }
}
