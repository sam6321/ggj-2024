using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitSign : MonoBehaviour
{
    [SerializeField]
    private FairyCounter counter;


    [SerializeField]
    private GameObject cantLeaveText;

    [SerializeField]
    private GameObject timeToLeave;

    public void OnInteract()
    {
        if(counter.CanExit())
        {
            Instantiate(timeToLeave, GameObject.Find("Canvas").transform).GetComponent<TextBox>().OnTextFinished.AddListener((confirm) => {
                if(!confirm)
                {
                    return;
                }

                if (counter.IsSpecialEnding())
                {
                    UnityEngine.SceneManagement.SceneManager.LoadScene("End");
                }
                else
                {
                    UnityEngine.SceneManagement.SceneManager.LoadScene("EndBad");
                }
            });
        }
        else
        {
            Instantiate(cantLeaveText, GameObject.Find("Canvas").transform);
        }
    }
}
