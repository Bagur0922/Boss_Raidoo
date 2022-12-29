using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Byuck : MonoBehaviour
{
    [SerializeField] bool trigger = false;
    public bool attacking = false;
    // Start is called before the first frame update
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        if (trigger)
        {
            Destroy(gameObject);
        }
    }
}
