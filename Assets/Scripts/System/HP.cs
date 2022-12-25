using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HP : MonoBehaviour
{


    [SerializeField] List<GameObject> Hearts;
    [SerializeField] PlayerMovement player;

    public int health = 3;

    public void Damaged()
    {
        health--;
        Hearts[health].SetActive(false);
        if (health < 0) player.Dead();
    }
    public void InitSet()
    {
        health = 3;
        foreach (GameObject go in Hearts)
        {
            go.SetActive(true);
        }
    }
}
