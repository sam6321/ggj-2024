using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressAnyKeyToExit : MonoBehaviour
{
    private float start = 0f;

    // Start is called before the first frame update
    void Start()
    {
        start = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.anyKeyDown && Time.time - start > 5f)
        {
            // quit unity app
            Application.Quit();
        }
    }
}
