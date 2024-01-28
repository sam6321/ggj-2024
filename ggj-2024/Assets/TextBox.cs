using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class TextBox : MonoBehaviour
{
    [SerializeField]
    private string[] textFragments = new string[1] { "" };

    [SerializeField]
    private float charsPerSecond = 10;

    [SerializeField]
    private GameObject coreTextBoxObject;

    [SerializeField]
    private Text text;

    [SerializeField]
    private Text yesOrNoTextObject;

    [SerializeField]
    private Text yesTextObject;    
    
    [SerializeField]
    private Text noTextObject;

    [SerializeField]
    private string yesOrNoText = "";

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private Sprite characterPortraitSprite;

    [SerializeField]
    private Image characterPortrait;

    [SerializeField]
    private UnityEvent<bool> onTextFinished = new();

    [SerializeField]
    private bool chooseRandom = false;

    public UnityEvent<bool> OnTextFinished => onTextFinished;

    private uint currentFragmentIndex = 0;
    private float percent = 0f;
    private float lastChirpTime = 0f;
    private bool yes = true;
    private float yesOrNoStartTime = 0f;

    public void SetTextFragments(string[] fragments)
    {
        textFragments = fragments;
    }

    private void Start()
    {
        if(characterPortraitSprite)
        {
            characterPortrait.sprite = characterPortraitSprite;
            characterPortrait.gameObject.SetActive(true);
        }
        else
        {
            var rt = coreTextBoxObject.GetComponent<RectTransform>();
            rt.offsetMin -= new Vector2(64, 0);
            rt.offsetMax -= new Vector2(64, 0);
        }

        currentFragmentIndex = 0;
        percent = 0f;

        if(textFragments.Length == 0)
        {
            // just so something displays
            textFragments = new string[1] { "" };
        }

        if (chooseRandom)
        {
            textFragments = new string[1]
            {
                textFragments[Random.Range(0, textFragments.Length)]
            };
        }

        for (int i = 0; i < textFragments.Length; i++)
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
        if (yesOrNoTextObject.gameObject.activeSelf)
        {
            UpdateYesOrNo();
            return;
        }

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
                    if(yesOrNoText == "")
                    {
                        Finished(false);
                    }
                    else
                    {
                        StartYesOrNo();
                    }
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

    private void Finished(bool result)
    {
        onTextFinished.Invoke(result);
        Destroy(gameObject);
    }

    private void StartYesOrNo()
    {
        text.gameObject.SetActive(false);
        yesOrNoTextObject.gameObject.SetActive(true);
        yesOrNoTextObject.text = yesOrNoText;
        yesTextObject.gameObject.SetActive(true);
        noTextObject.gameObject.SetActive(true);
        yesOrNoStartTime = Time.time;
    }

    private void UpdateYesOrNo()
    {
        if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S))
        {
            yes = !yes;
        }

        if(yes)
        {
            yesTextObject.text = "Yes <";
            noTextObject.text = "No";
        }
        else
        {
            yesTextObject.text = "Yes";
            noTextObject.text = "No <";
        }

        // delay for a sec so that they don't cycle through the text too fast
        if (Input.GetKeyDown(KeyCode.E) && Time.time > yesOrNoStartTime + 0.5f)
        {
            Finished(yes);
        }
    }
}
