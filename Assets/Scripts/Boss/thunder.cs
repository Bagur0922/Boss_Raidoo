using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class thunder : MonoBehaviour
{
    Animator anim;
    SpriteRenderer sr;

    Rigidbody2D rb;

    bool direction;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = GetComponentInParent<Transform>().position;
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        direction = GetComponentInParent<BossMovement>().direction;
        if (direction)
        {
            transform.localScale = new Vector2(1, 1);
            rb.velocity = new Vector2(20, 0);
        }
        else
        {
            transform.localScale = new Vector2(-1, 1);
            rb.velocity = new Vector2(-20, 0);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag != "Boss")
        {
            Destroy(gameObject);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
