using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public enum eTutorialState
{
    // 이걸 해야하는 상태
    Move = 0,
    Roll,
    Attack,
    End
}

public class Tuto_text : MonoBehaviour
{
    [SerializeField] Animator cvgAnim;
    [SerializeField] TextMeshProUGUI tm;
    [SerializeField] List<string> tutosaying;
    [SerializeField] List<GameObject> extras;
    CheckBox[] layout1 = new CheckBox[3];
    CheckBox[] layout2 = new CheckBox[3];

    [SerializeField, Range(0f, 2f)] float fadeDelay = 0.5f;
    [SerializeField] Image timerBar;
    public eTutorialState curState = eTutorialState.Move;

    [SerializeField] float needWalkTime = 3f;
    float curWalkTime = 0;
    bool timeCheck = false;
    [SerializeField] int needRollCnt = 3;
    int curRollCnt = 0;
    [SerializeField] int needAtkCnt = 3;
    int curAtkCnt = 0;

    void Start()
    {
        tm.text = tutosaying[0]; 
        for (int i = 0; i < layout1.Length; i++)
        {
            layout1[i] = extras[1].transform.GetChild(0).GetChild(i).GetComponent<CheckBox>();
            layout2[i] = extras[2].transform.GetChild(0).GetChild(i).GetComponent<CheckBox>();
        }
        extras[0].SetActive(true);
        extras[1].SetActive(false);
        extras[2].SetActive(false);
        timerBar.fillAmount = 0;
    }

    void Update()
    {
        if (timeCheck)
        {
            curWalkTime += Time.deltaTime;
            timerBar.fillAmount = curWalkTime / needWalkTime;
            if (curWalkTime > needWalkTime)
            {
                curState = eTutorialState.Roll;
                timerBar.fillAmount = 1;
                TextPrintAnimCall();
            }
            timeCheck = false;
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SceneCtrlManager.ins.LoadScene(eScene.Game);
        }
        cvgAnim.speed = 1 / fadeDelay;
    }
    public void TextPrintAnimCall()
    {
        cvgAnim.SetTrigger("FadeStart");
    }

    public void PlayerMove()
    {
        if (curState == eTutorialState.Move) timeCheck = true;
    }
    public void PlayerRoll()
    {
        if (curState == eTutorialState.Roll)
        {
            layout1[curRollCnt].CheckSet(true);
            if (++curRollCnt >= needRollCnt)
            {
                curState = eTutorialState.Attack;
                TextPrintAnimCall();
            }
        }  
    }
    public void PlayerAttack()
    {
        if (curState == eTutorialState.Attack)
        {
            layout2[curAtkCnt].CheckSet(true);
            if (++curAtkCnt >= needAtkCnt)
            {
                curState = eTutorialState.End;
                TextPrintAnimCall();
            }
        }
    }
    public void ChangeText()
    {
        tm.text = tutosaying[(int)curState];
        for (int i = 0; i < extras.Count; i++)
        {
            if ((int)curState == i) extras[i].SetActive(true);
            else extras[i].SetActive(false);
        }
    }
}
