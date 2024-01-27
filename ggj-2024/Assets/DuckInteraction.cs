using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DuckInteraction : MonoBehaviour
{
    [SerializeField]
    private GameObject textBoxPrefab;

    [SerializeField]
    // Facts populated from random fact api
    private List<string> randomFacts = new();

    [SerializeField]
    // Facts to display when we can't get a web request to the random fact apit
    private List<string> fallbackFacts = new() {
        "Ducks have regional accents.",
        "Female ducks pick their favorite male ducks to mate with based on dancing ability.",
        "Free from human intervention, some ducks can live up to 20 years.",
        "Ducks have better vision than you do.",
        "Duck bills are as sensitive as human fingertips and palms.",
        "Ducks are capable of abstract thinking and have deep emotional lives.",
        "Ducks are meticulously clean animals."
    };

    [SerializeField]
    // Inbuilt fixed duck responses that will be displayed
    private List<string> fixedDuckResponses = new() {
        "Quack",
        "Quack",
        "Quack",
        "Honk",
        "Quack?",
        "Quack."
    };

    [SerializeField]
    // Duck will respond with 3 fixed responses, then will have a chance of spitting out random facts
    private uint fixedResponseCount = 3;

    [SerializeField]
    // Never populate the random facts list with more than this many facts from the web api
    private uint maxRandomFacts = 10;

    [SerializeField]
    // Once the duck is producing random facts, what is the chance of the player getting a random fact vs an inbuilt one.
    private float chanceOfRandomFact = 0.5f;

    [SerializeField]
    // Set to true to never send web requests and only use fallback facts
    private bool noWebRequests = false;

    private void Start()
    {
        StartCoroutine(GetFactsCoroutine());
    }


    private string RandomResponse(List<string> array)
    {
        return array[Random.Range(0, array.Count)];
    }

    public void OnInteract()
    {
        if(fixedResponseCount > 0)
        {
            fixedResponseCount--;
            // choose a random option from the fixed responses
            CreateTextBox(RandomResponse(fixedDuckResponses));
            return;
        }

        // We might be done with fixed responses, so roll a random 0->1 and check if we're sending a random fact or a fallback fact
        if(Random.Range(0f, 1f) > chanceOfRandomFact)
        {
            // in this case, just send a fixed duck response
            CreateTextBox(RandomResponse(fixedDuckResponses));
            return;
        }

        // We chose to send a fact, send a fact from the appropriate list based on availability

        if(randomFacts.Count > 0)
        {
            // send random fact then remove it to avoid dupes
            int index = Random.Range(0, randomFacts.Count);
            string fact = randomFacts[index];
            randomFacts.RemoveAt(index);
            CreateTextBox(fact);
            return;
        }

        // Otherwise just send one from the fallbacks
        CreateTextBox(RandomResponse(fallbackFacts));
        
    }

    private void CreateTextBox(string text)
    {
        GameObject canvas = GameObject.Find("Canvas");
        GameObject textBoxInstance = Instantiate(textBoxPrefab, canvas.transform);
        textBoxInstance.GetComponent<TextBox>().SetTextFragments(new string[] { text });
    }

    private IEnumerator GetFactsCoroutine()
    {
        string uri = "https://uselessfacts.jsph.pl/api/v2/facts/random";
        int failCount = 0;

        while (true)
        {
            while(randomFacts.Count < maxRandomFacts)
            {
                if (!noWebRequests)
                {
                    using UnityWebRequest webRequest = UnityWebRequest.Get(uri);
                    webRequest.SetRequestHeader("Accept", "text/plain");

                    // Request and wait for the desired page.
                    yield return webRequest.SendWebRequest();

                    switch (webRequest.result)
                    {
                        case UnityWebRequest.Result.ConnectionError:
                        case UnityWebRequest.Result.DataProcessingError:
                            failCount++;
                            break;
                        case UnityWebRequest.Result.ProtocolError:
                            Debug.LogError("HTTP Error: " + webRequest.error);
                            break;
                        case UnityWebRequest.Result.Success:
                            // strip out the "> " from the front and also the source
                            string strippedFact = webRequest.downloadHandler.text;
                            if (strippedFact.StartsWith("> ")) {
                                strippedFact = strippedFact.Substring(2);
                            }
                            int source = strippedFact.IndexOf("\n\nSource:");
                            if(source > 0)
                            {
                                strippedFact = strippedFact[..source];
                            }
                            Debug.Log("Got a random fact: " + strippedFact);
                            randomFacts.Add(strippedFact);
                            break;
                    }

                    if (failCount > 10)
                    {
                        Debug.LogError("Too many failed requests, aborting");
                        yield break;
                    }
                }

                // don't hit the api too fast
                yield return new WaitForSecondsRealtime(1f);
            }

            // Check every second if we have enough facts
            yield return new WaitForSecondsRealtime(1f);
        }
    }
}
