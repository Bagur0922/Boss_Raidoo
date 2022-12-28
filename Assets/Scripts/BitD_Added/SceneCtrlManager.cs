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

    [SerializeField] float loadingTime = 1f;
    [SerializeField] float loadingWaitTime = 3f;
    [SerializeField] Animator loadingAnim;
    [System.NonSerialized] public int deadCnt = 0;

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
}
