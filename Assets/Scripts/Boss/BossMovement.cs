using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class BossMovement : MonoBehaviour
{
    Animator anim;
    SpriteRenderer sr;
    Rigidbody2D rb;

    public GameObject player;
    PlayerMovement playerMovement;
    public GameObject Camera;
    CameraShake camShake;

    [SerializeField] GameObject bar;
    [SerializeField] GameObject Dagger;
    [SerializeField] Transform daggerPoint;
    [SerializeField] BoxCollider2D hitBoxCol;


    float xmove;
    float thunderpos;
    float distance;
    float speed = 7;

    [SerializeField] float maxHp = 100;
    [SerializeField] float hp;

    bool startg = false;
    bool stop = false;
    bool downback = true;
    bool okd = true;
    bool stopUpdate = false;
    bool isDead = false;

    [SerializeField]bool changedir = true;
    [SerializeField]bool canWalk = false;
    [SerializeField] bool shake = false;
    [SerializeField] bool pron = true;
    [SerializeField] bool Trigger = false;

    public bool ready = true;
    public bool attacking = true;
    public bool damage = true;
    public bool ghosting = false;
    public bool direction = false;
    public bool counteren = true;
    public bool specialFirstSkillUsed = false;
    public bool specialSecondSkillUsed = false;
    
    [SerializeField] public int[] cool; //1�� ��� ����, 0�� ��Ÿ����. 0�� �����ϼ�, 1�� ������

    private void Awake()
    {
        //hp = maxHp;
    }
    void Start()
    {
        //SoundPlayer.instance.startBGM("Start");
        
        playerMovement = player.GetComponent<PlayerMovement>();
        camShake = Camera.GetComponent<CameraShake>();


        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        StartCoroutine(start());
    }
    IEnumerator start()
    {
        rb.velocity = new Vector2(-1, 0);
        yield return new WaitForSeconds(1.3f);
        rb.velocity = new Vector2(0, 0);
        yield return new WaitForSeconds(2.8f);
        playerMovement.anyaction = true;
        playerMovement.counteranyaction = true;
        startg = true;
    }

    void Update()
    {

        if (isDead)
            return;

        if (isForceMoveStart == true)
        {
            ForceMove();
        }

        if (stopUpdate)
            return;
        if (changedir)
        {
            look();
        }
        

        if (hp <= 0)
        {
            anim.SetTrigger("die");
            Destroy(hitBoxCol.gameObject);
            isDead = true;
            return;
        }
        if (!specialFirstSkillUsed && hp / maxHp <= 0.6f)
        {
            specialFirstSkillUsed = true;
            SpecialSkillStart();
            return;
        }
        else if(!specialSecondSkillUsed && hp / maxHp <= 0.3f)
        {
            specialFirstSkillUsed = false;
            SpecialSkillStart();
            return;
        }
        distance = player.transform.position.x - gameObject.transform.position.x;
        walk();
        value();
        skill();
    }
    void value()
    {
        if (camShake != null)
        {
            if (shake) camShake.shake = true;
            else camShake.shake = false;
        }
        
        if (pron)
        {
            player.GetComponent<SpriteRenderer>().enabled = true;
        }
        else
        {
            player.GetComponent<SpriteRenderer>().enabled = false;
        }

        if (Trigger)
        {
            player.GetComponent<PlayerMovement>().Trigger = true;
        }
        else
        {
            player.GetComponent<PlayerMovement>().Trigger = false;
        }
    }
    void look()
    {
        if (distance > 0 && changedir)
        {
            Debug.Log("look ������");
            gameObject.transform.localScale = new Vector3(1, 1, 1);
            direction = true;
        }
        else if (changedir)
        {
            Debug.Log("look ����");
            gameObject.transform.localScale = new Vector3(-1, 1, 1);
            direction = false;
        }
    }
    void skill()
    {
        if (ready)
        {
            if (transform.position.x < 0 && transform.position.x > -2 && cool[0] == 1
                || transform.position.x > 0 && transform.position.x < 2 && cool[0] == 1)
            {
                stop = true;
                canWalk = false;
                ready = false;
                StartCoroutine(thunder());
            }
            else if (Mathf.Abs(distance) < 2 && cool[1] == 1)
            {
                canWalk = false;
                ready = false;
                changedir = false;
                StartCoroutine(roll());
            }
            else if(Mathf.Abs(distance) < 1.5f && cool[2] == 1)
            {
                stop = true;
                canWalk = false;
                ready = false;
                changedir = false;
                StartCoroutine(nattack());
            }
        }
    }
    void walk()
    {
        if (canWalk)
        {
            if (Mathf.Abs(distance) > 2.5f)
            {
                anim.SetBool("isWalking", true);
                if (!direction)
                {
                    xmove = -0.5f;
                }
                if (direction)
                {
                    xmove = 0.5f;
                }
                Vector2 getvel = new Vector2(xmove * speed, 0);
                rb.velocity = getvel;
            }
            else
            {
                rb.velocity = new Vector2(0, 0);
                anim.SetBool("isWalking", false);
            }
        }
        else if(startg && stop)
        {
            anim.SetBool("isWalking", false);
            rb.velocity = new Vector2(0, 0);
        }
    }
    IEnumerator nattack()
    {
        cool[2] = 0;
        anim.SetTrigger("attack");
        yield return new WaitForSeconds(2/3);
        StartCoroutine(cooldown(2, 2.5f));
        stop = false;
        if (downback)
        {
            canWalk = true;
            ready = true;
            changedir = true;
        }
        yield return null;
    }
    IEnumerator roll()
    {
        ghosting = true;
        changedir = false;
        cool[1] = 0;
        damage = false;
        anim.SetTrigger("roll");
        speed = 14;
        if (!direction)
        {
            gameObject.transform.localScale = new Vector3(-1, 1, 1);
            xmove = -1;
        }
        if (direction)
        {
            gameObject.transform.localScale = new Vector3(1, 1, 1);
            xmove = 1;
        }
        Vector2 getvel = new Vector2(xmove * speed, 0);
        rb.velocity = getvel;
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(cooldown(1, 1.25f));
        rb.velocity = new Vector2(0, 0);
        speed = 7;
        ghosting = false;
        if (downback)
        {
            ready = true;
            changedir = true;
            canWalk = true;
        }
        yield return null;
    }
    IEnumerator thunder()
    {
        thunderpos = transform.position.x;
        anim.SetTrigger("thunder_start");
        changedir = false;
        yield return new WaitForSeconds(0.375f);
        stop = false;
        if (direction)
        {
            rb.velocity = new Vector2(140, 0);
            /*transform.position = new Vector2(transform.position.x + 7, transform.position.y);
            thunderpos = thunderpos + 7;*/
        }
        else
        {
            rb.velocity = new Vector2(-140, 0);
            /*transform.position = new Vector2(transform.position.x - 7, transform.position.y);
            thunderpos = thunderpos - 7;*/
        }
        yield return new WaitUntil(() => transform.position.x > thunderpos + 6 || transform.position.x < thunderpos - 6);
        rb.velocity = new Vector2(0, 0);
        anim.SetTrigger("thunder_end");
        yield return new WaitForSeconds(0.5f);
        anim.SetTrigger("backtostand");
        cool[0] = 0;
        StartCoroutine(cooldown(0, 7.5f));
        if (downback)
        {
            changedir = true;
            canWalk = true;
            ready = true;
        }
        /*if(transform.position.x < -9 || transform.position.x > 9)
        {
            transform.position = new Vector2(thunderpos, transform.position.y);
        }*/
        yield return null;
    }
    public IEnumerator down()
    {
        StartCoroutine(damaged());
        StartCoroutine(damaged());
        StopCoroutine(thunder());
        StopCoroutine(nattack());
        stop = true;
        canWalk = false;
        ready = false;
        changedir = false;
        damage = false;
        downback = false;
        anim.SetTrigger("down");
        yield return new WaitForSeconds(1.5f);
        downback = true;
        stop = false;
        canWalk = true;
        ready = true;
        changedir = true;
        cool[1] = 1;
        StartCoroutine(roll());
        yield return null;
    }
    IEnumerator damaged()
    {
        if (okd)
        {
            // ���� 22-12-27
            // ����� ġȯ
            // hp = hp - 5;
            hp -= ConstantValue.player_attack_damage;
            bar.GetComponent<Image>().fillAmount = hp / 100;
            okd = false;
        }
        yield return new WaitForSeconds(1.5f);
        okd = true;
    }
    public IEnumerator counter()
    {
        stop = true;
        canWalk = false;
        ready = false;
        anim.SetTrigger("counter");
        rb.velocity = new Vector2(0, 0);
        ghosting = false;
        changedir = false;
        yield return new WaitWhile(() => !playerMovement.counteranyaction);
        anim.SetTrigger("Throw");
    }
    IEnumerator cooldown(int skill, float time)
    {
        yield return new WaitForSeconds(time);
        cool[skill] = 1;
    }
    
    public void ThrowDagger()
    {
        var instance = Instantiate(Dagger);
        instance.transform.position = daggerPoint.position;
        instance.transform.localScale = new Vector3(transform.localScale.x, 1, 1);

    }
    public void ThrowEnd()
    {
        canWalk = true;
        stop = false;
        ready = true;
        changedir = true;
    }
    public void PlayerDead()
    {
        canWalk = false;
        ready = false;
    }
    void SpecialSkillEnd()
    {
        hitBoxCol.enabled = true;
        anim.SetTrigger("specialEnd");
    }
    void ForceOff()
    {
        anim.SetBool("Force", false);
    }
    public void AniCallClear()
    {
        playerMovement.ClearGame();
    }
}
