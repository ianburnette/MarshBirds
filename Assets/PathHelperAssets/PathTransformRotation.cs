using UnityEngine;
using System.Collections;

//[RequireComponent(typeof(MoveAlongPath))]
public class PathTransformRotation : MonoBehaviour {

    [Tooltip("If this is true, the transform will always face lookTarget.")]
    public bool lookAtTarget = false;
    [Tooltip("If this is true AND lookAtTarget is false, the transform will face a point on the path ahead of the direction it's travelling.")]
    public bool alignToPath = false;
    [Tooltip("If a transform is assigned here AND lookAtTarget is true, the transform will always face it.")]
    public Transform lookTarget;
    [Tooltip("If this is true, the look rotation will be smoothed. If it's false, it will be set instantly.")]
    public bool smoothLookRotation;
    [Tooltip("The speed at which the look rotation is smoothed. 5 is a good starting value.")]
    public float smoothSpeed = 5f;
    [Tooltip("If alignToPath is true, this is the distance along the path that the transform should look forward, measured in percentage of path length.")]
    [Range(0f, 100f)]
    public float lookForwardAmt = 25f;
    public Transform toSet;
    public bool ladder = false;

    private Quaternion targetRotation;
    public Transform referenceTransform;
    private PathNodeJumper pathMovementScript;
    private MoveAlongPath ladderPath;
    //this variable controls the direction that the transform looks when orienting to path. it is multiplied by horizontalInput.
    private float inputPercentageDivisor = .01f;
    private float calculatedLookPercentage;
    private float normalizedLookPercentage;
    private float foundPercentage;

    void Start () {

        if (toSet == null)
            toSet = transform;
        if (!ladder)
            pathMovementScript = GetComponent<PathNodeJumper>();
        else
            ladderPath = GetComponent<MoveAlongPath>();
        referenceTransform = new GameObject().transform;
        referenceTransform.transform.name = "PathRotationReferenceTransform";
        if (alignToPath)
            StartingLookPosition(true);
        else if (lookAtTarget)
            StartingLookPosition(false);
    }

	void FixedUpdate () {
        GetPercentage();
        SetReferencePosition();
        CalculateRotation();
        SetRotation();
	}

    void GetPercentage()
    {
        if (!ladder)
            foundPercentage = pathMovementScript.objectPathPosition;
        else
            foundPercentage = ladderPath.pathPercentage;
    }

    void SetReferencePosition()
    {
        referenceTransform.position = toSet.position;
    }

    void StartingLookPosition(bool pathState)
    {
        if (pathState)
            calculatedLookPercentage = foundPercentage - (lookForwardAmt / 100f);
        else if (lookTarget == null)
        {
                lookTarget = new GameObject().transform;
                lookTarget.position = Vector3.zero;
                lookTarget.name = "automaticallyCreatedLookTarget";
                Debug.Log("No lookTarget was assigned, so I created one.");
        }
    }

    void CalculateRotation()
    {
        if (lookAtTarget)
            referenceTransform.transform.LookAt(lookTarget);
        else if (alignToPath)
        {
            //Determine the percentage along the path that the transform should look at
            float hInput;
            if (ladder)
            {
                hInput = Input.GetAxis("Vertical");
                calculatedLookPercentage = foundPercentage + (lookForwardAmt / 100f) * -inputPercentageDivisor;
            }
            else {
                hInput = Input.GetAxis("Horizontal");
                if (hInput > 0)
                    calculatedLookPercentage = foundPercentage - (lookForwardAmt / 100f) + hInput * -inputPercentageDivisor;
                else if (hInput < 0)
                    calculatedLookPercentage = foundPercentage + (lookForwardAmt / 100f) + hInput * -inputPercentageDivisor;
            }


            normalizedLookPercentage = 0f;

            //Normalize this value if the percentage is outside 0-100 range
          /*  if (pathMovementScript.loop) //If the path is a closed loop, then the values loop around as well
            {
                if (calculatedLookPercentage > 1f)
                    normalizedLookPercentage = calculatedLookPercentage - 1f;
                else if (calculatedLookPercentage < 0f)
                    normalizedLookPercentage = 1f + calculatedLookPercentage;
                else
                    normalizedLookPercentage = calculatedLookPercentage;
            }*/
            //else //If the path is not a closed loop, just look at the end of the path as you get close to it
            //{
                normalizedLookPercentage = Mathf.Clamp(calculatedLookPercentage, 0f, 1f);
            //}

            //Set the reference transform's rotation to the calculated rotation
            if (!ladder) 
                referenceTransform.transform.LookAt(iTween.PointOnPath(iTweenPath.GetPath(pathMovementScript.pathName),(normalizedLookPercentage)));
            else
                referenceTransform.transform.LookAt(iTween.PointOnPath(iTweenPath.GetPath(ladderPath.pathName), (normalizedLookPercentage)), Vector3.up);
        }
    }

    void SetRotation()
    {
        if (smoothLookRotation) //Smoothe the transform's rotation to match the reference transform's rotation
        {
            //toSet.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.position - referenceTransform.position), smoothSpeed * Time.deltaTime);
            //Quaternion.Lerp(transform.rotation, Quaternion.Euler(Vector3.Project(referenceTransform.rotation.eulerAngles,transform.rotation.eulerAngles)), smoothSpeed * Time.deltaTime);
            //toSet.rotation = Quaternion.Lerp(transform.rotation, referenceTransform.rotation, smoothSpeed * Time.deltaTime);
            Quaternion currentRotation = transform.rotation;
            Quaternion goalRotation = toSet.rotation;
            toSet.eulerAngles = new Vector3(toSet.eulerAngles.x, 180, toSet.eulerAngles.z);
            toSet.rotation = Quaternion.Lerp(currentRotation, goalRotation, smoothSpeed * Time.deltaTime);
        }
        else //Set the transform's rotation to equal the reference transform's rotaton
        {
            toSet.rotation = referenceTransform.rotation;
        }
    }
}