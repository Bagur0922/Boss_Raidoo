using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class b_AttackBox : MonoBehaviour
{
    BoxCollider2D bc;

    [SerializeField] GameObject boss;

    // Start is called before the first frame update
    void Start() {
        bc = gameObject.GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (boss.GetComponent<BossMovement>().attacking)
        {
            bc.enabled = true;
        }
        else
        {
            bc.enabled = false;
        }
    }
}
