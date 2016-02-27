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
    public Transform myMovementReference, movementRefForward, movementRefBackward;

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
        movementRefForward = myUtility.transform.GetChild(3);
        movementRefBackward = myUtility.transform.GetChild(4);
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
            perpendicularVector = myMovementReference.forward;
            Vector3 distanceFromReference = transform.position - myMovementReference.position;

            //adjustedDistance = transform.position - myMovementReference.position;
            adjustedDistance = new Vector3(distanceFromReference.x, 0, distanceFromReference.z);
            //Debug.DrawRay(transform.position, adjustedDistance, Color.cyan);
            Vector3 velocity = FindVelocityVector(adjustedDistance);
            ModifyVelocity(velocity);
            if (rb.velocity.magnitude != 0)
            {
              
            }
            //transform.position = new Vector3(nodeScript.movementReferenceSmoothed.transform.position.x,transform.position.y,nodeScript.movementReferenceSmoothed.transform.position.z);
          //  Debug.DrawRay(transform.position, Vector3.forward * 5f, Color.blue);
            
          
           Debug.DrawRay(transform.position, rb.velocity, Color.yellow);
           Debug.DrawRay(transform.position, Vector3.Project(rb.velocity, adjustedDistance), Color.blue);
            //rb.velocity = Vector3.Project(rb.velocity, adjustedDistance);

            //transform.Translate(-adjustedDistance * adjustmentSpeed * Time.deltaTime, Space.World);
            adjustedDistance = Vector3.Project(adjustedDistance, perpendicularVector);
          

            Vector3 correctionVector3 = -adjustedDistance + perpendicularVector;
            Debug.DrawRay(transform.position, correctionVector3, Color.black);
            Debug.DrawRay(transform.position, adjustedDistance, Color.red);
            // rb.AddForce(-adjustedDistance * adjustmentSpeed * adjustedDistance.magnitude * Time.deltaTime);

        }
    }

    Vector3 FindVelocityVector(Vector3 correction)
    {
        Vector3 calcVel = rb.velocity;
        Vector3 pathVector = Vector3.zero;
        Vector3 forwardPos = new Vector3(movementRefForward.position.x, transform.position.y, movementRefForward.position.z);
        Vector3 backwardPos = new Vector3(movementRefBackward.position.x, transform.position.y, movementRefBackward.position.z);
        Debug.DrawRay(forwardPos, backwardPos-forwardPos);
        pathVector = backwardPos - forwardPos;
        calcVel = Vector3.Project(calcVel, pathVector);
        //correction.Normalize();
        correction = correction - Vector3.Project(correction, pathVector);
        Debug.DrawRay(transform.position, correction, Color.cyan);
        calcVel += -correction * adjustmentSpeed * Time.deltaTime;
        Debug.DrawRay(transform.position, calcVel, Color.magenta);
        
        return calcVel;
    }

    void ModifyVelocity(Vector3 vel)
    {
        rb.velocity = new Vector3(vel.x, rb.velocity.y, vel.z);
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