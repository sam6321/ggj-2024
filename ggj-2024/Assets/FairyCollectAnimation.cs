using UnityEngine;
using UnityEngine.U2D;

public class FairyCollectAnimation : MonoBehaviour
{
    public GameObject fairy;
    public GameObject fairyText;

    [SerializeField]
    private float controlPointDistance = 48f;

    [SerializeField]
    private float duration = 2.0f;

    private float startTime = 0.0f;

    private Vector3 startPoint;
    private Vector3 startControlPoint;
    private Vector3 endControlPoint;
    private Vector3 endPoint;

    private void Start()
    {
        PlayerMovement.Instance.AddBlocker(new PlayerMovement.Blocker
        {
            key = this,
            isInteractionBlocker = true,
            isMovementBlocker = true
        });

        // create a random curve between the fairy and the text and then animate the fairy across that curve
        startPoint = fairy.transform.position;
        endPoint = Camera.main.ScreenToWorldPoint(CentreOfRectTransform(fairyText.GetComponent<RectTransform>()));
        startControlPoint = startPoint + (Vector3)Random.insideUnitCircle * controlPointDistance;
        endControlPoint = endPoint;// + (Vector3)Random.insideUnitCircle * controlPointDistance;

        startTime = Time.time;


        var sr = fairy.GetComponent<SpriteRenderer>();
        sr.sortingLayerName = "AlwaysOnTop";
    }

    private void OnDestroy()
    {
        PlayerMovement.Instance.RemoveBlocker(this);
    }

    private Vector3 CentreOfRectTransform(RectTransform rect)
    {
        Vector3[] corners = new Vector3[4];
        rect.GetWorldCorners(corners);
        return (corners[0] + corners[2]) / 2;
    }


    private void Update()
    {
        float t = Mathf.Clamp01((Time.time - startTime) / duration);
        fairy.transform.position = BezierUtility.BezierPoint(startControlPoint, startPoint, endPoint, endControlPoint, t);
        fairy.transform.localScale = Vector3.one * (1.0f - t);

        if(t == 1.0f)
        {
            var fairyData = fairy.GetComponent<FairyInteract>().FairyData;
            GameObject.Find("FairyCounter").GetComponent<FairyCounter>().AddFairy(fairyData.type);
            Destroy(gameObject);
            Destroy(fairy);
        }
    }
}
