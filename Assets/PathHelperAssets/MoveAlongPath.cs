using UnityEngine;
using System.Collections;

public class MoveAlongPath : MonoBehaviour {

    [Tooltip("The location on the path that the transform will be set. This is controlled automatically.")]
    public float pathPercentage;
    [Tooltip("1-10 is a good range to start with")]
    public float moveSpeed = 5f;
    [Tooltip("If the transform should jump to the beginning of the path when it reaches the end, and vice versa. Best if last node is at same position as first.")]
    public bool loop = true;
    public bool controllable;

    public bool smoothMovement;
    public float smoothSpeed;

    public string pathName;
    private Transform referenceTransform;
   
    void Start()
    {
        referenceTransform = new GameObject().transform;
        referenceTransform.name = "PathMovementReferenceTransform";
    }

    void FixedUpdate () {
        if (controllable)
            GetControls();
        SetReferencePosition();
        SetPosition();
	}

    void GetControls() {
        pathPercentage -= Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime * Time.deltaTime;
        if (loop)
        {
            if (pathPercentage > 1f)
                pathPercentage = 0;
            else if (pathPercentage < 0f)
                pathPercentage = 1f;
        }
        else
            pathPercentage = Mathf.Clamp(pathPercentage, 0f, 1f);
    }

    void SetReferencePosition()
    {
        referenceTransform.position = iTween.PointOnPath(iTweenPath.GetPath(pathName), pathPercentage);
    }

    void SetPosition()
    {
        if (smoothMovement)
            transform.position = Vector3.Lerp(transform.position, referenceTransform.position, smoothSpeed * Time.deltaTime);
        else
            transform.position = referenceTransform.position;
    }
  
}

