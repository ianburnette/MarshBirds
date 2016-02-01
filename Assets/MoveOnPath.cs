using UnityEngine;
using System.Collections;

public class MoveOnPath : MonoBehaviour
{
    public GameObject movementUtility;
    public bool held;
    public string myPath;
    public float myCheckTime, myStartPercentage;
    private PathNodeJumper nodeScript;
    private GameObject gm, myUtility;
    public Transform myMovementReference;

    public Vector3 perpendicularVector, adjustedDistance;
    public float adjustmentSpeed;
    public Rigidbody rb;

    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameController");
        rb = GetComponent<Rigidbody>();
        SetupUtility();
    }

    void SetupUtility()
    {
        myUtility = (GameObject)Instantiate(movementUtility, Vector3.zero, Quaternion.identity);
        myUtility.transform.parent = gm.transform.GetChild(0).transform;
        myMovementReference = myUtility.transform.GetChild(5);
        nodeScript = myUtility.GetComponentInChildren<PathNodeJumper>();
        nodeScript.objectToSet = transform;
        nodeScript.pathName = myPath;
        nodeScript.checkTime = myCheckTime;
        nodeScript.objectPathPosition = myStartPercentage;
    }

    void FixedUpdate()
    {
        if (!held)
        {
            //transform.position = new Vector3(nodeScript.movementReferenceSmoothed.transform.position.x,transform.position.y,nodeScript.movementReferenceSmoothed.transform.position.z);
            Debug.DrawRay(transform.position, Vector3.forward * 5f, Color.blue);
            
            perpendicularVector = myMovementReference.forward;
            Vector3 distanceFromReference = transform.position - myMovementReference.position;
            
            //adjustedDistance = transform.position - myMovementReference.position;
            adjustedDistance = new Vector3(distanceFromReference.x, 0, distanceFromReference.z);
            Debug.DrawRay(transform.position, adjustedDistance, Color.cyan);
            transform.Translate(-adjustedDistance * adjustmentSpeed * Time.deltaTime, Space.World);
            adjustedDistance = Vector3.Project(adjustedDistance, perpendicularVector);
          

            Vector3 correctionVector3 = -adjustedDistance + perpendicularVector;
            Debug.DrawRay(transform.position, correctionVector3, Color.black);
            Debug.DrawRay(transform.position, adjustedDistance, Color.red);
            // rb.AddForce(-adjustedDistance * adjustmentSpeed * adjustedDistance.magnitude * Time.deltaTime);

        }
    }

    public void NewPathPos(float newValue)
    {
        nodeScript.objectPathPosition = newValue;
    }

    void OnDestroy()
    {
        Destroy(myUtility);
    }
}