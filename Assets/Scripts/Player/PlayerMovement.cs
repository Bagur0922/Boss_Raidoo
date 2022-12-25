using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;
    Animator anim;
    SpriteRenderer sr;

    [SerializeField] Sprite fly;
    [SerializeField] bool shake;
    [SerializeField] bool tuto = false;

    public GameObject boss;
    public GameObject Camera;

    public int speed = 2;

    int xmove; //�̵� ����, �������� 1, ������ -1. �������� 0
    int AttackXmove; //���ݿ� �̵�����


    bool canDash = true; //���� �� �ִ°�
    bool canMove = true; //������ �� �ִ°�
    bool canAttack = true; //������ �� �ִ°�
    bool attackmove = false; //�����ϰ� �ִ°�

    public bool damge = true; //�������� �Դ°�
    public bool isAttacking = false; //�����ϰ� �ִ°�
    public bool isDashing = false; //������ �ִ°�
    public bool ghosting = false; //�ܻ� On/Off
    public bool direction = true; //�÷��̾ ���� �ִ�  ����
    public bool anyaction = false; //���� ��������
    public bool Trigger = false;
    public bool counteranyaction = true;

    int alghost = 0; //�̹� �ܻ��� ������ �ִ°�
    int alback = 0; //�̹� ���ƿ��� �ִ°�

    
    
    // Start is called before the first frame update
    void Start()
    {
        if (tuto)
        {
            counteranyaction = true;
            anyaction = true;
        }
        rb = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
        sr = gameObject.GetComponent<SpriteRenderer>();
        StartCoroutine(comeback(0));
    }

    // Update is called once per frame
    void Update()
    {
        value();
        if (!counteranyaction || !anyaction || Time.timeScale == 0)
        {
            return;
        }
        //touchingwall = false;
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
    void value()
    {
        if (shake)
        {
            Camera.GetComponent<CameraShake>().shake = true;
        }
        else
        {
            Camera.GetComponent<CameraShake>().shake = false;
        }
    }
    void move()
    {
        if (Input.GetKey(KeyCode.LeftArrow) && canMove && !isDashing && !attackmove || isDashing && !direction && canMove && !attackmove || AttackXmove == -1 && attackmove)
        //���� ������ �� �ְ� �뽬 �Ұ����� ���¿��� ������ �����°� // ������ �� �ִ� ���¿��� ������ ���� �뽬 ���ΰ� // ������ ���� �������̰� ������ �� �ִ°�
        {
            direction = false;
            xmove = -1;
            anim.SetBool("isWalking", true);
            gameObject.transform.localScale = new Vector3(-1, 1, 1);
        }
        if (Input.GetKey(KeyCode.RightArrow) && canMove && !isDashing && !attackmove || isDashing && direction && canMove && !attackmove || AttackXmove == 1 && attackmove)
        //�����̶� ���� �������� ���°��θ� �ٲ�
        {
            direction = true;
            xmove = 1;
            anim.SetBool("isWalking", true);
            gameObject.transform.localScale = new Vector3(1, 1, 1);
        }
        if (!canMove || !Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow) && !isDashing)
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
        //�����̽��ٸ� ������ �뽬 �����ϸ� ���������� ������
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
            if (xmove != 0)
            {
                attackmove = true;
                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    AttackXmove = -1;
                }
                else if (Input.GetKey(KeyCode.RightArrow))
                {
                    AttackXmove = 1;
                }
                canAttack = false;
                speed = 10;
                anim.SetTrigger("Attack");
                //StartCoroutine(ghost(0.5f));
                StartCoroutine(comeback(0.5f));
            }
            else
            {
                canAttack = false;
                anim.SetTrigger("Attack");
                canMove = false;
                StartCoroutine(comeback(0.5f));
            }
        }
    }
    public IEnumerator damaged()
    {
        if (!counteranyaction)
            yield break;
        
        speed = 7;
        anim.SetTrigger("Damaged");
        canMove = false;
        canDash = false;
        canAttack = false;
        anyaction = false;
        if (!direction)
        {
            rb.velocity = new Vector2(10, 0);
        }
        else
        {
            rb.velocity = new Vector2(-10, 0);
        }
        yield return new WaitForSeconds(1/3);
        rb.velocity = new Vector2(0, 0);
        canMove = true;
        canDash = true;
        canAttack = true;
        anyaction = true;
        yield return null;
    }
    IEnumerator ghost(float time)
    {
        int current = 0;
        alghost++;
        current = alghost;
        ghosting = true;
        yield return new WaitForSeconds(time);
        if (alghost == current)
        {
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
            attackmove = false;
            canAttack = true;
            canDash = true;
            speed = 7;
            isDashing = false;
            canMove = true;
            alghost = 0;
        }

        yield return null;
    }
    public IEnumerator counter()
    {
        damge = false;
        counteranyaction = false;
        rb.velocity = new Vector2(0, 0);

        yield return new WaitWhile(() => !Trigger);

        anim.SetTrigger("counter");

        if (direction)
        {
            rb.velocity = new Vector2(-40, 0);
        }
        else
        {
            rb.velocity = new Vector2(40, 0);
        }

        yield return new WaitWhile(() => transform.position.x > -8  && transform.position.x < 8);

        rb.velocity = new Vector2(0, 0);
        
        anim.SetTrigger("fly_down");
    }

    public void OnFlyDownEnd()
    {
        damge = true;
        counteranyaction = true;
    }
}
