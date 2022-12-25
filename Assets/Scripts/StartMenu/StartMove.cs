using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMove : MonoBehaviour
{
    Animator anim;
    SpriteRenderer sr;

    [SerializeField] List<Sprite> now;

    [SerializeField]int selecting = 1; //1은 시작, 2는 설정 3은 크레딧
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && selecting == 0)
        {

        }
        if (Input.GetKeyDown(KeyCode.DownArrow) && selecting != 3)
        {
            selecting++;
        }
        else if(Input.GetKeyDown(KeyCode.UpArrow) && selecting != 1)
        {
            selecting--;
        }
        sr.sprite = now[selecting - 1];
    }
    public void startend()
    {
        anim.enabled = false;
    }
}
