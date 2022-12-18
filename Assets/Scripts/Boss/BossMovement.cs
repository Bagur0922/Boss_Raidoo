using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMovement : MonoBehaviour
{
    Animator anim;
    SpriteRenderer sr;
    Rigidbody2D rb;
    public GameObject player;

    bool changedir = true;
    bool watchingdir = false;
    bool ready = true;

    public bool attacking = true;
    public bool damage = true;

    [SerializeField] public int[] cool;

    float distance;
    // Start is called before the first frame update
    void Start()
    {
        SoundPlayer.instance.startBGM("Start");

        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        StartCoroutine(start());

        cool[0] = 1;
    }
    IEnumerator start()
    {
        rb.velocity = new Vector2(-1, 0);
        yield return new WaitForSeconds(1.3f);
        rb.velocity = new Vector2(0, 0);
        yield return new WaitForSeconds(1.45f);
        anim.SetTrigger("intro_end");
        yield return new WaitForSeconds(4.35f);
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
            watchingdir = true;
        }
        else if (changedir)
        {
            gameObject.transform.localScale = new Vector3(-1, 1, 1);
            watchingdir = false;
        }
    }
    void skill()
    {
        if (/*Input.GetKeyDown(KeyCode.A) && */Mathf.Abs(distance) < 5 && ready && cool[0] == 1)
        {
            ready = false;
            StartCoroutine(thunder());
        }
    }
    IEnumerator thunder()
    {
        anim.SetTrigger("thunder_start");
        changedir = false;
        yield return new WaitForSeconds(0.375f);
        if (watchingdir)
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
    IEnumerator cooldown(int skill, float time)
    {
        yield return new WaitForSeconds(time);
        cool[skill] = 1;
    }
}
