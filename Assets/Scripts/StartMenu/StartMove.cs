using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum eTitleType
{
    Main = 0,
    Option,
    Credit,
}
public enum eOptionType
{
    Master = 0,
    BGM,
    SE,
}
public class StartMove : MonoBehaviour
{
    Animator anim;
    SpriteRenderer sr;

    [SerializeField] List<Sprite> now;
    [SerializeField] int selecting = 1; //1�� ����, 2�� ���� 3�� ũ����
    [SerializeField] Transform volPosRoot;
    [SerializeField] GameObject optionDagger;
    [SerializeField] GameObject[] optionTarget = new GameObject[3];
    Transform[,] volumePos = new Transform[3, 9];
    int[] curVolTarget = new int[3];

    bool canStart;
    eTitleType curTitleType;
    eOptionType curOptionType;

    void Start()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        canStart = false;
        curTitleType = eTitleType.Main;
        curOptionType = eOptionType.Master;
        for (int i = 0; i < 3; i++)
        {
            Transform tmp = volPosRoot.GetChild(i);
            for (int j = 0; j < 9; j++)
            {
                
                volumePos[i, j] = tmp.GetChild(j);
            }
        }
        OptionInitSet();
    }

    void Update()
    {
        if (!canStart) return;

        switch (curTitleType)
        {
            case eTitleType.Main:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    // ������ Ȯ��Ű��� �����ϰ� ������
                    switch (selecting)
                    {
                        case 1:
                            SceneCtrlManager.ins.LoadScene(eScene.Tutorial);
                            break;
                        case 2:
                            canStart = false;
                            anim.SetBool("OptionWindow", true);
                            curTitleType = eTitleType.Option;
                            break;
                        case 3:
                            anim.SetBool("CreditWindow", true);
                            curTitleType = eTitleType.Credit;
                            break;
                    }
                }
                if (Input.GetKeyDown(KeyCode.DownArrow) && selecting != 3)
                {
                    selecting++;
                    anim.SetInteger("SelectMode", selecting);
                }
                else if (Input.GetKeyDown(KeyCode.UpArrow) && selecting != 1)
                {
                    selecting--;
                    anim.SetInteger("SelectMode", selecting);
                }
                break;
            case eTitleType.Option:
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    canStart = false;
                    anim.SetBool("OptionWindow", false);
                    curTitleType = eTitleType.Main;
                }
                if (Input.GetKeyDown(KeyCode.DownArrow) && curOptionType != eOptionType.SE)
                {
                    curOptionType++;
                    optionDagger.transform.position = volumePos[(int)curOptionType, curVolTarget[(int)curOptionType]].position;
                }
                else if (Input.GetKeyDown(KeyCode.UpArrow) && curOptionType != eOptionType.Master)
                {
                    curOptionType--;
                    optionDagger.transform.position = volumePos[(int)curOptionType, curVolTarget[(int)curOptionType]].position;
                }
                else if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    int tmp = curVolTarget[(int)curOptionType];
                    curVolTarget[(int)curOptionType] = tmp - 1 >= 0 ? --tmp : 0;
                    optionDagger.transform.position = volumePos[(int)curOptionType, curVolTarget[(int)curOptionType]].position;
                    optionTarget[(int)curOptionType].transform.position = volumePos[(int)curOptionType, curVolTarget[(int)curOptionType]].position;
                    AudioManager.ins.VolumeSet(curOptionType, curVolTarget[(int)curOptionType]);
                }
                else if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    int tmp = curVolTarget[(int)curOptionType];
                    curVolTarget[(int)curOptionType] = tmp + 1 <= 8 ? ++tmp : 8;
                    optionDagger.transform.position = volumePos[(int)curOptionType, curVolTarget[(int)curOptionType]].position;
                    optionTarget[(int)curOptionType].transform.position = volumePos[(int)curOptionType, curVolTarget[(int)curOptionType]].position;
                    AudioManager.ins.VolumeSet(curOptionType, curVolTarget[(int)curOptionType]);
                }
                break;
            case eTitleType.Credit:
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Escape))
                {
                    canStart = false;
                    anim.SetBool("CreditWindow", false);
                    curTitleType = eTitleType.Main;
                }
                break;
        }
        
    }
    public void CanStart()
    {
        canStart = true;
    }

    public void DaggerOn()
    {
        curVolTarget[(int)eOptionType.Master] = (int)Mathf.Round(AudioManager.ins.masterVolume * 8);
        curVolTarget[(int)eOptionType.BGM] = (int)Mathf.Round(AudioManager.ins.bgmVolume * 8);
        curVolTarget[(int)eOptionType.SE] = (int)Mathf.Round(AudioManager.ins.seVolume * 8);
        optionDagger.SetActive(true);
        optionDagger.transform.position = volumePos[(int)curOptionType, curVolTarget[(int)curOptionType]].position;
        for (int i = 0; i < optionTarget.Length; i++)
        {
            optionTarget[i].transform.position = volumePos[i, curVolTarget[i]].position;
            optionTarget[i].SetActive(true);
        }
    }
    public void DaggerOff()
    {
        canStart = true;
        optionDagger.SetActive(false);
        for (int i = 0; i < optionTarget.Length; i++)
        {
            optionTarget[i].SetActive(false);
        }
    }
    public void CreditOn()
    {

    }
    public void CreditOff()
    {
        canStart = true;
    }
    public void OptionInitSet()
    {
        curVolTarget[(int)eOptionType.Master] = (int)Mathf.Round(AudioManager.ins.masterVolume * 8);
        curVolTarget[(int)eOptionType.BGM] = (int)Mathf.Round(AudioManager.ins.bgmVolume * 8);
        curVolTarget[(int)eOptionType.SE] = (int)Mathf.Round(AudioManager.ins.seVolume * 8);
        optionDagger.SetActive(false);
        optionDagger.transform.position = volumePos[0, curVolTarget[0]].position;
        curOptionType = eOptionType.Master;
        for (int i = 0; i < optionTarget.Length; i++)
        {
            optionTarget[i].transform.position = volumePos[i, curVolTarget[i]].position;
            optionTarget[i].SetActive(false);
        }
    }
}
