using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class b_HitBox : MonoBehaviour
{
    public GameObject player;
    public GameObject boss;
    
    public bool b_dir;
    public bool p_dir;

    bool counter;
    bool damage;

    public float distance;

    // Start is called before the first frame update
    void Start()
    {
        
    }



    // Update is called once per frame
    void LateUpdate()
    {
        distance = boss.transform.position.x - player.transform.position.x;
        if(boss.transform.localScale  == new Vector3(1, 1, 1))
        {
            b_dir = true;
        }
        else if(boss.transform.localScale == new Vector3(-1, 1, 1))
        {
            b_dir = false;
        }
        if (player.transform.localScale == new Vector3(1, 1, 1))
        {
            p_dir = true;
        }
        else if (player.transform.localScale == new Vector3(-1, 1, 1))
        {
            p_dir = false;
        }
        if (b_dir && distance > 0 && p_dir || !b_dir && distance < 0 && !p_dir)
        {
            damage = true;
        }
        else
        {
            damage = false;
        }

        if(b_dir && distance < 0 && !p_dir || !b_dir && distance > 0 && p_dir)
        {
            counter = true;
        }
        else
        {
            counter = false;
        }

        if (damage && boss.GetComponent<BossMovement>().damage && Mathf.Abs(distance) < 3.6f && player.GetComponent<PlayerMovement>().isAttacking)
        {
            StartCoroutine(boss.GetComponent<BossMovement>().down());
        }
        else if(counter && boss.GetComponent<BossMovement>().damage && boss.GetComponent<BossMovement>().ready && Mathf.Abs(distance) < 3.6f && player.GetComponent<PlayerMovement>().isAttacking)
        {
            StartCoroutine(boss.GetComponent<BossMovement>().counter());
            StartCoroutine(player.GetComponent<PlayerMovement>().counter());
            if (p_dir)
            {
                player.transform.position = boss.transform.position + Vector3.left * 1.7f;
            }
            else
            {
                player.transform.position = boss.transform.position + Vector3.right * 1.7f;
            }
        }
    }
}
