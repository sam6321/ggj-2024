using UnityEngine;

public class FairyInteract : MonoBehaviour
{
    [SerializeField]
    private GameObject fairyInteractPrefab;

    [SerializeField]
    private FairyData fairyData;

    public FairyData FairyData => fairyData;

    public void OnInteract()
    {
        FairyCollectAnimation animation = Instantiate(fairyInteractPrefab).GetComponent<FairyCollectAnimation>();
        animation.fairy = gameObject;
        animation.fairyText = GameObject.Find("FairyText");
    }
}
