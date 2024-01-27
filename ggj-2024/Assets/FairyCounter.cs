using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FairyCounter : MonoBehaviour
{
    [SerializeField]
    private HashSet<FairyData.Fairy> found = new();

    public HashSet<FairyData.Fairy> Found => found;

    [SerializeField]

    private HashSet<FriendData.Friend> foundFriends = new();

    public HashSet<FriendData.Friend> FoundFriends => foundFriends;

    [SerializeField]
    private int total = 5;

    [SerializeField]
    private int totalFriends = 3;

    public int Total => total;

    public int TotalFriends => totalFriends;

    [SerializeField]
    private UnityEvent<HashSet<FairyData.Fairy>, int> onFairyFound = new();

    [SerializeField]
    private UnityEvent<HashSet<FriendData.Friend>, int> onFriendFound = new();

    public void Start()
    {
        onFairyFound.Invoke(found, total);
    }

    public void AddFairy(FairyData.Fairy fairy)
    {
        found.Add(fairy);
        onFairyFound.Invoke(found, total);
    }

    public void AddFriend(FriendData.Friend friend)
    {
        foundFriends.Add(friend);
        onFriendFound.Invoke(foundFriends, totalFriends);
    }
}
