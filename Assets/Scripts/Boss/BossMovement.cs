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

    float distance;
    // Start is called before the first frame update
    void Start(){
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update(){
        distance = player.transform.position.x - gameObject.transform.position.x;
        look();
        skill();
    }
    void look(){
        if(distance > 0 && changedir){
            gameObject.transform.localScale = new Vector3(1, 1, 1);
            watchingdir = true;
        }
        else if(changedir){
            gameObject.transform.localScale = new Vector3(-1, 1, 1);
            watchingdir = false;
        }
    }
    void skill()
    {
        if (/*Input.GetKeyDown(KeyCode.A) && */Mathf.Abs(distance) < 5 && ready){
            ready = false;
            StartCoroutine(thunder());
        }
    }
    IEnumerator thunder(){
        anim.SetTrigger("thunder_start");
        changedir = false;
        attacking = true;
        yield return new WaitForSeconds(0.375f);
        if (watchingdir)
        {
            //transform.position = transform.position + Vector3.right * 8;
            rb.AddForce(Vector2.right * 120f, ForceMode2D.Impulse);
        }
        else
        {
            //transform.position = transform.position + Vector3.left * 8;
            rb.AddForce(Vector2.left * 120f, ForceMode2D.Impulse);
        }
        yield return new WaitForSeconds(0.0645f);
        attacking = false;
        rb.velocity = new Vector2(0, 0);
        anim.SetTrigger("thunder_end");
        yield return new WaitForSeconds(0.5f);
        anim.SetTrigger("backtostand");
        changedir = true;
        ready = true;
        yield return null;
    }
    IEnumerator down()
    {
        anim.SetTrigger("down");
        ready = false;
        yield return new WaitForSeconds(1.5f);
        ready = true;
        yield return null;
    }
}
