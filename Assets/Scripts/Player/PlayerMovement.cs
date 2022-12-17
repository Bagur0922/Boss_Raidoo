using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    Rigidbody2D rb;
    Animator anim;

    public int speed = 2;

    int xmove; //이동 방향, 오른쪽은 1, 왼쪽은 -1. 오른쪽은 0

    
    bool canDash = true; //구를 수 있는가
    bool canMove = true; //움직일 수 있는가
    bool canAttack = true; //공격할 수 있는가
    bool isAttacking = false; //공격하고 있는가

    public bool isDashing = false; //구르고 있는가
    public bool ghosting = false; //잔상 On/Off
    public bool direction = true; //플레이어가 보고 있는  방향

    int alghost = 0; //이미 잔상이 나오고 있는가
    int alback = 0; //이미 돌아오고 있는가

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
        StartCoroutine(comeback(0));
        roll();
        Attack();
    }

    // Update is called once per frame
    void Update()
    {
        move();
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Attack();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            roll();
        }
    }
    void move()
    {
        if (Input.GetKey(KeyCode.LeftArrow) && canMove && !isDashing || isDashing && !direction && canMove || isAttacking && !direction && canMove)
            //각각 움직일 수 있고 대쉬 불가능한 상태에서 왼쪽을 눌렀는가 // 움직일 수 있는 상태에서 왼쪽을 보고 대쉬 중인가 // 왼쪽을 보고 공격중이고 움직일 수 있는가
        {
            direction = false;
            xmove = -1;
            anim.SetBool("isWalking", true);
            gameObject.transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (Input.GetKey(KeyCode.RightArrow) && canMove && !isDashing || isDashing && direction || isAttacking && !direction && canMove)
            //왼쪽이랑 동일 오른쪽을 보는가로만 바뀜
        {
            direction = true;
            xmove = 1;
            anim.SetBool("isWalking", true);
            gameObject.transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            xmove = 0;
            anim.SetBool("isWalking", false);
        }
        Vector2 getvel = new Vector2(xmove, 0) * speed;
        rb.velocity = getvel;
    }
    void roll()
    {
            if (canDash && !isAttacking)
                //스페이스바를 눌렀고 대쉬 가능하며 공격중이지 않은가
            {
                canMove = true;
                speed = 14;
                anim.SetTrigger("Roll");
                isDashing = true;
                canDash = false;
                StartCoroutine(ghost(0.25f));
                StartCoroutine(comeback(0.25f));
            }
    }
    void Attack()
    {
            if (canAttack)
            {
                if(xmove != 0)
                {
                    isAttacking = true;
                    canAttack = false;
                    speed = 10;
                    anim.SetTrigger("Attack");
                    StartCoroutine(ghost(0.5f));
                    isAttacking = true;
                    StartCoroutine(comeback(0.5f));
                }
                else
                {
                    isAttacking = true;
                    canAttack = false;
                    anim.SetTrigger("Attack");
                    //StartCoroutine(ghost(0.5f));
                    canMove = false;
                    StartCoroutine(comeback(0.5f));
                }
            }
    }
    public IEnumerator damaged()
    {
        speed = 7;
        anim.SetTrigger("Damaged");
        canMove = false;
        canDash = false;
        isAttacking = false;
        StartCoroutine(comeback(0.75f));
        yield return null;
    }
    IEnumerator ghost(float time){
        int current = 0;
        alghost++;
        current = alghost;
        ghosting = true;
        yield return new WaitForSeconds(time);
        if(alghost == current){
            ghosting = false;
            alghost = 0;
        }
        yield return null;
    }
    IEnumerator comeback(float time)
    {
        int current = 0;
        alback++;
        current = alback;
        yield return new WaitForSeconds(time);
        if (alback == current)
        {
            canAttack = true;
            canDash = true;
            speed = 7;
            isAttacking = false;
            isDashing = false;
            canMove = true;
            alghost = 0;
        }
        
        yield return null;
    }
}
