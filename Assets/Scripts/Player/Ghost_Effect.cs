using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost_Effect : MonoBehaviour
{
    bool direction = true;

    public GameObject player;
    public SpriteRenderer psr;
    public SpriteRenderer sr;

    public float delay;
    // Start is called before the first frame update
    void Start()
    {
        sr = gameObject.GetComponent<SpriteRenderer>();
        StartCoroutine(move());
        sr.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!player.GetComponent<PlayerMovement>().ghosting)
        {
            sr.enabled = false;
        }
        else
        {
            sr.enabled = true;
        }
    }
    IEnumerator move()
    {
        while (true)
        {
            gameObject.transform.position = player.transform.position; sr.sprite = psr.sprite;
            if (player.GetComponent<PlayerMovement>().direction)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            else if (!player.GetComponent<PlayerMovement>().direction)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            yield return new WaitForSeconds(delay);
            yield return null;
        }
        
    }
}
