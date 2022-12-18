using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class b_HitBox : MonoBehaviour
{
    public GameObject player;
    public GameObject boss;
    
    public bool b_dir;
    public bool p_dir;
    bool damage;

    public float distance;

    // Start is called before the first frame update
    void Start()
    {
        
    }



    // Update is called once per frame
    void Update()
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

        if (damage && boss.GetComponent<BossMovement>().damage && Mathf.Abs(distance) < 3.6f && player.GetComponent<PlayerMovement>().isAttacking)
        {
            StartCoroutine(boss.GetComponent<BossMovement>().down());
        }
    }
}
