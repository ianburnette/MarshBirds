using UnityEngine;
using System.Collections;

public class CameraFollowPath : MonoBehaviour {

    public string currentPath;
    public PathNodeJumper playerPathScript;
    public cameraLookAt cameraAimingScript;
    public MoveAlongPath pathScript;


	void FixedUpdate () {
        currentPath = playerPathScript.pathName;
        pathScript.pathName = "cam" + currentPath;
        pathScript.pathPercentage = playerPathScript.objectPathPosition;
	}
}
