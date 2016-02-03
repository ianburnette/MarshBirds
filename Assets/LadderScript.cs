using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class LadderScript : MonoBehaviour {

    public bool inLadderZone;
    public iTweenPath pathScript;

    public float upperPercentage, lowerPercentage;
    public string lowerPath, upperPath;
    public float upperPathDistMargin, lowerPathDistMargin, upperPathStartingPercentage, lowerPathStartingPercentage, upperSmoothingTime, lowerSmoothingTime;

    private PlayerLadder playerLadderScript;
    public MoveAlongPath pathMovement;
    private PathTransformRotation pathRotation;

	void Start () {
        playerLadderScript = GameObject.Find("Player").GetComponent<PlayerLadder>();
        pathScript = GetComponent<iTweenPath>();
	}

	void Update () {
        playerLadderScript.inZone = inLadderZone;       
    }

    public void TriggerEntered(int whichTrigger, Transform other)
    {
        inLadderZone = true;
        pathMovement = other.GetComponent<MoveAlongPath>();
        pathMovement.pathName = pathScript.pathName;
        if (whichTrigger == 0)
        {
            pathMovement.pathPercentage = upperPercentage;
            playerLadderScript.pathControlScript.pathName = upperPath;
            playerLadderScript.pathControlScript.distanceMargin = upperPathDistMargin;
            playerLadderScript.pathControlScript.objectPathPosition = upperPathStartingPercentage;
            playerLadderScript.pathControlScript.smootheTime= upperSmoothingTime;
        }
        else
        {
            pathMovement.pathPercentage = lowerPercentage;
            playerLadderScript.pathControlScript.pathName = lowerPath;
            playerLadderScript.pathControlScript.distanceMargin = lowerPathDistMargin;
            playerLadderScript.pathControlScript.objectPathPosition = lowerPathStartingPercentage;
            playerLadderScript.pathControlScript.smootheTime = lowerSmoothingTime;
        }
    }

    public void TriggerExited()
    {
        inLadderZone = false;
    }
}
