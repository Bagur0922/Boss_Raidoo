using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class p_HitBox : MonoBehaviour
{
    public GameObject player;
    public GameObject boss;
    public GameObject health;
    PlayerMovement playerMovement;
    HP hpUI;

    void Start()
    {
        playerMovement = player.GetComponent<PlayerMovement>();
        if (health != null) hpUI = health.GetComponent<HP>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other);
        if(other.GetComponent<b_AttackBox>() != null)
        {
            if (boss.GetComponent<BossMovement>().attacking && playerMovement.damge && other.tag == "b_AttackBox")
            {
                StartCoroutine(playerMovement.damaged());
                hpUI.Damaged();
            }
        }
        else if (other.GetComponent<thunder>() != null)
        {
            if (other.GetComponent<thunder>().attacking && playerMovement.damge && other.tag == "b_AttackBox")
            {
                StartCoroutine(playerMovement.damaged());
                hpUI.Damaged();
            }
        }
        else if (other.GetComponent<LightingMove>() != null)
        {
            if (other.GetComponent<LightingMove>().attacking && playerMovement.damge && other.tag == "b_AttackBox")
            {
                StartCoroutine(playerMovement.damaged());
                hpUI.Damaged();

                Destroy(other.gameObject);
            }
        }
    }
    
}
