using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ePauseSelect
{
    Continue = 0,
    Restart,
    Setting,
    Title,
    Quit,
}
public class SettingManager : MonoBehaviour
{
    public float timer = 0;
    [SerializeField] BossMovement boss;
    [SerializeField] GameObject settingObj;
    [SerializeField] Transform settingImgRoot;
    List<GameObject> settingImgs = new List<GameObject>();
    bool uiOn = false;
    ePauseSelect curIdx;


    private void Start()
    {
        curIdx = ePauseSelect.Continue;
        settingObj.SetActive(false);
        for (int i = 0; i < settingImgRoot.childCount; i++)
        {
            settingImgs.Add(settingImgRoot.GetChild(i).gameObject);
        }
    }
    void Update()
    {
        if(boss != null)
        {
            timer += Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Escape) && Time.timeScale == 1)
        {
            uiOn = true;
            settingObj.SetActive(true);
            curIdx = ePauseSelect.Continue;
            CurTargetSet(curIdx);
            Time.timeScale = 0;
        }
        else if(Time.timeScale == 0 && Input.GetKeyDown(KeyCode.Escape))
        {
            uiOn = false;
            settingObj.SetActive(false);
            Time.timeScale = 1;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) && uiOn)
        {
            curIdx--;
            if (curIdx < ePauseSelect.Continue) curIdx = ePauseSelect.Quit;
            CurTargetSet(curIdx);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) && uiOn)
        {
            curIdx++;
            if (curIdx > ePauseSelect.Quit) curIdx = ePauseSelect.Continue;
            CurTargetSet(curIdx);
        }
        else if (Input.GetKeyDown(KeyCode.Return) && uiOn)
        {
            uiOn = false;
            settingObj.SetActive(false);
            Time.timeScale = 1;
            switch (curIdx)
            {
                case ePauseSelect.Continue:
                    break;
                case ePauseSelect.Restart:
                    SceneCtrlManager.ins.LoadScene(eScene.Game);
                    break;
                case ePauseSelect.Setting:
                    // ¼¼ÆÃ......
                    break;
                case ePauseSelect.Title:
                    SceneCtrlManager.ins.LoadScene(eScene.Start);
                    break;
                case ePauseSelect.Quit:
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
#else
                    Application.Quit();
#endif
                    break;
            }
        }

    }
    void CurTargetSet(ePauseSelect e)
    {
        int target = (int)e;
        for (int i = 0; i < settingImgs.Count; i++)
        {
            if (i == target) settingImgs[i].SetActive(true);
            else settingImgs[i].SetActive(false);
        }
    }
}
