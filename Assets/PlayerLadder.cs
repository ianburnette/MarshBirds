using UnityEngine;
using System.Collections;

public class PlayerLadder : MonoBehaviour {

    public bool inZone, onLadder;
    public PathNodeJumper pathControlScript;
    public CharacterMotor motorScript;
    public PlayerMove moveScript;
    public Throwing throwScript;
    public Health healthScript;
    public PlayerInventory inventoryScript;
    public MoveAlongPath pathMovement;
    public PathTransformRotation pathRotation;
    private string pathName;

	void Update () {
        if (inZone && !onLadder)
            GetZoneLadderInput(); 
        else if (onLadder)
        {
            GetOnLadderInput();
            SetLadderPosition();
        }
	}

    void SetLadderPosition()
    {
         pathName = pathMovement.pathName;
         iTween.PutOnPath(gameObject, iTweenPath.GetPath(pathName), pathMovement.pathPercentage);
    }

    void GetOnLadderInput() {
        if (Input.GetButtonDown("Grab"))
        {
            LetGo();
        }if (Input.GetButtonDown("Jump"))
        {
            JumpOff();
        }
    }

    void GetZoneLadderInput()
    {
        if (Input.GetButtonDown("Grab"))
            GrabLadder();
    }

    void LetGo()
    {
        SetScripts(true);
        onLadder = false;
        SetPhysicsComponents(true);
    }

    void JumpOff()
    {
        print("jumping off");
        SetScripts(true);
        onLadder = false;
        SetPhysicsComponents(true);
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
    }

    void SetPhysicsComponents(bool state)
    {
        GetComponent<Rigidbody>().useGravity = state;
        GetComponent<Rigidbody>().isKinematic = !state;
        GetComponent<Collider>().isTrigger = !state;
    }

    void GrabLadder()
    {
        print("grabbing");
        SetScripts(false);
        onLadder = true;
        SetPhysicsComponents(false);
    }

    void SetScripts(bool state)
    {
       // gameObject.SetActive(state);
       // motorScript.enabled = state;
        moveScript.enabled = state;
        throwScript.enabled = state;
        healthScript.enabled = state;
        inventoryScript.enabled = state;
        pathMovement.enabled = !state;
        pathRotation.enabled = !state;
    }

    void OnDrawGizmos()
    {
        //pathName = pathMovement.pathName;
       // Gizmos.DrawSphere(iTween.PointOnPath(iTweenPath.GetPath(pathName), pathMovement.pathPercentage), .3f);
    }
}
