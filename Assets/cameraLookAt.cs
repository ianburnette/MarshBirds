using UnityEngine;
using System.Collections;

public class cameraLookAt : MonoBehaviour {

    public Transform target;
    public Transform rotationTarget;
    public float followDelay;
    public Transform gm;
    public Transform forwardTarget, backwardTarget, targetOverride;
    public bool overrideCameraTarget;
    public PlayerMove movementScript;
    public float movementMargin;

    void Start()
    {
        rotationTarget = new GameObject().transform;
        rotationTarget.name = "cameraRotationTarget";
        rotationTarget.parent = gm;
    }

    void DetermineTarget()
    {
        if (!overrideCameraTarget)
        {
            if (movementScript.publicMovementVector.x > movementMargin)
                target = forwardTarget;
            else if (movementScript.publicMovementVector.x < -movementMargin)
                target = backwardTarget;
        }
        else 
        {
            target = targetOverride;
        }
    }

	void Update () {
        DetermineTarget();
        if (followDelay <= 0)
            FollowImmediately();
        else
            FollowWithDelay();
	}

    void FollowImmediately()
    {
        transform.LookAt(target);
    }

    void FollowWithDelay()
    {
        rotationTarget.position = transform.position;
        rotationTarget.LookAt(target);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotationTarget.rotation, followDelay * Time.deltaTime);
    }
}
