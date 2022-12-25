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
    [SerializeField] List<string> tutosaying;
    public eTutorialState curState = eTutorialState.Move;

    void Start()
    {
        tm = GetComponent<TextMeshProUGUI>();
        tm.text = tutosaying[0];
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SceneCtrlManager.ins.LoadScene(eScene.Game);
        }
    }
    public void TextPrintAt(int idx)
    {
        Debug.Log("Call");
        tm.text = tutosaying[idx];
    }
}
