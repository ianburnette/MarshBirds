using UnityEngine;
using System.Collections;

public class FollowPlayerAlongPath : MonoBehaviour {

    public Transform player;
    public string pathName;
    public float pathPercentage;
    public float checkMargin, minMargin;
    public float smootheTime;
    public GameObject forwardChecker, backwardChecker;
    Transform referenceTransform;

	// Use this for initialization
	void Start () {
        referenceTransform = new GameObject().transform;
        referenceTransform.parent = transform;
	}
	
	// Update is called once per frame
	void Update () {
        pathPercentage = CalculatePercentage();
        iTween.PutOnPath(referenceTransform.gameObject, iTweenPath.GetPath(pathName), pathPercentage);
        transform.position = Vector3.Lerp(transform.position, referenceTransform.position, smootheTime * Time.deltaTime);
    }

    float CalculatePercentage()
    {
        float calculatedPercentage = pathPercentage;
       // Vector3 forwardCheck, backwardCheck;
        float forwardPercentageCheck = pathPercentage + checkMargin;
        float backwardPercentageCheck = pathPercentage - checkMargin;
        Mathf.Clamp(forwardPercentageCheck, 0f, 1f);
        Mathf.Clamp(backwardPercentageCheck, 0f, 1f);
        iTween.PutOnPath(forwardChecker, iTweenPath.GetPath(pathName), forwardPercentageCheck);
        iTween.PutOnPath(backwardChecker, iTweenPath.GetPath(pathName), backwardPercentageCheck);

      //  forwardCheck = iTween.PointOnPath(iTweenPath.GetPath(pathName), pathPercentage + checkMargin);
      //  backwardCheck = iTween.PointOnPath(iTweenPath.GetPath(pathName), pathPercentage - checkMargin);
        float forwardCheckDist = Vector3.Distance(forwardChecker.transform.position, player.transform.position);
        float backwardCheckDist = Vector3.Distance(backwardChecker.transform.position, player.transform.position);
      //  print("forward check is " + forwardCheck + " and back is " + backwardCheck);
        if (forwardCheckDist < backwardCheckDist && backwardCheckDist - forwardCheckDist > minMargin)
            calculatedPercentage = pathPercentage + checkMargin;
        else if (forwardCheckDist > backwardCheckDist && forwardCheckDist - backwardCheckDist > minMargin)
            calculatedPercentage = pathPercentage - checkMargin;
        return calculatedPercentage;
    }
}
