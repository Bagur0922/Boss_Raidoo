using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingManager : MonoBehaviour
{
    public float timer = 0;
    [SerializeField] BossMovement boss;

    void Update()
    {
        if(boss != null)
        {
            timer += Time.deltaTime;
        }
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
