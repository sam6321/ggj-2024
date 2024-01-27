using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class FitScreenSize : MonoBehaviour
{

    void Start()
    {
        SetSize();
    }

    void Update()
    {
        SetSize();
    }

    void SetSize()
    {
        transform.localScale = new Vector3(Screen.width, Screen.height, 1);
    }
}
