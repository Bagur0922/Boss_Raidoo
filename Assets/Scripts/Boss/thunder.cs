using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class thunder : MonoBehaviour
{
    Animator anim;
    SpriteRenderer sr;

    Rigidbody2D rb;

    public bool attacking = true;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        
        rb.velocity = new Vector2(50 * transform.localScale.x, 0);
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
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
