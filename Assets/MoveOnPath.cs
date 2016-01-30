using UnityEngine;
using System.Collections;

public class MoveOnPath : MonoBehaviour
{
    public GameObject movementUtility;
    public bool held;
    public string myPath;
    public float myCheckTime;
    private PathNodeJumper nodeScript;
    private GameObject gm, myUtility;

    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameController");
        SetupUtility();
    }

    void SetupUtility()
    {
        myUtility = (GameObject)Instantiate(movementUtility, Vector3.zero, Quaternion.identity);
        myUtility.transform.parent = gm.transform.GetChild(0).transform;
        nodeScript = myUtility.GetComponentInChildren<PathNodeJumper>();
        nodeScript.objectToSet = transform;
        nodeScript.pathName = myPath;
        nodeScript.checkTime = myCheckTime;
    }

    void FixedUpdate()
    {
        if (!held)
        {
            transform.position = new Vector3(nodeScript.movementReferenceSmoothed.transform.position.x,
                                             transform.position.y,
                                             nodeScript.movementReferenceSmoothed.transform.position.z);
        }
    }

    void OnDestroy()
    {
        Destroy(myUtility);
    }
}