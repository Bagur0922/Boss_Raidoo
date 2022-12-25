using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class p_HitBox : MonoBehaviour
{
    public GameObject player;
    public GameObject boss;
    public GameObject health;

    bool damage;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other);
        if(other.GetComponent<b_AttackBox>() != null)
        {
            if (boss.GetComponent<BossMovement>().attacking && damage && other.tag == "b_AttackBox")
            {
                StartCoroutine(player.GetComponent<PlayerMovement>().damaged());
                health.GetComponent<HP>().health--;
            }
        }
        else if (other.GetComponent<thunder>() != null)
        {
            if (other.GetComponent<thunder>().attacking && damage && other.tag == "b_AttackBox")
            {
                StartCoroutine(player.GetComponent<PlayerMovement>().damaged());
                health.GetComponent<HP>().health--;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        damage = player.GetComponent<PlayerMovement>().damge;
    }
}
