using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class thunder : MonoBehaviour
{
    Animator anim;
    SpriteRenderer sr;

    Rigidbody2D rb;

    public bool attacking = true;

    bool direction;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector2(GetComponentInParent<Transform>().position.x, -0.8f);
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        direction = GetComponentInParent<BossMovement>().direction;
        Debug.Log(transform.position.x);
        if (direction)
        {
            transform.localScale = new Vector2(1, 1);
            rb.velocity = new Vector2(50, 0);
        }
        else
        {
            transform.localScale = new Vector2(-1, 1);
            rb.velocity = new Vector2(-50, 0);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            Destroy(gameObject);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(transform.position.x > 9 && transform.position.x > 0 || transform.position.x < 0 && transform.position.x < -9)
        {
            Destroy(gameObject);
        }
    }
}
