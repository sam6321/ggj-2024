using UnityEngine;

public class FairyInteract : MonoBehaviour
{
    [SerializeField]
    private GameObject fairyInteractPrefab;

    [SerializeField]

    private GameObject fairyTextBoxPrefab;

    [SerializeField]
    private FairyData fairyData;

    public FairyData FairyData => fairyData;

    public void OnInteract()
    {
        GameObject parent = GameObject.Find("Canvas");
        TextBox textBox = Instantiate(fairyTextBoxPrefab, parent.transform).GetComponent<TextBox>();
        textBox.OnTextFinished.AddListener((playerSaidYes) =>
        {
            if (playerSaidYes)
            {
                FairyCollectAnimation animation = Instantiate(fairyInteractPrefab).GetComponent<FairyCollectAnimation>();
                animation.fairy = gameObject;
                animation.fairyText = GameObject.Find("FairyText");
            }
            else
            {
                GameObject.Find("FairyCounter").GetComponent<FairyCounter>().IgnoreFairy(fairyData.type);
            }
        });
    }
}
