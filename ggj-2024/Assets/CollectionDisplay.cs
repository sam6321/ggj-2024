using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectionDisplay : MonoBehaviour
{
    [SerializeField]
    private List<FairyData> fairies = new();

    [SerializeField]
    private List<Image> fairyImages = new();

    [SerializeField]
    private List<Text> fairyNames = new();

    [SerializeField]
    private List<FriendData> friends = new();

    [SerializeField]
    private List<Image> friendImages = new();

    [SerializeField]
    private List<Text> friendNames = new();

    [SerializeField]
    private GameObject collectionScreen;

    public void UpdateFriendFound(HashSet<FriendData.Friend> found, int total)
    {
        foreach(var friend in found)
        {
            var friendData = friends.Find(f => f.type == friend);
            var index = friends.FindIndex(f => f.type == friend);
            if (friendData != null && index >= 0)
            {
                friendImages[index].sprite = friendData.sprite;
                friendNames[index].text = friendData.friendName;
            }
        }
    }

    public void UpdateFairyCollection(HashSet<FairyData.Fairy> found, int total)
    {
        foreach(var fairy in found)
        {
            var fairyData = fairies.Find(f => f.type == fairy);
            var index = fairies.FindIndex(f => f.type == fairy);
            fairyImages[index].sprite = fairyData.sprite;
            fairyNames[index].text = fairyData.fairyName;
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            collectionScreen.SetActive(!collectionScreen.activeSelf);
        }
    }
}
