using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockPlayerOnActive : MonoBehaviour
{

    [SerializeField]
    private bool isInteractionBlocker = true;

    [SerializeField]
    private bool isMovementBlocker = true;

    private void OnEnable()
    {
        PlayerMovement.Instance.AddBlocker(new PlayerMovement.Blocker
        {
            key = this,
            isInteractionBlocker = isInteractionBlocker,
            isMovementBlocker = isMovementBlocker
        });
    }

    private void OnDisable()
    {
        PlayerMovement.Instance.RemoveBlocker(this);
    }
}
