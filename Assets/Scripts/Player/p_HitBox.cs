using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class p_HitBox : MonoBehaviour
{
    public GameObject player;
    public GameObject boss;

    bool attacking;
    bool Dashing;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (attacking && Dashing && other.tag == "b_AttackBox")
        {
            StartCoroutine(player.GetComponent<PlayerMovement>().damaged());
        }
    }
    // Update is called once per frame
    void Update()
    {
        attacking = boss.GetComponent<BossMovement>().attacking;
        Dashing = player.GetComponent<PlayerMovement>().damge;
    }
}
