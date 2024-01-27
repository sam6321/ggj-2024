using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FairyTextFormatter : MonoBehaviour
{
    [SerializeField]
    private FairyCounter fairyCounter;

    [SerializeField]
    private Text text;

    private void Start()
    {
        UpdateFairyText(fairyCounter.Found, fairyCounter.Total);
    }

    public void UpdateFairyText(HashSet<FairyData.Fairy> found, int total)
    {
        text.text = $"{found.Count} / {total}";
    }   
}
