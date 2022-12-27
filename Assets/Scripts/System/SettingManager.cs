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
    [SerializeField] GameObject settingDuru;
    [SerializeField] GameObject HPCanvas;
    [SerializeField] StartMove duru;
    List<GameObject> settingImgs = new List<GameObject>();
    bool uiOn = false;
    bool settingOn = false;
    ePauseSelect curIdx;


    private void Start()
    {
        curIdx = ePauseSelect.Continue;
        settingObj.SetActive(false);
        settingDuru.SetActive(true);
        HPCanvas.SetActive(true);
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

        if (Input.GetKeyDown(KeyCode.Escape) && !uiOn)
        {
            uiOn = true;
            settingObj.SetActive(true);
            curIdx = ePauseSelect.Continue;
            CurTargetSet(curIdx);
            Time.timeScale = 0;
        }
        else if(uiOn && Input.GetKeyDown(KeyCode.Escape))
        {
            if (settingOn)
            {
                VolumeSetting(false);
            }
            else
            {
                uiOn = false;
                settingObj.SetActive(false);
                Time.timeScale = 1;
            }
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) && uiOn && !settingOn)
        {
            curIdx--;
            if (curIdx < ePauseSelect.Continue) curIdx = ePauseSelect.Quit;
            CurTargetSet(curIdx);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) && uiOn && !settingOn)
        {
            curIdx++;
            if (curIdx > ePauseSelect.Quit) curIdx = ePauseSelect.Continue;
            CurTargetSet(curIdx);
        }
        else if ((Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space)) && uiOn && !settingOn)
        {
            switch (curIdx)
            {
                case ePauseSelect.Continue:
                    uiOn = false;
                    settingObj.SetActive(false);
                    Time.timeScale = 1;
                    break;
                case ePauseSelect.Restart:
                    uiOn = false;
                    settingObj.SetActive(false);
                    Time.timeScale = 1;
                    SceneCtrlManager.ins.ReloadGame();
                    break;
                case ePauseSelect.Setting:
                    VolumeSetting(true);
                    break;
                case ePauseSelect.Title:
                    uiOn = false;
                    settingObj.SetActive(false);
                    Time.timeScale = 1;
                    SoundPlayer.instance.init();
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
    void VolumeSetting(bool setState)
    {
        settingDuru.SetActive(setState);
        HPCanvas.SetActive(!setState);
        settingObj.SetActive(!setState);
        settingOn = setState;
        if (setState) duru.DaggerOn();
        else duru.DaggerOff();
    }
}
