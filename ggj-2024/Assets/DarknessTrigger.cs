using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarknessTrigger : MonoBehaviour
{
    [SerializeField]
    private DarknessController darknessController;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            darknessController.TransitionToDarkness();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            darknessController.TransitionToLight();
        }
    }
}
