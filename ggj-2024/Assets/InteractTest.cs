using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractTest : MonoBehaviour
{
    public void DoInteract()
    {
        if(transform.localScale.x > 1)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(2, 2, 2);
        }
    }
}
