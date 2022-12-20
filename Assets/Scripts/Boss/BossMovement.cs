using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMovement : MonoBehaviour
{
    Animator anim;
    SpriteRenderer sr;
    Rigidbody2D rb;
    public GameObject player;

    int xmove;

    bool changedir = true;

    public bool ready = true;
    public bool attacking = true;
    public bool damage = true;
    public bool ghosting = false;
    public bool direction = false;
    public bool counteren = true;

    [SerializeField] public int[] cool; //1은 사용 가능, 0은 쿨타임중. 0은 벽력일섬, 1은 구르기

    float distance;
    float speed;
    // Start is called before the first frame update
    void Start()
    {
        //SoundPlayer.instance.startBGM("Start");

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
        player.GetComponent<PlayerMovement>().anyaction = true;
    }
    // Update is called once per frame
    void Update()
    {
        distance = player.transform.position.x - gameObject.transform.position.x;
        look();
        skill();
        if (Input.GetKey(KeyCode.R))
        {
            changedir = false;
        }
        else
        {
            changedir = true;
        }
    }
    void look()
    {
        if (distance > 0 && changedir)
        {
            gameObject.transform.localScale = new Vector3(1, 1, 1);
            direction = true;
        }
        else if (changedir)
        {
            gameObject.transform.localScale = new Vector3(-1, 1, 1);
            direction = false;
        }
    }
    void skill()
    {
        if (Mathf.Abs(distance) < 5 && ready && cool[0] == 1)
        {
            ready = false;
            StartCoroutine(thunder());
        }
        else if (Mathf.Abs(distance) < 2 && ready && cool[1] == 1)
        {
            changedir = false;
            ready = false;
            StartCoroutine(roll());
        }
    }
    IEnumerator roll()
    {
        if(cool[1] == 1)
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
            StartCoroutine(cooldown(1, 2.5f));
            rb.velocity = new Vector2(0, 0);
            speed = 7;
            changedir = true;
            ghosting = false;
            ready = true;
        }
        
    }
    IEnumerator thunder()
    {
        anim.SetTrigger("thunder_start");
        changedir = false;
        yield return new WaitForSeconds(0.375f);
        if (direction)
        {
            rb.AddForce(Vector2.right * 120f, ForceMode2D.Impulse);
        }
        else
        {
            rb.AddForce(Vector2.left * 120f, ForceMode2D.Impulse);
        }
        yield return new WaitForSeconds(0.0645f);
        rb.velocity = new Vector2(0, 0);
        anim.SetTrigger("thunder_end");
        yield return new WaitForSeconds(0.5f);
        anim.SetTrigger("backtostand");
        changedir = true;
        ready = true;
        cool[0] = 0;
        StartCoroutine(cooldown(0, 15));
        yield return null;
    }
    public IEnumerator down()
    {
        changedir = false;
        damage = false;
        anim.SetTrigger("down");
        ready = false;
        yield return new WaitForSeconds(1.5f);
        damage = true;
        ready = true;
        changedir = true;
        yield return null;
    }
    public IEnumerator counter()
    {
        anim.SetTrigger("counter");
        rb.velocity = new Vector2(0, 0);
        ready = false;
        ghosting = false;
        changedir = false;
        yield return new WaitWhile(() => !player.GetComponent<PlayerMovement>().anyaction);
        ready = true;
        changedir = true;
    }
    IEnumerator cooldown(int skill, float time)
    {
        yield return new WaitForSeconds(time);
        cool[skill] = 1;
    }
}
