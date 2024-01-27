using UnityEngine;

public class GenericTextInteraction : MonoBehaviour
{
    [SerializeField]
    private GameObject textBoxPrefab;

    public void OnInteract()
    {
        GameObject canvas = GameObject.Find("Canvas");
        Instantiate(textBoxPrefab, canvas.transform);
    }
}
