using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MoveAlongPath))]
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

    private Quaternion targetRotation;
    private Transform referenceTransform;
    private MoveAlongPath pathMovementScript;
    //this variable controls the direction that the transform looks when orienting to path. it is multiplied by horizontalInput.
    private float inputPercentageDivisor = .01f;
    private float calculatedLookPercentage;
    private float normalizedLookPercentage;

    void Start () {
        
        pathMovementScript = GetComponent<MoveAlongPath>();
        referenceTransform = new GameObject().transform;
        referenceTransform.transform.name = "PathRotationReferenceTransform";
        if (alignToPath)
            StartingLookPosition(true);
        else if (lookAtTarget)
            StartingLookPosition(false);
            
    }

	void FixedUpdate () {
        SetReferencePosition();
        CalculateRotation();
        SetRotation();
	}

    void SetReferencePosition()
    {
        referenceTransform.position = transform.position;
    }

    void StartingLookPosition(bool pathState)
    {
        if (pathState)
            calculatedLookPercentage = pathMovementScript.pathPercentage - (lookForwardAmt / 100f);
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
            float hInput = Input.GetAxis("Horizontal");
            if (hInput > 0)
                calculatedLookPercentage = pathMovementScript.pathPercentage - (lookForwardAmt / 100f) + hInput * -inputPercentageDivisor;
            else if (hInput < 0)
                calculatedLookPercentage = pathMovementScript.pathPercentage + (lookForwardAmt / 100f) + hInput * -inputPercentageDivisor;
            normalizedLookPercentage = 0f;

            //Normalize this value if the percentage is outside 0-100 range
            if (pathMovementScript.loop) //If the path is a closed loop, then the values loop around as well
            {
                if (calculatedLookPercentage > 1f)
                    normalizedLookPercentage = calculatedLookPercentage - 1f;
                else if (calculatedLookPercentage < 0f)
                    normalizedLookPercentage = 1f + calculatedLookPercentage;
                else
                    normalizedLookPercentage = calculatedLookPercentage;
            }
            else //If the path is not a closed loop, just look at the end of the path as you get close to it
            {
                normalizedLookPercentage = Mathf.Clamp(calculatedLookPercentage, 0f, 1f);
            }

            //Set the reference transform's rotation to the calculated rotation
            referenceTransform.transform.LookAt(iTween.PointOnPath(iTweenPath.GetPath(pathMovementScript.pathName),(normalizedLookPercentage)));
        }
    }

    void SetRotation()
    {
        if (smoothLookRotation) //Smoothe the transform's rotation to match the reference transform's rotation
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, referenceTransform.rotation, smoothSpeed * Time.deltaTime);
        }else //Set hte transform's rotation to equal the reference transform's rotaton
        {
            transform.rotation = referenceTransform.rotation;
        }
    }
}