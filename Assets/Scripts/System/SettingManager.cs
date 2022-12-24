using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(Time.timeScale);
        if (Input.GetKeyDown(KeyCode.Escape) && Time.timeScale == 1)
        {
            Time.timeScale = 0;
        }
        else if(Time.timeScale == 0 && Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 1;
        }
    }
}
