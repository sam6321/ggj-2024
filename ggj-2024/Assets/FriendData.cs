using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FriendData", menuName = "ScriptableObjects/FriendData", order = 1)]
public class FriendData : ScriptableObject
{
    public enum Friend
    {
        Deer,
        Bird,
        Spider,
        BeanCan,
        Gnome
    }

    public Friend type;
    public string friendName;
    public string description;
    public Sprite sprite;
}
