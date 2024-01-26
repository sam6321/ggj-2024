using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractTest : MonoBehaviour
{

    [SerializeField]
    private GameObject prefab;

    public void DoInteract()
    {
        GameObject canvas = GameObject.Find("Canvas");
        Instantiate(prefab, canvas.transform);
    }
}
