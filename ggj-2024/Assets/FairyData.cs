using UnityEngine;

[CreateAssetMenu(fileName = "FairyData", menuName = "ScriptableObjects/FairyData", order = 1)]
public class FairyData : ScriptableObject
{
    public enum Fairy
    {
        DarkForest, // fame
        Forest, // mers
        Pond, // immer
        Garden, // fel
        Tree // propen
    }

    public Fairy type;
    public string fairyName;
    public string description;
    public Sprite sprite;
}
