using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class LadderScript : MonoBehaviour {

    public bool inLadderZone;
    public iTweenPath pathScript;
    private PlayerLadder playerLadderScript;
    private MoveAlongPath pathMovement;
    private PathTransformRotation pathRotation;

	// Use this for initialization
	void Start () {
        playerLadderScript = GameObject.Find("Player").GetComponent<PlayerLadder>();
        pathScript = GetComponent<iTweenPath>();
	}
	
	// Update is called once per frame
	void Update () {
        playerLadderScript.inZone = inLadderZone;       
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            inLadderZone = true;
            pathMovement = other.GetComponent<MoveAlongPath>();
            pathMovement.pathName = pathScript.pathName;
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Player")
            inLadderZone = false;
    }
}
