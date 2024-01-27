using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBox : MonoBehaviour
{
    [SerializeField]
    private string[] textFragments = new string[1] { "" };

    [SerializeField]
    private float charsPerSecond = 10;

    [SerializeField]
    private Text text;

    [SerializeField]
    private AudioSource audioSource;

    private uint currentFragmentIndex = 0;
    private float percent = 0f;
    private float lastChirpTime = 0f;

    public void SetTextFragments(string[] fragments)
    {
        textFragments = fragments;
    }

    private void Start()
    {
        currentFragmentIndex = 0;
        percent = 0f;

        if(textFragments.Length == 0)
        {
            // just so something displays
            textFragments = new string[1] { "" };
        }

        for(int i = 0; i < textFragments.Length; i++)
        {
            textFragments[i] = textFragments[i].Replace("\\n", "\n");
        }

        PlayerMovement.Instance.AddBlocker(new PlayerMovement.Blocker { 
            key = this,
            isInteractionBlocker = true,
            isMovementBlocker = true
        });
    }

    private void OnDestroy()
    {
        PlayerMovement.Instance.RemoveBlocker(this);
    }    

    private void Update()
    {
        percent = Mathf.Min(percent + Time.deltaTime * charsPerSecond / textFragments[currentFragmentIndex].Length, 1.0f);

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (percent < 1.0f)
            {
                percent = 1.0f;
            }
            else
            {
                currentFragmentIndex++;
                if (currentFragmentIndex > textFragments.Length - 1)
                {
                    Destroy(gameObject);
                    return;
                }
                percent = 0f;
            }
        }

        if (percent < 1.0f)
        {
            text.text = textFragments[currentFragmentIndex][..Mathf.FloorToInt(textFragments[currentFragmentIndex].Length * percent)];

            float delay = 2.0f / charsPerSecond;
            if(lastChirpTime + delay < Time.time)
            {
                lastChirpTime = Time.time;
                audioSource.PlayOneShot(audioSource.clip);
            }
        }
        else
        {
            text.text = textFragments[currentFragmentIndex];
        }
    }
}
