using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost_Effect : MonoBehaviour
{
    [SerializeField] bool pb; //true면 플레이어, false면 보스

    public GameObject player;
    PlayerMovement playerM;
    public SpriteRenderer psr;
    public SpriteRenderer sr;

    public float delay;

    void Start()
    {
        sr = gameObject.GetComponent<SpriteRenderer>();
        playerM = player.GetComponent<PlayerMovement>();
        StartCoroutine(move());
        sr.enabled = false;
    }

    void Update()
    {
        if (playerM == null) return;
        if (pb)
        {
            if (!playerM.ghosting)
            {
                sr.enabled = false;
            }
            else
            {
                sr.enabled = true;
            }
        }
        else if (!pb)
        {
            if(playerM != null)
            {
                if (!playerM.ghosting)
                {
                    sr.enabled = false;
                }
                else
                {
                    sr.enabled = true;
                }
            }
        }
    }
    IEnumerator move()
    {
        if (playerM != null)
            while (true)
            {
                if (pb)
                {
                    gameObject.transform.position = player.transform.position; sr.sprite = psr.sprite;
                    if (playerM.direction)
                    {
                        transform.localScale = new Vector3(1, 1, 1);
                    }
                    else if (!playerM.direction)
                    {
                        transform.localScale = new Vector3(-1, 1, 1);
                    }
                    yield return new WaitForSeconds(delay);
                }
                else if (!pb)
                {
                    gameObject.transform.position = player.transform.position; sr.sprite = psr.sprite;
                    if (playerM.direction)
                    {
                        transform.localScale = new Vector3(1, 1, 1);
                    }
                    else if (!playerM.direction)
                    {
                        transform.localScale = new Vector3(-1, 1, 1);
                    }
                    yield return new WaitForSeconds(delay);
                }
            }
        
    }
}
