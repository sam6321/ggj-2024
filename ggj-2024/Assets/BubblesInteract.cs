using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubblesInteract : MonoBehaviour
{
    [SerializeField]
    GameObject fairyPrefab;

    public void OnInteract()
    {
        Instantiate(fairyPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
