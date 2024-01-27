using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenRectScaler : MonoBehaviour
{
    [SerializeField]
    // Ensures that this will fit within the screen scaled down by this amount
    private float screenScale = 0.75f;

    private Vector2 lastRes = new();

    private void Start()
    {
        CheckScale();
    }

    // Update is called once per frame
    void Update()
    {
        CheckScale();
    }

    private void CheckScale()
    {
        // this is shit but whatever
        Vector2 res = new(Screen.width, Screen.height);
        if (res != lastRes)
        {
            lastRes = res;
            // update size of collection screen to suit the smallest width or height of the screen, snapped to a multiple of 0.25, 0.5, 1, 2 of the original size of the image
            Vector2 originalRes = GetComponent<RectTransform>().rect.size;
            Vector2 screenScaled = res * screenScale;
            Vector2 chosenRes = originalRes;
            foreach (Vector2 resToCheck in new Vector2[]
            {
                originalRes * new Vector2(0.25f, 0.25f),
                originalRes * new Vector2(0.5f, 0.5f),
                originalRes * new Vector2(1, 1),
                originalRes * new Vector2(1.25f, 1.25f),
                originalRes * new Vector2(1.5f, 1.5f),
                originalRes * new Vector2(1.75f, 1.75f),
                originalRes * new Vector2(2, 2),
            })
            {
                if (resToCheck.x <= screenScaled.x && resToCheck.y <= screenScaled.y)
                {
                    chosenRes = resToCheck;
                }
            }
            Vector3 finalScale = chosenRes / originalRes;
            finalScale.z = 1; // pixel perfect canvas can't handle 0 z scale
            transform.localScale = finalScale;
        }
    }
}
