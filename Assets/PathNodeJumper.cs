using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PathNodeJumper : MonoBehaviour {

    public Text debugText;
    public Transform player;
    public string pathName;
    public int currentPathIndex;
    public PlayerMove movementScript;
    public float distanceMargin;
    [Range(0f, 1f)]
    public float playerPathPosition, forwardPathPosition, backwardPathPosition;
    public float movementSpeed;

    public GameObject forwardCheckTransform, backwardCheckTransform, smoothedForwardGO, smoothedBackwardGO, movementReferenceSmoothed;
    public float movementVector, checkTime, checkMargin, smootheTime, movementMult;
    public Rigidbody rb;
    public float transformSmoothingTime;
    public float pathProximityMargin;
    public int solverTimer;

    PathProximityResolver pathSolver;

    void Start () {
        //  InvokeRepeating("UpdatePosition", 0, checkTime);
        pathSolver = GetComponent<PathProximityResolver>();
        InvokeRepeating("FindPositionOnPath", 0, checkTime);
    }
	
    void ClampValues()
    {
        Mathf.Clamp(playerPathPosition, 0f, 1f);
    }

    void SetTransformToPath()
    {
        
        iTween.PutOnPath(gameObject, iTweenPath.GetPath(pathName), playerPathPosition);
    }

    void SetSubTransforms()
    {
        iTween.PutOnPath(forwardCheckTransform, iTweenPath.GetPath(pathName), forwardPathPosition);
        iTween.PutOnPath(backwardCheckTransform, iTweenPath.GetPath(pathName), backwardPathPosition);
    }

    void CalculateSurroundingValues()
    {
        forwardPathPosition = playerPathPosition + distanceMargin;
        backwardPathPosition = playerPathPosition - distanceMargin;
        Mathf.Clamp(forwardPathPosition, 0f, 1f);
        Mathf.Clamp(backwardPathPosition, 0f, 1f);
    }

    void FindPositionOnPath()
    {
        float desiredPathLocation = PathProximityResolver.RecursiveBinarySearch(player.position, iTweenPath.GetPath(pathName), playerPathPosition - pathProximityMargin, playerPathPosition + pathProximityMargin, solverTimer);
        playerPathPosition = Mathf.Lerp(playerPathPosition, desiredPathLocation, smootheTime * Time.deltaTime);
    }

    void UpdatePosition()
    {
        float desiredPathLocation = 0f;
        if (1==1)// if (movementVector != 0f) //if I'm moving
        {
          //  debugText.text = "";
         //   debugText.text += "I'm moving!";
            float distToForward = Vector3.Distance(player.position, forwardCheckTransform.transform.position); //find player's distance to forward transform
            float distToBackward = Vector3.Distance(player.position, backwardCheckTransform.transform.position); // find player's distance to backward tranform
         //   debugText.text += "dist forward is " + distToForward + " and dist back is " + distToBackward;
        //    debugText.text += "margin is " + (Mathf.Abs(distToForward) - Mathf.Abs(distToBackward));
            if (Mathf.Abs(Mathf.Abs(distToForward) - Mathf.Abs(distToBackward)) > checkMargin) // if the distances aren't almost the same
            {
        //        debugText.text += " That's more than the margin!";

                if (distToForward < distToBackward)
                {//if I'm closer to forward
                    desiredPathLocation = playerPathPosition + movementSpeed; // move this transform up the path a little
              //      debugText.text += " I'm closer to the forward one!";
                }
                else
                {//I'm closer to the backward transform
                //    debugText.text += " I'm closer to the backward one!";
                    desiredPathLocation = playerPathPosition - movementSpeed; // move this transform down the path a little
                }
                playerPathPosition = Mathf.Lerp(playerPathPosition, desiredPathLocation, smootheTime * Time.deltaTime);
            }
        }
        else
        {
          //  debugText.text = "";
          //  debugText.text += "I'm stationary!";
            
        }
    }

    void FindPlayerVelocity()
    {
        movementSpeed = rb.velocity.magnitude  * movementMult * Time.deltaTime * Time.deltaTime;
    }

    void SetSmoothedTransforms()
    {
        movementReferenceSmoothed.transform.position = Vector3.Lerp(movementReferenceSmoothed.transform.position, transform.position, transformSmoothingTime * Time.deltaTime);
        smoothedForwardGO.transform.position = Vector3.Lerp(smoothedForwardGO.transform.position, forwardCheckTransform.transform.position, transformSmoothingTime * Time.deltaTime);
        smoothedBackwardGO.transform.position = Vector3.Lerp(smoothedBackwardGO.transform.position, backwardCheckTransform.transform.position, transformSmoothingTime * Time.deltaTime);
    }

    void Update () {

        FindPlayerVelocity();
        movementVector = movementScript.publicMovementVector.x;
        CalculateSurroundingValues();
        ClampValues();
        SetTransformToPath();
        SetSubTransforms();
        SetSmoothedTransforms();
    }
}
