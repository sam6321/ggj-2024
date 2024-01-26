using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [SerializeField]
    UnityEvent<GameObject> onInteract = new();

    public void OnInteract(GameObject source)
    {
        onInteract.Invoke(source);
    }
}
