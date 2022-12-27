using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class b_HitBox : MonoBehaviour
{
    public GameObject player;
    PlayerMovement playerM;
    public GameObject boss;
    BossMovement bossM;
    
    public bool b_dir;
    public bool p_dir;

    bool counter;
    bool damage;

    public float distance;

    void Start()
    {
        playerM = player.GetComponent<PlayerMovement>();
        bossM = boss.GetComponent<BossMovement>();
    }

    void LateUpdate()
    {
        if (bossM == null) return;
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

        if (damage && bossM.damage && Mathf.Abs(distance) < 3.6f
            && playerM.isAttacking)
        {
            if(bossM.isPlayspecialSkill == true)
                return;

            StartCoroutine(bossM.down());
        }
        else if(counter && bossM.damage && bossM.ready
            && Mathf.Abs(distance) < 3.6f && playerM.isAttacking)
        {
            playerM.damge = false;
            StartCoroutine(playerM.counter());
            StartCoroutine(bossM.counter());
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
