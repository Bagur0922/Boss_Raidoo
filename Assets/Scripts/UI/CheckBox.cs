using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckBox : MonoBehaviour
{
    GameObject onObj;
    GameObject offObj;
    private void Awake()
    {
        offObj = transform.GetChild(0).gameObject;
        onObj = transform.GetChild(1).gameObject;

        offObj.SetActive(true);
        onObj.SetActive(false);
    }

    public void CheckSet(bool state)
    {
        if (state)
        {
            offObj.SetActive(false);
            onObj.SetActive(true);
        }
        else
        {
            offObj.SetActive(true);
            onObj.SetActive(false);

        }
    }
}
