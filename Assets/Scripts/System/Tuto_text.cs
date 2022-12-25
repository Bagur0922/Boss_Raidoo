using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tuto_text : MonoBehaviour
{
    TextMeshProUGUI tm;
    [SerializeField] List<string> tutosaying;
    // Start is called before the first frame update
    void Start()
    {
        tm = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        tm.text = tutosaying[0];
    }
}
