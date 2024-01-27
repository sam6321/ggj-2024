using UnityEngine;

public class FriendInteract : MonoBehaviour
{
    [SerializeField]
    private FriendData.Friend friend;

    public void OnInteract()
    {
        GameObject.Find("FairyCounter").GetComponent<FairyCounter>().AddFriend(friend);
    }
}
