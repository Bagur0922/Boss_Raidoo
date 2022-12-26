using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;



public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;
    Animator anim;
    SpriteRenderer sr;

    [SerializeField] Tuto_text tutoText;
    [SerializeField] Sprite fly;
    [SerializeField] bool shake;
    [SerializeField] bool tuto = false;
    [SerializeField] GameObject restartMessage;
    [SerializeField] GameObject clearImage;
    [SerializeField] TextMeshProUGUI clearTimeText;

    public GameObject boss;
    BossMovement bossM;
    public GameObject Camera;
    CameraShake camShake;

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

    bool isDead = false;
    bool isClear = false;

    float timer;
    
    void Start()
    {
        if (tuto)
        {
            counteranyaction = true;
            anyaction = true;
        }
        else
        {
            restartMessage.SetActive(false);
            clearImage.SetActive(false);
        }
        rb = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
        sr = gameObject.GetComponent<SpriteRenderer>();
        bossM = boss.GetComponent<BossMovement>();
        camShake = Camera.GetComponent<CameraShake>();
        StartCoroutine(comeback(0));
    }

    void Update()
    {
        if (isClear)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneCtrlManager.ins.LoadScene(eScene.Start);
            }
        }
        if (isDead)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneCtrlManager.ins.ReloadGame();
            }
            return;
        }
        
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

        timer += Time.deltaTime;
    }
    void value()
    {
        if (camShake == null) return;
        if (shake)
        {
            camShake.shake = true;
        }
        else
        {
            camShake.shake = false;
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
            TutorialCheck(eTutorialState.Move);
        }
        if (Input.GetKey(KeyCode.RightArrow) && canMove && !isDashing && !attackmove || isDashing && direction && canMove && !attackmove || AttackXmove == 1 && attackmove)
        //�����̶� ���� �������� ���°��θ� �ٲ�
        {
            direction = true;
            xmove = 1;
            anim.SetBool("isWalking", true);
            gameObject.transform.localScale = new Vector3(1, 1, 1);
            TutorialCheck(eTutorialState.Move);
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
            TutorialCheck(eTutorialState.Roll);
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
            TutorialCheck(eTutorialState.Attack);
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
    public void Dead()
    {
        isDead = true;
        anim.SetTrigger("Dead");
        StartCoroutine(Restart());
        bossM.PlayerDead();
    }
    IEnumerator Restart()
    {
        // ������Ϸ��� ������ �Է����ּ���
        
        yield return new WaitForSeconds(1f);
        restartMessage.SetActive(true);
    }
    public void TutorialCheck(eTutorialState action)
    {
        if (!tuto || tutoText == null) return;

        switch (action)
        {
            case eTutorialState.Move:
                tutoText.PlayerMove();
                break;
            case eTutorialState.Roll:
                tutoText.PlayerRoll();
                break;
            case eTutorialState.Attack:
                tutoText.PlayerAttack();
                break;
                    
        }
    }
    public void ClearGame()
    {
        isClear = true;
        clearImage.SetActive(true);
        int tmpTime = Mathf.RoundToInt(timer);
        clearTimeText.text = string.Format("Ŭ����\n\n�ɸ� �ð�\n{0}�� {1}��", tmpTime / 60, tmpTime % 60);
        SoundPlayer.instance.init();
    }
}
