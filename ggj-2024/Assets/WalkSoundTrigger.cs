using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkSoundTrigger : MonoBehaviour
{
    [SerializeField]
    private PlayerMovement.WalkSound sound;
    public PlayerMovement.WalkSound Sound => sound;
}
