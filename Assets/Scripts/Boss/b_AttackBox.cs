using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class b_AttackBox : MonoBehaviour
{
    BoxCollider2D bc;

    [SerializeField] GameObject boss;
    BossMovement bossM;

    // Start is called before the first frame update
    void Start() {
        bc = gameObject.GetComponent<BoxCollider2D>();
        bossM = boss.GetComponent<BossMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (bossM == null) return;
        if (bossM.attacking)
        {
            bc.enabled = true;
        }
        else
        {
            bc.enabled = false;
        }
    }
}
