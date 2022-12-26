using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
    TextMeshProUGUI tm;
    Animator anim;
    [SerializeField] List<string> tutosaying;
    [SerializeField, Range(0f, 2f)] float fadeDelay = 0.5f;
    public eTutorialState curState = eTutorialState.Move;

    [SerializeField] float needWalkTime = 3f;
    float curWalkTime = 0;
    bool timeCheck = false;
    [SerializeField] int needRollCnt = 3;
    float curRollCnt = 0;
    [SerializeField] int needAtkCnt = 3;
    float curAtkCnt = 0;

    private void Awake()
    {
        tm = GetComponent<TextMeshProUGUI>();
        anim = GetComponent<Animator>();
    }
    void Start()
    {
        tm.text = tutosaying[0];
    }

    void Update()
    {
        if (timeCheck)
        {
            curWalkTime += Time.deltaTime;
            if (curWalkTime > needWalkTime)
            {
                curState = eTutorialState.Roll;
                TextPrintAnimCall();
            }
            timeCheck = false;
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SceneCtrlManager.ins.LoadScene(eScene.Game);
        }
        anim.speed = 1 / fadeDelay;
    }
    public void TextPrintAnimCall()
    {
        anim.SetTrigger("PrintText");
    }

    public void PlayerMove()
    {
        if (curState == eTutorialState.Move) timeCheck = true;
    }
    public void PlayerRoll()
    {
        if (curState == eTutorialState.Roll)
            if (++curRollCnt >= needRollCnt)
            {
                curState = eTutorialState.Attack;
                TextPrintAnimCall();
            }
    }
    public void PlayerAttack()
    {
        if (curState == eTutorialState.Attack)
            if (++curAtkCnt >= needAtkCnt)
            {
                curState = eTutorialState.End;
                TextPrintAnimCall();
            }
    }
    public void ChangeText()
    {
        tm.text = tutosaying[(int)curState];
    }
}
