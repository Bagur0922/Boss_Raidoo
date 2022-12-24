using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HP : MonoBehaviour
{
    [SerializeField] List<GameObject> Hearts;

    public int health = 3;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(health == 3)
        {
            Hearts[0].SetActive(true);
            Hearts[1].SetActive(true);
            Hearts[2].SetActive(true);
        }
        else if (health == 2)
        {
            Hearts[0].SetActive(true);
            Hearts[1].SetActive(true);
            Hearts[2].SetActive(false);
        }
        else if(health == 1)
        {
            Hearts[0].SetActive(true);
            Hearts[1].SetActive(false);
            Hearts[2].SetActive(false);
        }
        else
        {
            Hearts[0].SetActive(false);
            Hearts[1].SetActive(false);
            Hearts[2].SetActive(false);
        }
    }
}
